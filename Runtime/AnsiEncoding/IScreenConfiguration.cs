namespace HamerSoft.PuniTY.AnsiEncoding
{
    public readonly struct ScreenDimensions
    {
        public readonly int Rows;
        public readonly int Columns;

        public ScreenDimensions(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        internal bool IsValid => Rows >= 1 && Columns >= 1;
    }

    public readonly struct FontDimensions
    {
        public readonly int Width;
        public readonly int Height;

        public FontDimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }

        internal bool IsValid => Width > 0 && Height > 0;
    }

    public interface IScreenConfiguration
    {
        public int TabStopSize { get; }
        public ScreenDimensions ScreenDimensions { get; }
        public FontDimensions FontDimensions { get; }
        internal bool IsValid => ScreenDimensions.IsValid && FontDimensions.IsValid;
    }
}