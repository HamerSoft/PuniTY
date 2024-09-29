using System;
using System.Numerics;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public enum PointerMode
    {
        None = 0,
        NeverHide = 1,
        HideIfNotTracking = 2,
        AlwaysHideInWindow = 3,
        AlwaysHide = 4
    }

    public readonly struct Rect
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public Rect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"[x: {X}, y: {Y}, width: {Width}, height: {Height}]";
        }

        public override bool Equals(object obj)
        {
            return obj is Rect other
                   && other.X == X
                   && other.Y == Y
                   && other.Width == Width
                   && other.Height == Height;
        }

        public override int GetHashCode()
        {
            return (X, Y, Width, Height).GetHashCode();
        }

        public static bool operator ==(Rect a, object b) => a.Equals(b);
        public static bool operator !=(Rect a, object b) => !(a == b);

        public bool Contains(Vector2 position)
        {
            return position.X >= X
                   && position.Y >= Y
                   && position.X <= Width
                   && position.Y <= Height;
        }
    }

    public interface IPointerable
    {
        public event Action<IPointerMode> PointerModeChanged;
        internal void SetPointerMode(PointerMode mode);
    }

    public interface IPointer
    {
        public Vector2 Position { get; }
        public bool IsActive { get; }

        public void SetPosition(Vector2 position, Rect bounds);
        internal void Show();
        internal void Hide();
        internal void SetMode(IPointerMode mode);
    }

    public interface IPointerMode
    {
        public PointerMode Mode { get; }
        internal void Apply(IPointer pointer, Rect bounds);
    }

    public interface IPointerModeFactory
    {
        internal IPointerMode Create(IScreen screen, PointerMode pointerMode);
    }
}