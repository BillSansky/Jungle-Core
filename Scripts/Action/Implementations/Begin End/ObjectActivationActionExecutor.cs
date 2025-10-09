using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Actions
{
    public class ObjectActivationActionExecutor : MonoBehaviour
    {
     

        [SerializeReference] private List<IBeginEndAction> actions;

        public void OnEnable()
        {
            foreach (var action in actions)
            {
                action.Begin();
            }
        }

        public void OnDisable()
        {
            foreach (var action in actions)
            {
               action.End();
            }
        }
    }
}
