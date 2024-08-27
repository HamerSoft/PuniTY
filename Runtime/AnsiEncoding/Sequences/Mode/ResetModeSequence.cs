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
            throw new System.NotImplementedException();
        }

        protected override void ExecutePublicSequence(IScreen screen, int argument)
        {
            switch (argument)
            {
                case 2:
                    screen.ResetMode(AnsiMode.KeyBoardAction);
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