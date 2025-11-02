namespace Jungle.Actions
{
    /// <summary>
    /// Represents an action that can be executed immediately without requiring a duration or delay.
    /// </summary>
    public interface IImmediateAction
    {
        /// <summary>
        /// Performs the action once using the current configuration without waiting for a later update.
        /// </summary>
        void Execute();
    }
}