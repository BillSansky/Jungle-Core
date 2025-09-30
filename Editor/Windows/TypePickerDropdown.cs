
using System;
using System.Collections.Generic;
using System.Linq;
using Jungle.Attributes;
using Jungle.Editor; 
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

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

        // Group by JungleInfo.Category if present; otherwise by namespace
        var groups = types.GroupBy(t =>
        {
            var m = GetMeta(t);
            return string.IsNullOrEmpty(m.Category) ? (t.Namespace ?? "Global") : m.Category;
        }).OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase);

        foreach (var group in groups)
        {
            var groupItem = new AdvancedDropdownItem(group.Key);
            root.AddChild(groupItem);

            foreach (var t in group.OrderBy(t => GetMeta(t).Display, StringComparer.OrdinalIgnoreCase))
            {
                var meta = GetMeta(t);

                // Show description inline (tooltips don’t show in AdvancedDropdown)
                var label = string.IsNullOrEmpty(meta.Description)
                    ? meta.Display
                    : $"{meta.Display} — {meta.Description}";

                // Optional: keep it compact to avoid clipping (tweak maxLen if needed)
                label = TruncateSingleLine(label, 96);

                var item = new TypeItem(label, t);
                if (meta.Icon != null) item.icon = meta.Icon;

                groupItem.AddChild(item);
            }
        }

        return root;
    }

    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        if (item is TypeItem ti)
            onPicked?.Invoke(ti.Type);
    }

    private sealed class TypeItem : AdvancedDropdownItem
    {
        public readonly Type Type;

        public TypeItem(string name, Type type) : base(name)
        {
            Type = type;
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
}