using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Specifies where component lookups should search when resolving values from a context object.
/// </summary>
public enum ComponentRetrievalStrategy
{
    First,
    InObjectAndChildren,
}
/// <summary>
/// Fetches a value by using a UnityEngine.Object reference stored in the context.
/// </summary>
public abstract class ValueFromObjectContext<T> : ValueFromContext<T>
{
    public abstract ComponentRetrievalStrategy Strategy { get; }

    private object cachedContext;
    private T cachedComponent;
    private List<T> cachedComponents;
    private bool hasValidCache;
    /// <summary>
    /// Resolves a value for the provided context object, caching results when the context repeats.
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
    /// <summary>
    /// Extracts a <see cref="GameObject"/> from the supplied Unity object.
    /// </summary>
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
    /// <summary>
    /// Dispatches component retrieval based on the configured strategy.
    /// </summary>
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
    /// <summary>
    /// Returns a component located directly on the GameObject (or the GameObject itself when requested).
    /// </summary>
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
    /// <summary>
    /// Gathers components from the GameObject's hierarchy and caches them for multi-value access.
    /// </summary>
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

    public override bool HasMultipleValues =>
        Strategy == ComponentRetrievalStrategy.InObjectAndChildren && cachedComponents.Count > 1;
}