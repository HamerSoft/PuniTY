using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class SetModeSequence : ModeSequence
    {
        public override char Command => 'h';

        public SetModeSequence(ILogger logger) : base(logger)
        {
        }


        protected override void ExecutePrivateSequence(IScreen screen, int argument)
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
                    SetMode(screen, AnsiMode.BackarrowKeySendsBackspace);
                    break;
                case 69:
                    SetMode(screen, AnsiMode.LeftAndRightMargin);
                    break;
                case 80:
                    SetMode(screen, AnsiMode.SixelDisplayMode);
                    break;
                case 95:
                    SetMode(screen, AnsiMode.DoNotClearScreenWhenDECCOLM);
                    break;
                case 1000:
                    SetMode(screen, AnsiMode.SendMouseX_YOnButtonPressAndRelease);
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
            }
        }

        protected override void ExecutePublicSequence(IScreen screen, int argument)
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

        protected override void SetMode(IScreen screen, AnsiMode mode)
        {
            screen.SetMode(mode);
        }
    }
}