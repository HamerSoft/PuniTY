using System;
using System.Collections.Generic;
using System.Linq;
using HamerSoft.PuniTY.AnsiEncoding;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace AnsiEncoding.Input
{
    internal class InputTransmitter : IDisposable
    {
        protected const string Escape = "\x001b[";

        private readonly IInput _input;
        private readonly IScreen _screen;
        private Dictionary<bool, HashSet<MouseButton>> _mouseButtons;
        private Vector2 _mousePosition;
        private KeyCode _keyCode;
        private KeyCode[] _modifiers;
        private bool _pixelMode;
        private PointerReportStrategy _pointerReportStrategy;

        public InputTransmitter(IInput input, IScreen screen)
        {
            _pixelMode = true;
            _input = input;
            _screen = screen;
            _input.Pointer.ButtonPressed += PointerOnButtonPressed;
            _input.Pointer.Moved += PointerOnMoved;
            _input.KeyBoard.KeyPressed += KeyBoardOnKeyPressed;
            _mouseButtons = new Dictionary<bool, HashSet<MouseButton>>
            {
                { true, new HashSet<MouseButton>() },
                { false, new HashSet<MouseButton>() }
            };
        }

        private void PointerOnMoved(Vector2 position)
        {
            _mousePosition = position;
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

            _screen.Transmit(ToBytes(command));
        }

        private void TransmitMouseTracking()
        {
            var activeButton = _mouseButtons[true].FirstOrDefault();
            int value = (int)activeButton + GetMouseModifier(_keyCode);
            string command = $"{Escape}M{value}{GetMousePosition()}";

            _screen.Transmit(ToBytes(command));
        }

        private string GetMousePosition()
        {
            if (_pixelMode)
            {
                return $"{_mousePosition.X}{_mousePosition.Y}";
            }

            throw new NotImplementedException("Default mode not implemented yet");
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

        public void SetMouseReportingMode(PointerReportStrategy pointerReportStrategy)
        {
            _pointerReportStrategy = pointerReportStrategy;
        }
    }
}