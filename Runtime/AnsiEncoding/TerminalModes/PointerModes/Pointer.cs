using Vector2 = System.Numerics.Vector2;

namespace HamerSoft.PuniTY.AnsiEncoding.PointerModes
{
    public abstract class Pointer : IPointer
    {
        private Vector2 _position;
        private Rect _bounds;
        private IPointerMode _mode;
        public Vector2 Position { get; private set; }
        public bool IsActive { get; private set; }

        protected Pointer(IPointerMode mode, Vector2 position, Rect bounds)
        {
            _mode = mode;
            _position = position;
            _bounds = bounds;
        }

        public void SetPosition(Vector2 position, Rect bounds)
        {
            if (_position == Position && _bounds == bounds)
                return;

            _position = position;
            _bounds = bounds;

            _mode.Apply(this, bounds);
        }

        void IPointer.Show()
        {
            IsActive = true;
        }

        void IPointer.Hide()
        {
            IsActive = false;
        }

        void IPointer.SetMode(IPointerMode mode)
        {
            _mode = mode;
            mode.Apply(this, _bounds);
        }
    }
}