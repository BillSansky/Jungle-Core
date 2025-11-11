namespace Jungle.Actions
{
    /// <summary>
    /// Represents an action that can be executed immediately without requiring a duration or delay.
    /// </summary>
    public interface IImmediateAction : IProcessAction
    {
        void Execute() => StartProcess();

        void IProcessAction.Stop()
        {
        }

        bool IProcessAction.IsInstant => true;

        bool IProcessAction.HasDefinedDuration => true;

        float IProcessAction.Duration => 0f;

        bool IProcessAction.IsInProgress => false;
    }
}
