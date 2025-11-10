using System;
using Jungle.Values;
/// <summary>
/// Defines the context provider interface.
/// </summary>

public interface IContextProvider
{
    int IDCount { get; }

    Object GetContextObject(int id);

    Type GetContextType(int id);

    void RegisterContext();
}