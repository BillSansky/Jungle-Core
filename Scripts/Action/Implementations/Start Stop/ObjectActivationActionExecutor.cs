using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Actions
{
    public class OctoputsActionExecutor : MonoBehaviour
    {
     

        [SerializeReference] private List<ProcessAction> actions;

        public void OnEnable()
        {
            foreach (var action in actions)
            {
                action.Start();
            }
        }

        public void OnDisable()
        {
            foreach (var action in actions)
            {
               action.Stop();
            }
        }
    }
}
