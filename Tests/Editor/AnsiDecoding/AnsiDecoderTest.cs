using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Core.Logging;
using HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.Stubs;
using NUnit.Framework;
using UnityEngine;
using Cursor = AnsiEncoding.Cursor.Cursor;
using CursorMode = HamerSoft.PuniTY.AnsiEncoding.CursorMode;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;
using Screen = HamerSoft.PuniTY.AnsiEncoding.Screen;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    public abstract class AnsiDecoderTest
    {
        public readonly struct DefaultTestSetup
        {
            internal readonly int Rows;
            internal readonly int Columns;
            internal readonly Type[] Sequences;
            public readonly IModeFactory ModeFactory;

            public DefaultTestSetup(int rows, int columns, params Type[] sequences)
            {
                Rows = rows;
                Columns = columns;
                Sequences = sequences;
                ModeFactory = null;
            }

            public DefaultTestSetup(int rows, int columns, IModeFactory modeFactory, params Type[] sequences)
            {
                ModeFactory = modeFactory;
                Rows = rows;
                Columns = columns;
                Sequences = sequences;
            }

            public DefaultTestSetup(int rows, int columns)
            {
                Rows = rows;
                Columns = columns;
                Sequences = Array.Empty<Type>();
                ModeFactory = null;
            }
        }

        protected class MockCursor : ICursor
        {
            private CursorMode _mode;
            public Position Position { get; private set; }

            void ICursor.SetPosition(Position position)
            {
                Position = position;
            }
        }

        protected class MockScreen : Screen
        {
            public MockScreen(int rows, int columns, ILogger logger) : base(new Cursor(), logger,
                new DefaultScreenConfiguration(rows, columns, 8, new FontDimensions(10, 10)))
            {
            }
        }

        protected const char DefaultChar = 'a';
        protected const char EmptyCharacter = '\0';
        protected IScreen Screen => AnsiContext.Screen;
        protected const string Escape = "\x001b[";
        protected HamerSoft.PuniTY.Logging.ILogger Logger;
        private StubAnsiContext _context;

        internal StubAnsiContext AnsiContext
        {
            get => _context;
            set
            {
                _context?.Dispose();
                _context = value;
            }
        }

        protected DefaultTestSetup DefaultSetup => new DefaultTestSetup(2, 10);

        [SetUp]
        public virtual void SetUp()
        {
            Logger = new EditorLogger();
            AnsiContext = CreateTestContext(DoTestSetup());
        }

        protected abstract DefaultTestSetup DoTestSetup();

        private StubAnsiContext CreateTestContext(DefaultTestSetup defaultTestSetup)
        {
            if (defaultTestSetup.ModeFactory == null)
                return new StubAnsiContext(defaultTestSetup.Rows, defaultTestSetup.Columns, Logger,
                    CreateSequence(defaultTestSetup.Sequences));
            else
            {
                return new StubAnsiContext(defaultTestSetup.Rows, defaultTestSetup.Columns, Logger,
                    defaultTestSetup.ModeFactory,
                    CreateSequence(defaultTestSetup.Sequences));
            }
        }

        /// <summary>
        /// Create new instance(s) of sequence(s), using reflection (tests only!)
        /// </summary>
        /// <returns>instance(s) with logger injected</returns>
        protected ISequence[] CreateSequence(params Type[] types)
        {
            var sequences = new List<ISequence>();
            foreach (var type in types)
                if (type.IsSubclassOf(typeof(Sequence)))
                    sequences.Add((Sequence)Activator.CreateInstance(type));
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

            AnsiContext.Decoder.Decode(data.Concat(trailingBytes).ToArray());
        }

        [TearDown]
        public virtual void TearDown()
        {
            AnsiContext.Dispose();
        }

        protected void PopulateScreen(char @char = DefaultChar)
        {
            var iterator = new ScreenIterator(Screen);
            foreach (var _ in iterator)
                Screen.AddCharacter(@char);
            Screen.SetCursorPosition(new Position(1, 1));
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