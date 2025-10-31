using System;
using Jungle.Values;
using UnityEngine;

/// <summary>
/// Base class for values that pull their data from a context provider at runtime.
/// </summary>
public abstract class ValueFromContext<T> : IValue<T>
{
    [SerializeReference] private IContextKey contextKey;

    /// <summary>
    /// Resolves a value from the supplied context object.
    /// </summary>
    /// <param name="context">Context object used to resolve the value.</param>
    public abstract T GetValueFromContext(object context);

    /// <summary>
    /// Uses the configured context key to locate the source object and delegates value creation to subclasses.
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
    /// Indicates whether multiple values can be provided by this context source.
    /// </summary>
    public abstract bool HasMultipleValues { get; }
}
