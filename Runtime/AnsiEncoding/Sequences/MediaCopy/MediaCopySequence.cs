using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding.MediaCopy
{
    public class MediaCopySequence : CSISequence
    {
        private const char QuestionMarkAsPrivateIndicator = '?';
        private const int InvalidArgument = -1;
        public override char Command => 'i';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                context.LogWarning($"Failed to executed {nameof(GetType)}, no parameters given. Skipping command");
                return;
            }

            var paramsToParse = parameters.StartsWith(QuestionMarkAsPrivateIndicator)
                ? parameters.Substring(1, parameters.Length - 1)
                : parameters;

            if (!TryParseInt(paramsToParse, out var argument, "-1"))
            {
                context.LogWarning($"Failed to parse argument {nameof(GetType)}, no parameters invalid. Int Expected.");
                return;
            }

            if (InvalidArgument == argument)
            {
                context.LogWarning($"Failed to parse argument {nameof(GetType)}, parameter invalid. Int Expected.");
                return;
            }

            if (parameters.StartsWith(QuestionMarkAsPrivateIndicator))
                ExecuteDecSpecific(context, argument);
            else
                ExecuteNormal(context, argument);
        }

        private void ExecuteDecSpecific(IAnsiContext context, int argument)
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

            context.LogWarning("MediaCopySequence not implemented");
        }

        private void ExecuteNormal(IAnsiContext context, int argument)
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

            context.LogWarning("MediaCopySequence not implemented");
        }
    }
}