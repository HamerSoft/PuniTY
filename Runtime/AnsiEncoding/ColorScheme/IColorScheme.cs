using UnityEngine;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace HamerSoft.PuniTY.AnsiEncoding.ColorScheme
{
    public interface IColorScheme
    {
        public Color Black { get; }
        public Color Red { get; }
        public Color Green { get; }
        public Color Yellow { get; }
        public Color Blue { get; }
        public Color Magenta { get; }
        public Color Cyan { get; }
        public Color White { get; }
        public Color BrightBlack { get; }
        public Color BrightRed { get; }
        public Color BrightGreen { get; }
        public Color BrightYellow { get; }
        public Color BrightBlue { get; }
        public Color BrightMagenta { get; }
        public Color BrightCyan { get; }
        public Color BrightWhite { get; }

        public Color GetColor(AnsiColor color, ILogger logger, int?[] customColor);
    }
}