using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class CharacterPositionRelativeSequence : CSISequence
    {
        public override char Command => 'a';

        public CharacterPositionRelativeSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (!TryParseInt(parameters, out var relativeColumns))
            {
                Logger.LogWarning($"Cannot move cursor to relative column given: {parameters}. Int expected");
                return;
            }

            if (relativeColumns < 1 || screen.Cursor.Position.Column + relativeColumns > screen.Columns)
            {
                Logger.LogWarning(
                    $"Cannot move cursor forward columns given: {relativeColumns}. Moving would exceed screen boundaries.");
                return;
            }
            
            screen.SetCursorPosition(screen.Cursor.Position.AddColumns(screen, relativeColumns));
        }
    }
}