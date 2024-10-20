using System;
using System.Collections.Generic;
using System.Linq;
using HamerSoft.PuniTY.AnsiEncoding;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace AnsiEncoding.Input
{
    internal class InputTransmitter : IInputTransmitter
    {
        protected const string Escape = "\x001b[";

        private readonly IInput _input;
        private Dictionary<bool, HashSet<MouseButton>> _mouseButtons;
        private KeyCode _keyCode;
        private KeyCode[] _modifiers;
        private bool _pixelMode;
        private PointerReportStrategy _pointerReportStrategy;
        private event Action<byte[]> Output;

        event Action<byte[]> IInputTransmitter.Output
        {
            add => Output += value;
            remove => Output -= value;
        }

        public InputTransmitter(IInput input)
        {
            _pixelMode = true;
            _input = input;
            _input.Pointer.ButtonPressed += PointerOnButtonPressed;
            _input.Pointer.Moved += PointerOnMoved;
            _input.KeyBoard.KeyPressed += KeyBoardOnKeyPressed;
            _mouseButtons = new Dictionary<bool, HashSet<MouseButton>>
            {
                { true, new HashSet<MouseButton>() },
                { false, new HashSet<MouseButton>() }
            };
            _pointerReportStrategy = new CellReportStrategy(input.Pointer);
        }

        private void PointerOnMoved(Vector2 _)
        {
            if (_input.Pointer.IsTrackingEnabled)
                TransmitMouseTracking();
        }

        private void KeyBoardOnKeyPressed(KeyCode keyCode, KeyCode[] modifiers)
        {
            _keyCode = keyCode;
            _modifiers = modifiers;
        }

        private void PointerOnButtonPressed(MouseButton button, bool active)
        {
            _mouseButtons[active].Add(button);
            _mouseButtons[!active].Remove(button);
            if (_input.Pointer.IsTrackingEnabled)
                TransmitMouseTracking(button, active);
        }

        private void TransmitMouseTracking(MouseButton button, bool active)
        {
            var activeButton = active ? button : _mouseButtons[true].FirstOrDefault();
            int value = (int)activeButton + GetMouseModifier(_keyCode);
            string command = $"{Escape}M{value}{GetMousePosition()}";

            Transmit(ToBytes(command));
        }

        private void TransmitMouseTracking()
        {
            var activeButton = _mouseButtons[true].FirstOrDefault();
            int value = (int)activeButton + GetMouseModifier(_keyCode);
            string command = $"{Escape}M{value}{GetMousePosition()}";

            Transmit(ToBytes(command));
        }

        private string GetMousePosition()
        {
            var position = _pointerReportStrategy.GetPosition();
            return $"{position.X}{position.Y}";
        }

        private int GetMouseModifier(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.LeftShift:
                case KeyCode.RightShift:
                    return 8;

                case KeyCode.LeftAlt:
                case KeyCode.RightAlt:
                    return 16;

                case KeyCode.LeftControl:
                case KeyCode.RightControl:
                case KeyCode.LeftCommand:
                case KeyCode.RightCommand:
                    return 32;
            }

            return 0;
        }

        protected byte[] ToBytes(string toTransmit)
        {
            byte[] data = new byte[toTransmit.Length];
            int i = 0;
            foreach (char c in toTransmit)
            {
                data[i] = (byte)c;
                i++;
            }

            return data;
        }

        public void Dispose()
        {
            _input.Pointer.ButtonPressed -= PointerOnButtonPressed;
            _input.Pointer.Moved -= PointerOnMoved;
            _input.KeyBoard.KeyPressed -= KeyBoardOnKeyPressed;
        }

        void IInputTransmitter.SetMouseReportingMode(PointerReportStrategy pointerReportStrategy)
        {
            _pointerReportStrategy = pointerReportStrategy;
        }

        public void Transmit(string data)
        {
            Transmit(ToBytes(data));
        }

        public void Transmit(byte[] data)
        {
            Output?.Invoke(data);
        }
    }
}