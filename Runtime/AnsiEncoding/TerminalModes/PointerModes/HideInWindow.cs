namespace HamerSoft.PuniTY.AnsiEncoding.PointerModes
{
    internal class HideInWindow : IPointerMode
    {
        public PointerMode Mode => PointerMode.AlwaysHideInWindow;

        void IPointerMode.Apply(IPointer pointer, Rect bounds)
        {
            if (bounds.Contains(pointer.Position))
                pointer.Show();
            else
                pointer.Hide();
        }
    }
}