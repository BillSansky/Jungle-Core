using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using Jungle.Values.Primitives;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo(
        "Animator State Action",
        "Plays a specific animator state when the action starts and optionally restores the previous state on stop.",
        "d_AnimationClip",
        "Actions/State")]
    /// <summary>
    /// Implements the animator state state action action.
    /// </summary>
    [Serializable]
    public class AnimatorStateStateAction : IImmediateAction
    {
        [SerializeReference] private IGameObjectValue targetAnimatorObject = new GameObjectValue();
        [SerializeField] private IStringValue stateName = new StringValue("StateName");
        [SerializeField] private IIntValue layerIndex = new IntValue(0);
        [SerializeField] private bool startFromBeginning = true;
        [SerializeField] private float startNormalizedTime = 0f;


        private Animator cachedAnimator;
        private int previousStateHash;
        private float previousNormalizedTime;

        private bool isInProgress;
        private bool hasCompleted;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => true;

        public float Duration => 0f;

        public bool IsInProgress => isInProgress;

        public bool HasCompleted => hasCompleted;

        public void Start(Action callback = null)
        {
            if (isInProgress)
            {
                return;
            }

            var animator = ResolveAnimator();

            Debug.Assert(animator != null, "Animator object has not been assigned.");
            Debug.Assert(layerIndex.V >= 0, "Layer index must be greater than or equal to 0.");
            Debug.Assert(layerIndex.V < animator.layerCount, "Layer index must be less than the number of layers.");
            Debug.Assert(!string.IsNullOrEmpty(stateName.V), "State name must be provided.");

            var stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex.V);
            previousStateHash = stateInfo.fullPathHash;
            previousNormalizedTime = stateInfo.normalizedTime;

            var targetStateHash = Animator.StringToHash(stateName.V);

            if (startFromBeginning)
            {
                animator.Play(targetStateHash, layerIndex.V, Mathf.Clamp01(startNormalizedTime));
            }
            else
            {
                animator.Play(targetStateHash, layerIndex.V);
            }

            isInProgress = true;
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
            callback?.Invoke();
        }

        public void Interrupt()
        {
            if (!isInProgress)
            {
                return;
            }

            var animator = cachedAnimator ?? ResolveAnimator();
            var normalizedTime = Mathf.Repeat(previousNormalizedTime, 1f);
            animator.Play(previousStateHash, layerIndex.V, normalizedTime);

            isInProgress = false;
            hasCompleted = false;
        }

        private Animator ResolveAnimator()
        {
            var gameObject = targetAnimatorObject.G;
            cachedAnimator = gameObject.GetComponent<Animator>();

            return cachedAnimator;
        }
    }
}