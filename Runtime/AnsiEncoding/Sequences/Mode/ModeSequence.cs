using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public abstract class ModeSequence : CSISequence
    {
        private const char QuestionMarkAsPrivateIndicator = '?';
        private const int InvalidArgument = -1;

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                context.LogWarning($"Failed to executed {nameof(GetType)}, no parameters given. Skipping command");
                return;
            }

            var paramsToParse = parameters.StartsWith(QuestionMarkAsPrivateIndicator)
                ? parameters.Substring(1, parameters.Length - 1)
                : parameters;

            if (!TryParseInt(paramsToParse, out var argument, "-1"))
            {
                context.LogWarning($"Failed to parse argument {nameof(GetType)}, no parameters invalid. Int Expected.");
                return;
            }

            if (InvalidArgument == argument)
            {
                context.LogWarning($"Failed to parse argument {nameof(GetType)}, parameter invalid. Int Expected.");
                return;
            }

            if (parameters.StartsWith(QuestionMarkAsPrivateIndicator))
                ExecutePrivateSequence(context, argument);
            else
                ExecutePublicSequence(context, argument);
        }

        private void ExecutePrivateSequence(IAnsiContext context, int argument)
        {
            var modeContext = context.TerminalModeContext;
            switch (argument)
            {
                case 1:
                    SetMode(modeContext, AnsiMode.ApplicationCursorKeys);
                    break;
                case 2:
                    SetMode(modeContext, AnsiMode.DECANM);
                    break;
                case 3:
                    SetMode(modeContext, AnsiMode.DECCOLM);
                    break;
                case 4:
                    SetMode(modeContext, AnsiMode.SmoothScroll);
                    break;
                case 5:
                    SetMode(modeContext, AnsiMode.ReverseVideo);
                    break;
                case 6:
                    SetMode(modeContext, AnsiMode.Origin);
                    break;
                case 7:
                    SetMode(modeContext, AnsiMode.AutoWrap);
                    break;
                case 8:
                    SetMode(modeContext, AnsiMode.AutoRepeatKeys);
                    break;
                case 9:
                    SetMode(modeContext, AnsiMode.SendMouseXY);
                    break;
                case 10:
                    SetMode(modeContext, AnsiMode.ShowToolbar);
                    break;
                case 12:
                    SetMode(modeContext, AnsiMode.BlinkingCursor);
                    break;
                case 13:
                    SetMode(modeContext, AnsiMode.StartBlinkingCursor);
                    break;
                case 14:
                    SetMode(modeContext, AnsiMode.XORBlinkingCursor);
                    break;
                case 18:
                    SetMode(modeContext, AnsiMode.PrintFormFeed);
                    break;
                case 19:
                    SetMode(modeContext, AnsiMode.PrintExtentFullScreen);
                    break;
                case 25:
                    SetMode(modeContext, AnsiMode.ShowCursor);
                    break;
                case 30:
                    SetMode(modeContext, AnsiMode.ShowScrollbar);
                    break;
                case 35:
                    SetMode(modeContext, AnsiMode.EnableFontShiftingFunctions);
                    break;
                case 38:
                    SetMode(modeContext, AnsiMode.Tektronix);
                    break;
                case 40:
                    SetMode(modeContext, AnsiMode.Display80_132);
                    break;
                case 41:
                    SetMode(modeContext, AnsiMode.More_Fix);
                    break;
                case 42:
                    SetMode(modeContext, AnsiMode.NationalReplacementCharacters);
                    break;
                case 43:
                    SetMode(modeContext, AnsiMode.GraphicExtendedPrint);
                    break;
                case 44:
                    SetMode(modeContext, AnsiMode.MarginBell);
                    break;
                case 45:
                    SetMode(modeContext, AnsiMode.ReverseWrapAround);
                    break;
                case 46:
                    SetMode(modeContext, AnsiMode.XTLogging);
                    break;
                case 47:
                    SetMode(modeContext, AnsiMode.AlternateScreenBuffer);
                    break;
                case 66:
                    SetMode(modeContext, AnsiMode.ApplicationKeypad);
                    break;
                case 67:
                    SetMode(modeContext, AnsiMode.BackArrowKeySendsBackspace);
                    break;
                case 69:
                    SetMode(modeContext, AnsiMode.LeftAndRightMargin);
                    break;
                case 80:
                    SetMode(modeContext, AnsiMode.SixelDisplayMode);
                    break;
                case 95:
                    SetMode(modeContext, AnsiMode.DoNotClearScreenWhenDeccolm);
                    break;
                case 1000:
                    SetMode(modeContext, AnsiMode.SendMouseXYOnButtonPressAndRelease);
                    break;
                case 1001:
                    SetMode(modeContext, AnsiMode.UseHiliteMouseTracking);
                    break;
                case 1002:
                    SetMode(modeContext, AnsiMode.UseCellMotionMouseTracking);
                    break;
                case 1003:
                    SetMode(modeContext, AnsiMode.UseAllMotionMouseTracking);
                    break;
                case 1004:
                    SetMode(modeContext, AnsiMode.SendFocusIn_FocusOutEvents);
                    break;
                case 1005:
                    SetMode(modeContext, AnsiMode.EnableUTF_8Mouse);
                    break;
                case 1006:
                    SetMode(modeContext, AnsiMode.EnableSGRMouse);
                    break;
                case 1007:
                    SetMode(modeContext, AnsiMode.EnableAlternateScroll);
                    break;
                case 1010:
                    SetMode(modeContext, AnsiMode.ScrollToBottomOnTtyOutput);
                    break;
                case 1011:
                    SetMode(modeContext, AnsiMode.ScrollToBottomOnKeyPress);
                    break;
                case 1014:
                    SetMode(modeContext, AnsiMode.FastScroll);
                    break;
                case 1015:
                    SetMode(modeContext, AnsiMode.UrxvtMouse);
                    break;
                case 1016:
                    SetMode(modeContext, AnsiMode.SGRMousePixelMode);
                    break;
                case 1034:
                    SetMode(modeContext, AnsiMode.InterpretMetaKey);
                    break;
                case 1035:
                    SetMode(modeContext, AnsiMode.SpecialModifiersAltAndNumLockKeys);
                    break;
                case 1036:
                    SetMode(modeContext, AnsiMode.MetaSendsEscape);
                    break;
                case 1037:
                    SetMode(modeContext, AnsiMode.SendDEL_EditingKeypadDelete);
                    break;
                case 1039:
                    SetMode(modeContext, AnsiMode.AltSendsEscape);
                    break;
                case 1040:
                    SetMode(modeContext, AnsiMode.KeepSelection);
                    break;
                case 1041:
                    SetMode(modeContext, AnsiMode.SelectToClipBoard);
                    break;
                case 1042:
                    SetMode(modeContext, AnsiMode.BellsUrgent);
                    break;
                case 1043:
                    SetMode(modeContext, AnsiMode.PopOnBell);
                    break;
                case 1044:
                    SetMode(modeContext, AnsiMode.KeepClipBoard);
                    break;
                case 1045:
                    SetMode(modeContext, AnsiMode.ExtendedReverseWrapAround);
                    break;
                case 1046:
                    SetMode(modeContext, AnsiMode.EnableSwitchingAlternateScreenBuffer);
                    break;
                case 1047:
                    SetMode(modeContext, AnsiMode.UseAlternateScreenBuffer);
                    break;
                case 1048:
                    SetMode(modeContext, AnsiMode.SaveCursorAsDECSC);
                    break;
                case 1049:
                    SetMode(modeContext, AnsiMode.SaveCursorAsDECSC_AfterSwitchAlternateScreen);
                    break;
                case 1050:
                    SetMode(modeContext, AnsiMode.FunctionKey);
                    break;
                case 1051:
                    SetMode(modeContext, AnsiMode.Sun_FunctionKeys);
                    break;
                case 1052:
                    SetMode(modeContext, AnsiMode.HP_FunctionKeys);
                    break;
                case 1053:
                    SetMode(modeContext, AnsiMode.SCO_FunctionKeys);
                    break;
                case 1060:
                    SetMode(modeContext, AnsiMode.LegacyKeyboardEmulation);
                    break;
                case 1061:
                    SetMode(modeContext, AnsiMode.VT220KeyboardEmulation);
                    break;
                case 2001:
                    SetMode(modeContext, AnsiMode.ReadLineMouseButton_1);
                    break;
                case 2002:
                    SetMode(modeContext, AnsiMode.ReadLineMouseButton_2);
                    break;
                case 2003:
                    SetMode(modeContext, AnsiMode.ReadLineMouseButton_3);
                    break;
                case 2004:
                    SetMode(modeContext, AnsiMode.BracketedPaste);
                    break;
                case 2005:
                    SetMode(modeContext, AnsiMode.ReadLineCharacterQuoting);
                    break;
                case 2006:
                    SetMode(modeContext, AnsiMode.ReadLineNewLinePasting);
                    break;
                default:
                    context.LogWarning($"Failed to executed {nameof(GetType)}, unknown parameter: {argument}.");
                    break;
            }
        }

        private void ExecutePublicSequence(IAnsiContext context, int argument)
        {
            var modeContext = context.TerminalModeContext;
            switch (argument)
            {
                case 2:
                    SetMode(modeContext, AnsiMode.KeyBoardAction);
                    break;
                case 4:
                    SetMode(modeContext, AnsiMode.Insert);
                    break;
                case 12:
                    SetMode(modeContext, AnsiMode.SendReceive);
                    break;
                case 20:
                    SetMode(modeContext, AnsiMode.AutomaticNewLine);
                    break;
                default:
                    context.LogWarning($"Failed to executed {nameof(GetType)}, unknown parameter: {argument}.");
                    break;
            }
        }

        protected abstract void SetMode(IModeable modeable, AnsiMode mode);
    }
}