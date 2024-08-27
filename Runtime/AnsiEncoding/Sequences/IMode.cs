namespace HamerSoft.PuniTY.AnsiEncoding
{
    public interface IMode
    {
        public AnsiMode Mode { get; }
        public void SetMode(AnsiMode mode);
        public void ResetMode(AnsiMode mode);
        public bool HasMode(AnsiMode mode);
    }
}