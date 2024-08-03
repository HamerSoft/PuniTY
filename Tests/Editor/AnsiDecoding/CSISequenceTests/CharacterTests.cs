using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.Characters;
using HamerSoft.PuniTY.AnsiEncoding.Line;
using NUnit.Framework;

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
                CreateSequence(typeof(InsertCharacterSequence), typeof(InsertLineSequence)));
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

        [TestCase("1")]
        [TestCase("2")]
        [TestCase("10")]
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
        public void InsertCharacterSequence_Stops_InsertingCharacters_Once_GreaterThan_Buffer()
        {
            Assert.DoesNotThrow(() => { Decode($"{Escape}10000@"); });
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
    }
}