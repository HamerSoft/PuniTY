using System;
using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    [TestFixture]
    public class PositionTests : AnsiDecoderTest
    {
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(5, 10);
        }

        [TestCase(1, 1, 1, 2, 1)]
        [TestCase(1, 1, 1, 10, 9)]
        [TestCase(1, 1, 2, 1, 10)]
        [TestCase(1, 1, 5, 5, 44)]
        public void Position_AddColumns_Moves_Position_Forward_Also_Considering_Rows(int startRow, int startColumn,
            int expectedRow, int expectedColumn, int columnsToAdd)
        {
            Screen.SetCursorPosition(new Position(startRow, startColumn));
            var actualPosition = Screen.Cursor.Position.AddColumns(Screen, columnsToAdd);
            Assert.That(actualPosition, Is.EqualTo(new Position(expectedRow, expectedColumn)));
        }

        [Test]
        public void Position_Operator_Tests()
        {
            throw new NotImplementedException("add tests for operators");
        }
    }
}