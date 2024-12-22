using System.Numerics;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;

namespace Hamersoft.PuniTY.Editor.UI
{
    public class EditorPointer : Pointer
    {
        public EditorPointer(IPointerMode mode, Vector2 position, Rect bounds) : base(mode, position, bounds)
        {
        }
    }
}