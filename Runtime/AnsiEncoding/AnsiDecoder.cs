using System;
using System.Collections.Generic;
using System.Linq;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public sealed class AnsiDecoder :IDisposable
    {
        private readonly IScreen _screen;
        private readonly IEscapeCharacterDecoder _escapeCharacterDecoder;
        private readonly Dictionary<char, ISequence> _sequences;

        public AnsiDecoder(IScreen screen, IEscapeCharacterDecoder escapeCharacterDecoder,
            params ISequence[] acceptedSequences)
        {
            _screen = screen;
            _escapeCharacterDecoder = escapeCharacterDecoder;
            _sequences = acceptedSequences.ToDictionary(seq => seq.Command, seq => seq);
            
            _escapeCharacterDecoder.ProcessCommand += ProcessCommand;
            _escapeCharacterDecoder.ProcessOutput += ProcessOutput;
        }

        private void ProcessOutput(byte[] output)
        {
            throw new NotImplementedException();
        }

        private void ProcessCommand(byte command, string parameters)
        {
            var character = (char)command;
            if(_sequences.TryGetValue(character, out var sequence))
                sequence.Execute(_screen, parameters);
        }

        public void Dispose()
        {
            _escapeCharacterDecoder.ProcessCommand -= ProcessCommand;
            _escapeCharacterDecoder?.Dispose();
        }
    }
}