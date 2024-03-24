using System;
using System.Linq;
using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    public abstract class AnsiDecoderTest
    {
        internal EscapeCharacterDecoder EscapeCharacterDecoder;

        [SetUp]
        public virtual void SetUp()
        {
            EscapeCharacterDecoder = new EscapeCharacterDecoder();
        }

        protected void Decode(string input, params byte[] trailingBytes)
        {
            trailingBytes ??= Array.Empty<byte>();
            byte[] data = new byte[input.Length];
            int i = 0;
            foreach (char c in input)
            {
                data[i] = (byte)c;
                i++;
            }

            EscapeCharacterDecoder.Decode(data.Concat(trailingBytes).ToArray());
        }

        [TearDown]
        public virtual void TearDown()
        {
            EscapeCharacterDecoder.Dispose();
        }
    }
}