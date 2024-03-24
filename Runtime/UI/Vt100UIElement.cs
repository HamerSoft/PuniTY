using System;
using HamerSoft.PuniTY.ThirdParty.VT100Adapter;
using HamerSoft.Threads;
using UnityEngine;
using UnityEngine.UIElements;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace HamerSoft.PuniTY.UI
{
    public class Vt100UIElement : VisualElement, ITerminalUI
    {
        private const string VisualTreePath = "Punity/TerminalUIElement";
        private readonly ILogger _logger;
        private readonly TextField _output;
        private readonly TextField _input;

        public event Action Closed;
        public event Action<string> Written;
        public event Action<string> WrittenLine;
        public event Action<byte[]> WrittenByte;
        private bool _isFocused;
        private readonly Vt100UI _vt100;

        public Vt100UIElement(ILogger logger)
        {
            _logger = logger;
            CreateVisualTree();
            _output = this.Q<TextField>("output");
            _input = this.Q<TextField>("input");
            _vt100 = new Vt100UI(new Size(80, 25), 0, 12 * 5, new Size(12, 12));
        }

        public void Print(string message)
        {
          //  throw new NotImplementedException();
        }

        public async void Print(byte[] message)
        {
            await Dispatcher.ToMainThread();
            _output.SetValueWithoutNotify(_vt100.Parse(message));
        }

        public void Close()
        {
            _vt100.Stop();
            Closed?.Invoke();
        }

        public void Focus()
        {
            _isFocused = true;
            // _input.Focus();
            _input.Q("unity-text-input").Focus();
            RegisterCallback<KeyUpEvent>(OnKeyUp);
        }

        public void UnFocus()
        {
            _isFocused = false;
            UnregisterCallback<KeyUpEvent>(OnKeyUp);
        }

        private void OnKeyUp(KeyUpEvent keyboardEvent)
        {
            _vt100.DetectKeys(keyboardEvent);
            if (keyboardEvent.keyCode == KeyCode.Return && !string.IsNullOrWhiteSpace(_input.text))
            {
                Written?.Invoke(_input.text);
                _input.SetValueWithoutNotify("");
            }
        }

        private bool TryGetVisualTreeAsset(out VisualTreeAsset visualTreeAsset)
        {
            visualTreeAsset = Resources.Load<VisualTreeAsset>(VisualTreePath);
            return visualTreeAsset;
        }

        private void CreateVisualTree()
        {
            if (string.IsNullOrWhiteSpace(VisualTreePath)) return;

            if (!TryGetVisualTreeAsset(out var template))
            {
                Debug.LogWarning($"Could not load template at [{VisualTreePath}]");
                return;
            }

            template.CloneTree(this);
            style.flexGrow = 1;
        }
    }
}