namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class SaveCursor : Sequence
    {
        public override char Command => 's';

        public override void Execute(IScreen screen, string _)
        {
            screen.SaveCursor();
        }
    }

    public class SaveCursorDec : SaveCursor
    {
        public override char Command => '7';
    }
}