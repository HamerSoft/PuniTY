namespace HamerSoft.PuniTY.AnsiEncoding.ScrollSequences
{
    public abstract class ScrollSequence : Sequence
    {
        public abstract override char Command { get; }
        protected abstract Direction Direction { get; }
        
        public override void Execute(IScreen screen, string parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}