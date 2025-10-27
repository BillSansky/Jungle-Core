using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class MaterialValueComponent : ValueComponent<Material>
    {
        [SerializeField]
        private Material value;

        public override Material Value()
        {
            return value;
        }

        public override void SetValue(Material value)
        {
            this.value = value;
        }
    }

    [Serializable]
    public class MaterialValueFromComponent :
        ValueFromComponent<Material, MaterialValueComponent>, IMaterialValue
    {
    }
}
