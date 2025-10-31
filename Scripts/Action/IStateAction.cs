namespace Jungle.Actions
{
    /// <summary>
    /// Defines the IStateAction contract.
    /// </summary>
    public interface IStateAction
    {
        /// <summary>
        /// Handles the OnStateEnter event.
        /// </summary>
        void OnStateEnter();
        /// <summary>
        /// Handles the OnStateExit event.
        /// </summary>
        void OnStateExit();
    }
}