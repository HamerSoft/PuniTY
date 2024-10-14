using System.Numerics;
using HamerSoft.PuniTY.AnsiEncoding;

namespace AnsiEncoding.Input
{
    internal abstract class PointerReportStrategy
    {
        protected readonly IPointer Pointer;

        public PointerReportStrategy(IPointer pointer)
        {
            Pointer = pointer;
        }

        public abstract Vector2 GetPosition();
    }

    internal class PixelReportStrategy : PointerReportStrategy
    {
        public PixelReportStrategy(IPointer pointer) : base(pointer)
        {
        }

        public override Vector2 GetPosition()
        {
            return Pointer.Position;
        }
    }

    internal class CellReportStrategy : PointerReportStrategy
    {
        public CellReportStrategy(IPointer pointer) : base(pointer)
        {
        }

        public override Vector2 GetPosition()
        {
            return Vector2.Zero;
        }
    }
}