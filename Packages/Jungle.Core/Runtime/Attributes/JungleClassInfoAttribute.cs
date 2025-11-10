// JungleClassInfoAttribute.cs  (additions only)
using System;

namespace Jungle.Attributes
{
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Enum, AllowMultiple = false,
        Inherited = true)]
    /// <summary>
    /// Annotates Jungle types with metadata used by the editor selection menus.
    /// </summary>
    [Serializable]
    public class JungleClassInfoAttribute : Attribute
    {
        /// <summary>
        /// Gets the description shown in Jungle pickers.
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// Gets the icon path or localization key.
        /// </summary>
        public string IconPathOrKey { get; }
        /// <summary>
        /// Gets the grouping category shown in pickers.
        /// </summary>
        public string Category { get; }
        /// <summary>
        /// Gets the display name shown in pickers.
        /// </summary>
        public string DisplayName { get; }   // <-- NEW
        /// <summary>
        /// Gets whether the entry should render inline in inspectors.
        /// </summary>
        public bool DrawInline { get; }
        /// <summary>
        /// Creates metadata using only a description and inline flag.
        /// </summary>

        public JungleClassInfoAttribute(string description, bool drawInline = false)
        {
            Description = description;
            IconPathOrKey = null;
            Category = "General";
            DrawInline = drawInline;
        }
        /// <summary>
        /// Creates metadata with a custom icon reference.
        /// </summary>

        public JungleClassInfoAttribute(string description, string iconPathOrKey, bool drawInline = false)
        {
            Description = description;
            IconPathOrKey = iconPathOrKey;
            Category = "General";
            DrawInline = drawInline;
        }
        /// <summary>
        /// Creates metadata with explicit icon and category values.
        /// </summary>

        public JungleClassInfoAttribute(string description, string iconPathOrKey, string category, bool drawInline = false)
        {
            Description = description;
            IconPathOrKey = iconPathOrKey;
            Category = category ?? "General";
            DrawInline = drawInline;
        }

        // NEW: lets you override the name shown in the picker (optional)
        /// <summary>
        /// Creates metadata that overrides the picker display name.
        /// </summary>
        public JungleClassInfoAttribute(string displayName, string description, string iconPathOrKey, string category = "General", bool drawInline = false, bool isDisplayName = true)
        {
            DisplayName = displayName;
            Description = description;
            IconPathOrKey = iconPathOrKey;
            Category = string.IsNullOrEmpty(category) ? "General" : category;
            DrawInline = drawInline;
        }
    }
}