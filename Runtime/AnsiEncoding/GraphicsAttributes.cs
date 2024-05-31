using System;
using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public enum BlinkSpeed
    {
        None,
        Slow,
        Rapid,
    }

    public enum UnderlineMode
    {
        None,
        Single,
        Double,
    }

    public struct GraphicAttributes
    {
        public bool IsBold { get; set; }

        /// <summary>
        /// also called Dimmed
        /// </summary>
        public bool IsFaint { get; set; }

        public bool IsItalic { get; set; }
        public UnderlineMode UnderlineMode { get; set; }
        public BlinkSpeed BlinkSpeed { get; set; }
        public bool IsConcealed { get; set; }
        public AnsiColor Foreground { get; set; }
        public AnsiColor Background { get; set; }

        public GraphicAttributes(AnsiColor foreground, AnsiColor backGround)
        {
            IsBold = false;
            IsFaint = false;
            IsItalic = false;
            IsConcealed = false;
            Foreground = foreground;
            Background = backGround;
            UnderlineMode = UnderlineMode.None;
            BlinkSpeed = BlinkSpeed.None;
        }

        public void Reset()
        {
            IsBold = false;
            IsFaint = false;
            IsFaint = false;
            UnderlineMode = UnderlineMode.None;
            BlinkSpeed = BlinkSpeed.None;
            IsConcealed = false;
            Foreground = AnsiColor.White;
            Background = AnsiColor.Black;
        }
    }
}