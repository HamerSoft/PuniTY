using HamerSoft.PuniTY.AnsiEncoding;

namespace AnsiEncoding.Cursor
{
    internal class Cursor : ICursor
    {
        public Position Position { get; private set; }

        void ICursor.SetPosition(Position position)
        {
            Position = position;
        }
    }
}