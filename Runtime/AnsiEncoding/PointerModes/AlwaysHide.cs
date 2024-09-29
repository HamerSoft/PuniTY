namespace HamerSoft.PuniTY.AnsiEncoding.PointerModes
{
    internal class AlwaysHide : IPointerMode
    {
        public PointerMode Mode => PointerMode.AlwaysHide;

        void IPointerMode.Apply(IPointer pointer, Rect _)
        {
            pointer.Hide();
        }
    }
}