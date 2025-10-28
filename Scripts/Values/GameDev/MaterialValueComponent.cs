using System;
using System.Collections.Generic;
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

    public class MaterialListValueComponent : SerializedValueListComponent<Material>
    {
    }

    [Serializable]
    public class MaterialValueFromComponent :
        ValueFromComponent<Material, MaterialValueComponent>, IMaterialValue
    {
    }

    [Serializable]
    public class MaterialListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Material>, MaterialListValueComponent>
    {
    }
}
