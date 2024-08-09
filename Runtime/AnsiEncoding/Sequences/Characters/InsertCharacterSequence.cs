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

            if (!int.TryParse(parameters, out var charactersToInsert))
            {
                Logger.LogWarning($"Invalid insert character argument. Expected int: {parameters}");
                return;
            }

            screen.InsertCharacters(charactersToInsert);

            // emptyChars = Math.Clamp(emptyChars, 1, screen.Rows * screen.Columns);
            //
            // for (int i = 0; i < emptyChars; i++)
            //     screen.InsertCharacter(EmptyCharacter);
        }
    }
}