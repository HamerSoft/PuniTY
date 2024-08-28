using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public abstract class ModeSequence : CSISequence
    {
        private const char QuestionMark = '?';
        private const int InvalidArgument = -1;

        protected ModeSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Logger.LogWarning($"Failed to executed {nameof(GetType)}, no parameters given. Skipping command");
                return;
            }

            var paramsToParse = parameters.StartsWith(QuestionMark)
                ? parameters.Substring(1, parameters.Length - 1)
                : parameters;
            
            if (!TryParseInt(paramsToParse, out var argument, "-1"))
            {
                Logger.LogWarning($"Failed to parse argument {nameof(GetType)}, no parameters invalid. Int Expected.");
                return;
            }

            if (InvalidArgument == argument)
            {
                Logger.LogWarning($"Failed to parse argument {nameof(GetType)}, parameter invalid. Int Expected.");
                return;
            }

            if (parameters.StartsWith(QuestionMark))
                ExecutePrivateSequence(screen, argument);
            else
                ExecutePublicSequence(screen, argument);
        }

        protected abstract void ExecutePrivateSequence(IScreen screen, int argument);
        protected abstract void ExecutePublicSequence(IScreen screen, int argument);
        protected abstract void SetMode(IScreen screen, AnsiMode mode);
    }
}