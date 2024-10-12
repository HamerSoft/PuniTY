﻿using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding.Characters
{
    public class InsertCharacterSequence : CSISequence
    {
        public override char Command => '@';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";

            if (!int.TryParse(parameters, out var charactersToInsert))
            {
                context.LogWarning($"Invalid insert character argument. Expected int: {parameters}");
                return;
            }

            context.Screen.InsertCharacters(charactersToInsert);

            // emptyChars = Math.Clamp(emptyChars, 1, screen.Rows * screen.Columns);
            //
            // for (int i = 0; i < emptyChars; i++)
            //     screen.InsertCharacter(EmptyCharacter);
        }
    }
}