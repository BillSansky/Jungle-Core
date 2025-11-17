using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// Monitors a GameObject's active state and notifies listeners when the configured state is reached.
    /// The callback works by injecting an invisible relay component into the monitored GameObject.
    /// </summary>
    [Serializable]
    public sealed class GameObjectEventCallback : IEventMonitor
    {
        private enum ActivationState
        {
            Active,
            Inactive
        }

        [SerializeReference]
        [JungleClassSelection(typeof(IGameObjectValue))]
        private IGameObjectValue targetObject = new GameObjectValue();

        [SerializeField]
        private ActivationState targetState = ActivationState.Active;

        [SerializeReference]
        [JungleClassSelection(typeof(IMonitorCondition))]
        private IMonitorCondition monitorCondition = new NeverStopMonitorCondition();

        private readonly MonitorConditionEvaluator monitorConditionEvaluator = new();
        private Action monitoredCallback;
        private GameObject monitoredObject;
        private GameObjectActivationRelay relay;
        private Action<bool> relayHandler;
        private bool relayInjected;
        private bool isMonitoring;

        /// <inheritdoc />
        public IMonitorCondition MonitorCondition
        {
            get => monitorCondition;
            set => monitorCondition = value ?? new NeverStopMonitorCondition();
        }

        /// <inheritdoc />
        public void StartMonitoring(Action callbackAction)
        {
            var target = targetObject?.Value();
            if (target == null)
            {
                throw new InvalidOperationException("GameObjectEventCallback requires a valid GameObject reference.");
            }

            if (isMonitoring && monitoredObject == target)
            {
                monitoredCallback = monitorConditionEvaluator.CreateMonitoredCallback(callbackAction, EndMonitoring,
                    monitorCondition);
                EvaluateCurrentState();
                return;
            }

            StopMonitoringInternal();

            monitoredCallback = monitorConditionEvaluator.CreateMonitoredCallback(callbackAction, EndMonitoring,
                monitorCondition);
            monitoredObject = target;
            relay = AcquireRelay(target, out relayInjected);

            if (relayHandler == null)
            {
                relayHandler = OnRelayStateChanged;
            }

            relay.StateChanged += relayHandler;
            isMonitoring = true;
            EvaluateCurrentState();
        }

        /// <inheritdoc />
        public void EndMonitoring()
        {
            StopMonitoringInternal();
        }

        private void StopMonitoringInternal()
        {
            if (!isMonitoring)
            {
                return;
            }

            if (relay != null && relayHandler != null)
            {
                relay.StateChanged -= relayHandler;
            }

            if (relay != null && relayInjected)
            {
                DestroyRelay(relay);
            }

            relay = null;
            monitoredObject = null;
            relayInjected = false;
            isMonitoring = false;
            monitorConditionEvaluator.Reset();
            monitoredCallback = null;
        }

        private void EvaluateCurrentState()
        {
            if (!isMonitoring || monitoredObject == null)
            {
                return;
            }

            var isActive = monitoredObject.activeInHierarchy;
            if ((targetState == ActivationState.Active && isActive) ||
                (targetState == ActivationState.Inactive && !isActive))
            {
                NotifyCallback();
            }
        }

        private void OnRelayStateChanged(bool isActive)
        {
            if (!isMonitoring)
            {
                return;
            }

            if ((targetState == ActivationState.Active && isActive) ||
                (targetState == ActivationState.Inactive && !isActive))
            {
                NotifyCallback();
            }
        }

        private void NotifyCallback()
        {
            monitoredCallback?.Invoke();
        }

        private static GameObjectActivationRelay AcquireRelay(GameObject target, out bool created)
        {
            var relay = target.GetComponent<GameObjectActivationRelay>();
            if (relay == null)
            {
                relay = target.AddComponent<GameObjectActivationRelay>();
                relay.hideFlags = HideFlags.HideInInspector;
                created = true;
            }
            else
            {
                created = false;
            }

            return relay;
        }

        private static void DestroyRelay(GameObjectActivationRelay relay)
        {
            if (Application.isPlaying)
            {
                UnityEngine.Object.Destroy(relay);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(relay);
            }
        }
    }

    [AddComponentMenu("")]
    internal sealed class GameObjectActivationRelay : MonoBehaviour
    {
        public event Action<bool> StateChanged;

        private void OnEnable()
        {
            StateChanged?.Invoke(true);
        }

        private void OnDisable()
        {
            StateChanged?.Invoke(false);
        }
    }
}
