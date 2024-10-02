using System;
using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public interface IEscapeCharacterDecoder : IDisposable
    {
        public event Action<byte[]> ProcessOutput;
        internal event Action<SequenceType, char, string> ProcessCommand;
        public System.Text.Encoding Encoding { get; }

        /// <summary>
        /// Tell decoder to process the given data.
        /// 
        /// If an invalid byte is passed InvalidByteException or one
        /// of the its sub-classes is thrown. The decoder will try its
        /// best to survive any invalid data and should still be able
        /// to process data after an exception is thrown.
        /// </summary>
        public void Decode(byte[] data);

        public bool KeyPressed(KeyCode[] modifiers, KeyCode key);
    }
}