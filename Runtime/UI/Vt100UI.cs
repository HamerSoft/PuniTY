using System;
using HamerSoft.PuniTY.ThirdParty.VT100Adapter;
using libVT100;

namespace HamerSoft.PuniTY.UI
{
    public class Vt100UI : ITerminalUI
    {
        private readonly Size _dimensions;
        public event Action Closed;
        public event Action<string> Written;
        public event Action<byte[]> WrittenByte;
        public event Action<string> WrittenLine;

        private DynamicScreen _screen;
        private IAnsiDecoder _vt100;

        private Size _charSize;
        private int _border;
        private int _lineNumberWidth;

        public Vt100UI(Size dimensions, int border, int lineNumberWidth, Size charSize)
        {
            _charSize = charSize;
            _lineNumberWidth = _charSize.Width * 5;
            _dimensions = dimensions;
            _screen.TabSpaces = 4;

            _vt100 = new AnsiDecoder();
            _screen = new DynamicScreen((dimensions.Width - _lineNumberWidth - (_border * 2)) / _charSize.Width);
            _vt100.Encoding = System.Text.Encoding.UTF8;
            _vt100.Subscribe(_screen);
            _screen.CursorPosition = new Point(0, 0);

            Invalidate();
        }

        public void Print(string message)
        {
        }
    }
}