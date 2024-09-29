namespace HamerSoft.PuniTY.AnsiEncoding.PointerModes
{
    internal class NeverHide : IPointerMode
    {
        public PointerMode Mode => PointerMode.NeverHide;

        void IPointerMode.Apply(IPointer pointer, Rect _)
        {
            pointer.Show();
        }
    }
}