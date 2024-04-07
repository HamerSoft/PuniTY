namespace HamerSoft.PuniTY.AnsiEncoding
{
    public readonly struct Position
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        internal bool IsValid(IScreen screen)
        {
            return Row >= 1
                   && Column >= 1
                   && Row <= screen.Rows
                   && Column <= screen.Columns;
        }
    }
}