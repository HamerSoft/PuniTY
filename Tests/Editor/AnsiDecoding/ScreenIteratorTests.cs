using System;
using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding
{
    [TestFixture]
    public class ScreenIteratorTests : AnsiDecoderTest
    {
        private readonly int _screenRows = 25;
        private readonly int _screenColumns = 80;

        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(_screenRows, _screenColumns);
        }

        [Test]
        public void ScreenIterator_DoesNotThrow_When_Screen_IsNull()
        {
            Assert.DoesNotThrow(() => { new ScreenIterator(null); });
        }

        [Test]
        public void ScreenIterator_Returns_Immediately_When_Screen_Is_Null()
        {
            var iterations = 0;
            foreach (var _ in new ScreenIterator(null))
                iterations++;

            Assert.That(iterations, Is.EqualTo(0));
        }

        [Test]
        public void ScreenIterator_Returns_Immediately_When_EndPosition_IsGreaterThan_StartPosition()
        {
            var iterations = 0;
            foreach (var _ in new ScreenIterator(Screen, new Position(10, 10), new Position(1, 1)))
                iterations++;

            Assert.That(iterations, Is.EqualTo(0));
        }

        [Test]
        public void ScreenIterator_Iterates_Over_Entire_Screen_By_Default()
        {
            var iterations = 0;
            foreach (var _ in new ScreenIterator(Screen))
                iterations++;

            Assert.That(iterations, Is.EqualTo(2000));
        }

        [TestCase(1, 1, 1, 80, 80)]
        [TestCase(1, 50, 4, 20, 211)]
        [TestCase(4, 22, 9, 2, 381)]
        public void ScreenIterator_Iterates_Over_Area_Between_Start_And_EndPosition(int startRow, int startColumn,
            int endRow, int endColumn, int expectedIterations)
        {
            var iterations = 0;
            var iterator = new ScreenIterator(Screen, new Position(startRow, startColumn),
                new Position(endRow, endColumn));
            foreach (var _ in iterator)
                iterations++;

            Assert.That(iterations, Is.EqualTo(expectedIterations));
            Assert.That(iterator.CurrentPosition, Is.EqualTo(new Position(endRow, endColumn)));
        }

        [Test]
        public void ScreenIterator_Cannot_Go_Out_Of_Bounds()
        {
            foreach (var _ in new ScreenIterator(Screen, new Position(25, 79), new Position(25, 81)))
            {
            }
        }
    }
}