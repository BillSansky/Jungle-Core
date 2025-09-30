using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class BoundsValueComponent : ValueComponent<Bounds>
    {
        [SerializeField]
        private Bounds value = new Bounds(Vector3.zero, Vector3.one);

        public override Bounds GetValue()
        {
            return value;
        }
    }
}
