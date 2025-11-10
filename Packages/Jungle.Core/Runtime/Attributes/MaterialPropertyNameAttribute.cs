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
        /// Lists the supported material property categories.
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
        /// Gets the name of the field containing the renderer value reference.
        /// </summary>
        public string RendererFieldName { get; }
        
        /// <summary>
        /// Gets the material property filter applied to the dropdown.
        /// </summary>
        public PropertyType FilterType { get; }
        /// <summary>
        /// Initializes the attribute with the renderer field name and optional filter.
        /// </summary>
        
        public MaterialPropertyNameAttribute(string rendererFieldName, PropertyType filterType = PropertyType.All)
        {
            RendererFieldName = rendererFieldName;
            FilterType = filterType;
        }
    }
}
