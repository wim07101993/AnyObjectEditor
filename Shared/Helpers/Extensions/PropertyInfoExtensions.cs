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
            return attr != null && attr.IsId || attr == null && IdAttribute.Default.IsId;
        }

        public static bool IsTitle(this PropertyInfo This)
        {
            var attr = This.GetCustomAttribute<TitleAttribute>();
            return attr != null && attr.IsTitle || attr == null && TitleAttribute.Default.IsTitle;
        }

        public static bool IsSubtitle(this PropertyInfo This)
        {
            var attr = This.GetCustomAttribute<SubtitleAttribute>();
            return attr != null && attr.IsSubtitle || attr == null && SubtitleAttribute.Default.IsSubtitle;
        }

        public static bool IsDescription(this PropertyInfo This)
        {
            var attr = This.GetCustomAttribute<DescriptionAttribute>();
            return attr != null && attr.IsDescription || attr == null && DescriptionAttribute.Default.IsDescription;
        }

        public static bool IsPicture(this PropertyInfo This)
        {
            var attr = This.GetCustomAttribute<PictureAttribute>();
            return attr != null && attr.IsPicture || attr == null && PictureAttribute.Default.IsPicture;
        }

        public static bool IsBrowsable(this PropertyInfo This)
        {
            var attr = This.GetCustomAttribute<BrowsableAttribute>();
            return attr != null && attr.IsBrowsable || attr == null && BrowsableAttribute.Default.IsBrowsable;
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