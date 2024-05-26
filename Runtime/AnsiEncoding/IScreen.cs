using System;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public interface IScreen
    {
        public event Action<byte[]> Output;
        public int Rows { get; }
        public int Columns { get; }
        public ICursor Cursor { get; }

        public void SetCursorPosition(Position position);
        public ICharacter GetCharacter(Position position);
        public void MoveCursor(int cells, Direction direction);
        public void Erase(Position? from = null, Position? to = null);
        public void ClearSaved();
        void Scroll(int lines, Direction direction);
        public void SaveCursor();
        public void RestoreCursor();
        public void SetGraphicsRendition(params GraphicRendition[] _graphicRenditions);

        internal void Transmit(byte[] data);
    }
}