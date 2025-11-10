using System;
using UnityEngine;

namespace Jungle.Attributes
{
    /// <summary>
    /// Adds a contextual class selection button to component reference fields in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class JungleClassSelectionAttribute : PropertyAttribute
    {
        /// <summary>
        /// Base type used to populate the class selection menu. If null the field type will be used.
        /// </summary>
        public Type BaseType { get; }

        /// <summary>
        /// Creates a new attribute instance using the specified base type.
        /// </summary>
        /// <param name="baseType">The base component type that will populate the selection menu.</param>
        public JungleClassSelectionAttribute(Type baseType = null)
        {
            BaseType = baseType;
        }
    }
}
