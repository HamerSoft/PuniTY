namespace HamerSoft.PuniTY.AnsiEncoding
{
    public struct Character : ICharacter
    {
        public char Char { get; private set; }

        public Character(char c = ' ')
        {
            Char = c;
        }
    }
}