using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;
/// <summary>
/// Specifies how components are located on the source object.
/// </summary>

public enum ComponentRetrievalStrategy
{
    First,
    InObjectAndChildren,
}
/// <summary>
/// Extracts values from GameObject-based contexts.
/// </summary>

[JungleClassInfo("Value From Object Context", "Extracts values from GameObject-based contexts.", null, "Values/Context")]
public abstract class ValueFromObjectContext<T> : ValueFromContext<T>
{
    /// <summary>
    /// Gets how components are located from the context object.
    /// </summary>
    public abstract ComponentRetrievalStrategy Strategy { get; }

    private object cachedContext;
    private T cachedComponent;
    private List<T> cachedComponents;
    private bool hasValidCache;
    /// <summary>
    /// Retrieves the value from context.
    /// </summary>

    public override T GetValueFromContext(object context)
    {
        Debug.Assert(context != null, "Context object cannot be null");

        UnityEngine.Object unityObject = context as UnityEngine.Object;
        Debug.Assert(unityObject != null, "Context must be a Unity Object");

        // Check if we can use cached component
        if (hasValidCache && ReferenceEquals(cachedContext, context))
        {
            return cachedComponent;
        }

        GameObject targetGameObject = GetGameObjectFromContext(unityObject);

        if (targetGameObject == null)
        {
            Debug.LogWarning($"Could not extract GameObject from context of type {context.GetType()}");
            cachedContext = null;
            cachedComponent = default(T);
            cachedComponents?.Clear();
            hasValidCache = false;
            return default(T);
        }

        T component = GetValueBasedOnStrategy(targetGameObject);

        // Cache the results
        cachedContext = context;
        cachedComponent = component;
        hasValidCache = true;

        return component;
    }

    private GameObject GetGameObjectFromContext(UnityEngine.Object unityObject)
    {
        if (unityObject is GameObject gameObject)
        {
            return gameObject;
        }

        if (unityObject is Component component)
        {
            return component.gameObject;
        }

        // For other UnityEngine.Object types, we cannot extract a GameObject
        return null;
    }

    private T GetValueBasedOnStrategy(GameObject gameObject)
    {
        switch (Strategy)
        {
            case ComponentRetrievalStrategy.First:
                return GetComponentValue(gameObject);

            case ComponentRetrievalStrategy.InObjectAndChildren:
                return GetComponentInChildrenValue(gameObject);

            default:
                Debug.LogWarning($"Unknown strategy: {Strategy}");
                return default(T);
        }
    }

    private T GetComponentValue(GameObject gameObject)
    {
        if (typeof(T) == typeof(GameObject))
        {
            return (T)(object)gameObject;
        }

        if (typeof(Component).IsAssignableFrom(typeof(T)))
        {
            Component component = gameObject.GetComponent(typeof(T));
            return component != null ? (T)(object)component : default(T);
        }

        // Try to get a component that might provide the value
        return default(T);
    }

    private T GetComponentInChildrenValue(GameObject gameObject)
    {
        if (typeof(T) == typeof(GameObject))
        {
            cachedComponents?.Clear();
            return (T)(object)gameObject;
        }

        if (typeof(Component).IsAssignableFrom(typeof(T)))
        {
            // Get all components in children to support multiple values
            var components = gameObject.GetComponentsInChildren(typeof(T));

            if (components == null || components.Length == 0)
            {
                cachedComponents?.Clear();
                return default(T);
            }

            // Cache all components for Values property - reuse list to reduce allocations
            if (cachedComponents == null)
            {
                cachedComponents = new System.Collections.Generic.List<T>(components.Length);
            }
            else
            {
                cachedComponents.Clear();
                if (cachedComponents.Capacity < components.Length)
                {
                    cachedComponents.Capacity = components.Length;
                }
            }

            for (int i = 0; i < components.Length; i++)
            {
                cachedComponents.Add((T)(object)components[i]);
            }

            // Return the first component
            return cachedComponents[0];
        }

        cachedComponents?.Clear();
        return default(T);
    }
    /// <summary>
    /// Enumerates all available values from the provider.
    /// </summary>


    public IEnumerable<T> Values
    {
        get
        {
            if (cachedComponents is { Count: > 0 })
            {
                foreach (var component in cachedComponents)
                {
                    yield return component;
                }
            }
            else if (cachedComponent != null)
            {
                yield return cachedComponent;
            }
        }
    }
    /// <summary>
    /// Indicates whether multiple values are available.
    /// </summary>

    public override bool HasMultipleValues =>
        Strategy == ComponentRetrievalStrategy.InObjectAndChildren && cachedComponents.Count > 1;
}