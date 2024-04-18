namespace HamerSoft.PuniTY.AnsiEncoding
{
    public abstract class TransmitSequence : Sequence
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