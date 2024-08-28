﻿using HamerSoft.PuniTY.Logging;

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

                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;
                case 9:

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