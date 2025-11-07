#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Discovers <see cref="JunglePluginManifest"/> assets and aggregates the declared package
/// dependencies into a unified registry. Package metadata is resolved from the Jungle package
/// catalog so manifests only need to provide package names.
/// </summary>
[InitializeOnLoad]
public static class JunglePluginDependencyRegistry
{
    public sealed class ResolvedDependency
    {
        public ResolvedDependency(string packageName, string displayName, string installIdentifier, string catalogDescription, IReadOnlyList<string> sources, bool hasCatalogEntry)
        {
            PackageName = packageName;
            DisplayName = displayName;
            InstallIdentifier = installIdentifier;
            CatalogDescription = catalogDescription;
            Sources = sources;
            HasCatalogEntry = hasCatalogEntry;
        }

        public string PackageName { get; }
        public string DisplayName { get; }
        public string InstallIdentifier { get; }
        public string CatalogDescription { get; }
        public IReadOnlyList<string> Sources { get; }
        public bool HasCatalogEntry { get; }
    }

    private static readonly List<ResolvedDependency> cachedDependencies = new();
    private static readonly Dictionary<string, JunglePluginManifest> manifestCache = new(StringComparer.OrdinalIgnoreCase);
    private static string cachedSignature;

    static JunglePluginDependencyRegistry()
    {
        Refresh();
        EditorApplication.projectChanged += Refresh;
    }

    /// <summary>
    /// Raised when the registry contents change.
    /// </summary>
    public static event Action RegistryChanged;

    /// <summary>
    /// Current list of resolved dependencies.
    /// </summary>
    public static IReadOnlyList<ResolvedDependency> Dependencies => cachedDependencies;

    /// <summary>
    /// Signature representing the current dependency graph. Useful for caches.
    /// </summary>
    public static string RegistrySignature => cachedSignature ?? string.Empty;

    public static void Refresh()
    {
        var manifestGuids = AssetDatabase.FindAssets("t:JunglePluginManifest");
        var dependencySources = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        var discoveredPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var guid in manifestGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            discoveredPaths.Add(path);

            if (!manifestCache.TryGetValue(path, out var manifest) || manifest == null)
            {
                manifest = AssetDatabase.LoadAssetAtPath<JunglePluginManifest>(path);
                if (manifest == null)
                {
                    manifestCache.Remove(path);
                    continue;
                }

                manifestCache[path] = manifest;
            }

            var names = manifest.PackageNames;
            if (names == null)
            {
                continue;
            }

            foreach (var rawName in names)
            {
                var packageName = rawName?.Trim();
                if (string.IsNullOrEmpty(packageName))
                {
                    Debug.LogWarning($"A JunglePluginManifest at '{path}' declares an empty package entry. It will be ignored.");
                    continue;
                }

                if (!dependencySources.TryGetValue(packageName, out var sources))
                {
                    sources = new List<string>();
                    dependencySources.Add(packageName, sources);
                }

                if (!sources.Any(existing => string.Equals(existing, path, StringComparison.OrdinalIgnoreCase)))
                {
                    sources.Add(path);
                }
            }
        }

        var removedPaths = manifestCache.Keys.Where(path => !discoveredPaths.Contains(path)).ToList();
        foreach (var path in removedPaths)
        {
            manifestCache.Remove(path);
        }

        PackageCatalog.EnsureCache();

        var resolved = new List<ResolvedDependency>();
        foreach (var pair in dependencySources)
        {
            var packageName = pair.Key;
            var sources = pair.Value;

            var hasCatalogEntry = PackageCatalog.TryGet(packageName, out var catalogEntry);
            var installIdentifier = hasCatalogEntry && !string.IsNullOrEmpty(catalogEntry.gitUrl)
                ? catalogEntry.gitUrl
                : packageName;
            var displayName = hasCatalogEntry && !string.IsNullOrEmpty(catalogEntry.displayName)
                ? catalogEntry.displayName
                : packageName;
            var catalogDescription = hasCatalogEntry ? catalogEntry.description : string.Empty;

            if (!hasCatalogEntry)
            {
                Debug.LogWarning($"Jungle plugin dependency '{packageName}' is not defined in upm-sources.json. Declared in: {string.Join(", ", sources)}");
            }

            resolved.Add(new ResolvedDependency(packageName, displayName, installIdentifier, catalogDescription, sources.ToArray(), hasCatalogEntry));
        }

        resolved = resolved
            .OrderBy(dep => dep.PackageName, StringComparer.Ordinal)
            .ToList();

        var signatureBuilder = new StringBuilder();
        foreach (var dependency in resolved)
        {
            signatureBuilder.Append(dependency.PackageName);
            signatureBuilder.Append('|');
            signatureBuilder.Append(dependency.InstallIdentifier);
            signatureBuilder.Append('|');
            signatureBuilder.Append(dependency.DisplayName);
            signatureBuilder.Append(';');
        }
        var signature = signatureBuilder.ToString();

        if (signature == cachedSignature)
        {
            return;
        }

        cachedDependencies.Clear();
        cachedDependencies.AddRange(resolved);
        cachedSignature = signature;
        RegistryChanged?.Invoke();
    }

    private static class PackageCatalog
    {
        [Serializable]
        private class PackageEntry
        {
            public string displayName;
            public string packageName;
            public string gitUrl;
            public string description;
            public string imageUrl;
        }

        [Serializable]
        private class PackageConfig
        {
            public List<PackageEntry> packages = new();
        }

        private const string DefaultConfigPath = "Assets/UPM Package Hub/upm-sources.json";
        private const string DefaultConfigAssetSearch = "upm-sources t:TextAsset";

        private static readonly Dictionary<string, PackageEntry> cache = new(StringComparer.OrdinalIgnoreCase);
        private static string cachedPath;
        private static DateTime cachedTimestamp;

        public static void EnsureCache()
        {
            var path = ResolveConfigPath();
            var timestamp = GetTimestamp(path);

            if (cache.Count > 0 && string.Equals(path, cachedPath, StringComparison.Ordinal) && timestamp == cachedTimestamp)
            {
                return;
            }

            cache.Clear();
            cachedPath = path;
            cachedTimestamp = timestamp;

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return;
            }

            try
            {
                var json = File.ReadAllText(path);
                var config = JsonUtility.FromJson<PackageConfig>(json);
                if (config?.packages == null)
                {
                    return;
                }

                foreach (var entry in config.packages)
                {
                    if (entry == null)
                    {
                        continue;
                    }

                    var name = entry.packageName?.Trim();
                    if (string.IsNullOrEmpty(name))
                    {
                        continue;
                    }

                    cache[name] = entry;
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"Failed to parse Jungle package catalog at '{path}': {exception.Message}");
            }
        }

        public static bool TryGet(string packageName, out PackageEntry entry)
        {
            if (string.IsNullOrEmpty(packageName))
            {
                entry = null;
                return false;
            }

            EnsureCache();
            return cache.TryGetValue(packageName, out entry);
        }

        private static string ResolveConfigPath()
        {
            if (File.Exists(DefaultConfigPath))
            {
                return DefaultConfigPath;
            }

            var guids = AssetDatabase.FindAssets(DefaultConfigAssetSearch);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.EndsWith("upm-sources.json", StringComparison.OrdinalIgnoreCase))
                {
                    return path;
                }
            }

            return null;
        }

        private static DateTime GetTimestamp(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return DateTime.MinValue;
            }

            try
            {
                return File.GetLastWriteTimeUtc(path);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Failed to query Jungle package catalog timestamp at '{path}': {exception.Message}");
                return DateTime.MinValue;
            }
        }
    }
}
#endif
