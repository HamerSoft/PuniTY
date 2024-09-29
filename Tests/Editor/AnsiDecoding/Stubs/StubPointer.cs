using System.Numerics;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.Stubs
{
    internal class StubPointer : Pointer
    {
        public StubPointer(IPointerMode mode, Vector2 position, Rect bounds) : base(mode, position, bounds)
        {
        }
    }
}