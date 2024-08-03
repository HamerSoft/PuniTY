namespace HamerSoft.PuniTY.AnsiEncoding
{
    public struct Character : ICharacter
    {
        public char Char { get; private set; }
        public GraphicAttributes GraphicAttributes { get; }

        public Character(GraphicAttributes graphicAttributes, char c = '\0')
        {
            GraphicAttributes = graphicAttributes;
            Char = c;
        }

        public override string ToString()
        {
            return $"{Char}";
        }

        public static implicit operator char(Character ic) => ic.Char;
    }
}