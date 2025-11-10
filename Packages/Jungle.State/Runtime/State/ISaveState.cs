namespace Jungle.Utility
{
    /// <summary>
    /// Interface for all save states. Implement this to create custom state types.
    /// </summary>
    public interface ISaveState
    {
        bool Capture(UnityEngine.Object obj);
        bool Restore(UnityEngine.Object obj);
    }
}