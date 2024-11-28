namespace HamerSoft.PuniTY.AnsiEncoding
{
    public enum CursorMode
    {
        BlinkingBlock = 0,
        SteadyBlock = 1,
        BlinkingUnderline = 2,
        SteadyUnderLine = 3,
        BlinkingBar = 4,
        SteadyBar = 5
    }

    public interface ICursor
    {
        public Position Position { get; }
        internal void SetPosition(Position position);
    }
}