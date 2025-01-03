﻿using AnsiEncoding;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public abstract class Sequence : ISequence
    {
        private const char DefaultSeparator = ';';
        protected const char EmptyCharacter = '\0';
        public abstract SequenceType SequenceType { get; }
        public abstract char Command { get; }
        public abstract void Execute(IAnsiContext context, string parameters);

        protected bool TryParseInt(string parameters, out int value, string defaultValue = "1")
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = defaultValue;
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
                    output[i] = defaultValue;
                }
            }

            return output;
        }
    }
}