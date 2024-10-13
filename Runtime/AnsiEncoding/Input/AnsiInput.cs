using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;

namespace AnsiEncoding.Input
{
    public abstract class AnsiInput : IInput
    {
        public IPointer Pointer { get; }
        public IKeyboard KeyBoard { get; }

        public AnsiInput(Pointer pointer, Keyboard keyboard)
        {
            Pointer = pointer;
            KeyBoard = keyboard;
        }
    }
}