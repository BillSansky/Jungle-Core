using System.Collections;
using System.Collections.Generic;
using Jungle.Actions;
using Jungle.Attributes;
using Jungle.Utils;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Animates a material color property between two values over the action duration.
    /// </summary>
    [System.Serializable]
    public class MaterialColorLerpAction : LerpProcessAction<Color>, IStateAction
    {
        [SerializeField] 
        private Color highlightColor = Color.yellow;

        [SerializeField] private float highlightIntensity = 0.5f;

        [SerializeField] private int materialIndex = -1; // -1 means all materials, 0+ targets specific material index
        [SerializeField]
        [MaterialPropertyName("targetRenderer", MaterialPropertyNameAttribute.PropertyType.Color)]
        private string colorPropertyName = "_Color";

        [SerializeReference] [JungleClassSelection]
        private IRendererValue targetRenderer;

        private Dictionary<Renderer, Color[]> originalColorsMap = new();
        /// <summary>
        /// Caches each renderer's original color so the effect can animate from and back to it.
        /// </summary>
        protected override void OnBeforeStart()
        {
            base.OnBeforeStart();

            // Store original colors
            foreach (var renderer in targetRenderer.Values)
            {
                var materials = renderer.materials;
                var originalColors = new Color[materials.Length];

                if (materialIndex >= 0 && materialIndex < materials.Length)
                {
                    originalColors[materialIndex] = materials[materialIndex].GetColor(colorPropertyName);
                }
                else
                {
                    for (var i = 0; i < materials.Length; i++)
                    {
                        originalColors[i] = materials[i].GetColor(colorPropertyName);
                    }
                }

                originalColorsMap[renderer] = originalColors;
            }
        }
        /// <summary>
        /// Uses the cached original material color as the baseline for interpolation.
        /// </summary>
        protected override Color GetStartValue()
        {
            // Return the first renderer's first material color as reference
            // The actual lerping happens per-material in ApplyValue
            if (targetRenderer.Values != null)
            {
                foreach (var renderer in targetRenderer.Values)
                {
                    if (renderer != null && originalColorsMap.TryGetValue(renderer, out var colors))
                    {
                        return materialIndex >= 0 && materialIndex < colors.Length
                            ? colors[materialIndex]
                            : colors[0];
                    }
                }
            }

            return Color.white;
        }
        /// <summary>
        /// Returns the configured highlight color the renderer should approach.
        /// </summary>
        protected override Color GetEndValue()
        {
            return highlightColor;
        }
        /// <summary>
        /// Blends from the stored color toward the highlight while applying the intensity multiplier.
        /// </summary>
        protected override Color LerpValue(Color start, Color end, float t)
        {
            return Color.Lerp(start, end, t * highlightIntensity);
        }
        /// <summary>
        /// Writes the interpolated color to each targeted material slot.
        /// </summary>
        protected override void ApplyValue(Color value)
        {
            foreach (var renderer in targetRenderer.Values)
            {
                if (renderer == null) continue;

                var materials = renderer.materials;
                if (materialIndex >= 0 && materialIndex < materials.Length)
                {
                    materials[materialIndex].SetColor(colorPropertyName, Color.Lerp(
                        originalColorsMap[renderer][materialIndex],
                        highlightColor,
                        value.a * highlightIntensity));
                }
                else
                {
                    for (var i = 0; i < materials.Length; i++)
                    {
                        materials[i].SetColor(colorPropertyName, Color.Lerp(
                            originalColorsMap[renderer][i],
                            highlightColor,
                            value.a * highlightIntensity));
                    }
                }
            }
        }
        /// <summary>
        /// Starts the color tween when the parent state activates.
        /// </summary>
        public void OnStateEnter()
        {
            Start();
        }
        /// <summary>
        /// Stops the tween and restores the original colors when the state finishes.
        /// </summary>
        public void OnStateExit()
        {
            Interrupt();

            // Restore original colors
            foreach (var renderer in targetRenderer.Values)
            {
                if (renderer && originalColorsMap.ContainsKey(renderer))
                {
                    var materials = renderer.materials;
                    var originalColors = originalColorsMap[renderer];

                    if (materialIndex >= 0 && materialIndex < materials.Length)
                    {
                        materials[materialIndex].SetColor(colorPropertyName, originalColors[materialIndex]);
                    }
                    else
                    {
                        for (var i = 0; i < materials.Length; i++)
                        {
                            materials[i].SetColor(colorPropertyName, originalColors[i]);
                        }
                    }
                }
            }

            originalColorsMap.Clear();
        }
    }
}