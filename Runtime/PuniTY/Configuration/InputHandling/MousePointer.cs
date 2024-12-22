using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;
using Rect = HamerSoft.PuniTY.AnsiEncoding.Rect;
using Vector2 = System.Numerics.Vector2;

namespace HamerSoft.PuniTY.Configuration
{
    public class MousePointer : Pointer
    {
        public MousePointer(IPointerMode mode, Vector2 position, Rect bounds) : base(mode, position, bounds)
        {
        }
    }
}