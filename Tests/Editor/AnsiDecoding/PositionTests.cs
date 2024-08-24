using System;
using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;
using UnityEngine;

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
            // ==
            Assert.IsTrue(new Position(1, 1) == new Position(1, 1));
            Assert.IsFalse(new Position(1, 1) == new Position(1, 2));
            // !=
            Assert.IsTrue(new Position(1, 2) != new Position(1, 1));
            Assert.IsFalse(new Position(1, 1) != new Position(1, 1));
            // <
            Assert.IsTrue(new Position(1, 1) < new Position(1, 2));
            Assert.IsTrue(new Position(1, 1) < new Position(2, 1));
            Assert.IsFalse(new Position(1, 1) < new Position(1, 1));
            Assert.IsFalse(new Position(2, 1) < new Position(1, 1));
            Assert.IsFalse(new Position(2, 4) < new Position(2, 2));

            // >
            Assert.IsTrue(new Position(2, 1) > new Position(1, 2));
            Assert.IsTrue(new Position(2, 2) > new Position(2, 1));
            Assert.IsFalse(new Position(1, 1) > new Position(1, 1));
            Assert.IsFalse(new Position(1, 1) > new Position(1, 2));
            Assert.IsFalse(new Position(2, 4) > new Position(4, 2));

            // <=
            Assert.IsTrue(new Position(1, 1) <= new Position(1, 2));
            Assert.IsTrue(new Position(1, 1) <= new Position(2, 1));
            Assert.IsTrue(new Position(1, 1) <= new Position(1, 1));
            Assert.IsFalse(new Position(2, 1) <= new Position(1, 1));
            Assert.IsFalse(new Position(2, 4) <= new Position(2, 2));

            // >=
            Assert.IsTrue(new Position(2, 1) >= new Position(1, 2));
            Assert.IsTrue(new Position(2, 2) >= new Position(2, 1));
            Assert.IsTrue(new Position(1, 1) >= new Position(1, 1));
            Assert.IsFalse(new Position(1, 1) >= new Position(1, 2));
            Assert.IsFalse(new Position(2, 4) >= new Position(4, 2));
        }

        [Test]
        public void Positions_Add_Sums_Positions_Based_OnScreen_Dimensions()
        {
            Assert.That(new Position(1, 1).Add(Screen, new Position(0, 4)), Is.EqualTo(new Position(1, 5)));
            Assert.That(new Position(1, 1).Add(Screen, new Position(1, 0)), Is.EqualTo(new Position(2, 1)));
            Assert.That(new Position(1, 1).Add(Screen, new Position(0, 12)), Is.EqualTo(new Position(2, 3)));
            Assert.That(new Position(1, 1).Add(Screen, new Position(2, 4)), Is.EqualTo(new Position(3, 5)));
            Assert.That(new Position(1, 7).Add(Screen, new Position(2, 4)), Is.EqualTo(new Position(4, 1)));
        }

        [TestCase(1, 1, 2)]
        [TestCase(9, 1, 10)]
        [TestCase(10, 2, 1)]
        [TestCase(45, 5, 6)]
        public void Position_AddColumns_Adds_Columns_And_Rows_Based_On_Screen_Dimensions(int columnsToAdd,
            int expectedRow, int expectedColumn)
        {
            Assert.That(new Position(1, 1).AddColumns(Screen, columnsToAdd),
                Is.EqualTo(new Position(expectedRow, expectedColumn)));
        }

        [Test]
        public void Position_CanBe_Created_With_Tuple()
        {
            Assert.That(new Position(3, 4), Is.EqualTo(new Position((3, 4))));
        }

        [Test]
        public void Position_CanBe_Created_With_Vector2Int()
        {
            Assert.That(new Position(3, 4), Is.EqualTo(new Position(new Vector2Int(4, 3))));
        }

        [Test]
        public void Position_With_Column_Only_Changes_Column()
        {
            Assert.That(new Position(2, 8), Is.EqualTo(new Position(2, 1).WithColumn(8)));
        }

        [Test]
        public void Position_With_Row_Only_Changes_Row()
        {
            Assert.That(new Position(5, 8), Is.EqualTo(new Position(2, 8).WithRow(5)));
        }
    }
}