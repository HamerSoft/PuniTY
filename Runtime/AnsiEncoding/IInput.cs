using HamerSoft.PuniTY.AnsiEncoding;

namespace AnsiEncoding
{
    public interface IInput
    {
        public IPointer Pointer { get; }
        public IKeyboard KeyBoard { get; }
    }
}