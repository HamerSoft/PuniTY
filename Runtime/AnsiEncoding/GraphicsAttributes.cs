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

    public struct RgbColor
    {
        public int R, G, B;

        public RgbColor(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public override string ToString()
        {
            return $"R:{R}, G:{G}, B:{B}";
        }
    }

    public struct GraphicAttributes
    {
        private readonly AnsiColor _foreground;
        private readonly AnsiColor _backGround;
        public bool IsBold { get; set; }

        /// <summary>
        /// also called Dimmed
        /// </summary>
        public bool IsFaint { get; set; }

        public bool IsItalic { get; set; }
        public bool IsStrikeThrough { get; set; }
        public bool IsProportionalSpaced { get; set; }
        public UnderlineMode UnderlineMode { get; set; }
        public BlinkSpeed BlinkSpeed { get; set; }
        public bool IsConcealed { get; set; }
        public AnsiColor Foreground { get; set; }
        public AnsiColor Background { get; set; }
        public RgbColor ForegroundRGBColor { get; set; }
        public RgbColor BackgroundRGBColor { get; set; }

        public GraphicAttributes(AnsiColor foreground, AnsiColor backGround)
        {
            _backGround = backGround;
            _foreground = foreground;
            IsBold = false;
            IsFaint = false;
            IsItalic = false;
            IsConcealed = false;
            IsStrikeThrough = false;
            IsProportionalSpaced = false;
            BlinkSpeed = BlinkSpeed.None;
            UnderlineMode = UnderlineMode.None;
            Foreground = _foreground;
            Background = _backGround;
            ForegroundRGBColor = default;
            BackgroundRGBColor = default;
        }

        public void Reset()
        {
            IsBold = false;
            IsFaint = false;
            IsItalic = false;
            IsConcealed = false;
            IsStrikeThrough = false;
            IsProportionalSpaced = false;
            BlinkSpeed = BlinkSpeed.None;
            UnderlineMode = UnderlineMode.None;
            Foreground = _foreground;
            Background = _backGround;
            ForegroundRGBColor = default;
            BackgroundRGBColor = default;
        }

        public override bool Equals(object obj)
        {
            return obj is GraphicAttributes other
                   && other.Background == Background
                   && other.Foreground == Foreground
                   && other.IsConcealed == IsConcealed
                   && other.IsBold == IsBold
                   && other.IsFaint == IsFaint
                   && other.IsStrikeThrough == IsStrikeThrough
                   && other.IsItalic == IsItalic
                   && other.IsProportionalSpaced == IsProportionalSpaced
                   && other.BlinkSpeed == BlinkSpeed
                   && other.UnderlineMode == UnderlineMode
                   && other.ForegroundRGBColor.Equals(ForegroundRGBColor)
                   && other.BackgroundRGBColor.Equals(BackgroundRGBColor);
        }

        public override int GetHashCode()
        {
            return (BlinkSpeed, Foreground, IsConcealed, IsBold, IsFaint, IsStrikeThrough, IsItalic,
                IsProportionalSpaced, BlinkSpeed, UnderlineMode, ForegroundRGBColor, BackgroundRGBColor).GetHashCode();
        }
    }
}