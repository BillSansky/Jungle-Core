using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Material value", fileName = "MaterialValue")]
    public class MaterialValueAsset : ValueAsset<Material>
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

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Material list value", fileName = "MaterialListValue")]
    public class MaterialListValueAsset : SerializedValueListAsset<Material>
    {
    }

    [Serializable]
    public class MaterialValueFromAsset :
        ValueFromAsset<Material, MaterialValueAsset>, IMaterialValue
    {
    }

    [Serializable]
    public class MaterialListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Material>, MaterialListValueAsset>
    {
    }
}
