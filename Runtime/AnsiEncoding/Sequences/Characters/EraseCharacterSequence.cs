using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using UnityEngine;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace HamerSoft.PuniTY.AnsiEncoding.Characters
{
    public class EraseCharacterSequence : CSISequence
    {
        public override char Command => 'X';

        public EraseCharacterSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (!int.TryParse(parameters, out var charactersToErase))
            {
                Logger.LogWarning($"Cannot erase characters, invalid parameter: {parameters}. Int expected.");
            }

            var screen = context.Screen;
            screen.Erase(screen.Cursor.Position, screen.Cursor.Position.AddColumns(screen, charactersToErase));
        }
    }
}