using System;
using UnityEngine;

namespace AnsiEncoding
{
    public interface IKeyboard
    {
        internal event Action<KeyCode, KeyCode[]> KeyPressed; 
        public void PressKey(KeyCode code, KeyCode[] modifiers = null);
    }
}