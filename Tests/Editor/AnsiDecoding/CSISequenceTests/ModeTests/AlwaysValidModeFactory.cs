using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests.ModeTests
{
    /// <summary>
    /// This mode factory creates a valid mode for any value of the <see cref="AnsiMode"/> enum and is only used for testing
    /// </summary>
    internal class AlwaysValidModeFactory : IModeFactory
    {
        private class AlwaysValidMode : IMode
        {
            public void Enable(IScreen screen)
            {
            }

            public void Disable(IScreen screen)
            {
            }

            public void Apply(IScreen screen)
            {
            }
        }

        public IMode Create(AnsiMode mode, IScreen screen)
        {
            return new AlwaysValidMode();
        }
    }
}