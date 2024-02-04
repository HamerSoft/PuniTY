using System;
using UnityEngine;

namespace HamerSoft.PuniTY.Utilities
{
    internal class UnityEvents : MonoBehaviour
    {
        internal event Action ApplicationQuit;

        private void OnDestroy()
        {
            ApplicationQuit?.Invoke();
        }

        private void OnApplicationQuit()
        {
            Destroy(gameObject);
        }
    }
}