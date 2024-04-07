namespace HamerSoft.PuniTY.AnsiEncoding
{
    public interface IScreen
    {
        public int Rows { get; }
        public int Columns { get; }
        public ICursor Cursor { get; }

        public void SetCursorPosition(Position position);
        public ICharacter GetCharacter(Position position);
        public void MoveCursor(int cells, Direction direction);
        public void Erase(Position? from = null, Position? to = null);
        public void ClearSaved();
    }
}