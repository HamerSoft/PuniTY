using System;
using System.Text.RegularExpressions;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.Characters;
using HamerSoft.PuniTY.AnsiEncoding.Line;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    [TestFixture]
    public class CharacterTests : AnsiDecoderTest
    {
        private const int ScreenRows = 10;
        private const int ScreenColumns = 5;
        private const char DefaultChar = 'a';
        private const char EmptyCharacter = '\0';

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(ScreenRows, ScreenColumns);
            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder,
                CreateSequence(typeof(InsertCharacterSequence),
                    typeof(EraseCharacterSequence),
                    typeof(InsertLineSequence),
                    typeof(DeleteLineSequence),
                    typeof(RepeatPrecedingGraphicCharacter)));
            PopulateScreen();
        }

        private void PopulateScreen()
        {
            for (int j = 0; j < ScreenColumns; j++)
            for (int i = 0; i < ScreenRows; i++)
                Screen.AddCharacter(DefaultChar);
            Screen.SetCursorPosition(new Position(1, 1));
        }

        [Test]
        public void InsertCharacterSequence_By_Default_Adds_1_Character()
        {
            Decode($"{Escape}@");
            Assert.That(Screen.GetCharacter(new Position(1, 1)).Char, Is.EqualTo(EmptyCharacter));
        }

        [Test]
        public void EraseCharacterSequence_By_Default_Erases_1_Character()
        {
            Decode($"{Escape}X");
            Assert.That(Screen.GetCharacter(new Position(1, 1)).Char, Is.EqualTo(EmptyCharacter));
        }

        [TestCase("1")]
        [TestCase("2")]
        [TestCase("5")]
        [TestCase("7")]
        [TestCase("10")]
        [TestCase("13")]
        public void InsertCharacterSequence_Inserts_Chars_EqualTo_Parameter(string parameter)
        {
            int chars = int.Parse(parameter);
            int currentRow = 1;
            int currentColumn = 1;
            Decode($"{Escape}{parameter}@");
            for (int i = 1; i <= chars; i++)
            {
                Assert.That(Screen.GetCharacter(new Position(currentRow, currentColumn)).Char,
                    Is.EqualTo(EmptyCharacter));
                if (i == ScreenColumns)
                {
                    currentColumn = 1;
                    currentRow++;
                }
            }
        }

        [Test]
        public void InsertCharacterSequence_Increases_Buffer()
        {
            Decode($"{Escape}1@");
            Screen.Scroll(1, Direction.Up);
            int row = ScreenRows;
            int column = 1;
            var expectedCharacter = DefaultChar;
            do
            {
                var actual = Screen.GetCharacter(new Position(row, column)).Char;
                Assert.That(actual, Is.EqualTo(expectedCharacter));
                expectedCharacter = EmptyCharacter;
                column++;
            } while (column <= ScreenColumns);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(7)]
        [TestCase(10)]
        [TestCase(13)]
        public void EraseCharacterSequence_Resets_Characters_Equal_To_Parameter(int charactersToDelete)
        {
            int currentRow = 1;
            int currentColumn = 1;
            Decode($"{Escape}{charactersToDelete}X");
            for (int i = 1; i <= charactersToDelete; i++)
            {
                Assert.That(Screen.GetCharacter(new Position(currentRow, currentColumn)).Char,
                    Is.EqualTo(EmptyCharacter));
                if (i == ScreenColumns)
                {
                    currentColumn = 1;
                    currentRow++;
                }
            }
        }

        [Test]
        public void EraseCharacterSequence_Stops_ErasingCharacters_Once_GreaterThan_Buffer()
        {
            Assert.DoesNotThrow(() => { Decode($"{Escape}10000X"); });
        }

        [Test]
        public void InsertCharacterSequence_Stops_InsertingCharacters_Once_GreaterThan_Buffer()
        {
            Assert.DoesNotThrow(() => { Decode($"{Escape}10000@"); });
        }

        [Test]
        public void InsertLineSequence_Inserts_1_Line_From_Cursor_Starting_Down_By_Default()
        {
            Screen.Cursor.SetPosition(new Position(1, 1));
            Decode($"{Escape}L");
            PrintScreen();
            for (int i = 1; i <= ScreenColumns; i++)
            for (int j = 1; j <= 1; j++)
            {
                var actual = Screen.GetCharacter(new Position(j, i));
                Assert.That(actual.Char, Is.EqualTo(EmptyCharacter));
            }
        }

        [TestCase(1, 1)]
        [TestCase(1, 3)]
        [TestCase(10, 3)]
        public void InsertLineSequence_Inserts_Line_From_Cursor_Starting_Down(int startRow, int linesToInsert)
        {
            Screen.Cursor.SetPosition(new Position(startRow, 1));
            Decode($"{Escape}{linesToInsert}L");
            PrintScreen();
            for (int i = 1; i <= ScreenColumns; i++)
            for (int j = startRow; j <= linesToInsert; j++)
            {
                var actual = Screen.GetCharacter(new Position(j, i));
                Assert.That(actual.Char, Is.EqualTo(EmptyCharacter));
            }
        }

        [Test]
        public void DeleteLineSequence_Deletes_1_Line_From_Cursor_Starting_Down_By_Default()
        {
            Screen.Cursor.SetPosition(new Position(1, 1));
            Decode($"{Escape}M");
            for (int i = 1; i <= ScreenColumns; i++)
            for (int j = 1; j <= ScreenRows; j++)
            {
                var actual = Screen.GetCharacter(new Position(j, i));
                if (j < ScreenRows)
                    Assert.That(actual.Char, Is.EqualTo(DefaultChar));
                else
                    Assert.That(actual.Char, Is.EqualTo(EmptyCharacter));
            }
        }

        [TestCase(1, 1)]
        [TestCase(1, 3)]
        public void DeleteLineSequence_Deletes_Line_From_Cursor_Starting_Down(int startRow, int linesToDelete)
        {
            Screen.Cursor.SetPosition(new Position(startRow, 1));
            Decode($"{Escape}{linesToDelete}M");
            PrintScreen();
            for (int i = 1; i <= ScreenColumns; i++)
            for (int j = 1; j <= ScreenRows; j++)
            {
                var actual = Screen.GetCharacter(new Position(j, i));
                if (j <= ScreenRows - linesToDelete)
                    Assert.That(actual.Char, Is.EqualTo(DefaultChar));
                else
                    Assert.That(actual.Char, Is.EqualTo(EmptyCharacter));
            }
        }

        [Test]
        public void DeleteLineSequence_Deletes_Only_Lines_Within_Bounds()
        {
            Screen.Cursor.SetPosition(new Position(10, 1));
            Decode($"{Escape}{2}M");
            PrintScreen();
            for (int i = 1; i <= ScreenColumns; i++)
            for (int j = 1; j <= ScreenRows; j++)
            {
                var actual = Screen.GetCharacter(new Position(j, i));
                if (j < ScreenRows)
                    Assert.That(actual.Char, Is.EqualTo(DefaultChar));
                else
                    Assert.That(actual.Char, Is.EqualTo(EmptyCharacter));
            }
        }

        [Test]
        public void RepeatPrecedingGraphicCharacter_Repeats_By_1_By_Default()
        {
            Screen.AddCharacter('A');
            Decode($"{Escape}b");
            Assert.That(Screen.GetCharacter(new Position(1, 2)).Char, Is.EqualTo('A'));
        }

        [Test]
        public void RepeatPrecedingGraphicCharacter_Logs_Warning_When_At_StartPosition()
        {
            Decode($"{Escape}b");
            LogAssert.Expect(LogType.Warning, new Regex(""));
            Assert.That(Screen.GetCharacter(new Position(1, 2)).Char, Is.EqualTo('a'));
        }

        [TestCase(3)]
        [TestCase(8)]
        [TestCase(49)]
        public void RepeatPrecedingGraphicCharacter_Repeats_Character_By_Given_Parameter(int repeats)
        {
            Screen.AddCharacter('A');
            Decode($"{Escape}{repeats}b");
            PrintScreen();
            int index = 0;
            foreach (var character in new ScreenIterator(Screen))
            {
                Assert.That(character.Char, index <= repeats ? Is.EqualTo('A') : Is.EqualTo(DefaultChar));
                index++;
            }
        }
    }
}