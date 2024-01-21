using System.Threading;
using System.Threading.Tasks;
using HamerSoft.PuniTY;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY
{
    public class Api
    {
        internal IPunityServer Server;
        private StartArguments _startArguments;

        // [InitializeOnLoadMethod]
        public Api(ILogger logger = null)
        {
            Server = PunityFactory.CreateServer(logger);
        }

        public void StartServer(StartArguments startArguments)
        {
            _startArguments = startArguments;
            Server.Start(startArguments);
        }

        public IPunityTerminal OpenTerminal(ClientArguments startArguments, ILogger logger = null,
            ITerminalUI ui = null)
        {
            var terminal = PunityFactory.CreateTerminal(Server, PunityFactory.CreateClient(logger));
            terminal.Start(startArguments, ui);
            return terminal;
        }

        public async Task<IPunityTerminal> OpenTerminalAsync(ClientArguments startArguments, ILogger logger = null,
            ITerminalUI ui = null, CancellationToken token = default)
        {
            var terminal = PunityFactory.CreateTerminal(Server, PunityFactory.CreateClient(logger));
            await terminal.StartAsync(startArguments, ui, token);
            return terminal;
        }

        public void Stop()
        {
            Server?.Stop();
            Server = null;
        }
    }
}