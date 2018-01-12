using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using Shared.Helpers.Attributes;
using DescriptionAttribute = Shared.Helpers.Attributes.DescriptionAttribute;

namespace Shared.Helpers.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool HasAttribute<T>(this PropertyInfo This)
            => This.GetCustomAttributes().Any(x => x is T);

        public static bool IsId(this PropertyInfo This)
        {
            var attr = This.GetCustomAttribute<IdAttribute>();
            return attr != null && attr.Id || attr == null && IdAttribute.Default.Id;
        }

        public static bool IsTitle(this PropertyInfo This)
        {
            var attr = This.GetCustomAttribute<TitleAttribute>();
            return attr != null && attr.Title || attr == null && TitleAttribute.Default.Title;
        }

        public static bool IsSubtitle(this PropertyInfo This)
        {
            var attr = This.GetCustomAttribute<SubtitleAttribute>();
            return attr != null && attr.Subtitle || attr == null && SubtitleAttribute.Default.Subtitle;
        }

        public static bool IsDescription(this PropertyInfo This)
        {
            var attr = This.GetCustomAttribute<DescriptionAttribute>();
            return attr != null && attr.Description || attr == null && DescriptionAttribute.Default.Description;
        }

        public static bool IsPicture(this PropertyInfo This)
        {
            var attr = This.GetCustomAttribute<PictureAttribute>();
            return attr != null && attr.Picture || attr == null && PictureAttribute.Default.Picture;
        }

        public static bool IsBrowsable(this PropertyInfo This)
        {
            var attr = This.GetCustomAttribute<BrowsableAttribute>();
            return attr != null && attr.Browsable || attr == null && BrowsableAttribute.Default.Browsable;
        }

        public static bool HasNativeType(this PropertyInfo This)
            => This.PropertyType.IsNativeType();

        public static bool HasImageType(this PropertyInfo This)
            => typeof(BitmapImage).IsAssignableFrom(This.PropertyType);

        public static string GetDisplayName(this PropertyInfo This)
        {
            var displayName = This.GetCustomAttribute<DisplayNameAttribute>();
            return displayName != null
                ? displayName.DisplayName
                : This.Name;
        }
    }
}