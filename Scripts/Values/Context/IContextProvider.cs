using System;
using Jungle.Values;

public interface IContextProvider
{
    int IDCount { get; }

    Object GetContextObject(int id);

    Type GetContextType(int id);

    void RegisterContext();
}