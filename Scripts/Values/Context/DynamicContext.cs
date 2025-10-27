using System;
using System.Collections.Generic;
using UnityEngine;

public static class DynamicContext
{
    private static readonly Dictionary<Type, Context> Contexts = new Dictionary<Type, Context>();

    public class Context
    {
        public readonly Stack<IContextProvider> contextProviders = new Stack<IContextProvider>();

        public IContextProvider GetCurrentContextProvider()
        {
            return contextProviders.Count > 0 ? contextProviders.Peek() : null;
        }
    }

    public static void PushContext(IContextProvider contextProvider)
    {
        Debug.Assert(contextProvider != null, "Context provider cannot be null");

        Type key = contextProvider.GetType();

        if (!Contexts.TryGetValue(key, out var context))
        {
            context = new Context();
            Contexts[key] = context;
        }

        context.contextProviders.Push(contextProvider);
    }

    public static void PopContext(IContextProvider contextProvider)
    {
        Debug.Assert(contextProvider != null, "Context provider cannot be null");

        Type key = contextProvider.GetType();

        if (!Contexts.TryGetValue(key, out var context))
        {
            Debug.Assert(false, $"Attempted to pop context for type '{key.Name}' but no context exists");
            return;
        }

        Debug.Assert(context.contextProviders.Count > 0, 
            $"Attempted to pop context for type '{key.Name}' but the context stack is empty");

        var currentContextProvider = context.contextProviders.Peek();
        Debug.Assert(ReferenceEquals(currentContextProvider, contextProvider), 
            $"Attempted to pop context provider that is not on top of the stack for type '{key.Name}'. " +
            $"Expected {currentContextProvider?.GetType().Name}, but got {contextProvider.GetType().Name}");

        context.contextProviders.Pop();
    }

    public static IContextProvider GetContextProvider(Type providerType)
    {
        Debug.Assert(providerType != null, "Provider type cannot be null");

        if (Contexts.TryGetValue(providerType, out var context))
        {
            return context.GetCurrentContextProvider();
        }

        return null;
    }

    public static T GetContextProvider<T>() where T : class, IContextProvider
    {
        return GetContextProvider(typeof(T)) as T;
    }

    public static void ClearAllContexts()
    {
        Contexts.Clear();
    }

    public static void ClearContext(Type providerType)
    {
        Debug.Assert(providerType != null, "Provider type cannot be null");

        Contexts.Remove(providerType);
    }
}