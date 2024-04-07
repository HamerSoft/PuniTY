using System;
using System.Collections.Generic;
using System.Linq;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    /// <summary>
    /// VT100 screen
    /// <remarks>Origin is top-left with value (column) 1, (row) 1</remarks>
    /// </summary>
    public class Screen : IScreen
    {
        private readonly ILogger _logger;
        public int Rows { get; }
        public int Columns { get; }
        public ICursor Cursor { get; }
        private List<List<ICharacter>> _characters;
        private int _rowOffset;

        public Screen(int rows, int columns, ICursor cursor, ILogger logger)
        {
            _logger = logger;
            Rows = rows;
            Columns = columns;
            Cursor = cursor;
            Cursor.SetPosition(new Position(1, 1));
            _rowOffset = 0;
            _characters = new List<List<ICharacter>>(rows) { new(GenerateNewRow()) };
        }

        public void SetCursorPosition(Position position)
        {
            if (position.IsValid(this))
                Cursor.SetPosition(position);
        }

        public ICharacter GetCharacter(Position position)
        {
            return position.IsValid(this)
                ? _characters[position.Row + _rowOffset - 1][position.Column - 1]
                : new InvalidCharacter();
        }

        public void AddCharacter(char character)
        {
            switch (character)
            {
                case '\n':
                case '\r':
                    if (_characters.Count == Cursor.Position.Row)
                    {
                        _characters.Add(new List<ICharacter>(GenerateNewRow()));
                        _rowOffset++;
                    }

                    SetCursorPosition(new Position(Cursor.Position.Row + 1, 0));
                    break;
                default:
                    while (Cursor.Position.Row > _characters.Count)
                    {
                        _characters.Add(new List<ICharacter>(GenerateNewRow()));
                        if (Cursor.Position.Row > Rows)
                            _rowOffset++;
                    }

                    if (_characters[Cursor.Position.Row - 1].Count < Cursor.Position.Column)
                        _characters[Cursor.Position.Row - 1].Add(new Character(character));
                    else
                        _characters[Cursor.Position.Row - 1][Cursor.Position.Column - 1] = new Character(character);
                    MoveCursor(1, Direction.Forward);
                    break;
            }
        }

        private IEnumerable<ICharacter> GenerateNewRow()
        {
            return Enumerable.Repeat<ICharacter>(new Character(), Columns);
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
                    if (Cursor.Position.Column + cells <= Columns)
                        SetCursorPosition(new Position(Cursor.Position.Row, Cursor.Position.Column + cells));
                    else if (Cursor.Position.Row + 1 <= Rows)
                        SetCursorPosition(new Position(Cursor.Position.Row + 1,
                            cells));
                    else
                        _logger.LogWarning("Cannot move cursor forward.");
                    break;
                case Direction.Back:
                    if (Cursor.Position.Column - cells >= 1)
                        SetCursorPosition(new Position(Cursor.Position.Row, Cursor.Position.Column - cells));
                    else if (Cursor.Position.Row - 1 >= 1)
                        SetCursorPosition(new Position(Cursor.Position.Row - 1,
                            Columns - Math.Abs(cells - Cursor.Position.Column)));
                    else
                        _logger.LogWarning("Cannot move cursor backward.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void Erase(Position? from = null, Position? to = null)
        {
            from ??= new Position(1, 1);
            to ??= new Position(Rows, Columns);

            for (int i = from.Value.Row; i <= to.Value.Row; i++)
            for (int j = 1; j <= (i < to.Value.Row ? Columns : to.Value.Column); j++)
                if (i <= _characters.Count && j <= _characters[i - 1].Count)
                    _characters[i - 1][j - 1] = new InvalidCharacter();
        }

        public void ClearSaved()
        {
            _logger.Log("ScrollBack buffer not implemented.");
        }
    }
}