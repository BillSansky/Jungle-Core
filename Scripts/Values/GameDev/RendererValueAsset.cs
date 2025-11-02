using Jungle.Values;
using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity", fileName = "RendererValueAsset")]
    [JungleClassInfo("Renderer Value Asset", "ScriptableObject storing a renderer component.", null, "Values/Game Dev")]
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
    [JungleClassInfo("Renderer List Asset", "ScriptableObject storing a list of renderers.", null, "Values/Game Dev")]
    public class RendererListValueAsset : SerializedValueListAsset<Renderer>
    {
    }

    [Serializable]
    [JungleClassInfo("Renderer Value From Asset", "Reads a renderer component from a RendererValueAsset.", null, "Values/Game Dev")]
    public class RendererValueFromAsset : ValueFromAsset<Renderer, RendererValueAsset>, IRendererValue
    {
    }

    [Serializable]
    [JungleClassInfo("Renderer List From Asset", "Reads renderers from a RendererListValueAsset.", null, "Values/Game Dev")]
    public class RendererListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Renderer>, RendererListValueAsset>
    {
    }
}
