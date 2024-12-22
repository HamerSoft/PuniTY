using System.Threading;
using System.Threading.Tasks;
using AnsiEncoding;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Core.Logging;

namespace HamerSoft.PuniTY.Core
{
    public sealed class Api : IPunityApi
    {
        internal static Api Instance => _instance ??= new Api(new PunityServer(new EditorLogger()));
        private static Api _instance;
        private IPunityServer _server;
        private StartArguments _startArguments;

        private Api(IPunityServer server)
        {
            _server = server;
        }

        public void StartServer(StartArguments startArguments)
        {
            _startArguments = startArguments;
            _server.Start(startArguments);
        }

        public IPunityTerminal OpenTerminal(ClientArguments startArguments, IAnsiContext ansiContext,
            ITerminalUI ui = null)
        {
            var terminal = PunityFactory.CreateTerminal(_server, PunityFactory.CreateClient(ansiContext), ansiContext);
            terminal.Start(startArguments, ui);
            return terminal;
        }

        public async Task<IPunityTerminal> OpenTerminalAsync(ClientArguments startArguments, IAnsiContext ansiContext,
            ITerminalUI ui = null, CancellationToken token = default)
        {
            var terminal = PunityFactory.CreateTerminal(_server, PunityFactory.CreateClient(ansiContext), ansiContext);
            await terminal.StartAsync(startArguments, ui, token);
            return terminal;
        }

        public void Stop()
        {
            _server?.Stop();
        }
    }
}