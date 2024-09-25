using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.MediaCopy
{
    public class MediaCopySequence : CSISequence
    {
        private const char QuestionMarkAsPrivateIndicator = '?';
        private const int InvalidArgument = -1;
        public override char Command => 'i';

        public MediaCopySequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Logger.LogWarning($"Failed to executed {nameof(GetType)}, no parameters given. Skipping command");
                return;
            }

            var paramsToParse = parameters.StartsWith(QuestionMarkAsPrivateIndicator)
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

            if (parameters.StartsWith(QuestionMarkAsPrivateIndicator))
                ExecuteDecSpecific(screen, argument);
            else
                ExecuteNormal(screen, argument);
        }

        private void ExecuteDecSpecific(IScreen screen, int argument)
        {
            switch (argument)
            {
                case 1:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 10:
                    break;
                case 11:
                    break;
            }

            Logger.LogWarning("MediaCopySequence not implemented");
        }

        private void ExecuteNormal(IScreen screen, int argument)
        {
            switch (argument)
            {
                case 0:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 10:
                    break;
                case 11:
                    break;
            }

            Logger.LogWarning("MediaCopySequence not implemented");
        }
    }
}