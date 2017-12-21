﻿using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DatabaseManager.Helpers.Attributes;
using DescriptionAttribute = DatabaseManager.Helpers.Attributes.DescriptionAttribute;

namespace DatabaseManager.Helpers.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool HasAttribute<T>(this PropertyInfo This)
            => This.GetCustomAttributes().Any(x => x is T);

        public static bool IsTitle(this PropertyInfo This)
            => This.HasAttribute<TitleAttribute>() || TitleAttribute.Default.Title;

        public static bool IsSubtitle(this PropertyInfo This)
            => This.HasAttribute<SubtitleAttribute>() || SubtitleAttribute.Default.Subtitle;

        public static bool IsDescription(this PropertyInfo This)
            => This.HasAttribute<DescriptionAttribute>() || DescriptionAttribute.Default.Description;

        public static bool IsPicture(this PropertyInfo This)
            => This.HasAttribute<PictureAttribute>() || PictureAttribute.Default.Picture;

        public static bool IsBrowsable(this PropertyInfo This)
            => This.HasAttribute<BrowsableAttribute>() || BrowsableAttribute.Default.Browsable;

        public static string GetDisplayName(this PropertyInfo This)
        {
            var displayName = This.GetCustomAttribute<DisplayNameAttribute>();
            return displayName != null
                ? displayName.DisplayName
                : This.Name;
        }
    }
}