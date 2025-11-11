#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

/// <summary>
/// Resolves Jungle plugin dependencies by installing any missing packages through the Unity Package Manager APIs.
/// </summary>
[InitializeOnLoad]
public static class JunglePluginDependencyResolver
{
    private const string CacheKey = "Jungle.DependencyResolver.RegistrySignature";

    private static readonly Queue<JunglePluginDependencyRegistry.ResolvedDependency> installQueue = new();
    private static readonly Dictionary<string, PackageInfo> installedPackages = new(StringComparer.OrdinalIgnoreCase);

    private static ListRequest listRequest;
    private static AddRequest addRequest;
    private static string pendingSignature;

    static JunglePluginDependencyResolver()
    {
        JunglePluginDependencyRegistry.RegistryChanged += OnRegistryChanged;
        EditorApplication.delayCall += BeginResolution;
    }

    private static void OnRegistryChanged()
    {
        BeginResolution();
    }

    private static void BeginResolution()
    {
        if (listRequest != null || addRequest != null)
        {
            return;
        }

        pendingSignature = JunglePluginDependencyRegistry.RegistrySignature;
        if (string.IsNullOrEmpty(pendingSignature))
        {
            return;
        }

        var cached = EditorPrefs.GetString(CacheKey, string.Empty);
        if (cached == pendingSignature)
        {
            return;
        }

        listRequest = Client.List(true, true);
        EditorApplication.update += Pump;
    }

    private static void Pump()
    {
        if (listRequest != null)
        {
            if (!listRequest.IsCompleted)
            {
                return;
            }

            if (listRequest.Status == StatusCode.Success)
            {
                installedPackages.Clear();
                foreach (var info in listRequest.Result)
                {
                    if (!string.IsNullOrEmpty(info.name))
                    {
                        installedPackages[info.name] = info;
                    }
                }

                PrepareQueue();
            }
            else if (listRequest.Status >= StatusCode.Failure)
            {
                Debug.LogError($"Failed to query installed Jungle packages: {listRequest.Error?.message}");
            }

            listRequest = null;
        }

        if (addRequest != null)
        {
            if (!addRequest.IsCompleted)
            {
                return;
            }

            if (addRequest.Status == StatusCode.Success)
            {
                Debug.Log($"Installed Jungle dependency: {addRequest.Result.name} {addRequest.Result.version}");
            }
            else if (addRequest.Status >= StatusCode.Failure)
            {
                Debug.LogError($"Failed to install Jungle dependency: {addRequest.Error?.message}");
            }

            addRequest = null;
        }

        if (addRequest == null && installQueue.Count > 0)
        {
            var dependency = installQueue.Dequeue();
            var identifier = BuildInstallIdentifier(dependency);
            if (string.IsNullOrEmpty(identifier))
            {
                Debug.LogError($"Unable to install Jungle dependency '{dependency.PackageName}' because no install source was resolved.");
            }
            else
            {
                Debug.Log($"Installing Jungle dependency '{dependency.PackageName}' from '{identifier}'");
                addRequest = Client.Add(identifier);
                return;
            }
        }

        if (addRequest == null)
        {
            EditorApplication.update -= Pump;
            if (installQueue.Count == 0 && !string.IsNullOrEmpty(pendingSignature))
            {
                EditorPrefs.SetString(CacheKey, pendingSignature);
            }
        }
    }

    private static void PrepareQueue()
    {
        installQueue.Clear();

        var dependencies = JunglePluginDependencyRegistry.Dependencies;
        foreach (var dependency in dependencies)
        {
            if (!installedPackages.TryGetValue(dependency.PackageName, out var installedInfo))
            {
                installQueue.Enqueue(dependency);
                continue;
            }

            if (string.IsNullOrEmpty(dependency.VersionRequirement))
            {
                continue;
            }

            var installedVersion = installedInfo?.version;
            if (JunglePackageVersionUtility.IsAtLeast(installedVersion, dependency.VersionRequirement))
            {
                continue;
            }

            Debug.Log($"Queued Jungle dependency '{dependency.PackageName}' for update. Installed version: {installedVersion ?? "<unknown>"}, required: {dependency.VersionRequirement}.");
            installQueue.Enqueue(dependency);
        }

        if (installQueue.Count == 0)
        {
            EditorPrefs.SetString(CacheKey, pendingSignature ?? string.Empty);
        }
    }

    private static string BuildInstallIdentifier(JunglePluginDependencyRegistry.ResolvedDependency dependency)
    {
        var identifier = dependency.InstallIdentifier;

        if (!string.IsNullOrEmpty(dependency.VersionRequirement) && !string.IsNullOrEmpty(dependency.PackageName))
        {
            if (string.IsNullOrEmpty(identifier) || string.Equals(identifier, dependency.PackageName, StringComparison.OrdinalIgnoreCase))
            {
                return $"{dependency.PackageName}@{dependency.VersionRequirement}";
            }
        }

        return identifier;
    }
}
#endif
