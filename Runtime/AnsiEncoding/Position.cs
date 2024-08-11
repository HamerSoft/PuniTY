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

        public Position AddColumns(IScreen screen, int columnsToAdd)
        {
            int rowsToAdd = columnsToAdd / screen.Columns;
            columnsToAdd -= rowsToAdd * screen.Columns;
            return new Position(Row + rowsToAdd, Column + columnsToAdd);
        }

        public override string ToString()
        {
            return $"Position(row:{Row}, column:{Column})";
        }

        public static bool operator ==(Position a, object b) => a.Equals(b);
        public static bool operator !=(Position a, object b) => !(a == b);
    }
}