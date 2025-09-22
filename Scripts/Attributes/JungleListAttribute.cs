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
        public string ListTitle { get; }
        public string EmptyMessage { get; }
        public string UIElementName { get; }

        /// <summary>
        /// Creates a custom list attribute
        /// </summary>
        /// <param name="listTitle">Title to display above the list</param>
        /// <param name="emptyMessage">Message to show when list is empty</param>
        /// <param name="uiElementName">Name of the UI element in UXML to populate (optional, will be auto-generated from field name if not provided)</param>
        public JungleListAttribute(string listTitle, string emptyMessage = "No items", string uiElementName = null)
        {
            ListTitle = listTitle;
            EmptyMessage = emptyMessage;
            UIElementName = uiElementName;
        }
    }
}
