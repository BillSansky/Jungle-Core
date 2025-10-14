namespace Jungle.Actions
{
    /// <summary>
    /// Represents an action that can be executed immediately without requiring a duration or delay.
    /// </summary>
    public interface IImmediateAction
    {
        void Execute();
    }
}