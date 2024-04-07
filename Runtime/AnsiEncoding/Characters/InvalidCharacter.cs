namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal struct InvalidCharacter : ICharacter
    {
        public char Char => ' ';
        public bool IsValid => false;

        public override string ToString()
        {
            return $"{Char}";
        }
    }
}