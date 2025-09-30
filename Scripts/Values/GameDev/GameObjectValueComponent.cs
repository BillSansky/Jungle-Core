using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class GameObjectValueComponent : ValueComponent<GameObject>
    {
        [SerializeField]
        private GameObject value;

        public override GameObject GetValue()
        {
            return value;
        }
    }
}
