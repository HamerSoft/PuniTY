using System;
using System.Linq;
using System.Text;
using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    public abstract class AnsiDecoderTest
    {
        protected class MockCursor : ICursor
        {
            public Position Position { get; private set; }

            void ICursor.SetPosition(Position position)
            {
                Position = position;
            }
        }

        protected class MockScreen : Screen
        {
            public MockScreen(int rows, int columns) : base(rows, columns, new MockCursor())
            {
            }
        }

        protected AnsiDecoder AnsiDecoder;
        protected MockScreen Screen;
        internal EscapeCharacterDecoder EscapeCharacterDecoder;
        protected const string Escape = "\x001b[";

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

        protected void PrintScreen()
        {
            var sb = new StringBuilder();
            sb.Append(" ");
            for (int i = 0; i < Screen.Columns; i++)
                sb.Append($"|{i}|");

            for (int i = 0; i < Screen.Rows; i++)
            {
                var line = new StringBuilder();
                line.Append($"|{i}|");
            }
        }
    }
}