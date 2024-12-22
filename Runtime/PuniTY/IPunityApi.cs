using System.Threading;
using System.Threading.Tasks;
using AnsiEncoding;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY
{
    public interface IPunityApi
    {
        public void StartServer(StartArguments startArguments);

        public IPunityTerminal OpenTerminal(ClientArguments startArguments, IAnsiContext ansiContext,
            ITerminalUI ui = null);

        public Task<IPunityTerminal> OpenTerminalAsync(ClientArguments startArguments, IAnsiContext ansiContext,
            ITerminalUI ui = null, CancellationToken token = default);

        public void Stop();
    }
}