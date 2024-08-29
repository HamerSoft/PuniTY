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

        /// <summary>
        /// Inserts characters at the cursor position, shifting existing characters to the right.
        /// </summary>
        Insert = 2,

        /// <summary>
        /// Controls local echo behavior.
        /// When SRM is enabled, the terminal handles the display of characters typed on the keyboard locally, meaning the characters are immediately shown on the screen without needing to be echoed back by the host system.
        /// This reduces the need for round-trip communication between the terminal and the host, which can improve performance and responsiveness, especially in environments with high latency or limited bandwidth.
        /// When SRM is disabled, the terminal waits for the host to send back the characters for display, which can be useful for ensuring that the data received and displayed is exactly what the host has processed.
        /// </summary>
        SendReceive = 4,

        /// <summary>
        /// Controls how line feed (LF) characters are handled.
        /// When LNM is enabled, a line feed (LF) character also performs a carriage return (CR), moving the cursor to the beginning of the next line.
        /// When LNM is disabled, a line feed only moves the cursor down to the next line without returning to the beginning.
        /// </summary>
        AutomaticNewLine = 8,

        /// <summary>
        ///  Controls the sequences sent by the arrow keys.
        /// When DECCKM is enabled, the arrow keys send application-specific control sequences (e.g., ESC OA for up, ESC OB for down).
        /// When disabled, the arrow keys send standard ANSI sequences (e.g., ESC [ A for up, ESC [ B for down).
        /// </summary>
        ApplicationCursorKeys = 16,

        /// <summary>
        /// Enables the DECANM (ANSI/VT52 Mode), switching the terminal to ANSI mode.
        /// This is useful for ensuring compatibility with ANSI standard escape sequences.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        DECANM = 32,
        /// <summary>
        /// DEC Column Mode (DECCOLM) allows you to toggle between 80 and 132 columns on a DEC terminal, providing flexibility for different types of applications and display needs.
        /// </summary>
        DECCOLM = 64,
        /// <summary>
        /// Smooth Scroll (DECSCLM) provides a smoother scrolling experience by making the terminal scroll one line at a time.
        /// This can be particularly useful for readability and reducing eye strain when working with text-heavy applications
        /// </summary>
        SmoothScroll = 128,
        /// <summary>
        /// Reverse Video Mode (DECSCNM) is a feature in DEC terminals that inverts the foreground and background colors of the display.
        /// This can be useful for highlighting text or making certain information stand out.
        /// </summary>
        /// <remarks>While both DECSCNM and CSI 7 m involve inverting colors, DECSCNM applies to the entire terminal display, whereas CSI 7 m applies only to specific text segments.</remarks>
        ReverseVideo = 256
        
    }
}