namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal class MoveCursorDownSequence : MoveCursorSequence
    {
        public override char Command => 'B';
        protected override Direction Direction => Direction.Down;
    }
}