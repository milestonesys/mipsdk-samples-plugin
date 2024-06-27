using System.Windows.Media;

namespace SCToolbarPlugin.Client
{
    public static class ColorConverter
    {
        /// <summary>
        /// Converts Hex color codes for Red, Green and Blue into color names.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ConvertColorToCommonName(Color color)
        {
            string currentColorHex = color.ToString();
            switch (currentColorHex)
            {
                case "#FFFF0000":
                    return "Red";
                case "#FF008000":
                    return "Green";
                case "#FF0000FF":
                    return "Blue";
                default:
                    return "Unknown color " + currentColorHex;
            }
        }

        /// <summary>
        /// Converts System.Windows.Media.Color to System.Drawing.Color.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static System.Drawing.Color ConvertMediaColorToDrawingColor(Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
