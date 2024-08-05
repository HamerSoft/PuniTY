using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.Line
{
    public class InsertLineSequence : CSISequence
    {
        public override char Command => 'L';

        public InsertLineSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";

            if (!int.TryParse(parameters, out var linesToInsert))
            {
                Logger.LogWarning($"Cannot parse {parameters} to insert lines, int expected!");
                return;
            }

            screen.InsertLines(linesToInsert);
        }
    }
}