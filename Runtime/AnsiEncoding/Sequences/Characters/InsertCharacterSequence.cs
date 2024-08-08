using System;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.Characters
{
    public class InsertCharacterSequence : CSISequence
    {
        public override char Command => '@';

        public InsertCharacterSequence(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";

            if (!int.TryParse(parameters, out var emptyChars))
            {
                Logger.LogWarning($"Invalid empty character argument. Expected int but string given: {parameters}");
                return;
            }

            emptyChars = Math.Clamp(emptyChars, 1, screen.Rows * screen.Columns);

            for (int i = 0; i < emptyChars; i++)
                screen.AddCharacter(EmptyCharacter);
        }
    }
}