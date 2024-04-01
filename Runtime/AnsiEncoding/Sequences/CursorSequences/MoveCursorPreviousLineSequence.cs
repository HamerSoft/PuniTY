﻿namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal class MoveCursorPreviousLineSequence : Sequence
    {
        public override char Command => 'F';

        public override void Execute(IScreen screen, string parameters)
        {
            var cells = 1;
            if (string.IsNullOrWhiteSpace(parameters) || int.TryParse(parameters, out cells))
                screen.SetCursorPosition(new Position(screen.Cursor.Position.Row - cells, 1));
            else
            {
                Logger.Warning($"Failed to parse move cursor to beginning of the line {parameters} command.");
            }
        }
    }
}