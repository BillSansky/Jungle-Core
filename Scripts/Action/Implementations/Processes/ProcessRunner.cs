using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    public class ProcessRunner : MonoBehaviour
    {
        [JungleClassSelection(typeof(IProcessAction))]
        [SerializeReference]
        public IProcessAction Process;

        private void OnEnable()
        {
            if (Process != null)
            {
                Process.Start();
            }
        }

        private void OnDisable()
        {
            if (Process != null)
            {
                Process.Interrupt();
            }
        }
    }
}
