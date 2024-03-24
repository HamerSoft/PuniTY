using System;
using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;
using UnityEngine;
using Screen = HamerSoft.PuniTY.AnsiEncoding.Screen;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    [TestFixture]
    public class MoveCursorTests : AnsiDecoderTest
    {
        private AnsiDecoder _ansiDecoder;
        private MockScreen _screen;

        private class MockCursor : ICursor
        {
            public Vector2Int Position { get; private set; }

            void ICursor.SetPosition(Vector2Int position)
            {
                Position = position;
            }
        }

        private class MockScreen : Screen
        {
            public MockScreen(int width, int height) : base(width, height, new MockCursor())
            {
            }
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _screen = new MockScreen(10, 10);
            _ansiDecoder = new AnsiDecoder(_screen,
                EscapeCharacterDecoder,
                new MoveCursorBackSequence(),
                new MoveCursorForwardSequence(),
                new MoveCursorUpSequence(),
                new MoveCursorDownSequence());
        }

        [Test]
        public void When_CursorMove_Up_Command_Is_Executed_CursorMovesUp()
        {
            MoveCursor(1, Direction.Up);
            Assert.That(_screen.Cursor.Position.x, Is.EqualTo(0));
            Assert.That(_screen.Cursor.Position.y, Is.EqualTo(1));
        }
        [Test]
        public void When_CursorMove_UpMultiple_Command_Is_Executed_CursorMovesUp()
        {
            MoveCursor(3, Direction.Up);
            Assert.That(_screen.Cursor.Position.x, Is.EqualTo(0));
            Assert.That(_screen.Cursor.Position.y, Is.EqualTo(3));
        }

        [Test]
        public void When_CursorMove_Up_Command_Is_Executed_AtEdge_NothingHappens()
        {
            _screen.SetCursorPosition(new Vector2Int(0, 9));
            MoveCursor(1, Direction.Up);
            Assert.That(_screen.Cursor.Position.x, Is.EqualTo(0));
            Assert.That(_screen.Cursor.Position.y, Is.EqualTo(9));
        }

        [Test]
        public void When_CursorMove_Forward_Command_Is_Executed_CursorMovesForward()
        {
            MoveCursor(1, Direction.Forward);
            Assert.That(_screen.Cursor.Position.x, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.y, Is.EqualTo(0));
        }
        [Test]
        public void When_CursorMove_MultipleForward_Command_Is_Executed_CursorMovesForward()
        {
            MoveCursor(3, Direction.Forward);
            Assert.That(_screen.Cursor.Position.x, Is.EqualTo(3));
            Assert.That(_screen.Cursor.Position.y, Is.EqualTo(0));
        }

        [Test]
        public void When_CursorMove_Forward_Command_Is_Executed_AtEdge_NothingHappens()
        {
            _screen.SetCursorPosition(new Vector2Int(9, 0));
            MoveCursor(1, Direction.Forward);
            Assert.That(_screen.Cursor.Position.x, Is.EqualTo(9));
            Assert.That(_screen.Cursor.Position.y, Is.EqualTo(0));
        }

        [Test]
        public void When_CursorMove_Back_Command_Is_Executed_CursorMovesBack()
        {
            _screen.SetCursorPosition(new Vector2Int(1, 0));
            MoveCursor(1, Direction.Back);
            Assert.That(_screen.Cursor.Position.x, Is.EqualTo(0));
            Assert.That(_screen.Cursor.Position.y, Is.EqualTo(0));
        }
        
        [Test]
        public void When_CursorMove_MultipleBack_Command_Is_Executed_CursorMovesBack()
        {
            _screen.SetCursorPosition(new Vector2Int(3, 0));
            MoveCursor(3, Direction.Back);
            Assert.That(_screen.Cursor.Position.x, Is.EqualTo(0));
            Assert.That(_screen.Cursor.Position.y, Is.EqualTo(0));
        }

        [Test]
        public void When_CursorMove_Back_Command_Is_Executed_AtEdge_NothingHappens()
        {
            MoveCursor(1, Direction.Back);
            Assert.That(_screen.Cursor.Position.x, Is.EqualTo(0));
            Assert.That(_screen.Cursor.Position.y, Is.EqualTo(0));
        }

        [Test]
        public void When_CursorMove_Down_Command_Is_Executed_CursorMovesDown()
        {
            _screen.SetCursorPosition(new Vector2Int(0, 1));
            MoveCursor(1, Direction.Down);
            Assert.That(_screen.Cursor.Position.x, Is.EqualTo(0));
            Assert.That(_screen.Cursor.Position.y, Is.EqualTo(0));
        }
        
        [Test]
        public void When_CursorMove_MultipleDown_Command_Is_Executed_CursorMovesDown()
        {
            _screen.SetCursorPosition(new Vector2Int(0, 3));
            MoveCursor(3, Direction.Down);
            Assert.That(_screen.Cursor.Position.x, Is.EqualTo(0));
            Assert.That(_screen.Cursor.Position.y, Is.EqualTo(0));
        }

        [Test]
        public void When_CursorMove_Down_Command_Is_Executed_AtEdge_NothingHappens()
        {
            Assert.That(_screen.Cursor.Position.x, Is.EqualTo(0));
            Assert.That(_screen.Cursor.Position.y, Is.EqualTo(0));
        }

        private void MoveCursor(int cells, Direction direction)
        {
            var dir = direction switch
            {
                Direction.Up => "A",
                Direction.Down => "B",
                Direction.Forward => "C",
                Direction.Back => "D",
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
            Decode($"\x001b[{(cells == 1 ? "" : $"{cells}")}{dir}");
        }
    }
}