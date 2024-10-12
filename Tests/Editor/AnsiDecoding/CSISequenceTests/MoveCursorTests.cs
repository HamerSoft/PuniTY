using System;
using System.Text.RegularExpressions;
using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    [TestFixture]
    public class MoveCursorTests : AnsiDecoderTest
    {
        protected override DefaultTestSetup DoTestSetup()
        {
            return new DefaultTestSetup(10, 10, 
                typeof(MoveCursorBackSequence),
                typeof(MoveCursorForwardSequence),
                typeof(MoveCursorUpSequence),
                typeof(MoveCursorDownSequence),
                typeof(MoveCursorNextLineSequence),
                typeof(MoveCursorPreviousLineSequence),
                typeof(MoveCursorToColumn),
                typeof(SetCursorPositionSequence),
                typeof(CharacterPositionAbsoluteSequence),
                typeof(CharacterPositionRelativeSequence),
                typeof(LinePositionAbsoluteSequence),
                typeof(LinePositionRelativeSequence),
                typeof(HorizontalAndVerticalPositionSequence));
        }

        [Test]
        public void When_CursorMove_Up_Command_Is_Executed_CursorMovesUp()
        {
            Screen.SetCursorPosition(new Position(4, 1));
            MoveCursor(1, Direction.Up);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(3));
        }

        [Test]
        public void When_CursorMove_UpMultiple_Command_Is_Executed_CursorMovesUp()
        {
            Screen.SetCursorPosition(new Position(4, 1));
            MoveCursor(3, Direction.Up);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_Up_Command_Is_Executed_At_Screen_Mac_Nothings_Happens()
        {
            MoveCursor(1, Direction.Up);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_Forward_Command_Is_Executed_CursorMovesForward()
        {
            MoveCursor(1, Direction.Forward);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(2));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_MultipleForward_Command_Is_Executed_CursorMovesForward()
        {
            MoveCursor(3, Direction.Forward);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(4));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_Forward_Command_Is_Executed_AtEdge_Moved_To_Next_Line()
        {
            Screen.SetCursorPosition(new Position(1, 10));
            MoveCursor(1, Direction.Forward);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(2));
        }

        [Test]
        public void When_CursorMove_Forward_Command_Is_Executed_At_Screen_Max_Nothing_Happens()
        {
            Screen.SetCursorPosition(new Position(10, 10));
            MoveCursor(1, Direction.Forward);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(10));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(10));
            LogAssert.Expect(LogType.Warning, new Regex(""));
        }

        [Test]
        public void When_CursorMove_Back_Command_Is_Executed_CursorMovesBack()
        {
            Screen.SetCursorPosition(new Position(1, 2));
            MoveCursor(1, Direction.Back);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_MultipleBack_Command_Is_Executed_CursorMovesBack()
        {
            Screen.SetCursorPosition(new Position(2, 4));
            MoveCursor(3, Direction.Back);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(2));
        }

        [Test]
        public void When_CursorMove_Back_Command_Is_Executed_At_Screen_Minimum_NothingHappens()
        {
            MoveCursor(1, Direction.Back);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_Back_Command_Is_Executed_AtEdge_Moves_to_Previous_Line()
        {
            SetCursorPosition(2, 1);
            MoveCursor(1, Direction.Back);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(10));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_Down_Command_Is_Executed_CursorMovesDown()
        {
            MoveCursor(1, Direction.Down);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(2));
        }

        [Test]
        public void When_CursorMove_MultipleDown_Command_Is_Executed_CursorMovesDown()
        {
            Screen.SetCursorPosition(new Position(4, 2));
            MoveCursor(3, Direction.Down);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(2));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(7));
        }

        [Test]
        public void When_CursorMove_Down_Command_Is_Executed_AtEdge_NothingHappens()
        {
            Screen.SetCursorPosition(new Position(10, 1));
            MoveCursor(1, Direction.Down);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(10));
        }

        [Test]
        public void When_CursorMove_NextLine_IsExecuted_Cursor_Moves_To_Start_Down()
        {
            Screen.SetCursorPosition(new Position(1, 2));
            MoveCursor(1, true);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(2));
        }

        [Test]
        public void When_CursorMove_NextLine_Multiple_IsExecuted_Cursor_Moves_To_Start_Down()
        {
            Screen.SetCursorPosition(new Position(2, 5));
            MoveCursor(2, true);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(4));
        }

        [Test]
        public void When_CursorMove_PreviousLine_IsExecuted_Cursor_Moves_To_Start_Up()
        {
            Screen.SetCursorPosition(new Position(2, 2));
            MoveCursor(1, false);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(1));
        }

        [Test]
        public void When_CursorMove_PreviousLine_Multiple_IsExecuted_Cursor_Moves_To_Start_Up()
        {
            Screen.SetCursorPosition(new Position(4, 4));
            MoveCursor(2, false);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(2));
        }

        [Test]
        public void When_CursorMove_ToColumn_IsExecuted_Cursor_Moves_To_Column_On_Same_Line()
        {
            Screen.SetCursorPosition(new Position(5, 5));
            MoveToColumn(1);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(1));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(5));
        }

        [Test]
        public void When_CursorMove_ToColumn_Multiple_IsExecuted_Cursor_Moves_To_Column_N_On_Same_Line()
        {
            Screen.SetCursorPosition(new Position(5, 3));
            MoveToColumn(6);
            Assert.That(Screen.Cursor.Position.Column, Is.EqualTo(6));
            Assert.That(Screen.Cursor.Position.Row, Is.EqualTo(5));
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
            return new int[] { Screen.Cursor.Position.Row, Screen.Cursor.Position.Column };
        }

        [Test]
        public void CharacterPositionAbsoluteSequence_Moves_Cursor_To_Column_1_In_CurrentRow_By_Default()
        {
            Screen.SetCursorPosition(new Position(1, 4));
            Decode($"{Escape}`");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(1, 1)));
        }

        [TestCase(2)]
        [TestCase(4)]
        [TestCase(10)]
        public void CharacterPositionAbsoluteSequence_Moves_Cursor_To_Column_In_CurrentRow(int targetColumn)
        {
            Decode($"{Escape}{targetColumn}`");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(1, targetColumn)));
        }

        [TestCase(0)]
        [TestCase(11)]
        public void CharacterPositionAbsoluteSequence_Logs_Warning_When_Out_Of_Bounds(int targetColumn)
        {
            Decode($"{Escape}{targetColumn}`");
            LogAssert.Expect(LogType.Warning, new Regex(""));
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(1, 1)));
        }

        [Test]
        public void CharacterPositionRelativeeSequence_Moves_Cursor_To_Column_1_In_CurrentRow_By_Default()
        {
            Screen.SetCursorPosition(new Position(1, 4));
            Decode($"{Escape}a");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(1, 5)));
        }

        [TestCase(2, 2, 4)]
        [TestCase(4, 4, 8)]
        [TestCase(1, 9, 10)]
        public void CharacterPositionRelativeSequence_Moves_Cursor_To_Column_In_CurrentRow(int startColumn,
            int targetColumn, int expectedColumn)
        {
            Screen.SetCursorPosition(new Position(1, startColumn));
            Decode($"{Escape}{targetColumn}a");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(1, expectedColumn)));
        }

        [TestCase(0)]
        [TestCase(11)]
        public void CharacterPositionRelativeSequence_Logs_Warning_When_Out_Of_Bounds(int targetColumn)
        {
            Decode($"{Escape}{targetColumn}`");
            LogAssert.Expect(LogType.Warning, new Regex(""));
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(1, 1)));
        }

        [Test]
        public void LinePositionAbsoluteSequence_Moves_Cursor_To_Row_1_By_Default()
        {
            Screen.SetCursorPosition(new Position(4, 4));
            Decode($"{Escape}d");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(1, 4)));
        }

        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(11, 1)]
        public void LinePositionAbsoluteSequence_Moves_Cursor_To_Absolute_Row(int row, int expectedRow)
        {
            Decode($"{Escape}{row}d");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(expectedRow, 1)));
        }

        [Test]
        public void LinePositionRelativeSequence_Moves_Cursor_By_1_Row_By_Default()
        {
            Screen.SetCursorPosition(new Position(4, 4));
            Decode($"{Escape}e");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(5, 4)));
        }

        [TestCase(2, 3)]
        [TestCase(3, 4)]
        [TestCase(11, 1)]
        public void LinePositionRelativeSequence_Moves_Cursor_By_AddingRows(int row, int expectedRow)
        {
            Decode($"{Escape}{row}e");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(expectedRow, 1)));
        }

        [Test]
        public void HorizontalAndVerticalPositionSequence_Sets_Cursor_Position_To_1_Row_And_1_Column_By_Default()
        {
            Screen.SetCursorPosition(new Position(2, 2));
            Decode($"{Escape}f");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(1, 1)));

            Screen.SetCursorPosition(new Position(2, 2));
            Decode($"{Escape}1f");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(1, 1)));

            Screen.SetCursorPosition(new Position(2, 2));
            Decode($"{Escape};1f");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(1, 1)));
        }

        [TestCase(2, 2, 2, 2)]
        [TestCase(4, 2, 4, 2)]
        [TestCase(11, 11, 1, 1)]
        public void HorizontalAndVerticalPositionSequence_Sets_Cursor_Position_To_Parameters_Given(int row, int column,
            int expectedRow, int expectedColumn)
        {
            Decode($"{Escape}{row};{column}f");
            Assert.That(Screen.Cursor.Position, Is.EqualTo(new Position(expectedRow, expectedColumn)));
        }

        private void SetCursorPosition(int row, int column, bool forceSeparator = false)
        {
            if (row == 1 && column == 1)
            {
                Decode($"{Escape}H");
            }
            else if (row == 1)
            {
                Decode($"{Escape};{column}H");
            }
            else if (column == 1)
            {
                Decode($"{Escape}{row}{(forceSeparator ? ";" : "")}H");
            }
            else
            {
                Decode($"{Escape}{row};{column}H");
            }
        }

        private void MoveToColumn(int column)
        {
            Decode($"{Escape}{(column == 1 ? "" : $"{column}")}G");
        }

        private void MoveCursor(int columns, bool next)
        {
            Decode($"{Escape}{(columns == 1 ? "" : $"{columns}")}{(next ? 'E' : 'F')}");
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
            Decode($"{Escape}{(cells == 1 ? "" : $"{cells}")}{dir}");
        }
    }
}