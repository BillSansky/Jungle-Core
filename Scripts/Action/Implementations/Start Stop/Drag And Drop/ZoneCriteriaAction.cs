using System;
using System.Collections.Generic;
using Jungle.Actions;
using Jungle.Core;
using Jungle.DragContext;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jungle.Actions
{
    [Serializable]
    public class ZoneCriteriaAction : StartStopAction
    {
        [FormerlySerializedAs("zoneCriteria")] [SerializeField] private DragZoneFilter zoneFilter;
        [SerializeField] private DragZoneContextualValue dragZone;

        [SerializeReference] private List<StartStopAction> actions = new();
        [SerializeReference] private List<StartStopAction> fallbackActions = new();

        private DraggableObject currentDraggable;
        private bool criteriaFulfilledOnStart;

        public void StartActionWithContext(DraggableObject draggableInContext, DragZone dragZoneInContext)
        {
            UpdateContext(dragZoneInContext, draggableInContext);

            StartAction();
        }

        public void StartAction()
        {
            Start();
        }

        public void StopAction()
        {
            Stop();
        }

        protected override void OnStart()
        {
            criteriaFulfilledOnStart = zoneFilter.IsDragZoneValid(dragZone);

            if (criteriaFulfilledOnStart)
            {
                foreach (var action in actions)
                {
                    DragContextActionUtility.StartActionWithContext(action, currentDraggable, dragZone);
                }
            }
            else
            {
                foreach (var action in fallbackActions)
                {
                    DragContextActionUtility.StartActionWithContext(action, currentDraggable, dragZone);
                }
            }
        }

        protected override void OnStop()
        {
            if (criteriaFulfilledOnStart)
            {
                foreach (var action in actions)
                {
                    DragContextActionUtility.StopAction(action);
                }
            }
            else
            {
                foreach (var action in fallbackActions)
                {
                    DragContextActionUtility.StopAction(action);
                }
            }
        }

        public override void OneShot(DraggableObject draggableInContext, DragZone dragZoneInContext)
        {
            UpdateContext(dragZoneInContext, draggableInContext);

            if (zoneFilter.IsDragZoneValid(dragZone))
            {
                foreach (var action in actions)
                {
                    DragContextActionUtility.OneShot(action, currentDraggable, dragZone);
                }
            }
            else
            {
                foreach (var action in fallbackActions)
                {
                    DragContextActionUtility.OneShot(action, currentDraggable, dragZone);
                }
            }
        }

        public override void UpdateContext(DragZone dragZone, DraggableObject draggable)
        {
            this.dragZone.UpdateContext(dragZone, draggable);
            currentDraggable = draggable;
        }
    }
}
