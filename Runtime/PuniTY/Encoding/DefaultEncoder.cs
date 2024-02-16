using libVT100;

namespace HamerSoft.PuniTY.Encoding
{
    internal class DefaultEncoder : IEncoder
    {
        private readonly System.Text.Encoding _encoding;

        public DefaultEncoder(System.Text.Encoding encoding)
        {
            _encoding = encoding;
        }

        public byte[] Write(string message)
        {
            return _encoding.GetBytes(message);
        }

        public string Read(byte[] message)
        {
            return _encoding.GetString(message, 0, message.Length);
        }
    }
}