namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal readonly struct Character : ICharacter
    {
        public char Char { get; }
        public GraphicAttributes GraphicAttributes { get; }
        public bool IsProtected { get; }

        internal Character(GraphicAttributes graphicAttributes, bool isProtected = false, char c = '\0')
        {
            GraphicAttributes = graphicAttributes;
            Char = c;
            IsProtected = isProtected;
        }

        internal static ICharacter Invalid()
        {
            return new Character(default);
        }

        public override string ToString()
        {
            return $"{Char}";
        }

        public static implicit operator char(Character ic) => ic.Char;
    }
}