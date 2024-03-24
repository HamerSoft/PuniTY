using System;
using HamerSoft.PuniTY;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Core.Logging;
using HamerSoft.PuniTY.Encoding;
using HamerSoft.PuniTY.UI;
using UnityEditor;
using UnityEngine;

namespace Hamersoft.PuniTY.Editor.UI
{
    public class EditorTerminal : EditorWindow
    {
        private TerminalUIElement _terminalUi;
        private IPunityTerminal _terminal;

        [MenuItem("Tools/HamerSoft/Punity/Start Terminal")]
        public static void ShowTerminal()
        {
            EditorTerminal wnd = GetWindow<EditorTerminal>();
            wnd.titleContent = new GUIContent("Punity Terminal");
        }

        private void OnDestroy()
        {
            _terminal?.Stop();
        }

        public void CreateGUI()
        {
            rootVisualElement.Add(_terminalUi = PunityFactory.CreateUI() as TerminalUIElement);
            if (_terminal is not { IsRunning: true })
                _terminal = EditorApi.OpenTerminal(GetValidClientArguments(), new EditorLogger(), _terminalUi);
        }

        private void OnFocus()
        {
            Debug.Log("Gained focus");
            _terminalUi.Focus();
        }

        private void OnLostFocus()
        {
            Debug.Log("Lost focus");
            _terminalUi.UnFocus();
        }

        protected ClientArguments GetValidClientArguments(string ip = "127.0.0.1", uint port = 13000)
        {
            return new ClientArguments(ip, port, GetValidAppName(), new AnsiEncoder(), Environment.CurrentDirectory);
        }

        protected string GetValidAppName()
        {
            return Application.platform == RuntimePlatform.WindowsEditor
                ? @"C:\Program Files\Git\bin\sh.exe"
                : "/bin/bash/";
        }
    }
}