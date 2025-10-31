using Jungle.Values;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// ScriptableObject storing a Renderer reference for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity", fileName = "RendererValueAsset")]
    public class RendererValueAsset : ValueAsset<Renderer>
    {
        [SerializeField]
        private Renderer value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Renderer Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Renderer value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Renderer references for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Renderer list value", fileName = "RendererListValueAsset")]
    public class RendererListValueAsset : SerializedValueListAsset<Renderer>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Renderer reference from the assigned RendererValueAsset.
    /// </summary>
    [Serializable]
    public class RendererValueFromAsset : ValueFromAsset<Renderer, RendererValueAsset>, IRendererValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Renderer references from an assigned RendererListValueAsset.
    /// </summary>
    [Serializable]
    public class RendererListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Renderer>, RendererListValueAsset>
    {
    }
}
