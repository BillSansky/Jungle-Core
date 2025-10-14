using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Actions
{
    public class ObjectActivationActionExecutor : MonoBehaviour
    {
     

        [SerializeReference] private List<IStateAction> actions;

        public void OnEnable()
        {
            foreach (var action in actions)
            {
                action.OnStateEnter();
            }
        }

        public void OnDisable()
        {
            foreach (var action in actions)
            {
               action.OnStateExit();
            }
        }
    }
}
