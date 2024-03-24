using System;
using System.Text;
using HamerSoft.PuniTY;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Core.Logging;
using HamerSoft.PuniTY.Encoding;
using HamerSoft.PuniTY.UI;
using UnityEditor;
using UnityEngine;

namespace Hamersoft.PuniTY.Editor.UI
{
    public class EditorTerminalVT100 : EditorWindow
    {
        private Vt100UIElement _terminalUi;
        private IPunityTerminal _terminal;

        [MenuItem("Tools/HamerSoft/Punity/Start VT100Terminal")]
        public static void ShowTerminal()
        {
            EditorTerminalVT100 wnd = GetWindow<EditorTerminalVT100>();
            wnd.titleContent = new GUIContent("Punity VT100 Terminal");
        }

        private void OnDestroy()
        {
            _terminal?.Stop();
        }

        public void CreateGUI()
        {
            rootVisualElement.Add(_terminalUi = PunityFactory.CreateVT100UI() as Vt100UIElement);
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
            return new ClientArguments(ip, port, GetValidAppName(), new DefaultEncoder(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)), Environment.CurrentDirectory);
        }

        protected string GetValidAppName()
        {
            return Application.platform == RuntimePlatform.WindowsEditor
                ? @"C:\Program Files\Git\bin\sh.exe"
                : "/bin/bash/";
        }
    }
}