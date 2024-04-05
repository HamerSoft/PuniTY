﻿using System;
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
            from ??= new Position(1, 1);
            to ??= new Position(Rows, Columns);

            for (int i = from.Value.Row; i <= to.Value.Row; i++)
            for (int j = 1; j <= (i < to.Value.Row ? Columns : to.Value.Column); j++)
                _characters[i - 1][j - 1] = new Character(' ');
        }

        public void ClearSaved()
        {
            throw new NotImplementedException();
        }
    }
}