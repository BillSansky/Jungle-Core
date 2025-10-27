using System;
using Jungle.Values;

public interface IContextProvider
{
    int IDCount { get; }

    object GetContextObject(int id);

    Type GetContextType(int id);

    void RegisterContext();
}