using System;
/// <summary>
/// Defines the IContextKey contract.
/// </summary>
public interface IContextKey
{
    Type ContextType { get; }
    int ContextKey { get; }
    /// <summary>
    /// Returns a human-readable name for the context entry with the given identifier.
    /// </summary>
    string GetContextName(int id);
    /// <summary>
    /// Indicates whether the context key resolves to more than one value.
    /// </summary>
    bool HasMultipleValues(int id);

    object GetContextObject()
    {
        return DynamicContext.GetContextProvider(ContextType).GetContextObject(ContextKey);
    }
}