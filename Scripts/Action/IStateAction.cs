namespace Jungle.Actions
{
    /// <summary>
    /// Represents an action that reacts to state transitions, typically used by state machines.
    /// </summary>
    public interface IStateAction
    {
        /// <summary>
        /// Called when the owning state becomes active, allowing the action to begin its behavior.
        /// </summary>
        void OnStateEnter();

        /// <summary>
        /// Called when the owning state stops being active, giving the action a chance to clean up.
        /// </summary>
        void OnStateExit();
    }
}