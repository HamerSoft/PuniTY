using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using HamerSoft.PuniTY.Logging;
using HamerSoft.PuniTY.UI;

namespace HamerSoft.PuniTY
{
    internal class PunityFactory
    {
        internal static IPunityTerminal CreateTerminal(IPunityServer punityServer, IPunityClient client,
            ILogger logger = null)
        {
            return new PunityTerminal(punityServer, client, logger ?? new EditorLogger());
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

        // internal static ITerminalUI CreateVT100UI(ILogger logger = null)
        // {
        //     return new Vt100UIElement(logger ?? new EditorLogger());
        // }
    }
}