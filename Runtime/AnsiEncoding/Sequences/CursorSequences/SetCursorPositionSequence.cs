﻿namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class SetCursorPositionSequence : Sequence
    {
        public override char Command => 'H';

        public override void Execute(IScreen screen, string parameters)
        {
            var values = GetCommandArguments(parameters, 2, 1);
            screen.SetCursorPosition(new Position(values[0], values[1]));
        }
    }
}