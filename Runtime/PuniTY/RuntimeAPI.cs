using System.Threading;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using HamerSoft.PuniTY.Utilities;
using UnityEngine;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace HamerSoft.PuniTY
{
    public class RuntimeAPI
    {
        private static Api _api;
        private static UnityEvents _unityEvents;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StartApi()
        {
            StartNewApi();
        }

        private static void StartNewApi()
        {
            _unityEvents = new GameObject("UnitEvents").AddComponent<UnityEvents>();
            _unityEvents.ApplicationQuit += UnityEventsOnApplicationQuit;
            _api = new Api(new EditorLogger());
            _api.StartServer(new StartArguments("127.0.0.1", 13000));
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

        private static void UnityEventsOnApplicationQuit()
        {
            _unityEvents.ApplicationQuit -= UnityEventsOnApplicationQuit;
            _api.Stop();
        }
    }
}