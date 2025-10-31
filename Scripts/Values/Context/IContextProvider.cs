using System;
using Jungle.Values;
/// <summary>
/// Defines the IContextProvider contract.
/// </summary>
public interface IContextProvider
{
    int IDCount { get; }
    /// <summary>
    /// Retrieves a context object by its identifier.
    /// </summary>
    Object GetContextObject(int id);
    /// <summary>
    /// Returns the type of object stored for the given context identifier.
    /// </summary>
    Type GetContextType(int id);
    /// <summary>
    /// Publishes the provider so the dynamic context can discover it.
    /// </summary>
    void RegisterContext();
}