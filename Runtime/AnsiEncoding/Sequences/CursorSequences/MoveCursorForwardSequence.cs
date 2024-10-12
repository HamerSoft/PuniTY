namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal class MoveCursorForwardSequence : MoveCursorSequence
    {
        public override char Command => 'C';
        protected override Direction Direction => Direction.Forward;
    }
}