using Jungle.Values;
using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Defines the IRendererValue contract.
    /// </summary>
    public interface IRendererValue : IComponent<Renderer>
    {
    }
    /// <summary>
    /// Stores a Renderer reference directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class RendererValue : LocalValue<Renderer>, IRendererValue
    {
        public override bool HasMultipleValues => false;
    }
    /// <summary>
    /// Resolves a Renderer reference by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class RendererClassMembersValue : ClassMembersValue<Renderer>, IRendererValue
    {
    }
}
