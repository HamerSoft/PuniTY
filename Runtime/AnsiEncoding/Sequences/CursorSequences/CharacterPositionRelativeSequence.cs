﻿using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class CharacterPositionRelativeSequence : CSISequence
    {
        public override char Command => 'a';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (!TryParseInt(parameters, out var relativeColumns))
            {
                context.LogWarning($"Cannot move cursor to relative column given: {parameters}. Int expected");
                return;
            }

            var screen = context.Screen;
            if (relativeColumns < 1 || screen.Cursor.Position.Column + relativeColumns > screen.Columns)
            {
                context.LogWarning(
                    $"Cannot move cursor forward columns given: {relativeColumns}. Moving would exceed screen boundaries.");
                return;
            }
            
            screen.SetCursorPosition(screen.Cursor.Position.AddColumns(screen, relativeColumns));
        }
    }
}