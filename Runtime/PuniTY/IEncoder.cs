namespace HamerSoft.PuniTY
{
    public interface IEncoder
    {
        public byte[] Write(string message);
        public string Read(byte[] message);
    }
}