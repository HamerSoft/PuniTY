using AnsiEncoding;
using AnsiEncoding.Input;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;

namespace Tests.Editor.AnsiDecoding.Stubs
{
    public class StubInput : AnsiInput
    {
        public StubInput(Pointer pointer, Keyboard keyboard) : base(pointer, keyboard)
        {
        }
    }
}