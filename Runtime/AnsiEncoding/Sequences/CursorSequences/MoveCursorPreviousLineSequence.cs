using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal class MoveCursorPreviousLineSequence : Sequence
    {
        public override char Command => 'F';

        public override void Execute(IScreen screen, string parameters)
        {
            var cells = 1;
            if (string.IsNullOrWhiteSpace(parameters) || int.TryParse(parameters, out cells))
                screen.SetCursorPosition(new Vector2Int(1, screen.Cursor.Position.y + cells));
            else
            {
                Logger.Warning($"Failed to parse move cursor to beginning of the line {parameters} command.");
            }
        }
    }
}