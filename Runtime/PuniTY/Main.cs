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

        public IPunityTerminal OpenTerminal(StartArguments startArguments, ITerminalUI ui = null, ILogger logger = null)
        {
            var terminal = PunityFactory.CreateTerminal(Server, new PunityClient(logger));
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