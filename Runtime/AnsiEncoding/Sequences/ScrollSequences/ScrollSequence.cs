namespace HamerSoft.PuniTY.AnsiEncoding.ScrollSequences
{
    public abstract class ScrollSequence : Sequence
    {
        public abstract override char Command { get; }
        protected abstract Direction Direction { get; }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";
            
            if (!int.TryParse(parameters, out var lines))
            {
                Logger.Warning($"Cannot scroll {Direction}, invalid parameter: {parameters}. Must be an integer.");
                return;
            }

            screen.Scroll(lines, Direction);
        }
    }
}