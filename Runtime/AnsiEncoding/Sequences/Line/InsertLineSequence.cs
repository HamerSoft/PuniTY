using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.Line
{
    public class InsertLineSequence : CSISequence
    {
        public override char Command => 'L';
        
        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";

            if (!int.TryParse(parameters, out var linesToInsert))
            {
                context.LogWarning($"Cannot parse {parameters} to insert lines, int expected!");
                return;
            }

            context.Screen.InsertLines(linesToInsert);
        }
    }
}