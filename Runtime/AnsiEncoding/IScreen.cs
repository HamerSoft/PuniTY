using System;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public struct GraphicsPair
    {
        public int?[] Color;
        public GraphicRendition GraphicRendition;
    }

    public interface IScreen
    {
        public event Action<byte[]> Output;
        public int Rows { get; }
        public int Columns { get; }
        public ICursor Cursor { get; }
        internal IScreenConfiguration ScreenConfiguration { get; }

        public void SetCursorPosition(Position position);
        public void AddCharacter(char character);
        public ICharacter GetCharacter(Position position);
        public void MoveCursor(int cells, Direction direction);
        public void Erase(Position? from = null, Position? to = null);
        public void ClearSaved();
        void Scroll(int lines, Direction direction);
        public void SaveCursor();
        public void RestoreCursor();
        public void SetGraphicsRendition(params GraphicsPair[] _graphicRenditions);
        public void InsertLines(int linesToInsert);
        public void DeleteLines(int linesToDelete);

        internal void Transmit(byte[] data);
    }
}