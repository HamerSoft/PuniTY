using HamerSoft.PuniTY;
using HamerSoft.PuniTY.Core;

namespace PuniTY
{
    public class Main
    {
        public IPunityServer Server;
        private StartArguments _startArguments;

        // [InitializeOnLoadMethod]
        public Main(ILogger logger = null)
        {
            Server = PunityFactory.CreateServer(logger);
        }

        public void StartServer(StartArguments startArguments)
        {
            _startArguments = startArguments;
            Server.Start(startArguments);
        }

        public IPunityTerminal OpenTerminal(StartArguments startArguments, ILogger logger = null, ITerminalUI ui = null)
        {
            var terminal = PunityFactory.CreateTerminal(Server, PunityFactory.CreateClient(logger));
            terminal.Start(startArguments, ui);
            return terminal;
        }

        public void Stop()
        {
            Server?.Stop();
            Server = null;
        }
    }
}