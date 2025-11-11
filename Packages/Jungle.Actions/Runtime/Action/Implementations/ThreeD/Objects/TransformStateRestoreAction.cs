using System;
using Jungle.Attributes;
using Jungle.Utility;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Restores previously stored transform state.
    /// </summary>
    [Serializable]
    [JungleClassInfo(
        "Transform State Restore Action",
        "Restores stored transform information using the shared state recorder.",
        "d_Rigidbody Icon",
        "Actions/State")]
    public class TransformStateRestoreAction : IImmediateAction
    {
        private enum TransformStateType
        {
            Full,
            Position,
            Rotation,
            Scale
        }

        [SerializeReference]
        [JungleClassSelection]
        private ITransformValue targetTransforms = new TransformLocalValue();

        [SerializeField]
        private TransformStateType stateType = TransformStateType.Full;

        [SerializeField]
        private bool clearAfterRestore;

        public void StartProcess(Action callback = null)
        {
            foreach (var transform in targetTransforms.Values)
            {
                RestoreState(transform);

                if (clearAfterRestore)
                {
                    ObjectStateRecorder.ClearState(transform);
                }
            }

            callback?.Invoke();
        }

        private void RestoreState(Transform transform)
        {
            switch (stateType)
            {
                case TransformStateType.Full:
                    ObjectStateRecorder.Restore<TransformFullState>(transform);
                    break;
                case TransformStateType.Position:
                    ObjectStateRecorder.Restore<TransformPositionState>(transform);
                    break;
                case TransformStateType.Rotation:
                    ObjectStateRecorder.Restore<TransformRotationState>(transform);
                    break;
                case TransformStateType.Scale:
                    ObjectStateRecorder.Restore<TransformScaleState>(transform);
                    break;
            }
        }
    }
}
