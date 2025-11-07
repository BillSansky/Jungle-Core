#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject asset that declares Jungle package dependencies required by a plugin.
/// </summary>
public class JunglePluginManifest : ScriptableObject
{
    [SerializeField]
    private List<string> packageNames = new();

    /// <summary>
    /// Names of the Jungle packages required by this plugin.
    /// </summary>
    public IReadOnlyList<string> PackageNames => packageNames;
}
#endif
