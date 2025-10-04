using System;
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
    }

    [Serializable]
    public class MaterialValueFromAsset :
        ValueFromAsset<Material, MaterialValueAsset>, IMaterialValue
    {
    }
}
