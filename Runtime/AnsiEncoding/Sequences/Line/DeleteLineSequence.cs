using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding.Line
{
    /// <summary>
    /// csi
    /// </summary>
    public class DeleteLineSequence : CSISequence
    {
        public override char Command => 'M';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";

            if (!int.TryParse(parameters, out var linesToDelete))
            {
                context.LogWarning($"Cannot parse {parameters} to delete lines, int expected!");
                return;
            }

            context.Screen.DeleteLines(linesToDelete);
        }
    }
}