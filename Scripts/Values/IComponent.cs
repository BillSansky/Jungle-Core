using UnityEngine;

namespace Jungle.Values
{
    public interface IComponent
    {
        GameObject GameObject { get; }

        Transform Transform { get; }
    }

    public interface IComponent<out TComponent> : IValue<TComponent>, IComponent
        where TComponent : Component
    {
        public GameObject GameObject => Value().gameObject;

        public Transform Transform => Value().transform;
    }
}
