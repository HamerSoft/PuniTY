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

    public readonly struct RgbColor
    {
        public readonly int R;
        public readonly int G;
        public readonly int B;

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

        public static bool operator ==(RgbColor a, object b) => a.Equals(b);
        public static bool operator !=(RgbColor a, object b) => !(a == b);
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
        public RgbColor UnderLineColorRGBColor { get; set; }
        public RgbColor BackgroundRGBColor { get; set; }
        public bool IsFramed { get; set; }
        public bool IsEncircled { get; set; }
        public bool IsOverLined { get; set; }

        public GraphicAttributes(AnsiColor foreground, AnsiColor backGround)
        {
            _backGround = backGround;
            _foreground = foreground;
            IsBold = false;
            IsFaint = false;
            IsItalic = false;
            IsFramed = false;
            IsEncircled = false;
            IsConcealed = false;
            IsOverLined = false;
            IsStrikeThrough = false;
            IsProportionalSpaced = false;
            BlinkSpeed = BlinkSpeed.None;
            UnderlineMode = UnderlineMode.None;
            Foreground = _foreground;
            Background = _backGround;
            ForegroundRGBColor = default;
            UnderLineColorRGBColor = default;
            BackgroundRGBColor = default;
        }

        public void Reset()
        {
            IsBold = false;
            IsFaint = false;
            IsItalic = false;
            IsFramed = false;
            IsEncircled = false;
            IsConcealed = false;
            IsOverLined = false;
            IsStrikeThrough = false;
            IsProportionalSpaced = false;
            BlinkSpeed = BlinkSpeed.None;
            UnderlineMode = UnderlineMode.None;
            Foreground = _foreground;
            Background = _backGround;
            ForegroundRGBColor = default;
            BackgroundRGBColor = default;
            UnderLineColorRGBColor = default;
        }

        public override bool Equals(object obj)
        {
            return obj is GraphicAttributes other
                   && other.Background == Background
                   && other.Foreground == Foreground
                   && other.IsConcealed == IsConcealed
                   && other.IsEncircled == IsEncircled
                   && other.IsOverLined == IsOverLined
                   && other.IsBold == IsBold
                   && other.IsFaint == IsFaint
                   && other.IsFramed == IsFramed
                   && other.IsStrikeThrough == IsStrikeThrough
                   && other.IsItalic == IsItalic
                   && other.IsProportionalSpaced == IsProportionalSpaced
                   && other.BlinkSpeed == BlinkSpeed
                   && other.UnderlineMode == UnderlineMode
                   && other.ForegroundRGBColor.Equals(ForegroundRGBColor)
                   && other.UnderLineColorRGBColor.Equals(UnderLineColorRGBColor)
                   && other.BackgroundRGBColor.Equals(BackgroundRGBColor);
        }

        public override int GetHashCode()
        {
            return (BlinkSpeed,
                Foreground,
                IsConcealed,
                IsEncircled,
                IsOverLined,
                IsBold,
                IsFaint,
                IsFramed,
                IsStrikeThrough,
                IsItalic,
                IsProportionalSpaced,
                BlinkSpeed,
                UnderlineMode,
                ForegroundRGBColor,
                UnderLineColorRGBColor,
                BackgroundRGBColor).GetHashCode();
        }
    }
}