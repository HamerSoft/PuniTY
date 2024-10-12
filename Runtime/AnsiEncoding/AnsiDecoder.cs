using System;
using System.Collections.Generic;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal sealed class AnsiDecoder : IDisposable
    {
        private readonly IEscapeCharacterDecoder _escapeCharacterDecoder;
        private readonly Action<ISequence, string> _onSequenceExecute;
        private readonly Dictionary<SequenceType, Dictionary<char, ISequence>> _sequences;

        internal AnsiDecoder(IEscapeCharacterDecoder escapeCharacterDecoder,
            Action<ISequence, string> onSequenceExecute,
            params ISequence[] acceptedSequences)
        {
            _escapeCharacterDecoder = escapeCharacterDecoder;
            _onSequenceExecute = onSequenceExecute;
            _sequences = new Dictionary<SequenceType, Dictionary<char, ISequence>>();
            SetupSequenceTable(acceptedSequences);

            _escapeCharacterDecoder.ProcessCommand += ProcessCommand;
            _escapeCharacterDecoder.ProcessOutput += ProcessOutput;
        }

        private void SetupSequenceTable(ISequence[] sequences)
        {
            foreach (var sequence in sequences)
            {
                if (!_sequences.ContainsKey(sequence.SequenceType))
                    _sequences.Add(sequence.SequenceType, new Dictionary<char, ISequence>());

                if (!_sequences[sequence.SequenceType].TryAdd(sequence.Command, sequence))
                    throw new ArgumentException(
                        $"Cannot Add Sequence {nameof(sequence)}, with command '{sequence.Command}' and type {sequence.SequenceType} because it already exists: {_sequences[sequence.SequenceType][sequence.Command].GetType()}");
            }
        }

        private void ProcessOutput(byte[] output)
        {
            throw new NotImplementedException();
        }

        private void ProcessCommand(SequenceType sequenceType, char character, string parameters)
        {
            if (_sequences.TryGetValue(sequenceType, out var typedSequences) &&
                typedSequences.TryGetValue(character, out var sequence))
                _onSequenceExecute.Invoke(sequence, parameters);
        }

        public void Dispose()
        {
            _escapeCharacterDecoder.ProcessCommand -= ProcessCommand;
            _escapeCharacterDecoder.ProcessOutput -= ProcessOutput;
            _escapeCharacterDecoder?.Dispose();
        }
    }
}