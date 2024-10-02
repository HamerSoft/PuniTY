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
    }

    public interface IScreenConfiguration
    {
        public int TabStopSize { get; }
        public ScreenDimensions ScreenDimensions { get; }
    }
}