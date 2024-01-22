using HamerSoft.PuniTY;
using HamerSoft.PuniTY.UI;
using UnityEditor;
using UnityEngine;

namespace Hamersoft.PuniTY.Editor.UI
{
    public class EditorTerminal : EditorWindow
    {
        private TerminalUIElement _terminal;

        [MenuItem("Tools/HamerSoft/Punity/Start Terminal")]
        public static void ShowExample()
        {
            EditorTerminal wnd = GetWindow<EditorTerminal>();
            wnd.titleContent = new GUIContent("Punity Terminal");
        }

        public void CreateGUI()
        {
            rootVisualElement.Add(_terminal = PunityFactory.CreateUI() as TerminalUIElement);
        }

        private void OnFocus()
        {
            Debug.Log("Gained focus");
            _terminal.Focus();
        }

        private void OnLostFocus()
        {
            Debug.Log("Lost focus");
            _terminal.UnFocus();
        }
    }
}