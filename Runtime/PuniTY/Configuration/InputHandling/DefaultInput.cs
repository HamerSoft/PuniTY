using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding;

namespace HamerSoft.PuniTY.Configuration
{
    public class DefaultInput : IInput
    {
        public IPointer Pointer { get; }
        public IKeyboard KeyBoard { get; }

        public DefaultInput(IPointer pointer, IKeyboard keyboard)
        {
            Pointer = pointer;
            KeyBoard = keyboard;
        }
    }
}