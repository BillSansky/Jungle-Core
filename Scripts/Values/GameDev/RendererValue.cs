using Jungle.Attributes;
using Jungle.Values;
using System;
using UnityEngine;

namespace Jungle.Values
{
    public interface IRendererValue : IComponent<Renderer>
    {
    }

    [Serializable]
    [JungleClassInfo("Renderer Value", "Stores a renderer component directly on the owner.", null, "Values/Game Dev", true)]
    public class RendererValue : LocalValue<Renderer>, IRendererValue
    {
        public override bool HasMultipleValues => false;
    }

    [Serializable]
    [JungleClassInfo("Renderer Member Value", "Returns a renderer component from a component field, property, or method.", null, "Values/Game Dev")]
    public class RendererClassMembersValue : ClassMembersValue<Renderer>, IRendererValue
    {
    }
}
