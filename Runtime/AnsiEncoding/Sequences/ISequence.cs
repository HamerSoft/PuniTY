namespace HamerSoft.PuniTY.AnsiEncoding
{
    // /// <summary>
    // /// Command types as per https://en.wikipedia.org/wiki/ANSI_escape_code
    // /// </summary>
    // internal enum SequenceType
    // {
    //     Fe = 0,
    //     CSI = 1,
    //     SGR = 2,
    //     OSC = 3,
    //     Fs = 4,
    //     Fp = 5,
    //     nF = 6
    // }

    // internal interface ISequence
    // {
    //     // fe
    //     private const byte feStart = 0x40;
    //     private const byte feEnd = 0x5F;
    //
    //     // CSI
    //     private const byte csiStart = 0x20;
    //     private const byte csiEnd = 0x2F;
    //     private const byte csiFinalStart = 0x40;
    //     private const byte csiFinalEnd = 0x7E;
    //             
    //
    //     // Fs
    //     private const byte fsStart = 0x60;
    //     private const byte fsEnd = 0x7E;
    //
    //     // Fp
    //     private const byte fpStart = 0x30;
    //     private const byte fpEnd = 0x3F;
    //
    //     // nF
    //     private const byte nfStart = 0x20;
    //     private const byte nfEnd = 0x2F;
    //     private const byte nfFinalStart = 0x30;
    //     private const byte nfFinalEnd = 0x7E;
    //         
    //
    //     public static SequenceType ParseSequenceType(byte[] b)
    //     {
    //         bool inRange(byte start, byte end)
    //         {
    //             return b[0] >= start && b[0] <= end;
    //         }
    //
    //         if ()
    //     }
    // }


    public enum SequenceType
    {
        ESC = 0, // sequence starting with ESC (\x1B)
        CSI = 1, // Control Sequence Introducer: sequence starting with ESC [ or CSI (\x9B)
        DCS = 2, // Device Control String: sequence starting with ESC P or DCS (\x90)
        OSC = 3 //  Operating System Command: sequence starting with ESC ] or OSC (\x9D)
    }

    public interface ISequence
    {
        public SequenceType SequenceType { get; }
        public char Command { get; }
        public void Execute(IScreen screen, string parameters);
    }
}