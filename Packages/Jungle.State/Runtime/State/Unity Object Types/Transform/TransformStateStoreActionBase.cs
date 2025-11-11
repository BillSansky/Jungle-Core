using System;
using Jungle.Attributes;
using Jungle.Utility;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Base class for transform state store actions with shared configuration.
    /// </summary>
    [Serializable]
    public abstract class TransformStateStoreActionBase<TState> : IImmediateAction
        where TState : ISaveState, new()
    {
        [SerializeReference]
        [JungleClassSelection]
        protected ITransformValue targetTransforms = new TransformLocalValue();

        public void StartProcess(Action callback = null)
        {
            foreach (var transform in targetTransforms.Values)
            {
                ObjectStateRecorder.RecordOrReplace<TState>(transform);
            }

            callback?.Invoke();
        }
    }
}
