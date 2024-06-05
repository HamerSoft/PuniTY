using UnityEngine;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace HamerSoft.PuniTY.AnsiEncoding.ColorScheme
{
    public abstract class ColorScheme : IColorScheme
    {
        public abstract Color Black { get; }
        public abstract Color Red { get; }
        public abstract Color Green { get; }
        public abstract Color Yellow { get; }
        public abstract Color Blue { get; }
        public abstract Color Magenta { get; }
        public abstract Color Cyan { get; }
        public abstract Color White { get; }
        public abstract Color BrightBlack { get; }
        public abstract Color BrightRed { get; }
        public abstract Color BrightGreen { get; }
        public abstract Color BrightYellow { get; }
        public abstract Color BrightBlue { get; }
        public abstract Color BrightMagenta { get; }
        public abstract Color BrightCyan { get; }
        public abstract Color BrightWhite { get; }
        
        public Color GetColor(AnsiColor color, ILogger logger, int?[] customColor = null)
        {
            switch (color)
            {
                case AnsiColor.Black:
                    return Black;
                case AnsiColor.Red:
                    return Red;
                case AnsiColor.Green:
                    return Green;
                case AnsiColor.Yellow:
                    return Yellow;
                case AnsiColor.Blue:
                    return Blue;
                case AnsiColor.Magenta:
                    return Magenta;
                case AnsiColor.Cyan:
                    return Cyan;
                case AnsiColor.White:
                    return White;
                case AnsiColor.BrightBlack:
                    return BrightBlack;
                case AnsiColor.BrightRed:
                    return BrightRed;
                case AnsiColor.BrightGreen:
                    return BrightGreen;
                case AnsiColor.BrightYellow:
                    return BrightYellow;
                case AnsiColor.BrightBlue:
                    return BrightBlue;
                case AnsiColor.BrightMagenta:
                    return BrightMagenta;
                case AnsiColor.BrightCyan:
                    return BrightCyan;
                case AnsiColor.BrightWhite:
                    return BrightWhite;
                case AnsiColor.Rgb:
                    return new Color(customColor[0].Value, customColor[1].Value, customColor[3].Value);
            }

            logger.LogWarning($"Color {color} cannot be found in AnsiColor enum. Returning Black...");
            return Black;
        }
    }
}