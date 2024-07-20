﻿using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class MoveAndScrollUp : Sequence
    {
        public override char Command => 'M';

        public MoveAndScrollUp(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            screen.SetCursorPosition(new Position(screen.Cursor.Position.Row - 1, screen.Cursor.Position.Column));
        }
    }
}