using System;
using AnsiEncoding;
using UnityEngine;

namespace HamerSoft.PuniTY.Configuration
{
    public class Keyboard : IKeyboard
    {
        private event Action<KeyCode, KeyCode[]> _keyPressed;

        event Action<KeyCode, KeyCode[]> IKeyboard.KeyPressed
        {
            add => _keyPressed += value;
            remove => _keyPressed -= value;
        }

        public void PressKey(KeyCode code, KeyCode[] modifiers = null)
        {
            _keyPressed?.Invoke(code, modifiers);
        }
    }
}