using HamerSoft.PuniTY.AnsiEncoding;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

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
        private readonly IScreen _screen;

        public CellReportStrategy(IPointer pointer, IScreen screen) : base(pointer)
        {
            _screen = screen;
        }

        public override Vector2 GetPosition()
        {
            var cell = new Vector2(
                Mathf.CeilToInt(Pointer.Position.X / _screen.ScreenConfiguration.FontDimensions.Width),
                Mathf.CeilToInt(Pointer.Position.Y / _screen.ScreenConfiguration.FontDimensions.Height));

            if (cell.X <= 0 ||
                cell.Y <= 0 ||
                cell.X > _screen.Columns ||
                cell.Y > _screen.Rows)
                return Vector2.Zero;
            return cell;
        }
    }
}