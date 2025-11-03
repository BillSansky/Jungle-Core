using System;
/// <summary>
/// Defines the context key interface.
/// </summary>

public interface IContextKey
{
    Type ContextType { get; }
    int ContextKey { get; }
    string GetContextName(int id);

    bool HasMultipleValues(int id);

    object GetContextObject()
    {
        return DynamicContext.GetContextProvider(ContextType).GetContextObject(ContextKey);
    }
}