using UnityEngine;
using ILogger = NUnit.Framework.Internal.ILogger;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public abstract class Sequence : ISequence
    {
        private const char DefaultSeparator = ';';
        private readonly IScreen _screen;
        public abstract char Command { get; }
        protected ILogger Logger;

        public abstract void Execute(IScreen screen, string parameters);

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
                    Logger.Warning(
                        $"{GetType().Name} failed to parse parameters: {parameters}. Invalid integer {arguments[i]}.");
                    output[i] = defaultValue;
                }
            }

            return output;
        }
    }
}