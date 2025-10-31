using System;
using UnityEngine;

namespace Jungle.Attributes
{
    /// <summary>
    /// Attribute for material property name fields that provides a dropdown populated from the target renderer's materials.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class MaterialPropertyNameAttribute : PropertyAttribute
    {
        /// <summary>
        /// Enumerates the PropertyType values.
        /// </summary>
        public enum PropertyType
        {
            Float,
            Color,
            Vector,
            Texture,
            All
        }

        /// <summary>
        /// The name of the field containing the IRendererValue reference
        /// </summary>
        public string RendererFieldName { get; }
        
        /// <summary>
        /// Filter properties by type
        /// </summary>
        public PropertyType FilterType { get; }
        
        public MaterialPropertyNameAttribute(string rendererFieldName, PropertyType filterType = PropertyType.All)
        {
            RendererFieldName = rendererFieldName;
            FilterType = filterType;
        }
    }
}
