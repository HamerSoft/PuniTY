namespace HamerSoft.PuniTY.AnsiEncoding
{
    public enum GraphicRendition
    {
        /// all attributes off
        Reset = 0,

        /// Intensity: Bold
        Bold = 1,

        /// Intensity: Faint     not widely supported
        Faint = 2,

        /// Italic: on     not widely supported. Sometimes treated as inverse.
        Italic = 3,

        /// Underline: Single     not widely supported
        Underline = 4,

        /// Blink: Slow     less than 150 per minute
        BlinkSlow = 5,

        /// Blink: Rapid     MS-DOS ANSI.SYS; 150 per minute or more
        BlinkRapid = 6,

        /// Image: Negative     inverse or reverse; swap foreground and background
        Inverse = 7,

        /// Conceal     not widely supported
        Conceal = 8,
        
        //StrikeThrough
        StrikeThrough = 9,

        /// Font selection (not sure which)
        Font1 = 10,
        
        /// <summary>
        /// Not Implemented!
        /// </summary>
        AlternativeFont1 = 11,
        /// <summary>
        /// Not Implemented!
        /// </summary>
        AlternativeFont2 = 12,
        /// <summary>
        /// Not Implemented!
        /// </summary>
        AlternativeFont3= 13,
        /// <summary>
        /// Not Implemented!
        /// </summary>
        AlternativeFont4 = 14,
        /// <summary>
        /// Not Implemented!
        /// </summary>
        AlternativeFont5 = 15,
        /// <summary>
        /// Not Implemented!
        /// </summary>
        AlternativeFont6 = 16,
        /// <summary>
        /// Not Implemented!
        /// </summary>
        AlternativeFont7 = 17,
        /// <summary>
        /// Not Implemented!
        /// </summary>
        AlternativeFont8 = 18,
        /// <summary>
        /// Not Implemented!
        /// </summary>
        AlternativeFont9 = 19,
        
        /// <summary>
        /// Not implemented (Gothic font)
        /// </summary>
        Fraktur = 20,

        /// Underline: Double
        UnderlineDouble = 21,

        /// Intensity: Normal     not bold and not faint
        NormalIntensity = 22,

        /// Underline: None     
        NoUnderline = 24,

        /// Blink: off     
        NoBlink = 25,

        /// Image: Positive
        ///
        /// Not sure what this is supposed to be, the opposite of inverse???
        Positive = 27,

        /// Reveal,     conceal off
        Reveal = 28,

        /// Set foreground color, normal intensity
        ForegroundNormalBlack = 30,
        ForegroundNormalRed = 31,
        ForegroundNormalGreen = 32,
        ForegroundNormalYellow = 33,
        ForegroundNormalBlue = 34,
        ForegroundNormalMagenta = 35,
        ForegroundNormalCyan = 36,
        ForegroundNormalWhite = 37,
        ForegroundColor = 38,
        ForegroundNormalReset = 39,

        /// Set background color, normal intensity
        BackgroundNormalBlack = 40,
        BackgroundNormalRed = 41,
        BackgroundNormalGreen = 42,
        BackgroundNormalYellow = 43,
        BackgroundNormalBlue = 44,
        BackgroundNormalMagenta = 45,
        BackgroundNormalCyan = 46,
        BackgroundNormalWhite = 47,
        BackgroundColor = 48,
        BackgroundNormalReset = 49,

        /// Set foreground color, high intensity (aixtem)
        ForegroundBrightBlack = 90,
        ForegroundBrightRed = 91,
        ForegroundBrightGreen = 92,
        ForegroundBrightYellow = 93,
        ForegroundBrightBlue = 94,
        ForegroundBrightMagenta = 95,
        ForegroundBrightCyan = 96,
        ForegroundBrightWhite = 97,
        ForegroundBrightReset = 99,

        /// Set background color, high intensity (aixterm)
        BackgroundBrightBlack = 100,
        BackgroundBrightRed = 101,
        BackgroundBrightGreen = 102,
        BackgroundBrightYellow = 103,
        BackgroundBrightBlue = 104,
        BackgroundBrightMagenta = 105,
        BackgroundBrightCyan = 106,
        BackgroundBrightWhite = 107,
        BackgroundBrightReset = 109,

        CustomForeground8Bit = 200,
        CustomBackground8Bit = 201,
        CustomForegroundRGB = 202,
        CustomBackgroundRGB = 203
    }
}