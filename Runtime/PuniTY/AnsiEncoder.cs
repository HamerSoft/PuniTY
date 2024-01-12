using System.Text;
using System.Text.RegularExpressions;

namespace HamerSoft.PuniTY
{
    public class AnsiEncoder : IEncoder
    {
        private readonly UTF8Encoding _encoding;
        private readonly Regex _ansiRegex;

        public AnsiEncoder()
        {
            _encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
            _ansiRegex = new Regex(
                @"[\u001B\u009B][[\]()#;?]*(?:(?:(?:[a-zA-Z\d]*(?:;[a-zA-Z\d]*)*)?\u0007)|(?:(?:\d{1,4}(?:;\d{0,4})*)?[\dA-PRZcf-ntqry=><~]))");
        }

        public byte[] Write(string message)
        {
            return Encoding.ASCII.GetBytes(message);
        }

        public string Read(byte[] message)
        {
            var output = _encoding.GetString(message, 0, message.Length);
            return _ansiRegex.Replace(output, string.Empty);
        }
    }
}