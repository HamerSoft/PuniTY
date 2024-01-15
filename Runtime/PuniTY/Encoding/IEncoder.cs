namespace HamerSoft.PuniTY.Encoding
{
    public interface IEncoder
    {
        public byte[] Write(string message);
        public string Read(byte[] message);
    }
}