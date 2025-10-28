using Jungle.Values;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity", fileName = "RendererValueAsset")]
    public class RendererValueAsset : ValueAsset<Renderer>
    {
        [SerializeField]
        private Renderer value;

        public override Renderer Value()
        {
            return value;
        }

        public override void SetValue(Renderer value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Renderer list value", fileName = "RendererListValueAsset")]
    public class RendererListValueAsset : SerializedValueListAsset<Renderer>
    {
    }

    [Serializable]
    public class RendererValueFromAsset : ValueFromAsset<Renderer, RendererValueAsset>, IRendererValue
    {
    }

    [Serializable]
    public class RendererListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Renderer>, RendererListValueAsset>
    {
    }
}
