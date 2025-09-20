#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[InitializeOnLoad]
public static class UpmBootstrapper
{
    // Guard to prevent re-entry during assembly reloads
    private static bool _isRunning;

    // Queue of pending Add requests
    private static readonly Queue<string> _pending = new Queue<string>();
    private static AddRequest _current;
    private static string _bootstrapFolder; // Folder to delete after success
    private static string _runOnceKey;      // EditorPrefs key

    // Auto-run on load
    static UpmBootstrapper()
    {
        EditorApplication.delayCall += TryAutoRun;
    }

    // Menu for manual trigger (if needed)
    [MenuItem("Tools/Jungle/Packages/Install now", priority = 0)]
    public static void RunNowMenu()
    {
        if (SetupFromConfig(out var cfg) && PrepareRun(cfg))
        {
            Debug.Log("[Jungle Package] Manual install started.");
            StartNext();
        }
    }

    private static void TryAutoRun()
    {
        if (!SetupFromConfig(out var cfg)) return;

        // Build a project-scoped run-once key
        _runOnceKey = $"{Application.companyName}.{Application.productName}.{cfg.runOnceKey}".Replace(" ", "_");

        if (EditorPrefs.GetBool(_runOnceKey, false))
        {
            // Already ran in this project
            return;
        }

        if (!PrepareRun(cfg)) return;

        Debug.Log("[Jungle Package] Auto install started.");
        StartNext();
    }

    private static bool SetupFromConfig(out BootstrapConfig cfg)
    {
        cfg = default;

        // Find the config text asset by name
        var guids = AssetDatabase.FindAssets("upm-packages t:TextAsset");
        if (guids == null || guids.Length == 0) return false;

        // Prefer the one inside the expected folder
        string path = null;
        foreach (var guid in guids)
        {
            var p = AssetDatabase.GUIDToAssetPath(guid);
            if (p.EndsWith("upm-packages.json", StringComparison.OrdinalIgnoreCase))
            {
                path = p;
                break;
            }
        }
        if (string.IsNullOrEmpty(path)) return false;

        var json = File.ReadAllText(path);
        try
        {
            cfg = JsonUtility.FromJson<BootstrapConfig>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"[UPMBootstrapper] Failed to parse upm-packages.json: {e}");
            return false;
        }

        if (cfg == null || cfg.gitUrls == null || cfg.gitUrls.Length == 0)
        {
            Debug.LogWarning("[UPMBootstrapper] No gitUrls found in upm-packages.json.");
            return false;
        }

        _bootstrapFolder = Path.GetDirectoryName(path).Replace('\\', '/'); // Assets/UPM Bootstrapper
        return true;
    }

    private static bool PrepareRun(BootstrapConfig cfg)
    {
        if (_isRunning) return false;
        _isRunning = true;

        _pending.Clear();
        foreach (var url in cfg.gitUrls.Where(u => !string.IsNullOrWhiteSpace(u)))
            _pending.Enqueue(url.Trim());

        if (_pending.Count == 0)
        {
            _isRunning = false;
            return false;
        }

        _current = null;

        // Store options for later
        _deleteAfter = cfg.deleteBootstrapperAfterInstall;
        _cfgRunOnceKey = cfg.runOnceKey;

        // Subscribe to update loop
        EditorApplication.update -= Update;
        EditorApplication.update += Update;
        return true;
    }

    private static bool _deleteAfter;
    private static string _cfgRunOnceKey;

    private static void StartNext()
    {
        if (_current != null) return;

        if (_pending.Count == 0)
        {
            FinishAll(success: true);
            return;
        }

        var url = _pending.Dequeue();
        try
        {
            Debug.Log($"[UPMBootstrapper] Installing: {url}");
            _current = Client.Add(url);
        }
        catch (Exception e)
        {
            Debug.LogError($"[UPMBootstrapper] Failed to start install for {url}: {e}");
            // Continue with next
            _current = null;
            StartNext();
        }
    }

    private static void Update()
    {
        if (_current == null)
        {
            // Kick off next if idle
            StartNext();
            return;
        }

        if (!_current.IsCompleted) return;

        if (_current.Status == StatusCode.Success)
        {
            Debug.Log($"[UPMBootstrapper] Installed: {_current.Result?.packageId ?? "(unknown)"}");
        }
        else if (_current.Status >= StatusCode.Failure)
        {
            Debug.LogError($"[UPMBootstrapper] Error installing package: {_current.Error?.message}");
        }

        _current = null;

        // Proceed to next
        if (_pending.Count > 0) StartNext();
        else FinishAll(success: true);
    }

    private static void FinishAll(bool success)
    {
        // Clean update hook
        EditorApplication.update -= Update;

        // Mark run-once
        var key = _runOnceKey ?? $"{Application.companyName}.{Application.productName}.{_cfgRunOnceKey}".Replace(" ", "_");
        EditorPrefs.SetBool(key, true);

        if (success && _deleteAfter && !string.IsNullOrEmpty(_bootstrapFolder))
        {
            // Delete the bootstrapper folder on next tick to avoid asset database conflicts
            EditorApplication.delayCall += () =>
            {
                if (AssetDatabase.IsValidFolder(_bootstrapFolder))
                {
                    Debug.Log($"[UPMBootstrapper] Deleting bootstrapper folder: {_bootstrapFolder}");
                    AssetDatabase.DeleteAsset(_bootstrapFolder);
                    AssetDatabase.Refresh();
                }
            };
        }

        _isRunning = false;
        Debug.Log("[UPMBootstrapper] Done.");
    }

    [Serializable]
    private class BootstrapConfig
    {
        public string[] gitUrls;
        public bool deleteBootstrapperAfterInstall = true;
        public string runOnceKey = "UPMBootstrapper_v1";
    }
}
#endif
