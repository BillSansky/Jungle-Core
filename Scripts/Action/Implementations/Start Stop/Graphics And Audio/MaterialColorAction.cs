using System.Collections;
using System.Collections.Generic;
using Jungle.Actions;

using Jungle.Utils;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Actions
{
    [System.Serializable]
    public class MaterialColorAction : StartStopAction
    {
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private float highlightIntensity = 0.5f;

        [SerializeField] private float lerpDuration = 0.5f;
        [SerializeField] private AnimationCurve lerpCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField] private float lerpBackDuration = 0.5f;
        [SerializeField] private AnimationCurve lerpBackCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField] private int materialIndex = -1; // -1 means all materials, 0+ targets specific material index

        [SerializeReference] private IRendererValue targetRenderer;

        private Dictionary<Renderer, Color[]> originalColorsMap = new();

        private Coroutine colorLerpCoroutine;

      
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
            if (colorLerpCoroutine != null)
            {
                CoroutineRunner.StopManagedCoroutine(colorLerpCoroutine);

                foreach (var renderer in targetRenderer.Values)
                {
                    if (originalColorsMap.TryGetValue(renderer, out var originalColors))
                    {
                        var materials = renderer.materials;
                        if (materialIndex >= 0 && materialIndex < materials.Length)
                        {
                            materials[materialIndex].color = originalColors[materialIndex];
                        }
                        else
                        {
                            for (var i = 0; i < materials.Length; i++)
                            {
                                materials[i].color = originalColors[i];
                            }
                        }
                    }
                }
            }

            foreach (var renderer in targetRenderer.Values)
            {
                var materials = renderer.materials;
                var originalColors = new Color[materials.Length];

                if (materialIndex >= 0 && materialIndex < materials.Length)
                {
                    // Only store color for the specified material index
                    originalColors[materialIndex] = materials[materialIndex].color;
                }
                else
                {
                    // Store colors for all materials
                    for (var i = 0; i < materials.Length; i++)
                    {
                        originalColors[i] = materials[i].color;
                    }
                }

                originalColorsMap[renderer] = originalColors;
            }

            colorLerpCoroutine = CoroutineRunner.StartManagedCoroutine(LerpColors());
        }

        private IEnumerator LerpColors()
        {
            float elapsedTime = 0;

            while (elapsedTime < lerpDuration)
            {
                float normalizedTime = elapsedTime / lerpDuration;
                float currentIntensity = lerpCurve.Evaluate(normalizedTime) * highlightIntensity;

                foreach (var renderer in targetRenderer.Values)
                {
                    if (renderer == null) continue;

                    var materials = renderer.materials;
                    if (materialIndex >= 0 && materialIndex < materials.Length)
                    {
                        materials[materialIndex].color =
                            Color.Lerp(originalColorsMap[renderer][materialIndex], highlightColor, currentIntensity);
                    }
                    else
                    {
                        for (var i = 0; i < materials.Length; i++)
                        {
                            materials[i].color =
                                Color.Lerp(originalColorsMap[renderer][i], highlightColor, currentIntensity);
                        }
                    }
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        protected override void OnStop()
        {
            if (colorLerpCoroutine != null)
            {
                CoroutineRunner.StopManagedCoroutine(colorLerpCoroutine);
                colorLerpCoroutine = null;
            }

            colorLerpCoroutine = CoroutineRunner.StartManagedCoroutine(LerpBackColors());
        }

        private IEnumerator OneShotRoutine()
        {
            StartAction();
            yield return new WaitForSeconds(lerpDuration);
            StopAction();
        }

        private IEnumerator LerpBackColors()
        {
            float elapsedTime = 0;
            Dictionary<Renderer, Color[]> currentColors = new Dictionary<Renderer, Color[]>();

            foreach (var renderer in targetRenderer.Values)
            {
                var materials = renderer.materials;
                var colors = new Color[materials.Length];
                for (var i = 0; i < materials.Length; i++)
                {
                    colors[i] = materials[i].color;
                }

                currentColors[renderer] = colors;
            }

            while (elapsedTime < lerpBackDuration)
            {
                float normalizedTime = elapsedTime / lerpBackDuration;
                float currentIntensity = lerpBackCurve.Evaluate(normalizedTime);

                foreach (var renderer in targetRenderer.Values)
                {
                    if (renderer == null || !originalColorsMap.ContainsKey(renderer)) continue;

                    var materials = renderer.materials;
                    for (var i = 0; i < materials.Length; i++)
                    {
                        materials[i].color = Color.Lerp(currentColors[renderer][i], originalColorsMap[renderer][i],
                            currentIntensity);
                    }
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            foreach (var renderer in targetRenderer.Values)
            {
                if (renderer && originalColorsMap.ContainsKey(renderer))
                {
                    var materials = renderer.materials;
                    var originalColors = originalColorsMap[renderer];

                    for (var i = 0; i < materials.Length; i++)
                    {
                        materials[i].color = originalColors[i];
                    }
                }
            }

            originalColorsMap.Clear();
            colorLerpCoroutine = null;
        }

       
    }
}
