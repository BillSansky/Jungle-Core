using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;
/// <summary>
/// Resolves a value from the active Jungle context.
/// </summary>


[JungleClassInfo("Value From Context", "Resolves a value from the active Jungle context.", null, "Values/Context")]
public abstract class ValueFromContext<T> : IValue<T>
{
    /// <summary>
    /// Retrieves the value from context.
    /// </summary>
    [SerializeReference] private IContextKey contextKey;

    public abstract T GetValueFromContext(object context);
    /// <summary>
    /// Gets the value produced by this provider.
    /// </summary>

    public T Value()
    {
        Debug.Assert(contextKey != null, "ContextKey cannot be null");

        var contextValue = contextKey.GetContextObject();

        if (contextValue == null)
        {
            Debug.LogWarning($"No context object found for key '{contextKey.GetContextName(contextKey.ContextKey)}' (ID: {contextKey.ContextKey})");
            return default(T);
        }

        return GetValueFromContext(contextValue);
    }
    /// <summary>
    /// Indicates whether multiple values are available.
    /// </summary>

    public abstract bool HasMultipleValues { get; }
}


