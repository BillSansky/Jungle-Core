using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

/// <summary>
/// Abstract base class for singleton ScriptableObjects.
/// Automatically creates or loads the instance from Resources folder.
/// </summary>
/// <typeparam name="T">The type of the singleton ScriptableObject</typeparam>
public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T instance;

    /// <summary>
    /// Gets the singleton instance. Creates one if it doesn't exist.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // Try to load from Resources folder
                T[] instances = Resources.LoadAll<T>("");

                if (instances != null && instances.Length > 0)
                {
                    instance = instances[0];

                    if (instances.Length > 1)
                    {
                        Debug.LogWarning($"Multiple instances of {typeof(T).Name} found in Resources. Using the first one.");
                    }
                }
                else
                {
                    // No instance found, create a new one
#if UNITY_EDITOR
                    instance = CreateInstance();
#else
                    Debug.LogError($"No instance of {typeof(T).Name} found in Resources folder. Cannot create at runtime.");
#endif
                }
            }

            return instance;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Creates and saves a new instance of the ScriptableObject in the Resources folder.
    /// </summary>
    private static T CreateInstance()
    {
        T instance = ScriptableObject.CreateInstance<T>();

        // Ensure Resources folder exists
        string resourcesPath = "Assets/Resources";
        if (!Directory.Exists(resourcesPath))
        {
            Directory.CreateDirectory(resourcesPath);
        }

        // Create asset
        string assetPath = $"{resourcesPath}/{typeof(T).Name}.asset";
        AssetDatabase.CreateAsset(instance, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Created new {typeof(T).Name} instance at {assetPath}");

        return instance;
    }
#endif

    /// <summary>
    /// Called when the instance is loaded or created.
    /// Override this to perform initialization.
    /// </summary>
    protected virtual void OnEnable()
    {
        if (instance == null)
        {
            instance = this as T;
        }
    }
}
