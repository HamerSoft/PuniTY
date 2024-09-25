namespace HamerSoft.PuniTY.AnsiEncoding.TerminalModes
{
    public enum AnsiMode
    {
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
        SendReceive = 3,

        /// <summary>
        /// Controls how line feed (LF) characters are handled.
        /// When LNM is enabled, a line feed (LF) character also performs a carriage return (CR), moving the cursor to the beginning of the next line.
        /// When LNM is disabled, a line feed only moves the cursor down to the next line without returning to the beginning.
        /// </summary>
        AutomaticNewLine = 4,

        /// <summary>
        ///  Controls the sequences sent by the arrow keys.
        /// When DECCKM is enabled, the arrow keys send application-specific control sequences (e.g., ESC OA for up, ESC OB for down).
        /// When disabled, the arrow keys send standard ANSI sequences (e.g., ESC [ A for up, ESC [ B for down).
        /// </summary>
        ApplicationCursorKeys = 5,

        /// <summary>
        /// Enables the DECANM (ANSI/VT52 Mode), switching the terminal to ANSI mode.
        /// This is useful for ensuring compatibility with ANSI standard escape sequences.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        DECANM = 6,

        /// <summary>
        /// DEC Column Mode (DECCOLM) allows you to toggle between 80 and 132 columns on a DEC terminal, providing flexibility for different types of applications and display needs.
        /// </summary>
        DECCOLM = 7,

        /// <summary>
        /// Smooth Scroll (DECSCLM) provides a smoother scrolling experience by making the terminal scroll one line at a time.
        /// This can be particularly useful for readability and reducing eye strain when working with text-heavy applications
        /// </summary>
        SmoothScroll = 8,

        /// <summary>
        /// Reverse Video Mode (DECSCNM) is a feature in DEC terminals that inverts the foreground and background colors of the display.
        /// This can be useful for highlighting text or making certain information stand out.
        /// </summary>
        /// <remarks>While both DECSCNM and CSI 7 m involve inverting colors, DECSCNM applies to the entire terminal display, whereas CSI 7 m applies only to specific text segments.</remarks>
        ReverseVideo = 9,

        /// <summary>
        /// DECOM stands for DEC Origin Mode.
        /// It is a mode that determines whether cursor positioning is relative to the entire screen or just the scrolling region.
        /// </summary>
        /// <remarks>DECOM is particularly useful in applications that need to manage a specific area of the screen independently from the rest, such as text editors or terminal-based user interfaces.</remarks>
        /// <remarks>When DECOM is enabled: Cursor positions are relative to the defined scrolling region. This means that the home position (top-left corner) is at the top of the scrolling region, not the top of the screen.
        /// When DECOM is disabled: Cursor positions are relative to the entire screen. The home position is the top-left corner of the screen.</remarks>
        Origin = 10,

        /// <summary>
        /// DECAWM stands for DEC Auto-Wrap Mode and determines whether the cursor automatically moves to the beginning of the next line when it reaches the end of the current line.
        /// </summary>
        /// <remarks>DECAWM is useful in text editors and other applications where automatic line wrapping is desired. It ensures that text flows naturally from one line to the next without manual intervention.</remarks>
        /// <remarks>When DECAWM is enabled: The cursor automatically wraps to the beginning of the next line when it reaches the end of the current line.
        /// When DECAWM is disabled: The cursor stays at the end of the current line, and any additional characters overwrite the last character of the line.</remarks>
        AutoWrap = 11,

        /// <summary>
        /// DECARM stands for DEC Auto-Repeat Mode. It determines whether a key, when held down, will continuously send its character to the terminal.
        /// </summary>
        /// <remarks>DECARM is useful in scenarios where repeated input is necessary, such as in text editors or command-line interfaces where holding down a key to move the cursor or repeat a character is desired.</remarks>
        /// <remarks>When DECARM is enabled: Holding down a key will cause it to repeat, sending multiple instances of the character to the terminal.
        /// When DECARM is disabled: Holding down a key will only send a single instance of the character, regardless of how long the key is held.</remarks>
        AutoRepeatKeys = 12,

        /// <summary>
        /// This feature enables the terminal to send the coordinates of the mouse cursor whenever a mouse button is pressed. This is useful for applications that need to track mouse interactions within the terminal window.
        /// </summary>
        /// <remarks>ESC [ M CbCxCy
        /// ESC [ M is the prefix indicating a mouse event.
        /// Cb is the button and modifier state.
        /// Cx is the X coordinate (column).
        /// Cy is the Y coordinate (row).
        /// Clicking at (10, 5) might send: ESC [ M 32 10 5</remarks>
        SendMouseXY = 13,

        /// <summary>
        /// Show toolbar in the context of rxvt (a VT102 emulator for the X Window System) is a feature that allows users to display or hide the toolbar within the terminal emulator. 
        /// </summary>
        ShowToolbar = 14,

        /// <summary>
        /// Some users prefer a blinking cursor for better visibility, while others might find it distracting.
        /// This command allows users to toggle the cursor behavior based on their preference.
        /// </summary>
        /// <remarks>In scripts that set up a user’s environment, you might want to enable or disable the blinking cursor based on the context.
        ///  For example, a script could enable the blinking cursor when running in a development environment but disable it in a production environment where a static cursor is preferred.</remarks>
        /// <remarks> For users with visual impairments, a blinking cursor can make it easier to locate the cursor on the screen.</remarks>
        BlinkingCursor = 15,

        /// <summary>
        /// The “Start blinking cursor” mode, which can only be set via resource or menu, is a feature specific to certain terminal emulators like rxvt.
        /// This mode is not controlled by an ANSI escape sequence but rather through configuration files or graphical menu options.
        /// </summary>
        StartBlinkingCursor = 16,

        /// <summary>
        /// This sequence is specific to certain terminal emulators that support this feature, such as xterm and its derivatives.
        /// It allows for more flexible control over the cursor behavior and menu interactions.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        XORBlinkingCursor = 17,

        /// <summary>
        /// The “Print Form Feed” mode, also known as DECPFF, is a feature in DEC terminals that controls how form feeds (page breaks) are handled when printing.
        /// Determines whether a form feed character (ASCII 0x0C) will be sent to the printer when a form feed is encountered in the terminal’s output.
        /// </summary>
        /// <remarks>This mode is useful when you want to control the pagination of printed output directly from the terminal.</remarks>
        PrintFormFeed = 18,

        /// <summary>
        /// The “Set print extent to full screen” mode, also known as DECPEX, is a feature in DEC terminals that controls the extent of the printed area.
        /// </summary>
        /// <remarks>: DECPEX determines whether the print area is limited to the scrolling region or extends to the entire screen.
        /// When enabled, the print extent covers the full screen, allowing for more comprehensive printouts.</remarks>
        /// <remarks>This mode is useful when you need to capture and print the entire screen content, not just the portion within the scrolling region.</remarks>
        PrintExtentFullScreen = 19,

        /// <summary>
        /// The DECTCEM (Display Enable Cursor) control sequence is used to show or hide the cursor in a terminal emulator that supports VT220 or similar terminal standards.
        /// This is particularly useful in text-based user interfaces and command-line applications where cursor visibility can enhance or detract from the user experience.
        /// </summary>
        ShowCursor = 20,

        /// <summary>
        /// In rxvt (a VT102 terminal emulator for the X Window System), you can control the visibility of the scrollbar using specific resources in your configuration files.
        /// The scrollbar can be enabled or disabled based on your preferences, which is particularly useful for users who want to customize their terminal experience.
        /// </summary>
        /// <remarks> Users who prefer using the mouse for navigation might find the scrollbar useful for quickly scrolling through terminal output.</remarks>
        /// <remarks>On systems with limited resources, reducing the number of graphical elements can improve performance.</remarks>
        ShowScrollbar = 21,

        /// <summary>
        /// In rxvt (a VT102 terminal emulator for the X Window System), font-shifting functions allow users to dynamically change the font size within the terminal.
        /// This feature is particularly useful for users who need to adjust text size for better readability or to fit more content on the screen.
        /// </summary>
        /// <remarks>Users with visual impairments may need to increase the font size for better readability.</remarks>
        EnableFontShiftingFunctions = 22,

        /// <summary>
        /// The DECTEK (Tektronix mode) control sequence allows a terminal to switch into Tektronix 4014 emulation mode.
        /// This mode is used for rendering vector graphics, which is particularly useful for applications that require detailed graphical output, such as scientific plotting or CAD programs.
        /// </summary>
        Tektronix = 23,

        /// <summary>
        /// The 80 ⇒ 132 mode control sequence allows a terminal to switch between 80-column and 132-column display modes.
        /// This feature is particularly useful for applications that need to display more data horizontally, such as wide tables or logs.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Display80_132 = 24,

        /// <summary>
        /// The more command is a terminal pager program used to view the contents of a file one screen at a time.
        /// Sometimes, users may encounter issues with more when it interacts with terminal capabilities, especially those managed by the curses library.
        /// The “fix” often involves ensuring that the terminal settings are correctly configured to handle the display and input capabilities required by more.
        /// </summary>
        /// <remarks>Users working in different terminal emulators may experience inconsistent behavior with more. By configuring the curses resources, you can ensure that more works consistently across different terminal types.</remarks>
        /// <remarks>To configure the terminal for the more(1) fix, you typically modify the .Xresources or .Xdefaults file in your home directory.</remarks>
        // ReSharper disable once InconsistentNaming
        More_Fix = 25,

        /// <summary>
        /// The DECNRCM (National Replacement Character Set Mode) control sequence allows a VT220 terminal to switch between different national character sets.
        /// This feature is particularly useful for displaying text in various languages that require specific characters not available in the standard ASCII set.
        /// </summary>
        /// <remarks>Users working with text in multiple languages may need to display characters specific to those languages.  By enabling DECNRCM, the terminal can switch to the appropriate national character set, ensuring correct display of text.</remarks>
        NationalReplacementCharacters = 26,

        /// <summary>
        /// The DECGEPM (Graphic Expanded Print Mode) control sequence allows a VT340 terminal to switch into a mode that expands the graphics for printing.
        /// This mode is particularly useful for applications that need to print detailed graphics with enhanced clarity and size.
        /// </summary>
        /// <remarks>Researchers and scientists often need to print detailed graphs and charts for reports and presentations. By enabling DECGEPM, the terminal can expand the graphics, making them clearer and more readable when printed.</remarks>
        GraphicExtendedPrint = 27,

        /// <summary>
        /// The margin bell in xterm is a feature that triggers an audible bell when the cursor approaches the right margin of the terminal window.
        /// This can be particularly useful for users who need to be alerted when they are nearing the end of a line, such as when writing code or text that should not exceed a certain width.
        /// </summary>
        /// <remarks>Writers and programmers often need to keep their text within a certain width for readability or formatting purposes. Enabling the margin bell alerts them when they are nearing the right margin, helping them maintain the desired text width.</remarks>
        MarginBell = 28,

        /// <summary>
        /// The DECGPCM (Graphic Print Color Mode) control sequence allows a VT340 terminal to switch into a mode that supports color printing of graphics.
        /// This is particularly useful for applications that need to print detailed color graphics, such as scientific visualizations or engineering diagrams.
        /// </summary>
        GraphicPrintColor = 29,

        /// <summary>
        /// Reverse-wraparound mode is a terminal feature that allows the cursor to wrap from the beginning of one line to the end of the previous line when performing a backspace operation.
        /// This is the opposite of the standard wraparound mode, where the cursor moves from the end of one line to the beginning of the next line when typing characters.
        /// </summary>
        /// <remarks>When editing text, users might need to backspace across lines without manually moving the cursor to the previous line. Enabling reverse-wraparound mode allows users to seamlessly delete characters across line boundaries, improving the efficiency of text editing.</remarks>
        ReverseWrapAround = 30,

        /// <summary>
        /// The XTLOGGING feature in xterm allows the terminal to log its output to a file.
        /// However, this feature is typically disabled by default due to security concerns and must be enabled at compile-time.
        /// If logging is enabled, it can be controlled using specific escape sequences.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        XTLogging = 31,

        /// <summary>
        /// Graphic Print Background Mode is a setting found in some terminal emulators, particularly those that support advanced graphics capabilities like sixel graphics.
        /// </summary>
        /// <remarks>When this mode is enabled, the background color of the graphics image is printed along with the image itself. This is useful for printers that can handle color or grayscale images.</remarks>
        /// <remarks>When this mode is disabled, the background color is not printed. This is typically used for printers that can only print black and white images, as it avoids printing unnecessary background colors</remarks>
        GraphicPrintBackgroundMode = 32,

        /// <summary>
        /// feature in terminal emulators that allows applications to switch between two different screen buffers: the main screen buffer and the alternate screen buffer.
        /// </summary>
        AlternateScreenBuffer = 33,

        /// <summary>
        /// Feature that allows text and graphics to be printed in a rotated orientation.
        /// This mode is particularly useful for applications that require text to be displayed in a non-standard orientation, such as certain types of graphical interfaces or specialized printing tasks.
        /// </summary>
        GraphicRotatedPrint = 34,

        /// <summary>
        /// Feature that changes the behavior of the numeric keypad.
        /// This mode is often used by applications that require different input from the keypad, such as text editors or terminal-based programs.
        /// </summary>
        /// <remarks>In normal mode, the numeric keypad behaves like a standard keypad. With NumLock on, the keys generate numbers. With NumLock off, they act as arrow keys and other navigation keys.</remarks>
        /// <remarks>When this mode is enabled, the keypad sends different escape sequences, allowing applications to interpret the keys differently.</remarks>
        ApplicationKeypad = 35,

        /// <summary>
        /// Control sequence used in VT100 and later terminals to determine whether the backarrow key should send a Backspace (BS) character or a Delete (DEL) character.
        /// </summary>
        /// <remarks>This mode is particularly useful in environments where the expected behavior of the backarrow key varies. For example, some systems and applications expect the backarrow key to send a Backspace, while others expect it to send a Delete. By toggling DECBKM, users can ensure compatibility with different systems and applications.</remarks>
        /// <remarks>When DECBKM is enabled: The backarrow key sends a Backspace (BS) character, which is ASCII code 8.</remarks>
        /// <remarks>When DECBKM is disabled: The backarrow key sends a Delete (DEL) character, which is ASCII code 127.</remarks>
        BackArrowKeySendsBackspace = 36,

        /// <summary>
        /// Terminal feature that allows you to set specific left and right margins within which text will be confined.
        /// This mode is particularly useful for creating text layouts that need to be formatted within a specific area of the terminal screen.
        /// </summary>
        LeftAndRightMargin = 37,

        /// <summary>
        /// Terminal feature that allows the display of bitmap graphics directly within the terminal.
        /// This mode uses the SIXEL graphics format, which was originally developed by Digital Equipment Corporation (DEC).
        /// </summary>
        /// <remarks>Sixel Display Mode enables terminals to render bitmap images using a series of escape sequences.
        /// This allows for more complex and colorful graphics compared to traditional text-based displays.</remarks>
        SixelDisplayMode = 38,

        /// <summary>
        /// Terminal feature that controls whether the screen is cleared when the column mode is changed.
        /// </summary>
        /// <remarks>This mode is useful in scenarios where you want to preserve the current screen content while changing the column width.
        /// For example, in applications where maintaining the display context is important, enabling DECNCSM can prevent the screen from being cleared unnecessarily.</remarks>
        /// <remarks>When DECNCSM is enabled: Changing the column mode (e.g., switching between 80 and 132 columns) does not clear the screen.</remarks>
        /// <remarks>When DECNCSM is disabled: Changing the column mode clears the screen as a side effect.</remarks>
        DoNotClearScreenWhenDeccolm = 39,

        /// <summary>
        /// Terminal feature that allows the terminal to report the mouse’s X and Y coordinates whenever a mouse button is pressed or released.
        /// This feature is commonly used in terminal applications that require mouse interaction.
        /// </summary>
        /// <remarks>When Mouse Reporting is enabled: The terminal sends escape sequences that include the mouse’s X and Y coordinates whenever a mouse button is pressed or released.</remarks>
        /// <remarks>Escape Sequences: These sequences typically start with ESC [ M followed by three characters that encode the button state and the X and Y coordinates.</remarks>
        SendMouseXYOnButtonPressAndRelease = 40,

        /// <summary>
        /// Terminal feature that allows the terminal to highlight text as the mouse moves over it and report the coordinates of the highlighted area.
        /// This mode is particularly useful for applications that need to track mouse movements and selections within a specific region of the terminal.
        /// </summary>
        /// <remarks>When Hilite Mouse Tracking is enabled: The terminal highlights text as the mouse moves over it while a button is pressed and sends the coordinates of the highlighted area to the application upon release.</remarks>
        UseHiliteMouseTracking = 41,

        /// <summary>
        /// Terminal feature that allows the terminal to report the mouse’s X and Y coordinates whenever the mouse moves from one cell to another, even if no buttons are pressed.
        /// This mode is useful for applications that need to track mouse movements continuously.
        /// </summary>
        /// <remarks>When Cell Motion Mouse Tracking is enabled: The terminal sends escape sequences that include the mouse’s X and Y coordinates whenever the mouse moves to a new cell.</remarks>
        UseCellMotionMouseTracking = 42,

        /// <summary>
        /// Terminal feature that allows the terminal to report the mouse’s X and Y coordinates whenever the mouse moves, regardless of whether any buttons are pressed.
        /// This mode is useful for applications that need to track continuous mouse movements.
        /// </summary>
        /// <remarks>When All Motion Mouse Tracking is enabled: The terminal sends escape sequences that include the mouse’s X and Y coordinates whenever the mouse moves.</remarks>
        UseAllMotionMouseTracking = 43,

        /// <summary>
        /// Terminal feature that allows the terminal to notify the application when it gains or loses focus.
        /// This is useful for applications that need to respond to focus changes, such as updating the user interface or pausing/resuming activities.
        /// </summary>
        /// <remarks>FocusIn Event: This event is sent when the terminal window gains focus, meaning it becomes the active window and can receive keyboard input.</remarks>
        /// <remarks>FocusOut Event: This event is sent when the terminal window loses focus, meaning it is no longer the active window and cannot receive keyboard input.</remarks>
        // ReSharper disable once InconsistentNaming
        SendFocusIn_FocusOutEvents = 44,

        /// <summary>
        /// Terminal feature that allows the terminal to report mouse events using UTF-8 encoding.
        /// This mode is particularly useful for terminals that support UTF-8, ensuring that mouse event data is correctly interpreted and displayed.
        /// </summary>
        /// <remarks>When UTF-8 Mouse Mode is enabled: The terminal sends mouse event data using UTF-8 encoding.</remarks>
        // ReSharper disable once InconsistentNaming
        EnableUTF_8Mouse = 45,

        /// <summary>
        /// Terminal feature that allows the terminal to report mouse events using SGR (Select Graphic Rendition) encoding.
        /// This mode provides more detailed information about mouse events and supports higher coordinate values, making it suitable for modern applications and high-resolution displays.
        /// </summary>
        /// <remarks>When SGR Mouse Mode is enabled: The terminal sends mouse event data using SGR encoding, which includes the button state, X and Y coordinates, and additional modifiers.</remarks>
        // ReSharper disable once InconsistentNaming
        EnableSGRMouse = 46,

        /// <summary>
        /// Terminal feature that changes the behavior of the mouse scroll wheel when the terminal is in the alternate screen buffer.
        /// Instead of scrolling through the terminal’s scrollback history, the scroll wheel sends up and down arrow key events to the application running in the terminal.
        /// </summary>
        /// <remarks>When Alternate Scroll Mode is enabled: The scroll wheel sends up and down arrow key events to the application, allowing it to handle scrolling internally.</remarks>
        EnableAlternateScroll = 47,

        /// <summary>
        /// Feature that ensures the terminal window automatically scrolls to the bottom whenever new output is received
        /// This is particularly useful for monitoring real-time processes or logs, as it keeps the most recent output visible without requiring manual scrolling.
        /// </summary>
        /// <remarks>Automatic Scrolling: When enabled, the terminal will automatically scroll to the bottom of the output whenever new data is received. This ensures that the latest information is always visible.</remarks>
        ScrollToBottomOnTtyOutput = 48,

        /// <summary>
        /// Feature that ensures the terminal window scrolls to the bottom of the output whenever a key is pressed.
        /// This can be particularly useful in scenarios where you want to quickly view the latest output without continuous automatic scrolling.
        /// </summary>
        /// <remarks>Manual Scrolling: When enabled, the terminal will scroll to the bottom of the output whenever any key is pressed. This allows you to control when you want to view the latest output.</remarks>
        ScrollToBottomOnKeyPress = 49,

        /// <summary>
        /// The “fastScroll” resource in xterm is a feature that enhances the scrolling performance of the terminal by reducing the amount of screen updates during rapid output.
        /// This can be particularly useful when dealing with applications or commands that produce a large amount of output quickly.
        /// </summary>
        /// <remarks>Enhanced Scrolling Performance: When enabled, fastScroll reduces the frequency of screen updates during rapid output, which can significantly improve performance and responsiveness.</remarks>
        FastScroll = 50,

        /// <summary>
        /// Enabling Mouse Mode in urxvt (rxvt-unicode) allows the terminal to capture and respond to mouse events, such as clicks and scrolls.
        /// This can be particularly useful for applications that support mouse interactions within the terminal.
        /// </summary>
        /// <remarks>Mouse Event Handling: When enabled, urxvt can capture mouse events like clicks, scrolls, and drags, and pass them to applications running within the terminal.</remarks>
        UrxvtMouse = 51,

        /// <summary>
        /// Enabling SGR Mouse PixelMode in xterm allows the terminal to report mouse events with pixel precision rather than cell-based coordinates.
        /// This can be particularly useful for applications that require more precise mouse interactions within the terminal.
        /// </summary>
        /// <remarks>Pixel Precision: When enabled, SGR Mouse PixelMode reports mouse events with pixel-level accuracy, providing finer control compared to the default cell-based reporting.</remarks>
        // ReSharper disable once InconsistentNaming
        SGRMousePixelMode = 52,

        /// <summary>
        /// Enabling the “Interpret ‘meta’ key” feature in xterm allows the terminal to set the eighth bit of keyboard input when the Meta key is pressed.
        /// This can be useful for applications that rely on this specific input behavior.
        /// </summary>
        /// <remarks>Eighth Bit Setting: When enabled, pressing the Meta key (often the Alt key) will set the eighth bit of the keyboard input. This modifies the input character by adding 128 to its ASCII value.</remarks>
        InterpretMetaKey = 53,

        /// <summary>
        /// Enabling special modifiers for the Alt and NumLock keys in xterm allows the terminal to recognize these keys as modifiers, which can be useful for certain keybindings and functions.
        /// </summary>
        SpecialModifiersAltAndNumLockKeys = 54,

        /// <summary>
        /// Enabling the “metaSendsEscape” resource in xterm allows the terminal to send an ESC character when the Meta key (often the Alt key) modifies another key.
        /// This can be useful for applications that interpret ESC-prefixed sequences as special commands.
        /// </summary>
        /// <remarks>ESC Character Sending: When enabled, pressing the Meta key in combination with another key will send an ESC character followed by the modified key. For example, pressing Meta + A will send ESC A.</remarks>
        MetaSendsEscape = 55,

        /// <summary>
        /// Enabling the “Send DEL from the editing-keypad Delete key” feature in xterm configures the terminal to send the DEL (ASCII 127) character when the Delete key on the editing keypad is pressed.
        /// This can be useful for applications that expect the Delete key to send the DEL character instead of other sequences.
        /// </summary>
        /// <remarks>DEL Character Sending: When enabled, pressing the Delete key on the editing keypad will send the DEL character (ASCII 127) instead of the default VT220-style Remove escape sequence.</remarks>
        // ReSharper disable once InconsistentNaming
        SendDEL_EditingKeypadDelete = 56,

        /// <summary>
        /// Enabling the “altSendsEscape” resource in xterm configures the terminal to send an ESC character when the Alt key modifies another key.
        /// This can be useful for applications that interpret ESC-prefixed sequences as special commands.
        /// </summary>
        /// <remarks>ESC Character Sending: When enabled, pressing the Alt key in combination with another key will send an ESC character followed by the modified key. For example, pressing Alt + A will send ESC A.</remarks>
        AltSendsEscape = 57,

        /// <summary>
        /// Enabling the “keepSelection” resource in xterm ensures that text selections are preserved even when they are not highlighted.
        /// This can be useful for maintaining selections across different operations without losing the selected text.
        /// </summary>
        /// <remarks>When enabled, the text selection remains active even if it is not visually highlighted. This allows you to perform other tasks without losing the selection.</remarks>
        KeepSelection = 58,

        /// <summary>
        /// Enabling the “selectToClipboard” resource in xterm configures the terminal to use the CLIPBOARD selection for copy and paste operations.
        /// This can be useful for integrating xterm with modern applications that rely on the CLIPBOARD buffer for these functions.
        /// </summary>
        /// <remarks>CLIPBOARD Selection: When enabled, text selected in xterm is copied to the CLIPBOARD buffer, allowing it to be pasted into other applications using standard shortcuts like Ctrl+V.</remarks>
        SelectToClipBoard = 59,

        /// <summary>
        /// Enabling the “bellIsUrgent” resource in xterm configures the terminal to set the Urgency window manager hint when a Control-G (BEL) character is received.
        /// This can be useful for drawing attention to the terminal window when an important event occurs.
        /// </summary>
        /// <remarks>Urgency Hint: When enabled, receiving a Control-G (BEL) character will set the Urgency hint for the window.
        /// This typically causes the window to flash or the taskbar entry to blink, depending on the window manager.</remarks>
        BellsUrgent = 60,

        /// <summary>
        /// Enabling the “popOnBell” resource in xterm configures the terminal to raise the window to the top of the stack when a Control-G (BEL) character is received.
        /// This can be useful for drawing immediate attention to the terminal window when an important event occurs.
        /// </summary>
        /// <remarks>Window Raising: When enabled, receiving a Control-G (BEL) character will cause the terminal window to raise to the top of the window stack, making it the active window. This ensures that the user notices the event that triggered the bell.</remarks>
        PopOnBell = 61,

        /// <summary>
        /// Enabling the “keepClipboard” resource in xterm configures the terminal to reuse the most recent data copied to the CLIPBOARD.
        /// This ensures that the clipboard content remains available for pasting, even if the selection changes.
        /// </summary>
        /// <remarks>Persistent Clipboard Data: When enabled, the most recent data copied to the CLIPBOARD is retained and can be reused, even if the selection changes or other clipboard operations occur.</remarks>
        KeepClipBoard = 62,

        /// <summary>
        /// Enabling the “Extended Reverse-wraparound mode” (XTREVWRAP2) in xterm allows the terminal to wrap the cursor from the beginning of one line to the end of the previous line when moving backwards.
        /// This can be useful for applications that require more flexible cursor movement.
        /// </summary>
        /// <remarks>Reverse-Wraparound: When enabled, moving the cursor left from the beginning of a line will wrap it to the end of the previous line. This is an extension of the standard reverse-wraparound mode.</remarks>
        ExtendedReverseWrapAround = 63,

        /// <summary>
        /// Enabling switching to/from the Alternate Screen Buffer in xterm allows applications to use a separate screen buffer for temporary content, such as when using text editors or viewing man pages.
        /// This feature is particularly useful for terminfo-based systems.
        /// </summary>
        /// <remarks>When an application switches to the Alternate Screen Buffer, the current screen content is saved, and a new, empty screen buffer is used.
        /// Upon exiting the application, the original screen content is restored.</remarks>
        EnableSwitchingAlternateScreenBuffer = 64,

        /// <summary>
        /// The “Use Alternate Screen Buffer” feature in xterm allows applications to use a separate screen buffer for temporary content, similar to the “Enable switching to/from Alternate Screen Buffer” mode. 
        /// </summary>
        UseAlternateScreenBuffer = 65,

        /// <summary>
        /// The “Save cursor as in DECSC” feature in xterm allows the terminal to save the current cursor position and attributes, which can be restored later.
        /// This is particularly useful for applications that need to temporarily move the cursor and then return it to its original position.
        /// </summary>
        /// <remarks>Applications like text editors or pagers use these sequences to save the cursor position before displaying temporary content and restore it afterward.</remarks>
        // ReSharper disable once InconsistentNaming
        SaveCursorAsDECSC = 66,

        /// <summary>
        /// The “Save cursor as in DECSC, xterm. After saving the cursor, switch to the Alternate Screen Buffer” feature in xterm combines saving the cursor position and attributes with switching to the Alternate Screen Buffer.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        SaveCursorAsDECSC_AfterSwitchAlternateScreen = 67,

        /// <summary>
        /// The Set terminfo/termcap function-key mode in xterm is a feature that allows the terminal to interpret and handle function keys according to the terminfo or termcap database.
        /// This mode is crucial for ensuring that function keys (like F1, F2, etc.) and other special keys (like Home, End, etc.) send the correct escape sequences that applications expect.
        /// </summary>
        /// <remarks>Terminfo/Termcap: These databases define the capabilities and behaviors of terminals, including the sequences sent by function keys.</remarks>
        /// <remarks>Escape Sequences: Function keys send specific escape sequences that applications interpret.</remarks>
        FunctionKey = 68,

        /// <summary>
        /// The Set Sun function-key mode in xterm is a feature that allows the terminal to emulate the function key behavior of Sun keyboards.
        /// This mode is particularly useful for users who are accustomed to Sun workstations and want to maintain consistent key mappings when using xterm.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Sun_FunctionKeys = 69,

        /// <summary>
        /// The Set HP function-key mode in xterm is a feature that allows the terminal to emulate the function key behavior of HP (Hewlett-Packard) terminals. This mode is particularly useful for users who are accustomed to HP terminals and want to maintain consistent key mappings when using xterm.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        HP_FunctionKeys = 70,

        /// <summary>
        /// The Set SCO function-key mode in xterm is a feature that allows the terminal to emulate the function key behavior of SCO (Santa Cruz Operation) UNIX systems.
        /// This mode is particularly useful for users who are accustomed to SCO UNIX environments and want to maintain consistent key mappings when using xterm.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        SCO_FunctionKeys = 71,

        /// <summary>
        /// The Set legacy keyboard emulation mode in xterm, specifically for X11R6, allows the terminal to emulate the keyboard behavior of older X11 releases.
        /// This mode is useful for users who need compatibility with legacy applications or systems that expect the keyboard behavior of X11R6.
        /// </summary>
        LegacyKeyboardEmulation = 72,

        /// <summary>
        /// The Set VT220 keyboard emulation mode in xterm allows the terminal to emulate the keyboard behavior of a DEC VT220 terminal.
        /// This mode is particularly useful for users who need compatibility with applications or systems that expect the keyboard behavior of a VT220 terminal.
        /// </summary>
        VT220KeyboardEmulation = 73,

        /// <summary>
        /// To enable mouse support for moving the cursor in readline (such as in bash) using xterm, you can use specific escape sequences to enable mouse tracking.
        /// This allows you to click with mouse button-1 to move the cursor within the terminal.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ReadLineMouseButton_1 = 74,

        /// <summary>
        /// To enable mouse support for moving the cursor in readline (such as in bash) using xterm, you can use specific escape sequences to enable mouse tracking.
        /// This allows you to click with mouse button-2 to move the cursor within the terminal.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ReadLineMouseButton_2 = 75,

        /// <summary>
        /// To enable mouse support for moving the cursor in readline (such as in bash) using xterm, you can use specific escape sequences to enable mouse tracking.
        /// This allows you to click with mouse button-3 to move the cursor within the terminal.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ReadLineMouseButton_3 = 76,

        /// <summary>
        /// The Set bracketed paste mode in xterm is a feature that helps distinguish between pasted text and typed text.
        /// This is particularly useful for applications that need to handle pasted text differently, such as disabling auto-indentation or special key bindings during paste operations.
        /// </summary>
        BracketedPaste = 77,

        /// <summary>
        /// To enable readline character-quoting in xterm, you can use specific escape sequences and readline configurations.
        /// Character-quoting allows you to insert special characters into the command line without them being interpreted by the shell.
        /// </summary>
        /// <remarks>Functionality: Character-quoting in readline allows you to insert characters that would normally have special meanings (like !, $, etc.) by preceding them with a quoting character (usually \).</remarks>
        ReadLineCharacterQuoting = 78,

        /// <summary>
        /// To enable readline newline pasting in xterm, you can use specific escape sequences to ensure that pasted text, including newlines, is handled correctly.
        /// This feature is useful for pasting multi-line commands or text into the terminal without issues.
        /// </summary>
        /// <remarks>When readline newline pasting is enabled, xterm handles pasted text with newlines properly, ensuring that each newline is interpreted as intended by the application.</remarks>
        ReadLineNewLinePasting = 79
    }
}