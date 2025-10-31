﻿using System.Collections;
using System.Collections.Generic;
using Jungle.Actions;
using Jungle.Attributes;
using Jungle.Utils;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Animates a material float property between two values over the action duration.
    /// </summary>
    [System.Serializable]
    public class MaterialFloatLerpAction : LerpProcessAction<float>, IStateAction
    {
        [SerializeField] [MaterialPropertyName("targetRenderer", MaterialPropertyNameAttribute.PropertyType.Float)]
        private string propertyName = "_Metallic";

        [SerializeField] private float targetValue = 1.0f;
        [SerializeField] private float lerpIntensity = 1.0f;

        [SerializeField] private int materialIndex = -1; // -1 means all materials, 0+ targets specific material index

        [SerializeReference] [JungleClassSelection]
        private IRendererValue targetRenderer;

        private Dictionary<Renderer, float[]> originalValuesMap = new();
        /// <summary>
        /// Records the starting float for each targeted material before the effect begins.
        /// </summary>
        protected override void OnBeforeStart()
        {
            base.OnBeforeStart();

            // Store original float values
            foreach (var renderer in targetRenderer.Values)
            {
                var materials = renderer.materials;
                var originalValues = new float[materials.Length];

                if (materialIndex >= 0 && materialIndex < materials.Length)
                {
                    if (materials[materialIndex].HasProperty(propertyName))
                    {
                        originalValues[materialIndex] = materials[materialIndex].GetFloat(propertyName);
                    }
                }
                else
                {
                    for (var i = 0; i < materials.Length; i++)
                    {
                        if (materials[i].HasProperty(propertyName))
                        {
                            originalValues[i] = materials[i].GetFloat(propertyName);
                        }
                    }
                }

                originalValuesMap[renderer] = originalValues;
            }
        }
        /// <summary>
        /// Uses the stored material property value as the interpolation baseline.
        /// </summary>
        protected override float GetStartValue()
        {
            // Return the first renderer's first material float value as reference
            if (targetRenderer.Values != null)
            {
                foreach (var renderer in targetRenderer.Values)
                {
                    if (renderer != null && originalValuesMap.TryGetValue(renderer, out var values))
                    {
                        return materialIndex >= 0 && materialIndex < values.Length
                            ? values[materialIndex]
                            : values[0];
                    }
                }
            }

            return 0f;
        }
        /// <summary>
        /// Returns the target float value that should be written to the material property.
        /// </summary>
        protected override float GetEndValue()
        {
            return targetValue;
        }
        /// <summary>
        /// Computes a float that moves toward the target while respecting the configured intensity multiplier.
        /// </summary>
        protected override float LerpValue(float start, float end, float t)
        {
            return Mathf.Lerp(start, end, t * lerpIntensity);
        }
        /// <summary>
        /// Applies the interpolated float to each renderer's material property.
        /// </summary>
        protected override void ApplyValue(float value)
        {
            foreach (var renderer in targetRenderer.Values)
            {
                if (renderer == null) continue;

                var materials = renderer.materials;
                if (materialIndex >= 0 && materialIndex < materials.Length)
                {
                    if (materials[materialIndex].HasProperty(propertyName))
                    {
                        float lerpedValue = Mathf.Lerp(
                            originalValuesMap[renderer][materialIndex],
                            targetValue,
                            value);
                        materials[materialIndex].SetFloat(propertyName, lerpedValue);
                    }
                }
                else
                {
                    for (var i = 0; i < materials.Length; i++)
                    {
                        if (materials[i].HasProperty(propertyName))
                        {
                            float lerpedValue = Mathf.Lerp(
                                originalValuesMap[renderer][i],
                                targetValue,
                                value);
                            materials[i].SetFloat(propertyName, lerpedValue);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Kicks off the material property tween when the parent state activates.
        /// </summary>
        public void OnStateEnter()
        {
            Start();
        }
        /// <summary>
        /// Stops the tween and restores the original property values once the state ends.
        /// </summary>
        public void OnStateExit()
        {
            Interrupt();

            // Restore original float values
            foreach (var renderer in targetRenderer.Values)
            {
                if (renderer && originalValuesMap.ContainsKey(renderer))
                {
                    var materials = renderer.materials;
                    var originalValues = originalValuesMap[renderer];

                    if (materialIndex >= 0 && materialIndex < materials.Length)
                    {
                        if (materials[materialIndex].HasProperty(propertyName))
                        {
                            materials[materialIndex].SetFloat(propertyName, originalValues[materialIndex]);
                        }
                    }
                    else
                    {
                        for (var i = 0; i < materials.Length; i++)
                        {
                            if (materials[i].HasProperty(propertyName))
                            {
                                materials[i].SetFloat(propertyName, originalValues[i]);
                            }
                        }
                    }
                }
            }

            originalValuesMap.Clear();
        }
    }
}