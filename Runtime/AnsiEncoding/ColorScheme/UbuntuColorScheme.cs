using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding.ColorScheme
{
    public class UbuntuColorScheme : ColorScheme
    {
        public override Color Black => Color.black;
        public override Color Red => new Color(222, 56, 43);
        public override Color Green => new Color(57, 181, 74);
        public override Color Yellow => new Color(255, 199, 6);
        public override Color Blue => new Color(0, 111, 184);
        public override Color Magenta => new Color(118, 38, 113);
        public override Color Cyan => new Color(44, 181, 233);
        public override Color White => new Color(204, 204, 204);
        public override Color BrightBlack => new Color(128, 128, 128);
        public override Color BrightRed => new Color(255, 0, 0);
        public override Color BrightGreen => new Color(0, 255, 0);
        public override Color BrightYellow => new Color(255, 255, 0);
        public override Color BrightBlue => new Color(0, 0, 255);
        public override Color BrightMagenta => new Color(255, 0, 255);
        public override Color BrightCyan => new Color(0, 255, 255);
        public override Color BrightWhite => new Color(255, 255, 255);
    }
}