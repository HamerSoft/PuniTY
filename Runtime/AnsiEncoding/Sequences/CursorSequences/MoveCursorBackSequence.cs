namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal class MoveCursorBackSequence : MoveCursorSequence
    {
        public override char Command => 'D';
        protected override Direction Direction => Direction.Back;
    }
}