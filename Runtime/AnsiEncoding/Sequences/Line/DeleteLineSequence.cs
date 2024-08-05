using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.Line
{
    /// <summary>
    /// csi
    /// </summary>
    public class DeleteLineSequence : CSISequence
    {
        public override char Command => 'M';

        public DeleteLineSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";

            if (!int.TryParse(parameters, out var linesToDelete))
            {
                Logger.LogWarning($"Cannot parse {parameters} to delete lines, int expected!");
                return;
            }

            screen.DeleteLines(linesToDelete);
        }
    }
}