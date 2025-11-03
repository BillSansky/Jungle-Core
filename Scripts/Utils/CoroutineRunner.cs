using System.Collections;
using UnityEngine;

namespace Jungle.Utils
{
    /// <summary>
    /// Singleton MonoBehaviour that runs coroutines on behalf of runtime systems.
    /// </summary>
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner instance;
        /// <summary>
        /// Gets the lazily created coroutine runner singleton.
        /// </summary>

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
        /// Starts a coroutine managed by this runner.
        /// </summary>

        public static Coroutine StartManagedCoroutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }
        /// <summary>
        /// Stops a coroutine started through the runner.
        /// </summary>

        public static void StopManagedCoroutine(Coroutine coroutine)
        {
            if (coroutine != null && Instance != null)
            {
                Instance.StopCoroutine(coroutine);
            }
        }
        /// <summary>
        /// Stops every coroutine started through the runner.
        /// </summary>

        public static void StopAllManagedCoroutines()
        {
            if (Instance != null)
            {
                Instance.StopAllCoroutines();
            }
        }

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
