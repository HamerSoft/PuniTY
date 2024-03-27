﻿namespace HamerSoft.PuniTY.AnsiEncoding
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
            var cells = 1;
            if (string.IsNullOrWhiteSpace(parameters) || int.TryParse(parameters, out cells))
                screen.MoveCursor(cells, Direction);
            else
            {
                Logger.Warning($"Failed to parse move cursor command {Direction} => {parameters}");
            }
        }
    }
}