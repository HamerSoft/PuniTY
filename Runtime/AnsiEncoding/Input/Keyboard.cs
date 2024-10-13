using System;
using UnityEngine;

namespace AnsiEncoding
{
    public abstract class Keyboard : IKeyboard
    {
        private event Action<KeyCode, KeyCode[]> KeyPressed;

        event Action<KeyCode, KeyCode[]> IKeyboard.KeyPressed
        {
            add => KeyPressed += value;
            remove => KeyPressed -= value;
        }

        public void PressKey(KeyCode code, KeyCode[] modifiers = null)
        {
            KeyPressed?.Invoke(code, modifiers);
        }
    }
}