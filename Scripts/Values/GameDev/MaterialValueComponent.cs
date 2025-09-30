using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class MaterialValueComponent : ValueComponent<Material>
    {
        [SerializeField]
        private Material value;

        public override Material GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class MaterialValueFromComponent :
        ValueFromComponent<Material, MaterialValueComponent>, IMaterialValue
    {
    }
}
