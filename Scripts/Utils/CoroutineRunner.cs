using System.Collections;
using UnityEngine;

namespace Jungle.Utils
{
    /// <summary>
    /// Utility MonoBehaviour that hosts coroutines for non-MonoBehaviour objects.
    /// </summary>
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject("CoroutineRunner");
                    instance = go.AddComponent<CoroutineRunner>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }
        /// <summary>
        /// Starts a coroutine using the shared runner instance.
        /// </summary>
        public static Coroutine StartManagedCoroutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }
        /// <summary>
        /// Stops the supplied coroutine if the runner exists.
        /// </summary>
        public static void StopManagedCoroutine(Coroutine coroutine)
        {
            if (coroutine != null && Instance != null)
            {
                Instance.StopCoroutine(coroutine);
            }
        }
        /// <summary>
        /// Cancels every coroutine started through the runner.
        /// </summary>
        public static void StopAllManagedCoroutines()
        {
            if (Instance != null)
            {
                Instance.StopAllCoroutines();
            }
        }
        /// <summary>
        /// Ensures a single runner instance persists across scene loads.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
