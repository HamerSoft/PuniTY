using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public abstract class TransmitSequence : CSISequence
    {
        protected const string Escape = "\x001b[";

        protected byte[] ToBytes(string toTransmit)
        {
            byte[] data = new byte[toTransmit.Length];
            int i = 0;
            foreach (char c in toTransmit)
            {
                data[i] = (byte)c;
                i++;
            }

            return data;
        }
    }
}