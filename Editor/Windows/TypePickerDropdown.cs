
using System;
using System.Collections.Generic;
using System.Linq;
using Jungle.Attributes;
using Jungle.Editor; 
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

/// <summary>
/// Editor dropdown that lists concrete subclasses of a requested base type using
/// <see cref="JungleClassInfoAttribute"/> metadata for naming and grouping.
/// </summary>
public class TypePickerDropdown : AdvancedDropdown
{
    private readonly Type baseType;
    private readonly Action<Type> onPicked;

    private static readonly Dictionary<Type, Meta> MetaCache = new();

    private struct Meta
    {
        public string Display;
        public string Description;
        public string Category;
        public Texture2D Icon;
    }

    private TypePickerDropdown(Type baseType, Action<Type> onPicked)
        : base(new AdvancedDropdownState())
    {
        this.baseType = baseType;
        this.onPicked = onPicked;

        // Wider so description isn’t immediately clipped
        minimumSize = new Vector2(520, 540);
    }

    public static void Show(Rect anchorWorldRect, Type baseType, Action<Type> onPicked)
    {
        var dd = new TypePickerDropdown(baseType, onPicked);
        dd.Show(anchorWorldRect);
    }

    protected override AdvancedDropdownItem BuildRoot()
    {
        var root = new AdvancedDropdownItem(baseType.Name);
        root.AddChild(new TypeItem("(None)", null));

        var types = TypeCache.GetTypesDerivedFrom(baseType)
            .Where(t => !t.IsAbstract && !t.IsGenericType && t.GetConstructor(Type.EmptyTypes) != null)
            .OrderBy(t => t.FullName)
            .ToList();

        var categoryTree = BuildCategoryTree(types);

        AddTypesToDropdown(root, categoryTree.Types);
        BuildDropdownFromNode(root, categoryTree);

        return root;
    }

    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        if (item is TypeItem ti)
            onPicked?.Invoke(ti.Type);
    }

    /// <summary>
    /// Dropdown item that retains the <see cref="Type"/> represented by the row.
    /// </summary>
    private sealed class TypeItem : AdvancedDropdownItem
    {
        public readonly Type Type;

        public TypeItem(string name, Type type) : base(name)
        {
            Type = type;
        }
    }

    private static void AddTypesToDropdown(AdvancedDropdownItem parent, IEnumerable<Type> types)
    {
        var orderedTypes = types
            .OrderBy(t => GetMeta(t).Display, StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var type in orderedTypes)
        {
            var meta = GetMeta(type);

            var label = string.IsNullOrEmpty(meta.Description)
                ? meta.Display
                : $"{meta.Display} — {meta.Description}";

            label = TruncateSingleLine(label, 96);

            var item = new TypeItem(label, type);
            if (meta.Icon != null) item.icon = meta.Icon;

            parent.AddChild(item);
        }
    }

    private static void BuildDropdownFromNode(AdvancedDropdownItem parent, CategoryNode node)
    {
        var orderedChildren = node.Children.Values
            .OrderBy(child => child.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var child in orderedChildren)
        {
            if (!child.HasContent)
            {
                continue;
            }

            var groupItem = new AdvancedDropdownItem(child.Name);
            AddTypesToDropdown(groupItem, child.Types);
            BuildDropdownFromNode(groupItem, child);

            if (groupItem.children.Count > 0)
            {
                parent.AddChild(groupItem);
            }
        }
    }

    private static Meta GetMeta(Type t)
    {
        if (MetaCache.TryGetValue(t, out var m)) return m;

        string display = t.Name;
        string desc = null;
        string category = null;
        Texture2D icon = null;

        var info = (JungleClassInfoAttribute)Attribute.GetCustomAttribute(t, typeof(JungleClassInfoAttribute));
        if (info != null)
        {
            if (!string.IsNullOrWhiteSpace(info.DisplayName)) display = info.DisplayName;
            if (!string.IsNullOrWhiteSpace(info.Description)) desc = info.Description;
            if (!string.IsNullOrWhiteSpace(info.Category)) category = info.Category;

            if (!string.IsNullOrWhiteSpace(info.IconPathOrKey))
            {
                icon = IconLookup.ForKeyOrPath(info.IconPathOrKey);
            }
        }

        if (icon == null)
            icon = IconLookup.ForType(t);

        m = new Meta { Display = display, Description = desc, Category = category, Icon = icon };
        MetaCache[t] = m;
        return m;
    }
    
    private static string TruncateSingleLine(string s, int maxChars)
    {
        if (string.IsNullOrEmpty(s) || s.Length <= maxChars) return s;
        return s.Substring(0, Math.Max(0, maxChars - 1)) + "…";
    }

    private static CategoryNode BuildCategoryTree(IEnumerable<Type> types)
    {
        var root = new CategoryNode(null);

        foreach (var type in types)
        {
            var meta = GetMeta(type);
            var categoryPath = string.IsNullOrWhiteSpace(meta.Category)
                ? (type.Namespace ?? "Global")
                : meta.Category;

            var segments = categoryPath
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(segment => segment.Trim())
                .Where(segment => !string.IsNullOrEmpty(segment))
                .ToList();

            if (segments.Count == 0)
            {
                segments.Add("General");
            }

            var current = root;
            foreach (var segment in segments)
            {
                current = current.GetOrAddChild(segment);
            }

            current.Types.Add(type);
        }

        return root;
    }

    private sealed class CategoryNode
    {
        public string Name { get; }
        public Dictionary<string, CategoryNode> Children { get; }
        public List<Type> Types { get; }

        public bool HasContent => Types.Count > 0 || Children.Count > 0;

        public CategoryNode(string name)
        {
            Name = name;
            Children = new Dictionary<string, CategoryNode>(StringComparer.OrdinalIgnoreCase);
            Types = new List<Type>();
        }

        public CategoryNode GetOrAddChild(string name)
        {
            if (!Children.TryGetValue(name, out var child))
            {
                child = new CategoryNode(name);
                Children[name] = child;
            }

            return child;
        }
    }
}