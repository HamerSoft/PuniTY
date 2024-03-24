using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public interface IScreen
    {
        public int Width { get; }
        public int Height { get; }
        public ICursor Cursor { get; }

        public void SetCursorPosition(Vector2Int position);
        public void MoveCursor(int cells, Direction direction);
    }
}