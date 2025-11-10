using System;

namespace Jungle.Actions
{
    /// <summary>
    /// Represents an action that can be executed immediately without requiring a duration or delay.
    /// </summary>
    public interface IImmediateAction : IProcessAction
    {
        void Execute() => Start();

       

        void IProcessAction.Stop()
        {
            //do nothing
        }

        bool IProcessAction.IsInstant => true;

        bool IProcessAction.HasDefinedDuration =>true;

        float IProcessAction.Duration =>0;

        bool IProcessAction.IsInProgress => false;

    }
}