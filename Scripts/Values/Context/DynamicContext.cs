using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Stores named objects at runtime so actions can resolve context-specific references.
/// </summary>
public static class DynamicContext
{
    private static readonly Dictionary<Type, Context> Contexts = new Dictionary<Type, Context>();
    /// <summary>
    /// Represents a single context slot that resolves objects by key or cached reference.
    /// </summary>
    public class Context
    {
        public readonly Stack<IContextProvider> contextProviders = new Stack<IContextProvider>();
        /// <summary>
        /// Returns the provider at the top of the stack or null when no providers are registered.
        /// </summary>
        public IContextProvider GetCurrentContextProvider()
        {
            return contextProviders.Count > 0 ? contextProviders.Peek() : null;
        }
    }
    /// <summary>
    /// Pushes a provider onto the stack so subsequent lookups resolve against it.
    /// </summary>
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
    /// <summary>
    /// Removes the most recently pushed provider for the given type, validating stack order.
    /// </summary>
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
    /// <summary>
    /// Retrieves the active provider instance for the specified type, if one is registered.
    /// </summary>
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
    /// <summary>
    /// Removes every registered provider stack, resetting the dynamic context system.
    /// </summary>
    public static void ClearAllContexts()
    {
        Contexts.Clear();
    }
    /// <summary>
    /// Clears the provider stack associated with the specified type.
    /// </summary>
    public static void ClearContext(Type providerType)
    {
        Debug.Assert(providerType != null, "Provider type cannot be null");

        Contexts.Remove(providerType);
    }
}