using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// ScriptableObject that acts as a signal type identifier/key.
    /// Used to identify different types of signals in the SignalReceiver component.
    /// </summary>
    [CreateAssetMenu(fileName = "Signal Type", menuName = "Jungle/Events/Signal Type", order = 100)]
    public class SignalType : ScriptableObject
    {
        [TextArea(2, 4)]
        [SerializeField] private string description;

        public string Description => description;
    }
}

