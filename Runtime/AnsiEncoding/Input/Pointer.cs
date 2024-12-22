using System;
using Vector2 = System.Numerics.Vector2;

namespace HamerSoft.PuniTY.AnsiEncoding.PointerModes
{
    public abstract class Pointer : IPointer
    {
        private Rect _bounds;
        private IPointerMode _mode;
        private uint _trackingCounter;
        private event Action<MouseButton, bool> KeyPressed;
        private event Action<Vector2> Moved;

        event Action<Vector2> IPointer.Moved
        {
            add => Moved += value;
            remove => Moved -= value;
        }

        public Vector2 Position { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsTrackingEnabled => _trackingCounter > 0;

        protected Pointer(IPointerMode mode, Vector2 position, Rect bounds)
        {
            _mode = mode;
            SetPosition(position, bounds);
        }

        public void SetPosition(Vector2 position, Rect bounds)
        {
            if (position == Position && _bounds == bounds)
                return;

            Position = position;
            _bounds = bounds;
            _mode.Apply(this, bounds);
            Moved?.Invoke(Position);
        }

        public void PressButton(MouseButton mouseButton)
        {
            KeyPressed?.Invoke(mouseButton, true);
        }

        public void Release(MouseButton mouseButton)
        {
            KeyPressed?.Invoke(mouseButton, false);
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

        void IPointer.EnableTracking()
        {
            _trackingCounter++;
            if (_trackingCounter == 1)
                _mode.Apply(this, _bounds);
        }

        void IPointer.DisableTracking()
        {
            Math.Clamp(_trackingCounter--, 0, 20);
            if (_trackingCounter == 0)
                _mode.Apply(this, _bounds);
        }

        event Action<MouseButton, bool> IPointer.ButtonPressed
        {
            add => KeyPressed += value;
            remove => KeyPressed -= value;
        }

        public virtual void Dispose()
        {
        }
    }
}