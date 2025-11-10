using System;
using Jungle.Values;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Resolves a component reference from a context capable of supplying a <see cref="GameObject"/>.
/// </summary>
/// <typeparam name="TComponent">Type of the component to locate.</typeparam>
[Serializable]
public abstract class ComponentValueFromGameObjectContext<TComponent> : ValueFromContext<TComponent>, IComponent<TComponent>, IGameObjectContext
    where TComponent : Component
{
    /// <summary>
    /// Specifies how the target component is searched on the resolved <see cref="GameObject"/>.
    /// </summary>
    public enum ComponentSearchMode
    {
        OnObject,
        InChildren,
        InParent
    }

    [SerializeField]
    [FormerlySerializedAs("strategy")]
    private ComponentSearchMode searchMode = ComponentSearchMode.OnObject;

    /// <summary>
    /// Gets the configured component search mode.
    /// </summary>
    protected virtual ComponentSearchMode SearchMode => searchMode;

    /// <inheritdoc />
    public override bool HasMultipleValues => SearchMode == ComponentSearchMode.InChildren;

    /// <inheritdoc />
    public override TComponent GetValueFromContext(object context)
    {
        Debug.Assert(context != null, "Context object cannot be null");

        GameObject targetGameObject = ResolveGameObject(context);

        if (targetGameObject == null)
        {
            string contextDescription = context != null ? context.GetType().FullName : "null";
            Debug.LogWarning($"Could not locate GameObject from context of type {contextDescription}");
            return null;
        }

        return ResolveComponent(targetGameObject);
    }

    private TComponent ResolveComponent(GameObject gameObject)
    {
        switch (SearchMode)
        {
            case ComponentSearchMode.InChildren:
                return gameObject.GetComponentInChildren<TComponent>();
            case ComponentSearchMode.InParent:
                return gameObject.GetComponentInParent<TComponent>();
            default:
                return gameObject.GetComponent<TComponent>();
        }
    }

    /// <summary>
    /// Resolves the <see cref="GameObject"/> associated with the provided context value.
    /// </summary>
    /// <param name="context">Context value supplied by the Jungle context system.</param>
    /// <returns>The resolved <see cref="GameObject"/>, or <c>null</c> when unavailable.</returns>
    protected virtual GameObject ResolveGameObject(object context)
    {
        if (context is GameObject directGameObject)
        {
            return directGameObject;
        }

        if (context is Component componentContext)
        {
            return componentContext.gameObject;
        }

        if (context is IGameObjectContext gameObjectContext)
        {
            return gameObjectContext.GetGameObject();
        }

        if (context is IGameObjectReference gameObjectReference)
        {
            return gameObjectReference.GameObject;
        }

        if (context is UnityEngine.Object unityObject)
        {
            return unityObject as GameObject ?? (unityObject is Component component ? component.gameObject : null);
        }

        return null;
    }

    /// <inheritdoc />
    public GameObject GetGameObject()
    {
        TComponent component = Value();

        return component != null ? component.gameObject : null;
    }
}
