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
            }
        }

        protected override void ExecutePublicSequence(IScreen screen, int argument)
        {
            switch (argument)
            {
                case 2:
                    screen.SetMode(AnsiMode.KeyBoardAction);
                    break;
                case 4:
                    break;

                case 12:
                    break;

                case 20:
                    break;
                default:
                    Logger.LogWarning($"Failed to executed {nameof(GetType)}, unknown parameter: {argument}.");
                    break;
            }
        }
    }
}