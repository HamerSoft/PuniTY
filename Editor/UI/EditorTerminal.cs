using System;
using AnsiEncoding;
using HamerSoft.PuniTY;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Core.Logging;
using HamerSoft.PuniTY.Encoding;
using HamerSoft.PuniTY.UI;
using UnityEditor;
using UnityEngine;
using Keyboard = HamerSoft.PuniTY.Configuration.Keyboard;
using Rect = HamerSoft.PuniTY.AnsiEncoding.Rect;
using Screen = HamerSoft.PuniTY.AnsiEncoding.Screen;
using Vector2 = System.Numerics.Vector2;

namespace Hamersoft.PuniTY.Editor.UI
{
    public class EditorTerminal : EditorWindow
    {
        private TerminalUIElement _terminalUi;
        private IPunityTerminal _terminal;
        private IAnsiContext _ansiContext;

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
            try
            {
                _ansiContext = PunityFactory.CreateAnsiContext(
                    new DefaultInput(new EditorPointer(new NeverHide(), GetMousePosition(Event.current), GetRect()),
                        new Keyboard()),
                    new Screen.DefaultScreenConfiguration(25, 80, 8, new FontDimensions(10, 10)), new EditorLogger());
                rootVisualElement.Add(_terminalUi = PunityFactory.CreateUI() as TerminalUIElement);
                if (_terminal is not { IsRunning: true })
                    _terminal = EditorApi.OpenTerminal(GetValidClientArguments(), _ansiContext, _terminalUi);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Close();
            }
        }

        private Vector2 GetMousePosition(Event current)
        {
            return Event.current == null
                ? new Vector2(0, 0)
                : new Vector2(current.mousePosition.x, current.mousePosition.y);
        }

        private Rect GetRect()
        {
            var style = _terminalUi?.resolvedStyle;
            return style == null
                ? new Rect(0, 0, 0, 0)
                : new Rect(style.bottom, style.left, style.width, style.height);
        }

        private void OnFocus()
        {
            Debug.Log("Gained focus");
            _terminalUi?.Focus();
        }

        private void OnLostFocus()
        {
            Debug.Log("Lost focus");
            _terminalUi?.UnFocus();
        }

        private void Update()
        {
            _ansiContext?.Pointer.SetPosition(GetMousePosition(Event.current), GetRect());
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