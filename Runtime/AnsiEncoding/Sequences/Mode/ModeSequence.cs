using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public abstract class ModeSequence : CSISequence
    {
        private const char QuestionMarkAsPrivateIndicator = '?';
        private const int InvalidArgument = -1;

        protected ModeSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Logger.LogWarning($"Failed to executed {nameof(GetType)}, no parameters given. Skipping command");
                return;
            }

            var paramsToParse = parameters.StartsWith(QuestionMarkAsPrivateIndicator)
                ? parameters.Substring(1, parameters.Length - 1)
                : parameters;

            if (!TryParseInt(paramsToParse, out var argument, "-1"))
            {
                Logger.LogWarning($"Failed to parse argument {nameof(GetType)}, no parameters invalid. Int Expected.");
                return;
            }

            if (InvalidArgument == argument)
            {
                Logger.LogWarning($"Failed to parse argument {nameof(GetType)}, parameter invalid. Int Expected.");
                return;
            }

            if (parameters.StartsWith(QuestionMarkAsPrivateIndicator))
                ExecutePrivateSequence(screen, argument);
            else
                ExecutePublicSequence(screen, argument);
        }

        private void ExecutePrivateSequence(IScreen screen, int argument)
        {
            switch (argument)
            {
                case 1:
                    SetMode(screen, AnsiMode.ApplicationCursorKeys);
                    break;
                case 2:
                    SetMode(screen, AnsiMode.DECANM);
                    break;
                case 3:
                    SetMode(screen, AnsiMode.DECCOLM);
                    break;
                case 4:
                    SetMode(screen, AnsiMode.SmoothScroll);
                    break;
                case 5:
                    SetMode(screen, AnsiMode.ReverseVideo);
                    break;
                case 6:
                    SetMode(screen, AnsiMode.Origin);
                    break;
                case 7:
                    SetMode(screen, AnsiMode.AutoWrap);
                    break;
                case 8:
                    SetMode(screen, AnsiMode.AutoRepeatKeys);
                    break;
                case 9:
                    SetMode(screen, AnsiMode.SendMouseXY);
                    break;
                case 10:
                    SetMode(screen, AnsiMode.ShowToolbar);
                    break;
                case 12:
                    SetMode(screen, AnsiMode.BlinkingCursor);
                    break;
                case 13:
                    SetMode(screen, AnsiMode.StartBlinkingCursor);
                    break;
                case 14:
                    SetMode(screen, AnsiMode.XORBlinkingCursor);
                    break;
                case 18:
                    SetMode(screen, AnsiMode.PrintFormFeed);
                    break;
                case 19:
                    SetMode(screen, AnsiMode.PrintExtentFullScreen);
                    break;
                case 25:
                    SetMode(screen, AnsiMode.ShowCursor);
                    break;
                case 30:
                    SetMode(screen, AnsiMode.ShowScrollbar);
                    break;
                case 35:
                    SetMode(screen, AnsiMode.EnableFontShiftingFunctions);
                    break;
                case 38:
                    SetMode(screen, AnsiMode.Tektronix);
                    break;
                case 40:
                    SetMode(screen, AnsiMode.Display80_132);
                    break;
                case 41:
                    SetMode(screen, AnsiMode.More_Fix);
                    break;
                case 42:
                    SetMode(screen, AnsiMode.NationalReplacementCharacters);
                    break;
                case 43:
                    SetMode(screen, AnsiMode.GraphicExtendedPrint);
                    break;
                case 44:
                    SetMode(screen, AnsiMode.MarginBell);
                    break;
                case 45:
                    SetMode(screen, AnsiMode.ReverseWrapAround);
                    break;
                case 46:
                    SetMode(screen, AnsiMode.XTLogging);
                    break;
                case 47:
                    SetMode(screen, AnsiMode.AlternateScreenBuffer);
                    break;
                case 66:
                    SetMode(screen, AnsiMode.ApplicationKeypad);
                    break;
                case 67:
                    SetMode(screen, AnsiMode.BackArrowKeySendsBackspace);
                    break;
                case 69:
                    SetMode(screen, AnsiMode.LeftAndRightMargin);
                    break;
                case 80:
                    SetMode(screen, AnsiMode.SixelDisplayMode);
                    break;
                case 95:
                    SetMode(screen, AnsiMode.DoNotClearScreenWhenDeccolm);
                    break;
                case 1000:
                    SetMode(screen, AnsiMode.SendMouseXYOnButtonPressAndRelease);
                    break;
                case 1001:
                    SetMode(screen, AnsiMode.UseHiliteMouseTracking);
                    break;
                case 1002:
                    SetMode(screen, AnsiMode.UseCellMotionMouseTracking);
                    break;
                case 1003:
                    SetMode(screen, AnsiMode.UseAllMotionMouseTracking);
                    break;
                case 1004:
                    SetMode(screen, AnsiMode.SendFocusIn_FocusOutEvents);
                    break;
                case 1005:
                    SetMode(screen, AnsiMode.EnableUTF_8Mouse);
                    break;
                case 1006:
                    SetMode(screen, AnsiMode.EnableSGRMouse);
                    break;
                case 1007:
                    SetMode(screen, AnsiMode.EnableAlternateScroll);
                    break;
                case 1010:
                    SetMode(screen, AnsiMode.ScrollToBottomOnTtyOutput);
                    break;
                case 1011:
                    SetMode(screen, AnsiMode.ScrollToBottomOnKeyPress);
                    break;
                case 1014:
                    SetMode(screen, AnsiMode.FastScroll);
                    break;
                case 1015:
                    SetMode(screen, AnsiMode.UrxvtMouse);
                    break;
                case 1016:
                    SetMode(screen, AnsiMode.SGRMousePixelMode);
                    break;
                case 1034:
                    SetMode(screen, AnsiMode.InterpretMetaKey);
                    break;
                case 1035:
                    SetMode(screen, AnsiMode.SpecialModifiersAltAndNumLockKeys);
                    break;
                case 1036:
                    SetMode(screen, AnsiMode.MetaSendsEscape);
                    break;
                case 1037:
                    SetMode(screen, AnsiMode.SendDEL_EditingKeypadDelete);
                    break;
                case 1039:
                    SetMode(screen, AnsiMode.AltSendsEscape);
                    break;
                case 1040:
                    SetMode(screen, AnsiMode.KeepSelection);
                    break;
                case 1041:
                    SetMode(screen, AnsiMode.SelectToClipBoard);
                    break;
                case 1042:
                    SetMode(screen, AnsiMode.BellsUrgent);
                    break;
                case 1043:
                    SetMode(screen, AnsiMode.PopOnBell);
                    break;
                case 1044:
                    SetMode(screen, AnsiMode.KeepClipBoard);
                    break;
                case 1045:
                    SetMode(screen, AnsiMode.ExtendedReverseWrapAround);
                    break;
                case 1046:
                    SetMode(screen, AnsiMode.EnableSwitchingAlternateScreenBuffer);
                    break;
                case 1047:
                    SetMode(screen, AnsiMode.UseAlternateScreenBuffer);
                    break;
                case 1048:
                    SetMode(screen, AnsiMode.SaveCursorAsDECSC);
                    break;
                case 1049:
                    SetMode(screen, AnsiMode.SaveCursorAsDECSC_AfterSwitchAlternateScreen);
                    break;
                case 1050:
                    SetMode(screen, AnsiMode.FunctionKey);
                    break;
                case 1051:
                    SetMode(screen, AnsiMode.Sun_FunctionKeys);
                    break;
                case 1052:
                    SetMode(screen, AnsiMode.HP_FunctionKeys);
                    break;
                case 1053:
                    SetMode(screen, AnsiMode.SCO_FunctionKeys);
                    break;
                case 1060:
                    SetMode(screen, AnsiMode.LegacyKeyboardEmulation);
                    break;
                case 1061:
                    SetMode(screen, AnsiMode.VT220KeyboardEmulation);
                    break;
                case 2001:
                    SetMode(screen, AnsiMode.ReadLineMouseButton_1);
                    break;
                case 2002:
                    SetMode(screen, AnsiMode.ReadLineMouseButton_2);
                    break;
                case 2003:
                    SetMode(screen, AnsiMode.ReadLineMouseButton_3);
                    break;
                case 2004:
                    SetMode(screen, AnsiMode.BracketedPaste);
                    break;
                case 2005:
                    SetMode(screen, AnsiMode.ReadLineCharacterQuoting);
                    break;
                case 2006:
                    SetMode(screen, AnsiMode.ReadLineNewLinePasting);
                    break;
            }
        }

        private void ExecutePublicSequence(IScreen screen, int argument)
        {
            switch (argument)
            {
                case 2:
                    SetMode(screen, AnsiMode.KeyBoardAction);
                    break;
                case 4:
                    SetMode(screen, AnsiMode.Insert);
                    break;
                case 12:
                    SetMode(screen, AnsiMode.SendReceive);
                    break;
                case 20:
                    SetMode(screen, AnsiMode.AutomaticNewLine);
                    break;
                default:
                    Logger.LogWarning($"Failed to executed {nameof(GetType)}, unknown parameter: {argument}.");
                    break;
            }
        }

        protected abstract void SetMode(IScreen screen, AnsiMode mode);
    }
}