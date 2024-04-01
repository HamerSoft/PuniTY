namespace HamerSoft.PuniTY.AnsiEncoding
{
    public interface ICursor
    {
        public Position Position { get; }
        
        internal void SetPosition(Position position);
    }
}