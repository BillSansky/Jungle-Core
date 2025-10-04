// SortingOrderAction2D.cs

using System;
using System.Collections.Generic;
using Jungle.Actions;
using Jungle.Core;
using Jungle.DragContext;
using UnityEngine;

namespace Jungle.Actions
{
    [Serializable]
    public class SortingOrderAction2D : StartStopAction
    {
        [SerializeReference] private FlexibleSpriteRendererValue targetRenderer = new();
        [SerializeField] private int dragSortingOrder = 100;

        private Dictionary<SpriteRenderer, int> originalSortingOrders = new();
        private bool skipStop;

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
            var renderers = targetRenderer.Value;

            // Clear previous data
            originalSortingOrders.Clear();

            int sortingOrderOffset = 0;

            foreach (var renderer in renderers)
            {
                if (!renderer) continue;

                // Store original sorting order
                originalSortingOrders[renderer] = renderer.sortingOrder;
                renderer.sortingOrder = dragSortingOrder + sortingOrderOffset;

                sortingOrderOffset++;
            }
        }

        protected override void OnStop()
        {
            if (skipStop)
            {
                return;
            }

            // Restore original sorting orders
            foreach (var kvp in originalSortingOrders)
            {
                if (kvp.Key)
                    kvp.Key.sortingOrder = kvp.Value;
            }
        }

        public override void OneShot(DraggableObject draggableInContext, DragZone dragZoneInContext)
        {
            UpdateContext(dragZoneInContext, draggableInContext);
            skipStop = true;
            StartAction();
            StopAction();
            skipStop = false;
        }

        public override void UpdateContext(DragZone dragZone, DraggableObject draggable)
        {
            targetRenderer.UpdateContext(dragZone, draggable);
        }
    }
}
