using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class ResetModeSequence : ModeSequence
    {
        public override char Command => 'l';

        public ResetModeSequence(ILogger logger) : base(logger)
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
            screen.ResetMode(mode);
        }
    }
}