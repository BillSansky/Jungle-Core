// JungleClassInfoAttribute.cs  (additions only)
using System;

namespace Jungle.Attributes
{
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Enum, AllowMultiple = false,
        Inherited = true)]
    [Serializable]
    public class JungleClassInfoAttribute : Attribute
    {
        public string Description { get; }
        public string IconPathOrKey { get; }
        public string Category { get; }
        public string DisplayName { get; }   // <-- NEW

        public JungleClassInfoAttribute(string description)
        {
            Description = description;
            IconPathOrKey = null;
            Category = "General";
        }

        public JungleClassInfoAttribute(string description, string iconPathOrKey)
        {
            Description = description;
            IconPathOrKey = iconPathOrKey;
            Category = "General";
        }

        public JungleClassInfoAttribute(string description, string iconPathOrKey, string category)
        {
            Description = description;
            IconPathOrKey = iconPathOrKey;
            Category = category ?? "General";
        }

        // NEW: lets you override the name shown in the picker (optional)
        public JungleClassInfoAttribute(string displayName, string description, string iconPathOrKey, string category = "General", bool isDisplayName = true)
        {
            DisplayName = displayName;
            Description = description;
            IconPathOrKey = iconPathOrKey;
            Category = string.IsNullOrEmpty(category) ? "General" : category;
        }
    }
}