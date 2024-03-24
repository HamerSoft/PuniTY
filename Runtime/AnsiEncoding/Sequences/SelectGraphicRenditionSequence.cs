namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class SelectGraphicRenditionSequence : ISequence
    {
        public char Command => 'm';

        public SelectGraphicRenditionSequence()
        {
        }

        public void Execute(IScreen screen, string parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}