using System.Windows.Media;

namespace Shared.Helpers.Extensions
{
    public static class ColorExtensions
    {
        public static Color ToMediaColor(this System.Drawing.Color This)
            => new Color {A = This.A, R = This.R, G = This.G, B = This.G};

        public static System.Drawing.Color ToDrawingColor(this Color This)
            => System.Drawing.Color.FromArgb(This.A, This.R, This.G, This.B);
    }
}