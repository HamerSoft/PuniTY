using HamerSoft.PuniTY.AnsiEncoding;

namespace AnsiEncoding.Cursor
{
    internal class Cursor : ICursor
    {
        private CursorMode _mode;
        public Position Position { get; private set; }

        CursorMode ICursor.Mode
        {
            get => _mode;
            set => _mode = value;
        }

        void ICursor.SetPosition(Position position)
        {
            Position = position;
        }
    }
}