using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding.ScrollSequences
{
    public abstract class ScrollSequence : CSISequence
    {
        public abstract override char Command { get; }
        protected abstract Direction Direction { get; }

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";

            if (!int.TryParse(parameters, out var lines))
            {
                context.LogWarning($"Cannot scroll {Direction}, invalid parameter: {parameters}. Must be an integer.");
                return;
            }

            context.Screen.Scroll(lines, Direction);
        }
    }
}