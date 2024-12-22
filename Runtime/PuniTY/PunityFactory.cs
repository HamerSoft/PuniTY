using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using HamerSoft.PuniTY.Logging;
using HamerSoft.PuniTY.UI;

namespace HamerSoft.PuniTY
{
    internal class PunityFactory
    {
        internal static IPunityTerminal CreateTerminal(IPunityServer punityServer, IPunityClient client,
            IAnsiContext ansiContext,
            ILogger logger = null)
        {
            var terminal =  new PunityTerminal(punityServer, client, logger ?? new EditorLogger());
            terminal.BytesReceived += ansiContext.Decoder.Decode;
            ansiContext.InputTransmitter.Output += terminal.Write;
            return terminal;
        }

        internal static IPunityServer CreateServer(ILogger logger = null)
        {
            return new PunityServer(logger ?? new EditorLogger());
        }

        internal static IPunityClient CreateClient(ILogger logger = null)
        {
            return new PunityClient(System.Guid.NewGuid(), logger ?? new EditorLogger());
        }

        internal static ITerminalUI CreateUI(ILogger logger = null)
        {
            return new TerminalUIElement(logger ?? new EditorLogger());
        }

        internal static IAnsiContext CreateAnsiContext(IInput input = null, IScreenConfiguration screenConfiguration = null,
            ILogger logger = null)
        {
            return new AnsiContext(input,
                screenConfiguration ?? new Screen.DefaultScreenConfiguration(25, 80, 8, new FontDimensions(10, 10)),
                logger ?? new EditorLogger());
        }

        // internal static ITerminalUI CreateVT100UI(ILogger logger = null)
        // {
        //     return new Vt100UIElement(logger ?? new EditorLogger());
        // }
    }
}