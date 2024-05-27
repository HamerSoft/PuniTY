using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding.ColorScheme
{
    public readonly struct UbuntuColorScheme : IColorScheme
    {
        public Color Black => Color.black;
        public Color Red => new Color(222, 56, 43);
        public Color Green => new Color(57, 181, 74);
        public Color Yellow => new Color(255, 199, 6);
        public Color Blue => new Color(0, 111, 184);
        public Color Magenta => new Color(118, 38, 113);
        public Color Cyan => new Color(44, 181, 233);
        public Color White => new Color(204, 204, 204);
        public Color BrightBlack => new Color(128, 128, 128);
        public Color BrightRed => new Color(255, 0, 0);
        public Color BrightGreen => new Color(0, 255, 0);
        public Color BrightYellow => new Color(255, 255, 0);
        public Color BrightBlue => new Color(0, 0, 255);
        public Color BrightMagenta => new Color(255, 0, 255);
        public Color BrightCyan => new Color(0, 255, 255);
        public Color BrightWhite => new Color(255, 255, 255);
    }
}