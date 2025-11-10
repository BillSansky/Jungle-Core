using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Values.Context.Impl
{
    /// <summary>
    /// Resolves a <see cref="Component"/> reference from a context-supplied <see cref="GameObject"/>.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Component Value From Context", "Resolves a component reference from a context-provided GameObject.", null, "Values/Game Dev")]
    public class ComponentValueFromContextObject : ComponentValueFromGameObjectContext<Component>, IComponentValue
    {
    }

    /// <summary>
    /// Resolves an <see cref="AudioSource"/> reference from a context-supplied <see cref="GameObject"/>.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Audio Source Value From Context", "Resolves an audio source component from a context-provided GameObject.", null, "Values/Game Dev")]
    public class AudioSourceValueFromContextObject : ComponentValueFromGameObjectContext<AudioSource>, IAudioSourceValue
    {
    }

    /// <summary>
    /// Resolves a <see cref="Collider"/> reference from a context-supplied <see cref="GameObject"/>.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Collider Value From Context", "Resolves a collider component from a context-provided GameObject.", null, "Values/Game Dev")]
    public class ColliderValueFromContextObject : ComponentValueFromGameObjectContext<Collider>, IColliderValue
    {
    }

    /// <summary>
    /// Resolves a <see cref="Renderer"/> reference from a context-supplied <see cref="GameObject"/>.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Renderer Value From Context", "Resolves a renderer component from a context-provided GameObject.", null, "Values/Game Dev")]
    public class RendererValueFromContextObject : ComponentValueFromGameObjectContext<Renderer>, IRendererValue
    {
    }

    /// <summary>
    /// Resolves a <see cref="Rigidbody"/> reference from a context-supplied <see cref="GameObject"/>.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Rigidbody Value From Context", "Resolves a rigidbody component from a context-provided GameObject.", null, "Values/Game Dev")]
    public class RigidbodyValueFromContextObject : ComponentValueFromGameObjectContext<Rigidbody>, IRigidbodyValue
    {
    }
}
