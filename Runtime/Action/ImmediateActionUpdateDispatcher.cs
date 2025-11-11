using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Executes configured <see cref="IImmediateAction"/> collections during Unity update callbacks.
    /// </summary>
    public class ImmediateActionUpdateDispatcher : MonoBehaviour
    {
        [SerializeReference]
        [JungleClassSelection(typeof(IImmediateAction))]
        public List<IImmediateAction> updateActions = new List<IImmediateAction>();

        [SerializeReference]
        [JungleClassSelection(typeof(IImmediateAction))]
        public List<IImmediateAction> fixedUpdateActions = new List<IImmediateAction>();

        [SerializeReference]
        [JungleClassSelection(typeof(IImmediateAction))]
        public List<IImmediateAction> lateUpdateActions = new List<IImmediateAction>();

        private void Update()
        {
            ExecuteActions(updateActions);
        }

        private void FixedUpdate()
        {
            ExecuteActions(fixedUpdateActions);
        }

        private void LateUpdate()
        {
            ExecuteActions(lateUpdateActions);
        }

        private static void ExecuteActions(List<IImmediateAction> actions)
        {
            for (var i = 0; i < actions.Count; i++)
            {
                var action = actions[i];

                if (action == null)
                {
                    continue;
                }

                action.Execute();
            }
        }
    }
}
