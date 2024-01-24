using System.Threading;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY
{
    public interface IPunityApi
    {
        public void StartServer(StartArguments startArguments);

        public IPunityTerminal OpenTerminal(ClientArguments startArguments, ILogger logger = null,
            ITerminalUI ui = null);

        public Task<IPunityTerminal> OpenTerminalAsync(ClientArguments startArguments, ILogger logger = null,
            ITerminalUI ui = null, CancellationToken token = default);

        public void Stop();
    }
}