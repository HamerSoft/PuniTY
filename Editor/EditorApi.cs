using System;
using System.Threading;
using System.Threading.Tasks;
using AnsiEncoding;
using HamerSoft.PuniTY;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Core;
using UnityEditor;
using UnityEngine;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace Hamersoft.PuniTY
{
    [InitializeOnLoad]
    public static class EditorApi
    {
        static EditorApi()
        {
            EditorApplication.playModeStateChanged -= EditorApplicationOnplayModeStateChanged;
            EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            StartNewApi();
        }

        private static void StartNewApi()
        {
            Api.Instance.Stop();
            Api.Instance.StartServer(new StartArguments("127.0.0.1", 13000));
        }

        public static void Start(StartArguments startArguments)
        {
            Api.Instance?.StartServer(startArguments);
        }

        public static void Stop()
        {
            Api.Instance?.Stop();
        }

        public static IPunityTerminal OpenTerminal(ClientArguments startArguments, IAnsiContext ansiContext,
            ITerminalUI ui = null)
        {
            return Api.Instance.OpenTerminal(startArguments, ansiContext, ui);
        }

        public static async Task<IPunityTerminal> OpenTerminalAsync(ClientArguments startArguments,
            IAnsiContext ansiContext,
            ITerminalUI ui = null, CancellationToken token = default)
        {
            return await Api.Instance.OpenTerminalAsync(startArguments, ansiContext, ui, token);
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
                    Stop();
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