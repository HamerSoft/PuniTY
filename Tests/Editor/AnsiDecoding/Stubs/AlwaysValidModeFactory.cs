using AnsiEncoding;
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
            public void Dispose()
            {
            }

            public void Enable()
            {
            }

            public void Disable()
            {
            }

            public void Apply()
            {
            }
        }

        IMode IModeFactory.Create(AnsiMode mode, IAnsiContext context)
        {
            return new AlwaysValidMode();
        }

        IPointerMode IModeFactory.Create(PointerMode pointerMode, IAnsiContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}