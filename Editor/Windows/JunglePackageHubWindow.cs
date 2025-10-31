#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
/// <summary>
/// Editor window that lists recommended Jungle packages and short descriptions for onboarding.
/// </summary>
public class JunglePackageHubWindow : EditorWindow
{
    [Serializable] private class PackageEntry
    {
        public string displayName;
        public string packageName;   // com.company.package
        public string gitUrl;
        public string description;
        public string imageUrl;      // optional
    }
    [Serializable] private class PackageConfig
    {
        public List<PackageEntry> packages = new();
    }
    /// <summary>
    /// Stores metadata for a package hub image so the UI can render icons with tooltips.
    /// </summary>
    private class ImageRecord
    {
        public string URL;
        public Texture2D TEX;
        public UnityWebRequest Req;
        public bool Failed;
    }

    // State
    private PackageConfig config = new();
    private readonly Dictionary<string, bool> installed = new();   // packageName -> installed?
    private readonly Dictionary<string, ImageRecord> images = new(); // url -> record
    private ListRequest listRequest;
    private AddRequest addRequest;
    private string configPath;
    private double lastPump;

    // UI refs
    private VisualTreeAsset windowUxml;
    private VisualTreeAsset itemUxml;
    private ScrollView listView;
    private Button reloadBtn;
    private Button refreshBtn;
    private Label footerMsg;

    private const string PackageRoot = "Packages/com.jungle.core";
    private const string DefaultConfigAssetSearch = "upm-sources t:TextAsset";
    /// <summary>
    /// Opens the window.
    /// </summary>
    [MenuItem("Tools/Jungle/Package Hub")]
    public static void Open()
    {
        var w = GetWindow<JunglePackageHubWindow>("Jungle Package Hub");
        w.minSize = new Vector2(620, 420);
        w.Show();
    }
    /// <summary>
    /// Sets up the UI tree and starts polling package data when the window opens.
    /// </summary>
    private void OnEnable()
    {
        // Load UXML/USS
        windowUxml = Load<VisualTreeAsset>($"{PackageRoot}/Editor/Packaging/Resources/JunglePackageHub.uxml");
        itemUxml   = Load<VisualTreeAsset>($"{PackageRoot}/Editor/Packaging/Resources/JunglePackageItem.uxml");

        // Build root from UXML
        rootVisualElement.Clear();
        if (windowUxml != null)
            windowUxml.CloneTree(rootVisualElement);

        // Attach USS styles
        TryAddStyle(rootVisualElement, $"{PackageRoot}/Editor/Resources/JungleEditorStyles.uss");
        TryAddStyle(rootVisualElement, $"{PackageRoot}/Editor/Resources/JunglePackageListStyles.uss");

        // Query UI
        listView   = rootVisualElement.Q<ScrollView>("package-list");
        reloadBtn  = rootVisualElement.Q<Button>("btn-reload-config");
        refreshBtn = rootVisualElement.Q<Button>("btn-refresh-installed");
        footerMsg  = rootVisualElement.Q<Label>("footer-msg");

        // Wire toolbar
        reloadBtn?.RegisterCallback<ClickEvent>(_ => { LoadConfig(); RefreshInstalled(); });
        refreshBtn?.RegisterCallback<ClickEvent>(_ => RefreshInstalled());

        // Init data
        configPath = FindConfigPath();
        LoadConfig();
        RefreshInstalled();

        EditorApplication.update += Pump;
    }
    /// <summary>
    /// Stops background polling and disposes pending image downloads when the window closes.
    /// </summary>
    private void OnDisable()
    {
        EditorApplication.update -= Pump;
        foreach (var rec in images.Values) rec.Req?.Dispose();
    }

    // -------- UI Refresh --------
    /// <summary>
    /// Rebuilds the UI list of available packages.
    /// </summary>
    private void RebuildList()
    {
        listView?.Clear();

        if (config?.packages == null || config.packages.Count == 0)
        {
            footerMsg.text = configPath == null
                ? "No upm-sources.json found.\nPut it anywhere under Assets and click Reload."
                : $"No packages defined in: {configPath}";
            return;
        }

        foreach (var p in config.packages)
        {
            var row = itemUxml.Instantiate();
            row.AddToClassList("jungle-card");

            // Bind fields
            var title   = row.Q<Label>("pkg-title");
            var desc    = row.Q<Label>("pkg-desc");
            var status  = row.Q<Label>("pkg-status");
            var img     = row.Q<Image>("pkg-image");
            var btnInst = row.Q<Button>("btn-install");
            var btnOpen = row.Q<Button>("btn-open");

            title.text = string.IsNullOrEmpty(p.displayName) ? p.packageName : p.displayName;
            desc.text  = p.description ?? "";

            // Status & button state
            var isInstalled = installed.TryGetValue(p.packageName ?? "", out var b) && b;
            status.text = isInstalled ? "Installed" : "Not installed";
            status.RemoveFromClassList("status-installed");
            status.RemoveFromClassList("status-missing");
            status.AddToClassList(isInstalled ? "status-installed" : "status-missing");
            btnInst.SetEnabled(!isInstalled && addRequest == null);

            // Buttons
            btnInst.clicked += () => StartInstall(p);
            btnOpen.clicked += () => Application.OpenURL(ExtractRepoWebUrl(p.gitUrl));

            // Image (async fetch)
            var tex = GetImageForPackage(p);
            img.image = tex ?? (Texture2D)EditorGUIUtility.Load("d_UnityEditor.InspectorWindow.png");

            // Meta (optional: show packageName/gitUrl in tooltips)
            title.tooltip = p.packageName;
            btnOpen.tooltip = p.gitUrl;

            listView.Add(row);
        }

        footerMsg.text = "";
    }

    // -------- Config I/O --------
    /// <summary>
    /// Determines the configuration file path for package metadata.
    /// </summary>
    private string FindConfigPath()
    {
        var direct = "Assets/UPM Package Hub/upm-sources.json";
        if (File.Exists(direct)) return direct;

        var guids = AssetDatabase.FindAssets(DefaultConfigAssetSearch);
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.EndsWith("upm-sources.json", StringComparison.OrdinalIgnoreCase))
                return path;
        }
        return null;
    }
    /// <summary>
    /// Loads package configuration data from disk.
    /// </summary>
    private void LoadConfig()
    {
        config = new PackageConfig();
        if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
        {
            RebuildList();
            return;
        }

        try
        {
            var json = File.ReadAllText(configPath);
            config = JsonUtility.FromJson<PackageConfig>(json) ?? new PackageConfig();
        }
        catch (Exception e)
        {
            Debug.LogError($"[Jungle Hub] Failed to read '{configPath}': {e.Message}");
            config = new PackageConfig();
        }

        // Pre-warm images
        images.Clear();
        if (config?.packages != null)
        {
            foreach (var p in config.packages)
            {
                var url = ResolveImageUrl(p);
                if (!string.IsNullOrEmpty(url)) BeginFetchImage(url);
            }
        }

        RebuildList();
    }

    // -------- UPM ops --------
    /// <summary>
    /// Refreshes the list of installed packages.
    /// </summary>
    private void RefreshInstalled()
    {
        installed.Clear();
        listRequest = Client.List(true);
        footerMsg.text = "Refreshing installed packages…";
    }
    /// <summary>
    /// Begins installing the selected package.
    /// </summary>
    private void StartInstall(PackageEntry p)
    {
        try
        {
            addRequest = Client.Add(p.gitUrl);
            footerMsg.text = $"Installing {p.displayName ?? p.packageName}…";
        }
        catch (Exception e)
        {
            Debug.LogError($"[Jungle Hub] Add failed for {p.gitUrl}: {e.Message}");
            addRequest = null;
            footerMsg.text = $"Install error: {e.Message}";
        }
    }
    /// <summary>
    /// Advances the asynchronous install process.
    /// </summary>
    private void Pump()
    {
        if (EditorApplication.timeSinceStartup - lastPump > 0.05f)
        {
            lastPump = EditorApplication.timeSinceStartup;
            // repaint via UI Toolkit is cheap; we only touch footer text below
        }

        // UPM list
        if (listRequest != null && listRequest.IsCompleted)
        {
            if (listRequest.Status == StatusCode.Success)
            {
                foreach (var info in listRequest.Result)
                    installed[info.name] = true;

                RebuildList();
                footerMsg.text = "Installed list refreshed.";
            }
            else if (listRequest.Status >= StatusCode.Failure)
            {
                footerMsg.text = $"List error: {listRequest.Error?.message}";
                Debug.LogError($"[Jungle Hub] List error: {listRequest.Error?.message}");
            }
            listRequest = null;
        }

        // UPM add
        if (addRequest != null && addRequest.IsCompleted)
        {
            if (addRequest.Status == StatusCode.Success)
            {
                footerMsg.text = $"Installed: {addRequest.Result?.name} {addRequest.Result?.version}";
                RefreshInstalled(); // will rebuild on completion
            }
            else if (addRequest.Status >= StatusCode.Failure)
            {
                footerMsg.text = $"Install error: {addRequest.Error?.message}";
                Debug.LogError($"[Jungle Hub] Install error: {addRequest.Error?.message}");
                RebuildList();
            }
            addRequest = null;
        }

        // Image downloads
        foreach (var rec in images.Values)
        {
            if (rec.Req == null) continue;
#if UNITY_2020_2_OR_NEWER
            if (!rec.Req.isDone) continue;
            bool ok = rec.Req.result == UnityWebRequest.Result.Success;
#else
            if (!rec.req.isDone) continue;
            bool ok = !rec.req.isNetworkError && !rec.req.isHttpError;
#endif
            if (ok)
            {
                var tex = DownloadHandlerTexture.GetContent(rec.Req);
                tex.wrapMode = TextureWrapMode.Clamp;
                rec.TEX = tex;
            }
            else rec.Failed = true;

            rec.Req.Dispose();
            rec.Req = null;

            // Update any row using this texture
            RebuildList(); // simple; could be optimized to only replace the image
        }
    }

    // -------- Images --------
    /// <summary>
    /// Retrieves the image asset for the specified package entry.
    /// </summary>
    private Texture2D GetImageForPackage(PackageEntry p)
    {
        var url = ResolveImageUrl(p);
        if (string.IsNullOrEmpty(url)) return null;

        if (images.TryGetValue(url, out var rec))
        {
            if (rec.TEX != null) return rec.TEX;
            if (rec.Failed) return null;
            return null; // still loading
        }

        BeginFetchImage(url);
        return null;
    }
    /// <summary>
    /// Starts fetching the package image asynchronously.
    /// </summary>
    private void BeginFetchImage(string url)
    {
        if (images.ContainsKey(url)) return;
        var rec = new ImageRecord { URL = url };
        try
        {
            var req = UnityWebRequestTexture.GetTexture(url);
            req.SendWebRequest();
            rec.Req = req;
        }
        catch
        {
            rec.Failed = true;
        }
        images[url] = rec;
    }
    /// <summary>
    /// Constructs the URL used to download the package image.
    /// </summary>
    private string ResolveImageUrl(PackageEntry p)
    {
        if (!string.IsNullOrEmpty(p.imageUrl)) return p.imageUrl;
        var m = Regex.Match(p.gitUrl ?? "", @"github\.com[:/](?<owner>[^/]+)/(?<repo>[^/#\.]+)");
        if (m.Success)
        {
            var owner = m.Groups["owner"].Value;
            var repo  = m.Groups["repo"].Value;
            return $"https://opengraph.githubassets.com/1/{owner}/{repo}";
        }
        return null;
    }
    /// <summary>
    /// Extracts the web URL from a repository slug.
    /// </summary>
    private string ExtractRepoWebUrl(string gitUrl)
    {
        if (string.IsNullOrEmpty(gitUrl)) return "https://github.com/";
        var cleaned = gitUrl;
        var hash = cleaned.IndexOf('#');
        if (hash >= 0) cleaned = cleaned.Substring(0, hash);
        cleaned = cleaned.Replace(".git", "");

        if (cleaned.StartsWith("git@github.com"))
        {
            var m = Regex.Match(cleaned, @"git@github\.com:(?<owner>[^/]+)/(?<repo>[^\.]+)");
            if (m.Success) return $"https://github.com/{m.Groups["owner"].Value}/{m.Groups["repo"].Value}";
        }
        return cleaned;
    }

    // -------- Helpers --------
    private static T Load<T>(string path) where T : UnityEngine.Object
        => AssetDatabase.LoadAssetAtPath<T>(path);
    /// <summary>
    /// Attempts to load and apply the specified style sheet.
    /// </summary>
    private static void TryAddStyle(VisualElement root, string ussPath)
    {
        var sheet = Load<StyleSheet>(ussPath);
        if (sheet != null) root.styleSheets.Add(sheet);
    }
}
#endif
