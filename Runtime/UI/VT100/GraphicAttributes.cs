using System;
using UnityEngine;

namespace HamerSoft.PuniTY.UI.VT100
{
    internal struct GraphicAttributes
    {
        private Color _foregroundTextColorRgb;
        private Color _backgroundTextColorRgb;

        internal bool IsBold { get; set; }
        internal bool IsFaint { get; set; }
        internal bool IsItalic { get; set; }
        internal Underline Underline { get; set; }
        internal Blink Blink { get; set; }
        internal bool Conceal { get; set; }
        internal TextColor ForegroundTextColor { get; set; }
        internal TextColor BackgroundTextColor { get; set; }

        public Color ForegroundTextColorColor
        {
            get
            {
                if (ForegroundTextColor == TextColor.Rgb)
                    return _foregroundTextColorRgb;
                else
                    return TextColorToColor(ForegroundTextColor);
            }
            set
            {
                ForegroundTextColor = TextColor.Rgb;
                _foregroundTextColorRgb = value;
            }
        }

        public Color BackgroundTextColorColor
        {
            get
            {
                if (BackgroundTextColor == TextColor.Rgb)
                    return _backgroundTextColorRgb;
                else
                    return TextColorToColor(BackgroundTextColor);
            }
            set
            {
                BackgroundTextColor = TextColor.Rgb;
                _backgroundTextColorRgb = value;
            }
        }

        internal Color TextColorToColor(TextColor _textColor)
        {
            switch (_textColor)
            {
                case TextColor.Black:
                    return Color.black;
                case TextColor.Red:
                    return Color.red;
                case TextColor.Green:
                    return Color.green;
                case TextColor.Yellow:
                    return Color.yellow;
                case TextColor.Blue:
                    return Color.blue;
                case TextColor.Magenta:
                    return Color.magenta;
                case TextColor.Cyan:
                    return Color.cyan;
                case TextColor.White:
                    return Color.white;
                case TextColor.BrightBlack:
                    return Color.gray;
                case TextColor.BrightRed:
                    return Color.red; // todo fix this and following colors
                case TextColor.BrightGreen:
                    return Color.green; // <- this 
                case TextColor.BrightYellow:
                    return Color.yellow; // <- this
                case TextColor.BrightBlue:
                    return Color.blue; // <- this
                case TextColor.BrightMagenta:
                    return Color.magenta; // <- this
                case TextColor.BrightCyan:
                    return Color.cyan; // <- this
                case TextColor.BrightWhite:
                    return Color.gray;
            }

            throw new ArgumentOutOfRangeException("_textColor", "Unknown color value.");
            return Color.clear;
        }

        public void Reset()
        {
            IsBold = false;
            IsFaint = false;
            IsItalic = false;
            Underline = Underline.None;
            Blink = Blink.None;
            Conceal = false;
            ForegroundTextColor = TextColor.White;
            BackgroundTextColor = TextColor.Black;
        }
    }
}