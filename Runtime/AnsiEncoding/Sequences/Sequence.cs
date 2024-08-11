using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public abstract class Sequence : ISequence
    {
        private const char DefaultSeparator = ';';
        protected const char EmptyCharacter = '\0';
        public abstract SequenceType SequenceType { get; }
        public abstract char Command { get; }
        private readonly IScreen _screen;
        protected ILogger.ILogger Logger;

        public Sequence(ILogger.ILogger logger)
        {
            Logger = logger;
        }

        public abstract void Execute(IScreen screen, string parameters);

        protected bool TryParseInt(string parameters, out int value)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";
            return int.TryParse(parameters, out value);
        }

        protected int[] GetCommandArguments(string parameters, int expectedAmount, int defaultValue,
            char separator = DefaultSeparator)
        {
            var output = new int[expectedAmount];
            var arguments = parameters.Split(separator);
            for (int i = 0; i < expectedAmount; i++)
            {
                if (i >= arguments.Length)
                    output[i] = defaultValue;
                else if (string.IsNullOrWhiteSpace(arguments[i]))
                    output[i] = defaultValue;
                else if (int.TryParse(arguments[i], out var value))
                {
                    output[i] = value;
                }
                else
                {
                    Logger.LogWarning(
                        $"{GetType().Name} failed to parse parameters: {parameters}. Invalid integer {arguments[i]}.");
                    output[i] = defaultValue;
                }
            }

            return output;
        }
    }
}