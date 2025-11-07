namespace Jungle.Actions
{
    /// <summary>
    /// Represents an action that can be executed immediately without requiring a duration or delay.
    /// </summary>
    public interface IImmediateAction : IProcessAction
    {
        void IProcessAction.Interrupt()
        {
            //do nothing
            HasCompleted = false;
        }

        bool IProcessAction.HasDefinedDuration => false;

        float IProcessAction.Duration => -1;

        bool IProcessAction.IsInProgress => false;
        
    }
}