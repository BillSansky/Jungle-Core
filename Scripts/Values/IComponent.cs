using UnityEngine;

namespace Jungle.Values
{
    public interface IComponent<out TComponent> : IValue<TComponent>
        where TComponent : Component
    {
        public GameObject GameObject => Value().gameObject;

        public Transform Transform => Value().transform;
    }
}
