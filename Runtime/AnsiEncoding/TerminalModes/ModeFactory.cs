using System;
using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;

namespace HamerSoft.PuniTY.AnsiEncoding.TerminalModes
{
    internal class ModeFactory : IModeFactory
    {
        IPointerMode IModeFactory.Create(PointerMode pointerMode, IAnsiContext context)
        {
            switch (pointerMode)
            {
                case PointerMode.NeverHide:
                    return new NeverHide();
                case PointerMode.HideIfNotTracking:
                    return new HideWhenTrackingDisabled(context.Screen);
                case PointerMode.AlwaysHideInWindow:
                    return new HideInWindow();
                case PointerMode.AlwaysHide:
                    return new AlwaysHide();
                default:
                    throw new ArgumentOutOfRangeException(nameof(pointerMode), pointerMode, null);
            }
        }
        IMode IModeFactory.Create(AnsiMode mode, IAnsiContext context)
        {
            IMode terminalMode = null;
            switch (mode)
            {
                case AnsiMode.KeyBoardAction:
                    break;
                case AnsiMode.Insert:
                    break;
                case AnsiMode.SendReceive:
                    break;
                case AnsiMode.AutomaticNewLine:
                    break;
                case AnsiMode.ApplicationCursorKeys:
                    break;
                case AnsiMode.DECANM:
                    break;
                case AnsiMode.DECCOLM:
                    break;
                case AnsiMode.SmoothScroll:
                    break;
                case AnsiMode.ReverseVideo:
                    break;
                case AnsiMode.Origin:
                    break;
                case AnsiMode.AutoWrap:
                    break;
                case AnsiMode.AutoRepeatKeys:
                    break;
                case AnsiMode.SendMouseXY:
                    break;
                case AnsiMode.ShowToolbar:
                    break;
                case AnsiMode.BlinkingCursor:
                    break;
                case AnsiMode.StartBlinkingCursor:
                    break;
                case AnsiMode.XORBlinkingCursor:
                    break;
                case AnsiMode.PrintFormFeed:
                    break;
                case AnsiMode.PrintExtentFullScreen:
                    break;
                case AnsiMode.ShowCursor:
                    break;
                case AnsiMode.ShowScrollbar:
                    break;
                case AnsiMode.EnableFontShiftingFunctions:
                    break;
                case AnsiMode.Tektronix:
                    break;
                case AnsiMode.Display80_132:
                    break;
                case AnsiMode.More_Fix:
                    break;
                case AnsiMode.NationalReplacementCharacters:
                    break;
                case AnsiMode.GraphicExtendedPrint:
                    break;
                case AnsiMode.MarginBell:
                    break;
                case AnsiMode.GraphicPrintColor:
                    break;
                case AnsiMode.ReverseWrapAround:
                    break;
                case AnsiMode.XTLogging:
                    break;
                case AnsiMode.GraphicPrintBackgroundMode:
                    break;
                case AnsiMode.AlternateScreenBuffer:
                    break;
                case AnsiMode.GraphicRotatedPrint:
                    break;
                case AnsiMode.ApplicationKeypad:
                    break;
                case AnsiMode.BackArrowKeySendsBackspace:
                    break;
                case AnsiMode.LeftAndRightMargin:
                    break;
                case AnsiMode.SixelDisplayMode:
                    break;
                case AnsiMode.DoNotClearScreenWhenDeccolm:
                    break;
                case AnsiMode.SendMouseXYOnButtonPressAndRelease:
                    break;
                case AnsiMode.UseHiliteMouseTracking:
                    break;
                case AnsiMode.UseCellMotionMouseTracking:
                    break;
                case AnsiMode.UseAllMotionMouseTracking:
                    break;
                case AnsiMode.SendFocusIn_FocusOutEvents:
                    break;
                case AnsiMode.EnableUTF_8Mouse:
                    break;
                case AnsiMode.EnableSGRMouse:
                    break;
                case AnsiMode.EnableAlternateScroll:
                    break;
                case AnsiMode.ScrollToBottomOnTtyOutput:
                    break;
                case AnsiMode.ScrollToBottomOnKeyPress:
                    break;
                case AnsiMode.FastScroll:
                    break;
                case AnsiMode.UrxvtMouse:
                    break;
                case AnsiMode.SGRMousePixelMode:
                    break;
                case AnsiMode.InterpretMetaKey:
                    break;
                case AnsiMode.SpecialModifiersAltAndNumLockKeys:
                    break;
                case AnsiMode.MetaSendsEscape:
                    break;
                case AnsiMode.SendDEL_EditingKeypadDelete:
                    break;
                case AnsiMode.AltSendsEscape:
                    break;
                case AnsiMode.KeepSelection:
                    break;
                case AnsiMode.SelectToClipBoard:
                    break;
                case AnsiMode.BellsUrgent:
                    break;
                case AnsiMode.PopOnBell:
                    break;
                case AnsiMode.KeepClipBoard:
                    break;
                case AnsiMode.ExtendedReverseWrapAround:
                    break;
                case AnsiMode.EnableSwitchingAlternateScreenBuffer:
                    break;
                case AnsiMode.UseAlternateScreenBuffer:
                    break;
                case AnsiMode.SaveCursorAsDECSC:
                    break;
                case AnsiMode.SaveCursorAsDECSC_AfterSwitchAlternateScreen:
                    break;
                case AnsiMode.FunctionKey:
                    break;
                case AnsiMode.Sun_FunctionKeys:
                    break;
                case AnsiMode.HP_FunctionKeys:
                    break;
                case AnsiMode.SCO_FunctionKeys:
                    break;
                case AnsiMode.LegacyKeyboardEmulation:
                    break;
                case AnsiMode.VT220KeyboardEmulation:
                    break;
                case AnsiMode.ReadLineMouseButton_1:
                    break;
                case AnsiMode.ReadLineMouseButton_2:
                    break;
                case AnsiMode.ReadLineMouseButton_3:
                    break;
                case AnsiMode.BracketedPaste:
                    break;
                case AnsiMode.ReadLineCharacterQuoting:
                    break;
                case AnsiMode.ReadLineNewLinePasting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            return terminalMode;
        }
    }
}