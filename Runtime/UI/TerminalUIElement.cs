using System;
using HamerSoft.Threads;
using UnityEngine;
using UnityEngine.UIElements;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace HamerSoft.PuniTY.UI
{
    public class TerminalUIElement : VisualElement, ITerminalUI
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

        public TerminalUIElement(ILogger logger)
        {
            _logger = logger;
            CreateVisualTree();
            _output = this.Q<TextField>("output");
            _input = this.Q<TextField>("input");
        }

        public async void Print(string message)
        {
            await Dispatcher.ToMainThread();
            _output.value += message;
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