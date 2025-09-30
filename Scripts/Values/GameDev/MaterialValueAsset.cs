using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Material Value", fileName = "MaterialValue")]
    public class MaterialValueAsset : ValueAsset<Material>
    {
        [SerializeField]
        private Material value;

        public override Material GetValue()
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
