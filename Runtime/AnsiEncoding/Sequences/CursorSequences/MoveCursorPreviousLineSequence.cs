﻿using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal class MoveCursorPreviousLineSequence : CSISequence
    {
        public override char Command => 'F';

        public MoveCursorPreviousLineSequence(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IAnsiContext context, string parameters)
        {
            var cells = 1;
            var screen = context.Screen;
            if (string.IsNullOrWhiteSpace(parameters) || int.TryParse(parameters, out cells))
                screen.SetCursorPosition(new Position(screen.Cursor.Position.Row - cells, 1));
            else
            {
                Logger.LogWarning($"Failed to parse move cursor to beginning of the line {parameters} command.");
            }
        }
    }
}