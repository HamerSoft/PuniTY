using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class SetCursorPosition : Sequence
    {
        public override char Command => 'H';

        public override void Execute(IScreen screen, string parameters)
        {
            var values = GetCommandArguments(parameters, 2, 1);
            screen.SetCursorPosition(new Vector2Int(values[0], values[1]));
        }
    }
}