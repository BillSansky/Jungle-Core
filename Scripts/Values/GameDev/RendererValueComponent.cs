using Jungle.Values;
using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values
{
    [JungleClassInfo("Renderer Value Component", "Component exposing a renderer component.", null, "Values/Game Dev")]
    public class RendererValueComponent : ValueComponent<Renderer>
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

    [JungleClassInfo("Renderer List Component", "Component exposing a list of renderers.", null, "Values/Game Dev")]
    public class RendererListValueComponent : SerializedValueListComponent<Renderer>
    {
    }

    [Serializable]
    [JungleClassInfo("Renderer Value From Component", "Reads a renderer component from a RendererValueComponent.", null, "Values/Game Dev")]
    public class RendererValueFromComponent : ValueFromComponent<Renderer, RendererValueComponent>, IRendererValue
    {
    }

    [Serializable]
    [JungleClassInfo("Renderer List From Component", "Reads renderers from a RendererListValueComponent.", null, "Values/Game Dev")]
    public class RendererListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Renderer>, RendererListValueComponent>
    {
    }
}
