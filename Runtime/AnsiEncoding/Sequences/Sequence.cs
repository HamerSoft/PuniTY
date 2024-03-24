using NUnit.Framework.Internal;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public abstract class Sequence : ISequence
    {
        private readonly IScreen _screen;
        public abstract char Command { get; }
        protected ILogger Logger;

        public abstract void Execute(IScreen screen, string parameters);
    }
}