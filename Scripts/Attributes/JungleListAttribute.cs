// JungleListAttribute.cs
using System;
using UnityEngine;

namespace Jungle.Attributes
{
    /// <summary>
    /// Attribute to automatically generate custom list UI for serialized lists
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class JungleListAttribute : PropertyAttribute
    {
        /// <summary>Title to display above the list</summary>
        public string ListTitle { get; }
        /// <summary>Message to show when list is empty</summary>
        public string EmptyMessage { get; }
        /// <summary>Name of the UI element in UXML to populate (optional)</summary>
        public string UIElementName { get; }

        /// <summary>
        /// Optional base/interface type used to drive the + type picker for managed-reference lists (List&lt;T&gt; where T is interface/abstract).
        /// If null, we try to infer from the field element type.
        /// </summary>
        public Type BaseType { get; }

        /// <param name="listTitle">Header title; defaults to nicified field name when null/empty</param>
        /// <param name="emptyMessage">Empty-state text</param>
        /// <param name="uiElementName">Optional UXML element name to mount into</param>
        /// <param name="baseType">Optional base/interface type for the + picker on managed-reference lists</param>
        public JungleListAttribute(
            string listTitle = null,
            string emptyMessage = "No items",
            string uiElementName = null,
            Type baseType = null)
        {
            ListTitle = listTitle;
            EmptyMessage = emptyMessage;
            UIElementName = uiElementName;
            BaseType = baseType;
        }
    }
}