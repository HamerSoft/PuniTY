namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class RestoreCursor : Sequence
    {
        public override char Command => 'u';

        public override void Execute(IScreen screen, string _)
        {
            screen.RestoreCursor();
        }
    }

    public class RestoreCursorDec : RestoreCursor
    {
        public override char Command => '8';
    }
}