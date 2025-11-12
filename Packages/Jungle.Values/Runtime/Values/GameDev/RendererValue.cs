using Jungle.Attributes;
using Jungle.Values;
using System;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Provides access to a Renderer reference.
    /// </summary>
    public interface IRendererValue : IValue<Renderer>
    {
    }
    public interface ISettableRendererValue : IRendererValue, IValueSableValue<Renderer>
    {
    }
    /// <summary>
    /// Stores a renderer component directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Renderer Value", "Stores a renderer component directly on the owner.", null, "Game Dev", true)]
    public class RendererValue : LocalValue<Renderer>, ISettableRendererValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
    }
    /// <summary>
    /// Returns a renderer component from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Renderer Member Value", "Returns a renderer component from a component field, property, or method.", null, "Game Dev")]
    public class RendererClassMembersValue : ClassMembersValue<Renderer>, IRendererValue
    {
    }
}
