using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IMaterialValue : IValue<Material>
    {
    }

    [Serializable]
    public class MaterialValue : LocalValue<Material>, IMaterialValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class MaterialMethodInvokerValue : MethodInvokerValue<Material>, IMaterialValue
    {
    }
}
