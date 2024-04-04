namespace HamerSoft.PuniTY.AnsiEncoding
{
    public struct Character : ICharacter
    {
        public char Char { get; private set; }
        public bool IsValid => true;

        public Character(char c = ' ')
        {
            Char = c;
        }

        public override string ToString()
        {
            return $"{Char}";
        }

        public static implicit operator char(Character ic) => ic.Char;
    }
}