using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;


[JungleClassInfo("Value From Context", "Resolves a value from the active Jungle context.", null, "Values/Context")]
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


