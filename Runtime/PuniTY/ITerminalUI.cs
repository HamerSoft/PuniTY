using System;

namespace HamerSoft.PuniTY
{
    /// <summary>
    /// UI component for Punity terminals.
    /// </summary>
    public interface ITerminalUI
    {
        /// <summary>
        /// Event fired when the terminal UI is closed
        /// </summary>
        public event Action Closed;

        /// <summary>
        /// Event fired when written to the terminal UI
        /// </summary>
        public event Action<string> Written;

        /// <summary>
        /// Event fired when written a byte to terminal UI
        /// </summary>
        public event Action<byte[]> WrittenByte;

        /// <summary>
        /// Event fired when written line to terminal UI
        /// </summary>
        public event Action<string> WrittenLine;

        /// <summary>
        /// Print to the screen when data is received from client
        /// </summary>
        /// <param name="message">the ansi encoded message</param>
        public void Print(string message);
        
        /// <summary>
        /// Print to the screen when data is received from client
        /// </summary>
        /// <param name="message">the ansi encoded bytes</param>
        public void Print(byte[] message);

        /// <summary>
        /// Close the UI
        /// </summary>
        public void Close();
    }
}