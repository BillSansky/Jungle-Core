using System;

namespace Jungle.Attributes
{
    /// <summary>
    /// Attribute to provide metadata for any type including description and optional visual for contextual menus
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Enum, AllowMultiple = false,
        Inherited = true)]
    [Serializable]
    public class JungleInfoAttribute : Attribute
    {
        public string Description { get; }
        public string IconPath { get; }
        public string Category { get; }

        /// <summary>
        /// Creates an ActionInfo attribute with description only
        /// </summary>
        /// <param name="description">Description of the element</param>
        public JungleInfoAttribute(string description)
        {
            Description = description;
            IconPath = null;
        }

        /// <summary>
        /// Creates an ActionInfo attribute with description and icon
        /// </summary>
        /// <param name="description">Description of the element</param>
        /// <param name="iconPath">Path to the icon resource (can be Resources path or builtin icon name)</param>
        public JungleInfoAttribute(string description, string iconPath)
        {
            Description = description;
            IconPath = iconPath;
            Category = "General";
        }

        /// <summary>
        /// Creates an ActionInfo attribute with description, icon and category
        /// </summary>
        /// <param name="description">Description of the element</param>
        /// <param name="iconPath">Path to the icon resource (can be Resources path or builtin icon name)</param>
        /// <param name="category">Category to group similar types together</param>
        public JungleInfoAttribute(string description, string iconPath, string category)
        {
            Description = description;
            IconPath = iconPath;
            Category = category ?? "General";
        }
    }
}