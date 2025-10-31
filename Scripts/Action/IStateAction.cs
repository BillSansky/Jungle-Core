namespace Jungle.Actions
{
    /// <summary>
    /// Exposes lifecycle hooks for actions that should react to state entry and exit.
    /// </summary>
    public interface IStateAction
    {
        /// <summary>
        /// Signals that the owning state just activated so the action can start its behaviour.
        /// </summary>
        void OnStateEnter();
        /// <summary>
        /// Signals that the state is leaving so the action can clean up or stop.
        /// </summary>
        void OnStateExit();
    }
}