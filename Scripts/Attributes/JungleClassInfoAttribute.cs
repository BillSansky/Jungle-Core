// JungleClassInfoAttribute.cs  (additions only)
using System;

namespace Jungle.Attributes
{
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Enum, AllowMultiple = false,
        Inherited = true)]
    /// <summary>
    /// Attribute that stores metadata about Jungle classes for use in tooling.
    /// </summary>
    [Serializable]
    public class JungleClassInfoAttribute : Attribute
    {
        public string Description { get; }
        public string IconPathOrKey { get; }
        public string Category { get; }
        public string DisplayName { get; }   // <-- NEW
        public bool DrawInline { get; }

        public JungleClassInfoAttribute(string description, bool drawInline = false)
        {
            Description = description;
            IconPathOrKey = null;
            Category = "General";
            DrawInline = drawInline;
        }

        public JungleClassInfoAttribute(string description, string iconPathOrKey, bool drawInline = false)
        {
            Description = description;
            IconPathOrKey = iconPathOrKey;
            Category = "General";
            DrawInline = drawInline;
        }

        public JungleClassInfoAttribute(string description, string iconPathOrKey, string category, bool drawInline = false)
        {
            Description = description;
            IconPathOrKey = iconPathOrKey;
            Category = category ?? "General";
            DrawInline = drawInline;
        }

        // NEW: lets you override the name shown in the picker (optional)
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