using UnityEditor.UIElements;
using UnityEngine;

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

        public Position((int, int) position)
        {
            Row = position.Item1;
            Column = position.Item2;
        }

        public Position(Vector2Int position)
        {
            Row = position.y;
            Column = position.x;
        }

        internal bool IsValid(IScreen screen)
        {
            if (screen == null)
                return false;

            return Row >= 1
                   && Column >= 1
                   && Row <= screen.Rows
                   && Column <= screen.Columns;
        }

        public Position WithRow(int row)
        {
            return new Position(row, Column);
        }

        public Position WithColumn(int column)
        {
            return new Position(Row, column);
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

        public static bool operator >(Position a, object b) => b is Position other &&
                                                               (a.Row > other.Row || a.Row == other.Row &&
                                                                   a.Column > other.Column);

        public static bool operator <(Position a, object b) => b is Position other &&
                                                               (a.Row < other.Row || a.Row == other.Row &&
                                                                   a.Column < other.Column);

        public static bool operator >=(Position a, object b) => b is Position other &&
                                                                (a.Row > other.Row || a.Row == other.Row &&
                                                                    a.Column >= other.Column);

        public static bool operator <=(Position a, object b) => b is Position other &&
                                                                (a.Row < other.Row || a.Row == other.Row &&
                                                                    a.Column <= other.Column);
    }
}