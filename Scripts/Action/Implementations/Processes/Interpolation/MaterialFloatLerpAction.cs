using System.Collections;
using System.Collections.Generic;
using Jungle.Actions;
using Jungle.Attributes;
using Jungle.Utils;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Actions
{
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

        protected override void OnBeforeStart()
        {
            base.OnBeforeStart();

            // Store original float values
            foreach (var renderer in targetRenderer.Refs)
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

        protected override float GetStartValue()
        {
            // Return the first renderer's first material float value as reference
            if (targetRenderer.Refs != null)
            {
                foreach (var renderer in targetRenderer.Refs)
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

        protected override float GetEndValue()
        {
            return targetValue;
        }

        protected override float LerpValue(float start, float end, float t)
        {
            return Mathf.Lerp(start, end, t * lerpIntensity);
        }

        protected override void ApplyValue(float value)
        {
            foreach (var renderer in targetRenderer.Refs)
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

        public void OnStateEnter()
        {
            Start();
        }

        public void OnStateExit()
        {
            Interrupt();

            // Restore original float values
            foreach (var renderer in targetRenderer.Refs)
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