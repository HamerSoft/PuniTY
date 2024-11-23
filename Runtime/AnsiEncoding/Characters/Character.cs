namespace HamerSoft.PuniTY.AnsiEncoding
{
    public readonly struct Character : ICharacter
    {
        public char Char { get;  }
        public Position Position { get; }
        public GraphicAttributes GraphicAttributes { get; }

        public Character(GraphicAttributes graphicAttributes, Position position, char c = '\0')
        {
            Position = position;
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