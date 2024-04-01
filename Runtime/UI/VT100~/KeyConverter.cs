using System;
using UnityEngine;

namespace HamerSoft.PuniTY.UI
{
    internal static class KeyConverter
    {
        // public static KeyCode Convert(Keys keys)
        // {
        //     switch (keys)
        //     {
        //         case Keys.KeyCode:
        //             throw new ArgumentException($"KeyCode unknown {keys}")
        //             break;
        //         case Keys.Modifiers:
        //             break;
        //         case Keys.None:
        //             break;
        //         case Keys.LButton:
        //             break;
        //         case Keys.RButton:
        //             break;
        //         case Keys.Cancel:
        //             break;
        //         case Keys.MButton:
        //             break;
        //         case Keys.XButton1:
        //             break;
        //         case Keys.XButton2:
        //             break;
        //         case Keys.Back:
        //             break;
        //         case Keys.Tab:
        //             break;
        //         case Keys.LineFeed:
        //             break;
        //         case Keys.Clear:
        //             break;
        //         case Keys.Return:
        //             break;
        //         case Keys.ShiftKey:
        //             break;
        //         case Keys.ControlKey:
        //             break;
        //         case Keys.Menu:
        //             break;
        //         case Keys.Pause:
        //             break;
        //         case Keys.Capital:
        //             break;
        //         case Keys.KanaMode:
        //             break;
        //         case Keys.JunjaMode:
        //             break;
        //         case Keys.FinalMode:
        //             break;
        //         case Keys.HanjaMode:
        //             break;
        //         case Keys.Escape:
        //             break;
        //         case Keys.IMEConvert:
        //             break;
        //         case Keys.IMENonconvert:
        //             break;
        //         case Keys.IMEAccept:
        //             break;
        //         case Keys.IMEModeChange:
        //             break;
        //         case Keys.Space:
        //             break;
        //         case Keys.Prior:
        //             break;
        //         case Keys.Next:
        //             break;
        //         case Keys.End:
        //             break;
        //         case Keys.Home:
        //             break;
        //         case Keys.Left:
        //             break;
        //         case Keys.Up:
        //             break;
        //         case Keys.Right:
        //             break;
        //         case Keys.Down:
        //             break;
        //         case Keys.Select:
        //             break;
        //         case Keys.Print:
        //             break;
        //         case Keys.Execute:
        //             break;
        //         case Keys.Snapshot:
        //             break;
        //         case Keys.Insert:
        //             break;
        //         case Keys.Delete:
        //             break;
        //         case Keys.Help:
        //             break;
        //         case Keys.D0:
        //             break;
        //         case Keys.D1:
        //             break;
        //         case Keys.D2:
        //             break;
        //         case Keys.D3:
        //             break;
        //         case Keys.D4:
        //             break;
        //         case Keys.D5:
        //             break;
        //         case Keys.D6:
        //             break;
        //         case Keys.D7:
        //             break;
        //         case Keys.D8:
        //             break;
        //         case Keys.D9:
        //             break;
        //         case Keys.A:
        //             break;
        //         case Keys.B:
        //             break;
        //         case Keys.C:
        //             break;
        //         case Keys.D:
        //             break;
        //         case Keys.E:
        //             break;
        //         case Keys.F:
        //             break;
        //         case Keys.G:
        //             break;
        //         case Keys.H:
        //             break;
        //         case Keys.I:
        //             break;
        //         case Keys.J:
        //             break;
        //         case Keys.K:
        //             break;
        //         case Keys.L:
        //             break;
        //         case Keys.M:
        //             break;
        //         case Keys.N:
        //             break;
        //         case Keys.O:
        //             break;
        //         case Keys.P:
        //             break;
        //         case Keys.Q:
        //             break;
        //         case Keys.R:
        //             break;
        //         case Keys.S:
        //             break;
        //         case Keys.T:
        //             break;
        //         case Keys.U:
        //             break;
        //         case Keys.V:
        //             break;
        //         case Keys.W:
        //             break;
        //         case Keys.X:
        //             break;
        //         case Keys.Y:
        //             break;
        //         case Keys.Z:
        //             break;
        //         case Keys.LWin:
        //             break;
        //         case Keys.RWin:
        //             break;
        //         case Keys.Apps:
        //             break;
        //         case Keys.Sleep:
        //             break;
        //         case Keys.NumPad0:
        //             break;
        //         case Keys.NumPad1:
        //             break;
        //         case Keys.NumPad2:
        //             break;
        //         case Keys.NumPad3:
        //             break;
        //         case Keys.NumPad4:
        //             break;
        //         case Keys.NumPad5:
        //             break;
        //         case Keys.NumPad6:
        //             break;
        //         case Keys.NumPad7:
        //             break;
        //         case Keys.NumPad8:
        //             break;
        //         case Keys.NumPad9:
        //             break;
        //         case Keys.Multiply:
        //             break;
        //         case Keys.Add:
        //             break;
        //         case Keys.Separator:
        //             break;
        //         case Keys.Subtract:
        //             break;
        //         case Keys.Decimal:
        //             break;
        //         case Keys.Divide:
        //             break;
        //         case Keys.F1:
        //             break;
        //         case Keys.F2:
        //             break;
        //         case Keys.F3:
        //             break;
        //         case Keys.F4:
        //             break;
        //         case Keys.F5:
        //             break;
        //         case Keys.F6:
        //             break;
        //         case Keys.F7:
        //             break;
        //         case Keys.F8:
        //             break;
        //         case Keys.F9:
        //             break;
        //         case Keys.F10:
        //             break;
        //         case Keys.F11:
        //             break;
        //         case Keys.F12:
        //             break;
        //         case Keys.F13:
        //             break;
        //         case Keys.F14:
        //             break;
        //         case Keys.F15:
        //             break;
        //         case Keys.F16:
        //             break;
        //         case Keys.F17:
        //             break;
        //         case Keys.F18:
        //             break;
        //         case Keys.F19:
        //             break;
        //         case Keys.F20:
        //             break;
        //         case Keys.F21:
        //             break;
        //         case Keys.F22:
        //             break;
        //         case Keys.F23:
        //             break;
        //         case Keys.F24:
        //             break;
        //         case Keys.NumLock:
        //             break;
        //         case Keys.Scroll:
        //             break;
        //         case Keys.LShiftKey:
        //             break;
        //         case Keys.RShiftKey:
        //             break;
        //         case Keys.LControlKey:
        //             break;
        //         case Keys.RControlKey:
        //             break;
        //         case Keys.LMenu:
        //             break;
        //         case Keys.RMenu:
        //             break;
        //         case Keys.BrowserBack:
        //             break;
        //         case Keys.BrowserForward:
        //             break;
        //         case Keys.BrowserRefresh:
        //             break;
        //         case Keys.BrowserStop:
        //             break;
        //         case Keys.BrowserSearch:
        //             break;
        //         case Keys.BrowserFavorites:
        //             break;
        //         case Keys.BrowserHome:
        //             break;
        //         case Keys.VolumeMute:
        //             break;
        //         case Keys.VolumeDown:
        //             break;
        //         case Keys.VolumeUp:
        //             break;
        //         case Keys.MediaNextTrack:
        //             break;
        //         case Keys.MediaPreviousTrack:
        //             break;
        //         case Keys.MediaStop:
        //             break;
        //         case Keys.MediaPlayPause:
        //             break;
        //         case Keys.LaunchMail:
        //             break;
        //         case Keys.SelectMedia:
        //             break;
        //         case Keys.LaunchApplication1:
        //             break;
        //         case Keys.LaunchApplication2:
        //             break;
        //         case Keys.OemSemicolon:
        //             break;
        //         case Keys.Oemplus:
        //             break;
        //         case Keys.Oemcomma:
        //             break;
        //         case Keys.OemMinus:
        //             break;
        //         case Keys.OemPeriod:
        //             break;
        //         case Keys.OemQuestion:
        //             break;
        //         case Keys.Oem3:
        //             break;
        //         case Keys.OemOpenBrackets:
        //             break;
        //         case Keys.OemPipe:
        //             break;
        //         case Keys.OemCloseBrackets:
        //             break;
        //         case Keys.Oem7:
        //             break;
        //         case Keys.Oem8:
        //             break;
        //         case Keys.Oem102:
        //             break;
        //         case Keys.ProcessKey:
        //             break;
        //         case Keys.Packet:
        //             break;
        //         case Keys.Attn:
        //             break;
        //         case Keys.Crsel:
        //             break;
        //         case Keys.Exsel:
        //             break;
        //         case Keys.EraseEof:
        //             break;
        //         case Keys.Play:
        //             break;
        //         case Keys.Zoom:
        //             break;
        //         case Keys.NoName:
        //             break;
        //         case Keys.Pa1:
        //             break;
        //         case Keys.OemClear:
        //             break;
        //         case Keys.Shift:
        //             break;
        //         case Keys.Control:
        //             break;
        //         case Keys.Alt:
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(keys), keys, null);
        //     }
        // }

        public static Keys ConvertKey(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.None:
                    return Keys.None;
                    break;
                case KeyCode.Backspace:
                    return Keys.Back;
                    break;
                case KeyCode.Delete:
                    return Keys.Delete;
                    break;
                case KeyCode.Tab:
                    return Keys.Tab;
                    break;
                case KeyCode.Clear:
                    return Keys.Clear;
                    break;
                case KeyCode.Return:
                    return Keys.Return;
                    break;
                case KeyCode.Pause:
                    return Keys.Pause;
                    break;
                case KeyCode.Escape:
                    return Keys.Escape;
                    break;
                case KeyCode.Space:
                    return Keys.Space;
                    break;
                case KeyCode.Keypad0:
                    return Keys.NumPad0;
                    break;
                case KeyCode.Keypad1:
                    return Keys.NumPad1;
                    break;
                case KeyCode.Keypad2:
                    return Keys.NumPad2;
                    break;
                case KeyCode.Keypad3:
                    return Keys.NumPad3;
                    break;
                case KeyCode.Keypad4:
                    return Keys.NumPad4;
                    break;
                case KeyCode.Keypad5:
                    return Keys.NumPad5;
                    break;
                case KeyCode.Keypad6:
                    return Keys.NumPad6;
                    break;
                case KeyCode.Keypad7:
                    return Keys.NumPad7;
                    break;
                case KeyCode.Keypad8:
                    return Keys.NumPad8;
                    break;
                case KeyCode.Keypad9:
                    return Keys.NumPad9;
                    break;
                case KeyCode.KeypadPeriod:
                    return Keys.OemPeriod;
                    break;
                case KeyCode.KeypadDivide:
                    return Keys.Divide;
                    break;
                case KeyCode.KeypadMultiply:
                    return Keys.Multiply;
                    break;
                case KeyCode.KeypadMinus:
                    return Keys.OemMinus;
                    break;
                case KeyCode.KeypadPlus:
                    return Keys.Oemplus;
                    break;
                case KeyCode.KeypadEnter:
                    return Keys.Enter;
                    break;
                case KeyCode.KeypadEquals:
                    return Keys.Oemplus;
                    break;
                case KeyCode.UpArrow:
                    return Keys.Up;
                    break;
                case KeyCode.DownArrow:
                    return Keys.Down;
                    break;
                case KeyCode.RightArrow:
                    return Keys.Right;
                    break;
                case KeyCode.LeftArrow:
                    return Keys.Left;
                    break;
                case KeyCode.Insert:
                    return Keys.Insert;
                    break;
                case KeyCode.Home:
                    return Keys.Home;
                    break;
                case KeyCode.End:
                    return Keys.End;
                    break;
                case KeyCode.PageUp:
                    return Keys.PageUp;
                    break;
                case KeyCode.PageDown:
                    return Keys.PageDown;
                    break;
                case KeyCode.F1:
                    return Keys.F1;
                    break;
                case KeyCode.F2:
                    return Keys.F2;
                    break;
                case KeyCode.F3:
                    return Keys.F3;
                    break;
                case KeyCode.F4:
                    return Keys.F4;
                    break;
                case KeyCode.F5:
                    return Keys.F5;
                    break;
                case KeyCode.F6:
                    return Keys.F6;
                    break;
                case KeyCode.F7:
                    return Keys.F7;
                    break;
                case KeyCode.F8:
                    return Keys.F8;
                    break;
                case KeyCode.F9:
                    return Keys.F9;
                    break;
                case KeyCode.F10:
                    return Keys.F10;
                    break;
                case KeyCode.F11:
                    return Keys.F11;
                    break;
                case KeyCode.F12:
                    return Keys.F12;
                    break;
                case KeyCode.F13:
                    return Keys.F13;
                    break;
                case KeyCode.F14:
                    return Keys.F14;
                    break;
                case KeyCode.F15:
                    return Keys.F15;
                    break;
                case KeyCode.Alpha0:
                    return Keys.D0;
                    break;
                case KeyCode.Alpha1:
                    return Keys.D1;
                    break;
                case KeyCode.Alpha2:
                    return Keys.D2;
                    break;
                case KeyCode.Alpha3:
                    return Keys.D3;
                    break;
                case KeyCode.Alpha4:
                    return Keys.D4;
                    break;
                case KeyCode.Alpha5:
                    return Keys.D5;
                    break;
                case KeyCode.Alpha6:
                    return Keys.D6;
                    break;
                case KeyCode.Alpha7:
                    return Keys.D7;
                    break;
                case KeyCode.Alpha8:
                    return Keys.D8;
                    break;
                case KeyCode.Alpha9:
                    return Keys.D8;
                    break;
                case KeyCode.Exclaim:
                    return Keys.D1;
                    break;
                case KeyCode.DoubleQuote:
                    return Keys.D2;
                    break;
                case KeyCode.Hash:
                    return Keys.D3;
                    break;
                case KeyCode.Dollar:
                    return Keys.D4;
                    break;
                case KeyCode.Percent:
                    return Keys.D5;
                    break;
                case KeyCode.Ampersand:
                    return Keys.D6;
                    break;
                case KeyCode.Quote:
                    return Keys.OemQuotes;
                    break;
                case KeyCode.LeftParen:
                    return Keys.D8;
                    break;
                case KeyCode.RightParen:
                    return Keys.D0;
                    break;
                case KeyCode.Asterisk:
                    return Keys.D8;
                    break;
                case KeyCode.Plus:
                    return Keys.Add;
                    break;
                case KeyCode.Comma:
                    return Keys.Oemcomma;
                    break;
                case KeyCode.Minus:
                    return Keys.OemMinus;
                    break;
                case KeyCode.Period:
                    return Keys.OemPeriod;
                    break;
                case KeyCode.Slash:
                    throw new ArgumentException($"Unknown keycode {keyCode}");
                    break;
                case KeyCode.Colon:
                    throw new ArgumentException($"Unknown keycode {keyCode}");
                    break;
                case KeyCode.Semicolon:
                    return Keys.OemSemicolon;
                    break;
                case KeyCode.Less:
                    throw new ArgumentException($"Unknown keycode {keyCode}");
                    break;
                case KeyCode.Equals:
                    throw new ArgumentException($"Unknown keycode {keyCode}");
                    break;
                case KeyCode.Greater:
                    throw new ArgumentException($"Unknown keycode {keyCode}");
                    break;
                case KeyCode.Question:
                    return Keys.OemQuestion;
                    break;
                case KeyCode.At:
                    throw new ArgumentException($"Unknown keycode {keyCode}");
                    break;
                case KeyCode.LeftBracket:
                    return Keys.OemOpenBrackets;
                    break;
                case KeyCode.Backslash:
                    return Keys.OemBackslash;
                    break;
                case KeyCode.RightBracket:
                    return Keys.OemCloseBrackets;
                    break;
                case KeyCode.Caret:
                    throw new ArgumentException($"Unknown keycode {keyCode}");
                    break;
                case KeyCode.Underscore:
                    throw new ArgumentException($"Unknown keycode {keyCode}");
                    break;
                case KeyCode.BackQuote:
                    throw new ArgumentException($"Unknown keycode {keyCode}");
                    break;
                case KeyCode.A:
                    return Keys.A;
                    break;
                case KeyCode.B:
                    return Keys.B;
                    break;
                case KeyCode.C:
                    return Keys.C;
                    break;
                case KeyCode.D:
                    return Keys.D;
                    break;
                case KeyCode.E:
                    return Keys.E;
                    break;
                case KeyCode.F:
                    return Keys.F;
                    break;
                case KeyCode.G:
                    return Keys.G;
                    break;
                case KeyCode.H:
                    return Keys.H;
                    break;
                case KeyCode.I:
                    return Keys.I;
                    break;
                case KeyCode.J:
                    return Keys.J;
                    break;
                case KeyCode.K:
                    return Keys.K;
                    break;
                case KeyCode.L:
                    return Keys.L;
                    break;
                case KeyCode.M:
                    return Keys.M;
                    break;
                case KeyCode.N:
                    return Keys.N;
                    break;
                case KeyCode.O:
                    return Keys.O;
                    break;
                case KeyCode.P:
                    return Keys.P;
                    break;
                case KeyCode.Q:
                    return Keys.Q;
                    break;
                case KeyCode.R:
                    return Keys.R;
                    break;
                case KeyCode.S:
                    return Keys.S;
                    break;
                case KeyCode.T:
                    return Keys.T;
                    break;
                case KeyCode.U:
                    return Keys.U;
                    break;
                case KeyCode.V:
                    return Keys.V;
                    break;
                case KeyCode.W:
                    return Keys.W;
                    break;
                case KeyCode.X:
                    return Keys.X;
                    break;
                case KeyCode.Y:
                    return Keys.Y;
                    break;
                case KeyCode.Z:
                    return Keys.Z;
                    break;
                case KeyCode.LeftCurlyBracket:
                    return Keys.OemOpenBrackets;
                    break;
                case KeyCode.Pipe:
                    return Keys.OemPipe;
                    break;
                case KeyCode.RightCurlyBracket:
                    return Keys.OemCloseBrackets;
                    break;
                case KeyCode.Tilde:
                    return Keys.Oemtilde;
                    break;
                case KeyCode.Numlock:
                    return Keys.NumLock;
                    break;
                case KeyCode.CapsLock:
                    return Keys.CapsLock;
                    break;
                case KeyCode.ScrollLock:
                    return Keys.Scroll;
                    break;
                case KeyCode.RightShift:
                    return Keys.RShiftKey;
                    break;
                case KeyCode.LeftShift:
                    return Keys.LShiftKey;
                    break;
                case KeyCode.RightControl:
                    return Keys.RControlKey;
                    break;
                case KeyCode.LeftControl:
                    return Keys.LControlKey;
                    break;
                case KeyCode.RightAlt:
                    return Keys.Alt;
                    break;
                case KeyCode.LeftAlt:
                    return Keys.Alt;
                    break;
                case KeyCode.LeftMeta:
                    return Keys.LWin;
                    break;
                case KeyCode.LeftWindows:
                    return Keys.LWin;
                    break;
                case KeyCode.RightMeta:
                    return Keys.RWin;
                    break;
                case KeyCode.RightWindows:
                    return Keys.RWin;
                    break;
                case KeyCode.AltGr:
                    return Keys.Alt;
                    break;
                case KeyCode.Help:
                    break;
                case KeyCode.Print:
                    return Keys.Print;
                    break;
                case KeyCode.SysReq:
                    break;
                case KeyCode.Break:
                    break;
                case KeyCode.Menu:
                    return Keys.Menu;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(keyCode), keyCode, null);
            }

            return Keys.None;
        }
    }
}