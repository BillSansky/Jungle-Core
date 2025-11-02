using System;
using Jungle.Actions;
using Jungle.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Actions
{
    [JungleClassInfo("Unity Event Action", "Invokes UnityEvents when actions start, stop, or on one-shot.", "d_UnityEvent Icon", "Actions/State")]
    [Serializable]
    public class UnityEventAction : IStateAction
    {
        [SerializeField] private UnityEvent onStart = new();
        [SerializeField] private UnityEvent onStop = new();
        [SerializeField] private UnityEvent onOneShot = new();
        
        public void OnStateEnter()
        {
            onStart?.Invoke();
        }

        public void OnStateExit()
        {
            onStop?.Invoke();
        }

        
    }
}
