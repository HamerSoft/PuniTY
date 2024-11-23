namespace HamerSoft.PuniTY.AnsiEncoding
{
    public interface ICharacter
    {
        public char Char { get; }
        public Position Position { get; }
        public GraphicAttributes GraphicAttributes { get; }
    }
}