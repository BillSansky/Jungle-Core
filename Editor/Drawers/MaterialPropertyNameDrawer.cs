﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Editor
{
    /// <summary>
    /// Displays shader property names in a popup so users can pick the material slot to target.
    /// </summary>
    [CustomPropertyDrawer(typeof(MaterialPropertyNameAttribute))]
    public class MaterialPropertyNameDrawer : PropertyDrawer
    {
        private const string NoPropertiesFound = "<No Properties Found>";
        private const string NoRendererSet = "<Set Renderer First>";
        /// <summary>
        /// Handles the OnGUI event.
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label.text, "MaterialProperty attribute can only be used on string fields");
                return;
            }
            var attr = attribute as MaterialPropertyNameAttribute;
            var propertyNames = GetMaterialPropertiesFromRenderer(property, attr);

            EditorGUI.BeginProperty(position, label, property);
            
            if (propertyNames.Count == 0)
            {
                // Show a message if no properties found
                Rect messageRect = new Rect(position.x, position.y, position.width * 0.5f, position.height);
                Rect fieldRect = new Rect(position.x + position.width * 0.5f, position.y, position.width * 0.5f, position.height);
                
                EditorGUI.LabelField(messageRect, label.text);
                EditorGUI.LabelField(fieldRect, NoRendererSet, EditorStyles.miniLabel);
            }
            else
            {
                int currentIndex = propertyNames.IndexOf(property.stringValue);
                if (currentIndex < 0) currentIndex = 0;

                int newIndex = EditorGUI.Popup(position, label.text, currentIndex, propertyNames.ToArray());
                if (newIndex >= 0 && newIndex < propertyNames.Count)
                {
                    property.stringValue = propertyNames[newIndex];
                }
            }
            
            EditorGUI.EndProperty();
        }
        /// <summary>
        /// Gathers material properties from the provided renderer.
        /// </summary>
        private List<string> GetMaterialPropertiesFromRenderer(SerializedProperty property, Jungle.Attributes.MaterialPropertyNameAttribute attr)
        {
            // Find the parent object that contains both the property field and the renderer field
            var targetObject = property.serializedObject.targetObject;
            var parentProperty = GetParentProperty(property);

            if (parentProperty != null)
            {
                // Try to get the field value through reflection
                var rendererValue = GetFieldValueByName(parentProperty, attr.RendererFieldName);
                if (rendererValue is IRendererValue iRendererValue)
                {
                    try
                    {
                        var renderers = iRendererValue.Values;
                        if (renderers != null)
                        {
                            var rendererList = renderers.ToList();
                            if (rendererList.Count == 0)
                            {
                                return new List<string>();
                            }

                            // Collect properties from each renderer separately
                            List<HashSet<string>> propertiesPerRenderer = new List<HashSet<string>>();

                            foreach (var renderer in rendererList)
                            {
                                if (renderer != null)
                                {
                                    var rendererProperties = new HashSet<string>();
                                    CollectPropertiesFromRenderer(renderer, rendererProperties, attr.FilterType);
                                    propertiesPerRenderer.Add(rendererProperties);
                                }
                            }

                            // If no valid renderers, return empty list
                            if (propertiesPerRenderer.Count == 0)
                            {
                                return new List<string>();
                            }

                            // Find intersection of all property sets (only properties that all renderers have)
                            var commonProperties = propertiesPerRenderer[0];
                            for (int i = 1; i < propertiesPerRenderer.Count; i++)
                            {
                                commonProperties.IntersectWith(propertiesPerRenderer[i]);
                            }

                            return commonProperties.OrderBy(p => p).ToList();
                        }
                    }
                    catch
                    {
                        // Silently continue if Values isn't available
                    }
                }
            }

            return new List<string>();
        }
        /// <summary>
        /// Collects property names from the renderer's materials.
        /// </summary>
        private void CollectPropertiesFromRenderer(Renderer renderer, HashSet<string> properties, Jungle.Attributes.MaterialPropertyNameAttribute.PropertyType filterType)
        {
            if (renderer == null) return;

            var materials = renderer.sharedMaterials;
            if (materials == null) return;

            foreach (var material in materials)
            {
                if (material != null && material.shader != null)
                {
                    var shader = material.shader;
                    int propertyCount = shader.GetPropertyCount();
                    
                    for (int i = 0; i < propertyCount; i++)
                    {
                        var propType = shader.GetPropertyType(i);
                        
                        bool shouldAdd = ShouldIncludeProperty(propType, filterType);
                        
                        if (shouldAdd)
                        {
                            properties.Add(shader.GetPropertyName(i));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Determines whether a material property should appear in the dropdown.
        /// </summary>
        private bool ShouldIncludeProperty(UnityEngine.Rendering.ShaderPropertyType propType, Jungle.Attributes.MaterialPropertyNameAttribute.PropertyType filterType)
        {
            switch (filterType)
            {
                case MaterialPropertyNameAttribute.PropertyType.All:
                    return true;
                    
                case MaterialPropertyNameAttribute.PropertyType.Float:
                    return propType == UnityEngine.Rendering.ShaderPropertyType.Float ||
                           propType == UnityEngine.Rendering.ShaderPropertyType.Range;
                    
                case MaterialPropertyNameAttribute.PropertyType.Color:
                    return propType == UnityEngine.Rendering.ShaderPropertyType.Color;
                    
                case MaterialPropertyNameAttribute.PropertyType.Vector:
                    return propType == UnityEngine.Rendering.ShaderPropertyType.Vector;
                    
                case MaterialPropertyNameAttribute.PropertyType.Texture:
                    return propType == UnityEngine.Rendering.ShaderPropertyType.Texture;
                    
                default:
                    return false;
            }
        }
        /// <summary>
        /// Returns the parent SerializedProperty of the current field.
        /// </summary>
        private SerializedProperty GetParentProperty(SerializedProperty property)
        {
            var path = property.propertyPath;
            var lastDotIndex = path.LastIndexOf('.');
            if (lastDotIndex < 0) return null;

            var parentPath = path.Substring(0, lastDotIndex);
            return property.serializedObject.FindProperty(parentPath);
        }
        /// <summary>
        /// Uses reflection to fetch a field value by name.
        /// </summary>
        private object GetFieldValueByName(SerializedProperty parentProperty, string fieldName)
        {
            if (parentProperty == null || parentProperty.managedReferenceValue == null)
                return null;

            var parentObject = parentProperty.managedReferenceValue;
            var type = parentObject.GetType();
            
            var field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                return field.GetValue(parentObject);
            }

            return null;
        }
    }
}
#endif
