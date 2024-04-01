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
            public Position Position { get; private set; }

            void ICursor.SetPosition(Position position)
            {
                Position = position;
            }
        }

        private class MockScreen : Screen
        {
            public MockScreen(int rows, int columns) : base(rows, columns, new MockCursor())
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
                new MoveCursorDownSequence(),
                new MoveCursorNextLineSequence(),
                new MoveCursorPreviousLineSequence(),
                new MoveCursorToColumn(),
                new SetCursorPositionSequence());
        }

        [Test]
        public void When_CursorMove_Up_Command_Is_Executed_CursorMovesUp()
        {
            MoveCursor(1, Direction.Up);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(2));
        }

        [Test]
        public void When_CursorMove_UpMultiple_Command_Is_Executed_CursorMovesUp()
        {
            MoveCursor(3, Direction.Up);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(4));
        }

        [Test]
        public void When_CursorMove_Up_Command_Is_Executed_AtEdge_NothingHappens()
        {
            _screen.SetCursorPosition(new Position(10,1));
            MoveCursor(1, Direction.Up);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(10));
        }

        [Test]
        public void When_CursorMove_Forward_Command_Is_Executed_CursorMovesForward()
        {
            MoveCursor(1, Direction.Forward);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(2));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_MultipleForward_Command_Is_Executed_CursorMovesForward()
        {
            MoveCursor(3, Direction.Forward);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(4));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_Forward_Command_Is_Executed_AtEdge_NothingHappens()
        {
            _screen.SetCursorPosition(new Position(1, 10));
            MoveCursor(1, Direction.Forward);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(10));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_Back_Command_Is_Executed_CursorMovesBack()
        {
            _screen.SetCursorPosition(new Position(1,2));
            MoveCursor(1, Direction.Back);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_MultipleBack_Command_Is_Executed_CursorMovesBack()
        {
            _screen.SetCursorPosition(new Position(2, 4));
            MoveCursor(3, Direction.Back);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(2));
        }

        [Test]
        public void When_CursorMove_Back_Command_Is_Executed_AtEdge_NothingHappens()
        {
            MoveCursor(1, Direction.Back);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_Down_Command_Is_Executed_CursorMovesDown()
        {
            _screen.SetCursorPosition(new Position(1, 1));
            MoveCursor(1, Direction.Down);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_MultipleDown_Command_Is_Executed_CursorMovesDown()
        {
            _screen.SetCursorPosition(new Position(4, 2));
            MoveCursor(3, Direction.Down);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(2));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_Down_Command_Is_Executed_AtEdge_NothingHappens()
        {
            MoveCursor(1, Direction.Down);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_NextLine_IsExecuted_Cursor_Moves_To_Start_Down()
        {
            _screen.SetCursorPosition(new Position(1, 2));
            MoveCursor(1, true);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(2));
        }


        [Test]
        public void When_CursorMove_NextLine_Multiple_IsExecuted_Cursor_Moves_To_Start_Down()
        {
            _screen.SetCursorPosition(new Position(2,5));
            MoveCursor(2, true);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(4));
        }

        [Test]
        public void When_CursorMove_PreviousLine_IsExecuted_Cursor_Moves_To_Start_Up()
        {
            _screen.SetCursorPosition(new Position(2,2));
            MoveCursor(1, false);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_PreviousLine_Multiple_IsExecuted_Cursor_Moves_To_Start_Up()
        {
            _screen.SetCursorPosition(new Position(4,4));
            MoveCursor(2, false);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(2));
        }

        [Test]
        public void When_CursorMove_ToColumn_IsExecuted_Cursor_Moves_To_Column_On_Same_Line()
        {
            _screen.SetCursorPosition(new Position(5, 5));
            MoveToColumn(1);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(5));
        }

        [Test]
        public void When_CursorMove_ToColumn_Multiple_IsExecuted_Cursor_Moves_To_Column_N_On_Same_Line()
        {
            _screen.SetCursorPosition(new Position(5, 3));
            MoveToColumn(6);
            Assert.That(_screen.Cursor.Position.Column, Is.EqualTo(6));
            Assert.That(_screen.Cursor.Position.Row, Is.EqualTo(5));
        }

        [TestCase(1, 1, ExpectedResult = new[] { 1, 1 })]
        [TestCase(2, 1, ExpectedResult = new[] { 2, 1 })]
        [TestCase(1, 2, ExpectedResult = new[] { 1, 2 })]
        [TestCase(3, 3, ExpectedResult = new[] { 3, 3 })]
        [TestCase(3, 3, ExpectedResult = new[] { 3, 3 })]
        [TestCase(0, 1, ExpectedResult = new[] { 1, 1 })]
        [TestCase(1, 0, ExpectedResult = new[] { 1, 1 })]
        [TestCase(11, 1, ExpectedResult = new[] { 1, 1 })]
        [TestCase(1, 11, ExpectedResult = new[] { 1, 1 })]
        public int[] When_CursorMove_ToPosition_IsExecuted_Cursor_Moves_To_Row_N_Column_M(int row, int column)
        {
            SetCursorPosition(row, column);
            return new int[] { _screen.Cursor.Position.Row, _screen.Cursor.Position.Column };
        }


        private void SetCursorPosition(int row, int column, bool forceSeparator = false)
        {
            if (row == 1 && column == 1)
            {
                Decode($"\x001b[H");
            }
            else if (row == 1)
            {
                Decode($"\x001b[;{column}H");
            }
            else if (column == 1)
            {
                Decode($"\x001b[{row}{(forceSeparator ? ";" : "")}H");
            }
            else
            {
                Decode($"\x001b[{row};{column}H");
            }
        }

        private void MoveToColumn(int column)
        {
            Decode($"\x001b[{(column == 1 ? "" : $"{column}")}G");
        }

        private void MoveCursor(int columns, bool next)
        {
            Decode($"\x001b[{(columns == 1 ? "" : $"{columns}")}{(next ? 'E' : 'F')}");
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