using System;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    [Flags]
    public enum AnsiMode : ulong
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
        KeyBoardAction = 1UL,

        /// <summary>
        /// Inserts characters at the cursor position, shifting existing characters to the right.
        /// </summary>
        Insert = 1UL << 1,

        /// <summary>
        /// Controls local echo behavior.
        /// When SRM is enabled, the terminal handles the display of characters typed on the keyboard locally, meaning the characters are immediately shown on the screen without needing to be echoed back by the host system.
        /// This reduces the need for round-trip communication between the terminal and the host, which can improve performance and responsiveness, especially in environments with high latency or limited bandwidth.
        /// When SRM is disabled, the terminal waits for the host to send back the characters for display, which can be useful for ensuring that the data received and displayed is exactly what the host has processed.
        /// </summary>
        SendReceive = 1UL << 2,

        /// <summary>
        /// Controls how line feed (LF) characters are handled.
        /// When LNM is enabled, a line feed (LF) character also performs a carriage return (CR), moving the cursor to the beginning of the next line.
        /// When LNM is disabled, a line feed only moves the cursor down to the next line without returning to the beginning.
        /// </summary>
        AutomaticNewLine = 1UL << 3,

        /// <summary>
        ///  Controls the sequences sent by the arrow keys.
        /// When DECCKM is enabled, the arrow keys send application-specific control sequences (e.g., ESC OA for up, ESC OB for down).
        /// When disabled, the arrow keys send standard ANSI sequences (e.g., ESC [ A for up, ESC [ B for down).
        /// </summary>
        ApplicationCursorKeys = 1UL << 4,

        /// <summary>
        /// Enables the DECANM (ANSI/VT52 Mode), switching the terminal to ANSI mode.
        /// This is useful for ensuring compatibility with ANSI standard escape sequences.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        DECANM = 1UL << 5,

        /// <summary>
        /// DEC Column Mode (DECCOLM) allows you to toggle between 80 and 132 columns on a DEC terminal, providing flexibility for different types of applications and display needs.
        /// </summary>
        DECCOLM = 1UL << 6,

        /// <summary>
        /// Smooth Scroll (DECSCLM) provides a smoother scrolling experience by making the terminal scroll one line at a time.
        /// This can be particularly useful for readability and reducing eye strain when working with text-heavy applications
        /// </summary>
        SmoothScroll = 1UL << 7,

        /// <summary>
        /// Reverse Video Mode (DECSCNM) is a feature in DEC terminals that inverts the foreground and background colors of the display.
        /// This can be useful for highlighting text or making certain information stand out.
        /// </summary>
        /// <remarks>While both DECSCNM and CSI 7 m involve inverting colors, DECSCNM applies to the entire terminal display, whereas CSI 7 m applies only to specific text segments.</remarks>
        ReverseVideo = 1UL << 8,

        /// <summary>
        /// DECOM stands for DEC Origin Mode.
        /// It is a mode that determines whether cursor positioning is relative to the entire screen or just the scrolling region.
        /// </summary>
        /// <remarks>DECOM is particularly useful in applications that need to manage a specific area of the screen independently from the rest, such as text editors or terminal-based user interfaces.</remarks>
        /// <remarks>When DECOM is enabled: Cursor positions are relative to the defined scrolling region. This means that the home position (top-left corner) is at the top of the scrolling region, not the top of the screen.
        /// When DECOM is disabled: Cursor positions are relative to the entire screen. The home position is the top-left corner of the screen.</remarks>
        Origin = 1UL << 9,

        /// <summary>
        /// DECAWM stands for DEC Auto-Wrap Mode and determines whether the cursor automatically moves to the beginning of the next line when it reaches the end of the current line.
        /// </summary>
        /// <remarks>DECAWM is useful in text editors and other applications where automatic line wrapping is desired. It ensures that text flows naturally from one line to the next without manual intervention.</remarks>
        /// <remarks>When DECAWM is enabled: The cursor automatically wraps to the beginning of the next line when it reaches the end of the current line.
        /// When DECAWM is disabled: The cursor stays at the end of the current line, and any additional characters overwrite the last character of the line.</remarks>
        AutoWrap = 1UL << 10,

        /// <summary>
        /// DECARM stands for DEC Auto-Repeat Mode. It determines whether a key, when held down, will continuously send its character to the terminal.
        /// </summary>
        /// <remarks>DECARM is useful in scenarios where repeated input is necessary, such as in text editors or command-line interfaces where holding down a key to move the cursor or repeat a character is desired.</remarks>
        /// <remarks>When DECARM is enabled: Holding down a key will cause it to repeat, sending multiple instances of the character to the terminal.
        /// When DECARM is disabled: Holding down a key will only send a single instance of the character, regardless of how long the key is held.</remarks>
        AutoRepeatKeys = 1UL << 11,

        /// <summary>
        /// This feature enables the terminal to send the coordinates of the mouse cursor whenever a mouse button is pressed. This is useful for applications that need to track mouse interactions within the terminal window.
        /// </summary>
        /// <remarks>ESC [ M CbCxCy
        /// ESC [ M is the prefix indicating a mouse event.
        /// Cb is the button and modifier state.
        /// Cx is the X coordinate (column).
        /// Cy is the Y coordinate (row).
        /// Clicking at (10, 5) might send: ESC [ M 32 10 5</remarks>
        SendMouseXY = 1UL << 12,

        /// <summary>
        /// Show toolbar in the context of rxvt (a VT102 emulator for the X Window System) is a feature that allows users to display or hide the toolbar within the terminal emulator. 
        /// </summary>
        ShowToolbar = 1UL << 13,

        /// <summary>
        /// Some users prefer a blinking cursor for better visibility, while others might find it distracting.
        /// This command allows users to toggle the cursor behavior based on their preference.
        /// </summary>
        /// <remarks>In scripts that set up a user’s environment, you might want to enable or disable the blinking cursor based on the context.
        ///  For example, a script could enable the blinking cursor when running in a development environment but disable it in a production environment where a static cursor is preferred.</remarks>
        /// <remarks> For users with visual impairments, a blinking cursor can make it easier to locate the cursor on the screen.</remarks>
        BlinkingCursor = 1UL << 14,

        /// <summary>
        /// The “Start blinking cursor” mode, which can only be set via resource or menu, is a feature specific to certain terminal emulators like rxvt.
        /// This mode is not controlled by an ANSI escape sequence but rather through configuration files or graphical menu options.
        /// </summary>
        StartBlinkingCursor = 1UL << 15,
        /// <summary>
        /// This sequence is specific to certain terminal emulators that support this feature, such as xterm and its derivatives.
        /// It allows for more flexible control over the cursor behavior and menu interactions.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        XORBlinkingCursor = 1UL << 16,
        /// <summary>
        /// The “Print Form Feed” mode, also known as DECPFF, is a feature in DEC terminals that controls how form feeds (page breaks) are handled when printing.
        /// Determines whether a form feed character (ASCII 0x0C) will be sent to the printer when a form feed is encountered in the terminal’s output.
        /// </summary>
        /// <remarks>This mode is useful when you want to control the pagination of printed output directly from the terminal.</remarks>
        PrintFormFeed = 1UL << 16,
        /// <summary>
        /// The “Set print extent to full screen” mode, also known as DECPEX, is a feature in DEC terminals that controls the extent of the printed area.
        /// </summary>
        /// <remarks>: DECPEX determines whether the print area is limited to the scrolling region or extends to the entire screen.
        /// When enabled, the print extent covers the full screen, allowing for more comprehensive printouts.</remarks>
        /// <remarks>This mode is useful when you need to capture and print the entire screen content, not just the portion within the scrolling region.</remarks>
        PrintExtentFullScreen  = 1UL << 17,
        
    }
}