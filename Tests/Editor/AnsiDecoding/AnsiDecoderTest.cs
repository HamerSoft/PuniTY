using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.Core.Logging;
using NUnit.Framework;
using UnityEngine;
using ILogger = NUnit.Framework.Internal.ILogger;
using Object = System.Object;
using Screen = HamerSoft.PuniTY.AnsiEncoding.Screen;

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
            public MockScreen(int rows, int columns) : base(new Dimensions(rows, columns), new MockCursor(),
                new EditorLogger(), new DefaultScreenConfiguration())
            {
            }
        }

        protected AnsiDecoder AnsiDecoder;
        protected MockScreen Screen;
        internal EscapeCharacterDecoder EscapeCharacterDecoder;
        protected const string Escape = "\x001b[";
        protected HamerSoft.PuniTY.Logging.ILogger Logger;

        [SetUp]
        public virtual void SetUp()
        {
            Logger = new EditorLogger();
            EscapeCharacterDecoder = new EscapeCharacterDecoder();
        }

        /// <summary>
        /// Create new instance(s) of sequence(s), using reflection (tests only!)
        /// </summary>
        /// <returns>instance(s) with logger injected</returns>
        protected Sequence[] CreateSequence(params Type[] types)
        {
            var sequences = new List<Sequence>();
            foreach (var type in types)
                if (type.IsSubclassOf(typeof(Sequence)))
                    sequences.Add((Sequence)Activator.CreateInstance(type, new object[] { Logger }));
                else
                    throw new ArgumentException($"Invalid Sequence Type: {type.FullName}");

            return sequences.ToArray();
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
            for (int i = 1; i <= Screen.Columns; i++)
                sb.Append($"|{i}|");

            sb.Append("\r\n");
            for (int i = 1; i <= Screen.Rows; i++)
            {
                var line = new StringBuilder();
                line.Append($"|{i}|");
                for (int j = 1; j <= Screen.Columns; j++)
                {
                    line.Append($"|{Screen.GetCharacter(new Position(i, j))}|");
                }

                sb.AppendLine(line.ToString());
            }

            Debug.Log(sb.ToString());
        }
    }
}