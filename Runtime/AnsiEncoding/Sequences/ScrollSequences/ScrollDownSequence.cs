using AnsiEncoding;
namespace HamerSoft.PuniTY.AnsiEncoding.ScrollSequences
{
    public class ScrollDownSequence : ScrollSequence
    {
        public override char Command => 'T';
        protected override Direction Direction => Direction.Down;
        
        public override void Execute(IAnsiContext context, string parameters)
        {
            if (parameters.Contains(';'))
            {
                context.LogWarning("Mouse tracking not implemented! Skipping command!");
                return;
            }

            base.Execute(context, parameters);
        }
    }
}