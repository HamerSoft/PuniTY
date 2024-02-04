using System;
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
        internal static Api API;
        private static bool _exitedEditMode;

        static EditorApi()
        {
            EditorApplication.playModeStateChanged -= EditorApplicationOnplayModeStateChanged;
            EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (API == null)
            {
                StartNewApi();
            }
            else
            {
                Debug.Log("Punity API already running!");
            }
        }
        
        private static void Close()
        {
            
        }

        private static void StartNewApi()
        {
            API?.Stop();
            API = new Api(new EditorLogger());
            API.StartServer(new StartArguments("127.0.0.1", 13000));
        }

        public static void Start(StartArguments startArguments)
        {
            API?.StartServer(startArguments);
        }

        public static void Stop()
        {
            API?.Stop();
        }

        public static IPunityTerminal OpenTerminal(ClientArguments startArguments, ILogger logger = null,
            ITerminalUI ui = null)
        {
            return API?.OpenTerminal(startArguments, logger, ui);
        }

        public static async Task<IPunityTerminal> OpenTerminalAsync(ClientArguments startArguments,
            ILogger logger = null,
            ITerminalUI ui = null, CancellationToken token = default)
        {
            if (API != null)
                return await API.OpenTerminalAsync(startArguments, logger, ui, token);
            return default;
        }

        private static void EditorApplicationOnplayModeStateChanged(PlayModeStateChange playMode)
        {
            switch (playMode)
            {
                case PlayModeStateChange.EnteredEditMode:

                    Debug.Log("Starting Punity server while entering edit mode!");
                    StartNewApi();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    Debug.Log("Stopping Punity server while exiting edit mode!");
                    _exitedEditMode = true;
                    API?.Stop();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(playMode), playMode, null);
            }
        }
    }
}