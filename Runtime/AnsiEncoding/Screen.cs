using System;
using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    /// <summary>
    /// VT100 screen
    /// <remarks>Origin is top-left with value (column) 1, (row) 1</remarks>
    /// </summary>
    public class Screen : IScreen
    {
        public int Width { get; }
        public int Height { get; }
        public ICursor Cursor { get; }

        public Screen(int width, int height, ICursor cursor)
        {
            Width = width;
            Height = height;
            Cursor = cursor;
            Cursor.SetPosition(new Vector2Int(1, 1));
        }

        public void SetCursorPosition(Vector2Int position)
        {
            if (position.x >= 1
                && position.x <= Width
                && position.y >= 1
                && position.y <= Height)
            {
                Cursor.SetPosition(position);
            }
        }

        public void MoveCursor(int cells, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    SetCursorPosition(new Vector2Int(Cursor.Position.x, Cursor.Position.y + cells));
                    break;
                case Direction.Down:
                    SetCursorPosition(new Vector2Int(Cursor.Position.x, Cursor.Position.y - cells));
                    break;
                case Direction.Forward:
                    SetCursorPosition(new Vector2Int(Cursor.Position.x + cells, Cursor.Position.y));
                    break;
                case Direction.Back:
                    SetCursorPosition(new Vector2Int(Cursor.Position.x - cells, Cursor.Position.y));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}