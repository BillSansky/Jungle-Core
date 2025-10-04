using System.Collections;
using UnityEngine;

namespace Jungle.Utils
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("CoroutineRunner");
                    _instance = go.AddComponent<CoroutineRunner>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        public static Coroutine StartManagedCoroutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        public static void StopManagedCoroutine(Coroutine coroutine)
        {
            if (coroutine != null && Instance != null)
            {
                Instance.StopCoroutine(coroutine);
            }
        }

        public static void StopAllManagedCoroutines()
        {
            if (Instance != null)
            {
                Instance.StopAllCoroutines();
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
