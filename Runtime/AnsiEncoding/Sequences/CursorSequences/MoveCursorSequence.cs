namespace HamerSoft.PuniTY.AnsiEncoding
{
    public enum Direction
    {
        Up,
        Down,
        Forward,
        Back
    }

    internal abstract class MoveCursorSequence : Sequence
    {
        public abstract override char Command { get; }
        protected abstract Direction Direction { get; }

        public override void Execute(IScreen screen, string parameters)
        {
            var param = parameters.Split(';');
            var defaultCells = 1;
            screen.MoveCursor(int.TryParse(param[0], out var cells) ? cells : defaultCells, Direction);
        }
    }
}