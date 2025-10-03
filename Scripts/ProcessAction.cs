using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle
{
    public class ProcessAction : MonoBehaviour
    {
        [SerializeField]
        private bool startOnAwake;

        [SerializeField]
        private UnityEvent onTransferStarted = new UnityEvent();

        [SerializeField]
        private UnityEvent onTransferComplete = new UnityEvent();

        [SerializeField]
        private UnityEvent onTransferFailed = new UnityEvent();

        private Func<IEnumerator> currentProcedureFactory;
        private Coroutine activeRoutine;
        private bool isTransferInProgress;
        private bool isTransferComplete;

        public UnityEvent OnTransferComplete => onTransferComplete;

        public UnityEvent OnTransferFailed => onTransferFailed;

        public UnityEvent OnTransferStarted => onTransferStarted;

        public bool IsTransferInProgress => isTransferInProgress;

        public bool IsTransferComplete => isTransferComplete;

        public void Start()
        {
            if (!startOnAwake)
            {
                return;
            }

            if (currentProcedureFactory == null)
            {
                return;
            }

            BeginProcedure(currentProcedureFactory);
        }

        public void Cancel()
        {
            var wasRunning = activeRoutine != null || isTransferInProgress;

            if (activeRoutine != null)
            {
                StopCoroutine(activeRoutine);
                activeRoutine = null;
            }

            if (!wasRunning)
            {
                return;
            }

            isTransferInProgress = false;
            isTransferComplete = false;
            onTransferFailed.Invoke();
        }

        public void Complete()
        {
            if (!isTransferInProgress)
            {
                return;
            }

            isTransferInProgress = false;
            isTransferComplete = true;
            onTransferComplete.Invoke();
        }

        public void StartScaleLerp(Transform target, Vector3 from, Vector3 to, float duration, AnimationCurve curve = null, bool useUnscaledTime = false)
        {
            StartProcedure(() => ScaleLerpRoutine(target, from, to, duration, curve, useUnscaledTime));
        }

        public void StartPositionLerp(Transform target, Vector3 from, Vector3 to, float duration, bool useLocalSpace = false, AnimationCurve curve = null, bool useUnscaledTime = false)
        {
            StartProcedure(() => PositionLerpRoutine(target, from, to, duration, useLocalSpace, curve, useUnscaledTime));
        }

        public void StartRotationLerp(Transform target, Quaternion from, Quaternion to, float duration, bool useLocalSpace = false, AnimationCurve curve = null, bool useUnscaledTime = false)
        {
            StartProcedure(() => RotationLerpRoutine(target, from, to, duration, useLocalSpace, curve, useUnscaledTime));
        }

        public void StartCanvasGroupFade(CanvasGroup target, float from, float to, float duration, AnimationCurve curve = null, bool useUnscaledTime = false)
        {
            StartProcedure(() => CanvasGroupFadeRoutine(target, from, to, duration, curve, useUnscaledTime));
        }

        public void StartMaterialColorLerp(Renderer target, Color from, Color to, float duration, int materialIndex = 0, string colorProperty = "_Color", AnimationCurve curve = null, bool useUnscaledTime = false)
        {
            StartProcedure(() => MaterialColorLerpRoutine(target, from, to, duration, materialIndex, colorProperty, curve, useUnscaledTime));
        }

        private void StartProcedure(Func<IEnumerator> procedureFactory)
        {
            currentProcedureFactory = procedureFactory;
            BeginProcedure(procedureFactory);
        }

        private void BeginProcedure(Func<IEnumerator> procedureFactory)
        {
            if (procedureFactory == null)
            {
                return;
            }

            Cancel();

            isTransferInProgress = true;
            isTransferComplete = false;
            onTransferStarted.Invoke();

            activeRoutine = StartCoroutine(RunProcedure(procedureFactory));
        }

        private IEnumerator RunProcedure(Func<IEnumerator> procedureFactory)
        {
            var routine = procedureFactory();
            while (routine.MoveNext())
            {
                yield return routine.Current;
            }

            Complete();
        }

        private IEnumerator ScaleLerpRoutine(Transform target, Vector3 from, Vector3 to, float duration, AnimationCurve curve, bool useUnscaledTime)
        {
            var elapsed = 0f;

            if (duration <= 0f)
            {
                target.localScale = to;
                yield break;
            }

            target.localScale = from;

            while (elapsed < duration)
            {
                elapsed += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                if (curve != null)
                {
                    t = curve.Evaluate(t);
                }

                target.localScale = Vector3.LerpUnclamped(from, to, t);
                yield return null;
            }

            target.localScale = to;
        }

        private IEnumerator PositionLerpRoutine(Transform target, Vector3 from, Vector3 to, float duration, bool useLocalSpace, AnimationCurve curve, bool useUnscaledTime)
        {
            var elapsed = 0f;

            if (duration <= 0f)
            {
                if (useLocalSpace)
                {
                    target.localPosition = to;
                }
                else
                {
                    target.position = to;
                }

                yield break;
            }

            if (useLocalSpace)
            {
                target.localPosition = from;
            }
            else
            {
                target.position = from;
            }

            while (elapsed < duration)
            {
                elapsed += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                if (curve != null)
                {
                    t = curve.Evaluate(t);
                }

                var value = Vector3.LerpUnclamped(from, to, t);
                if (useLocalSpace)
                {
                    target.localPosition = value;
                }
                else
                {
                    target.position = value;
                }

                yield return null;
            }

            if (useLocalSpace)
            {
                target.localPosition = to;
            }
            else
            {
                target.position = to;
            }
        }

        private IEnumerator RotationLerpRoutine(Transform target, Quaternion from, Quaternion to, float duration, bool useLocalSpace, AnimationCurve curve, bool useUnscaledTime)
        {
            var elapsed = 0f;

            if (duration <= 0f)
            {
                if (useLocalSpace)
                {
                    target.localRotation = to;
                }
                else
                {
                    target.rotation = to;
                }

                yield break;
            }

            if (useLocalSpace)
            {
                target.localRotation = from;
            }
            else
            {
                target.rotation = from;
            }

            while (elapsed < duration)
            {
                elapsed += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                if (curve != null)
                {
                    t = curve.Evaluate(t);
                }

                var value = Quaternion.LerpUnclamped(from, to, t);
                if (useLocalSpace)
                {
                    target.localRotation = value;
                }
                else
                {
                    target.rotation = value;
                }

                yield return null;
            }

            if (useLocalSpace)
            {
                target.localRotation = to;
            }
            else
            {
                target.rotation = to;
            }
        }

        private IEnumerator CanvasGroupFadeRoutine(CanvasGroup target, float from, float to, float duration, AnimationCurve curve, bool useUnscaledTime)
        {
            var elapsed = 0f;

            if (duration <= 0f)
            {
                target.alpha = to;
                yield break;
            }

            target.alpha = from;

            while (elapsed < duration)
            {
                elapsed += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                if (curve != null)
                {
                    t = curve.Evaluate(t);
                }

                target.alpha = Mathf.LerpUnclamped(from, to, t);
                yield return null;
            }

            target.alpha = to;
        }

        private IEnumerator MaterialColorLerpRoutine(Renderer target, Color from, Color to, float duration, int materialIndex, string colorProperty, AnimationCurve curve, bool useUnscaledTime)
        {
            var elapsed = 0f;
            var materials = target.materials;

            if (materialIndex < 0 || materialIndex >= materials.Length)
            {
                yield break;
            }

            var material = materials[materialIndex];

            if (duration <= 0f)
            {
                material.SetColor(colorProperty, to);
                yield break;
            }

            material.SetColor(colorProperty, from);

            while (elapsed < duration)
            {
                elapsed += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                if (curve != null)
                {
                    t = curve.Evaluate(t);
                }

                var color = Color.LerpUnclamped(from, to, t);
                material.SetColor(colorProperty, color);
                yield return null;
            }

            material.SetColor(colorProperty, to);
        }
    }
}
