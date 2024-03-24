namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal class MoveCursorUpSequence : MoveCursorSequence
    {
        public override char Command => 'A';
        protected override Direction Direction => Direction.Up;
    }
}