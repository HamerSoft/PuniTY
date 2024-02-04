using System.Threading;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY
{
    public class ApiSingleton : IPunityApi
    {
        internal static ApiSingleton Instance => _instance ??= new ApiSingleton();
        private static ApiSingleton _instance;
        private static readonly Api _api;

        static ApiSingleton()
        {
            _api = new Api(new EditorLogger());
        }

        public void StartServer(StartArguments startArguments)
        {
            _api?.StartServer(startArguments);
        }

        public IPunityTerminal OpenTerminal(ClientArguments startArguments, ILogger logger = null, ITerminalUI ui = null)
        {
            return _api.OpenTerminal(startArguments, logger, ui);
        }

        public async Task<IPunityTerminal> OpenTerminalAsync(ClientArguments startArguments, ILogger logger = null, ITerminalUI ui = null,
            CancellationToken token = default)
        {
            return await _api.OpenTerminalAsync(startArguments, logger, ui, token);
        }

        public void Stop()
        {
           _api?.Stop();
        }
    }
}