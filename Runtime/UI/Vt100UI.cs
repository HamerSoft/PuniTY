using System;
using System.Windows.Forms;
using HamerSoft.PuniTY.ThirdParty.VT100Adapter;
using libVT100;
using UnityEngine;
using UnityEngine.UIElements;

namespace HamerSoft.PuniTY.UI
{
    internal class Vt100UI
    {
        private readonly Size _dimensions;
        private DynamicScreen _screen;
        private IAnsiDecoder _vt100;

        public event Action<Rect> MarkAsDirty;
        private Size _charSize;
        private int _border;
        private int _lineNumberWidth;

        internal Vt100UI(Size dimensions, int border, int lineNumberWidth, Size charSize)
        {
            _charSize = charSize;
            _lineNumberWidth = _charSize.Width * 5;
            _dimensions = dimensions;

            _screen = new DynamicScreen(300);
            _screen.TabSpaces = 4;
            _vt100 = new AnsiDecoder();
            _vt100.Encoding = System.Text.Encoding.UTF8;
            _vt100.Subscribe(_screen);
            _screen.CursorPosition = new Point(0, 0);
        }

        internal void DetectKeys(KeyUpEvent e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            // Get current cursor
            var rect = GetCursorRect(_screen.CursorPosition);
            //rect.Inflate(1, 1);
            var key = KeyConverter.ConvertKey(e.keyCode);
            switch (key)
            {
                case Keys.Home:
                    Invalidate(rect);
                    (_screen as IAnsiDecoderClient).MoveCursorTo(null, new Point(0, 0));
                    rect = GetCursorRect(_screen.CursorPosition);
                    //rect.Inflate(1, 1);
                    Invalidate(rect);
                    break;

                case Keys.End:
                    Invalidate(rect);
                    (_screen as IAnsiDecoderClient).MoveCursorTo(null,
                        new Point(_screen.Width - 1, _screen.Height - 1));
                    rect = GetCursorRect(_screen.CursorPosition);
                    //rect.Inflate(1, 1);
                    Invalidate(rect);
                    break;


                case Keys.Down:
                    if (_screen.CursorPosition.Y < _screen.Lines.Count)
                    {
                        Invalidate(rect);
                        (_screen as IAnsiDecoderClient).MoveCursor(null, Direction.Down, 1);
                        rect = GetCursorRect(_screen.CursorPosition);
                        // rect.Inflate(1, 1);
                        Invalidate(rect);
                    }

                    break;

                case Keys.Up:
                    if (_screen.CursorPosition.Y > 0)
                    {
                        Invalidate(rect);
                        (_screen as IAnsiDecoderClient).MoveCursor(null, Direction.Up, 1);
                        rect = GetCursorRect(_screen.CursorPosition);
                        //rect.Inflate(1, 1);
                        Invalidate(rect);
                    }

                    break;

                case Keys.Right:
                    if (_screen.CursorPosition.Y < _screen.Width)
                    {
                        Invalidate(rect);
                        (_screen as IAnsiDecoderClient).MoveCursor(null, Direction.Forward, 1);
                        rect = GetCursorRect(_screen.CursorPosition);
                        //rect.Inflate(1, 1);
                        Invalidate(rect);
                    }

                    break;

                case Keys.Left:
                    if (_screen.CursorPosition.X > 0)
                    {
                        Invalidate(rect);
                        (_screen as IAnsiDecoderClient).MoveCursor(null, Direction.Backward, 1);
                        rect = GetCursorRect(_screen.CursorPosition);
                        //rect.Inflate(1, 1);
                        Invalidate(rect);
                    }
                    else if (_screen.CursorPosition.Y > 0)
                    {
                        Invalidate(rect);
                        (_screen as IAnsiDecoderClient).MoveCursor(null, Direction.Up, 1);
                        (_screen as IAnsiDecoderClient).MoveCursorToColumn(null, _screen.Width - 1);
                        rect = GetCursorRect(_screen.CursorPosition);
                        //rect.Inflate(1, 1);
                        Invalidate(rect);
                    }

                    break;
            }
        }

        private void Invalidate(Rect rect)
        {
            MarkAsDirty?.Invoke(rect);
        }

        private Rect GetCursorRect(Point cursorPosition)
        {
            return new Rect(
                new Vector2(cursorPosition.X * _charSize.Width + _lineNumberWidth + _border + 2,
                    cursorPosition.Y * _charSize.Height + _border + 1),
                new Vector2(_charSize.Width, _charSize.Height - 1));
        }

        public string Parse(byte[]message)
        {
            _vt100.Input(message);
            var output = _screen.ToString();
            return output.Trim();
        }

        public void Stop()
        {
            _screen = null;
            _vt100.Dispose();
        }
    }
}