using System.Threading;
using System.Threading.Tasks;
using HamerSoft.PuniTY;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using UnityEditor;
using UnityEngine;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace Hamersoft.PuniTY
{
    [InitializeOnLoad]
    public static class EditorApi
    {
        private static readonly Api _api;

        static EditorApi()
        {
            if (_api == null)
            {
                _api?.Stop();
                _api = new Api(new EditorLogger());
                _api.StartServer(new StartArguments("127.0.0.1", 13000));
            }
            else
            {
                Debug.Log("Api already running");
            }
        }

        public static void Start(StartArguments startArguments)
        {
            _api?.StartServer(startArguments);
        }

        public static void Stop()
        {
            _api?.Stop();
        }

        public static IPunityTerminal OpenTerminal(ClientArguments startArguments, ILogger logger = null,
            ITerminalUI ui = null)
        {
            return _api?.OpenTerminal(startArguments, logger, ui);
        }

        public static async Task<IPunityTerminal> OpenTerminalAsync(ClientArguments startArguments,
            ILogger logger = null,
            ITerminalUI ui = null, CancellationToken token = default)
        {
            if (_api != null)
                return await _api.OpenTerminalAsync(startArguments, logger, ui, token);
            return default;
        }
        
    }
}