using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class MoveCursorToColumn : Sequence
    {
        public override char Command => 'G';
        public override void Execute(IScreen screen, string parameters)
        {
            int column = 1;
            if (string.IsNullOrWhiteSpace(parameters) || int.TryParse(parameters, out column))
                screen.SetCursorPosition(new Vector2Int(column, screen.Cursor.Position.y));
            else
            {
                Logger.Warning($"Failed to parse move cursor to column {parameters} command.");
            }
        }
    }
}