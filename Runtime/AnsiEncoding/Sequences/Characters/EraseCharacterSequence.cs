using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding.Characters
{
    public class EraseCharacterSequence : CSISequence
    {
        public override char Command => 'X';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (!int.TryParse(parameters, out var charactersToErase))
            {
                context.LogWarning($"Cannot erase characters, invalid parameter: {parameters}. Int expected.");
            }

            var screen = context.Screen;
            screen.Erase(screen.Cursor.Position, screen.Cursor.Position.AddColumns(screen, charactersToErase));
        }
    }
}