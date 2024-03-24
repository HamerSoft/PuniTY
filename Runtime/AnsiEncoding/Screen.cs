using System;
using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding
{
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
        }

        public void SetCursorPosition(Vector2Int position)
        {
            if (position.x >= 0
                && position.x < Width
                && position.y >= 0
                && position.y < Height)
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