using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using UnityEngine;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class CharacterPositionAbsoluteSequence : CSISequence
    {
        public override char Command => '`';

        public CharacterPositionAbsoluteSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";
            if (!int.TryParse(parameters, out var targetColumn))
            {
                Logger.LogWarning($"Cannot move cursor to column given: {parameters}. Int expected");
                return;
            }

            if (targetColumn < 1 || targetColumn > screen.Columns)
            {
                Logger.LogWarning(
                    $"Cannot move cursor to column given: {targetColumn}. Column must be greater than 0 and smaller than {screen.Columns + 1}.");
                return;
            }

            screen.SetCursorPosition(new Position(screen.Cursor.Position.Row, targetColumn));
        }
    }
}