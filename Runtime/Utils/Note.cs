using UnityEngine;

namespace Jungle.Utils
{
    [AddComponentMenu("Jungle/Utils/Note")]
    public class Note : MonoBehaviour
    {
#if UNITY_EDITOR
        [TextArea(3, 10)]
        public string noteText = "Enter your note here..."; 

        [Space]
        public Color noteColor = Color.yellow;

        public bool showInPlayMode;
#endif
    }
}