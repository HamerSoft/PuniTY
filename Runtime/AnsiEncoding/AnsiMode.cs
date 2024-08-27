using System;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    [Flags]
    public enum AnsiMode
    {
        /// <summary>
        /// No mode active
        /// </summary>
        None = 0,
        /// <summary>
        /// Keyboard Action Mode (KAM) is a setting in the public Set Mode for terminals.
        /// When KAM is enabled, it locks the keyboard, preventing any input from being sent to the terminal.
        /// This can be useful in scenarios where you want to temporarily disable user input, such as during a critical operation or when displaying sensitive information.
        /// </summary>
        KeyBoardAction = 1,
    }
}