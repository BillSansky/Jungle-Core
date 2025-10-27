using System;
using Jungle.Values;
using UnityEngine;


public abstract class ValueFromContext<T> : IValue<T>
{
    [SerializeReference] private IContextKey contextKey;

    public abstract T GetValueFromContext(object context);

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

    public abstract bool HasMultipleValues { get; }
}


