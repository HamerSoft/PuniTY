using System.Runtime.CompilerServices;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using HamerSoft.PuniTY.Logging;
using HamerSoft.PuniTY.UI;

namespace HamerSoft.PuniTY
{
    public class PunityFactory
    {
        public static IPunityTerminal CreateTerminal(IPunityServer punityServer, IPunityClient client,
            ILogger logger = null)
        {
            return new PunityTerminal(punityServer, client, logger ?? new EditorLogger());
        }

        public static IPunityServer CreateServer(ILogger logger = null)
        {
            return new PunityServer(logger ?? new EditorLogger());
        }

        public static IPunityClient CreateClient(ILogger logger = null)
        {
            return new PunityClient(System.Guid.NewGuid(), logger ?? new EditorLogger());
        }

        public ITerminalUI CreateUI(ILogger logger = null)
        {
            return new TerminalUI(logger ?? new EditorLogger());
        }
    }
}