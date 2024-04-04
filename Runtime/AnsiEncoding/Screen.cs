using System;
using System.Collections.Generic;
using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    /// <summary>
    /// VT100 screen
    /// <remarks>Origin is top-left with value (column) 1, (row) 1</remarks>
    /// </summary>
    public class Screen : IScreen
    {
        public int Rows { get; }
        public int Columns { get; }
        public ICursor Cursor { get; }
        private List<List<ICharacter>> _characters;

        public Screen(int rows, int columns, ICursor cursor)
        {
            Rows = rows;
            Columns = columns;
            Cursor = cursor;
            Cursor.SetPosition(new Position(1, 1));
            PopulateCharacters(rows, columns);
        }

        private void PopulateCharacters(int rows, int columns)
        {
            _characters = new List<List<ICharacter>>(rows);
            for (var i = 0; i < rows; i++)
            {
                var column = new List<ICharacter>(columns);
                for (var j = 0; j < columns; j++)
                    column.Add(new Character());
                _characters.Add(column);
            }
        }

        public void SetCursorPosition(Position position)
        {
            if (IsValidPosition(position))
            {
                Cursor.SetPosition(position);
            }
        }

        private bool IsValidPosition(Position position)
        {
            return position.Row >= 1
                   && position.Row <= Rows
                   && position.Column >= 1
                   && position.Column <= Columns;
        }

        public ICharacter Character(Position position)
        {
            var isValid = IsValidPosition(position);
            if (isValid)
                return _characters[position.Row - 1][position.Column - 1];

            return new InvalidCharacter();
        }

        public void SetCharacter(ICharacter character, Position position)
        {
            _characters[position.Row - 1][position.Column - 1] = character;
        }

        public void MoveCursor(int cells, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    SetCursorPosition(new Position(Cursor.Position.Row - cells, Cursor.Position.Column));
                    break;
                case Direction.Down:
                    SetCursorPosition(new Position(Cursor.Position.Row + cells, Cursor.Position.Column));
                    break;
                case Direction.Forward:
                    SetCursorPosition(new Position(Cursor.Position.Row, Cursor.Position.Column + cells));
                    break;
                case Direction.Back:
                    SetCursorPosition(new Position(Cursor.Position.Row, Cursor.Position.Column - cells));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void Erase(Position? from = null, Position? to = null)
        {
            // null == clear everything
            // true == clear to end of screen
            // false == clear to beginning of screen
            bool? clearingForward = from == null ? true : (to == null ? false : null);
            from ??= new Position(1, 1);
            to ??= new Position(Rows, Columns);

            if (clearingForward.HasValue == false)
            {
                for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    _characters[i][j] = new Character(' ');
            }
            else if (clearingForward.Value)
            {
                for (int i = from.Value.Row - 1; i <= to.Value.Row - 1; i++)
                {
                    if (i < to.Value.Row - 1)
                        for (int j = 0; j < Columns; j++)
                        {
                            _characters[i][j] = new Character(' ');
                        }
                    else
                        for (int j = 0; j < to.Value.Column; j++)
                        {
                            _characters[i][j] = new Character(' ');
                        }
                }
            }
            else
            {
               // fix me
            }
        }

        public void ClearSaved()
        {
            throw new NotImplementedException();
        }
    }
}