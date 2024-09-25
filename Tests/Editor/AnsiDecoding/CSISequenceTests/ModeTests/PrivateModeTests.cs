using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests.ModeTests;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    [TestFixture]
    public class PrivateModeTests : AnsiDecoderTest
    {
        public const int Rows = 25;
        public const int Columns = 80;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(Rows, Columns, new AlwaysValidModeFactory());
            AnsiDecoder = new AnsiDecoder(Screen, EscapeCharacterDecoder,
                CreateSequence(
                    typeof(SetModeSequence),
                    typeof(ResetModeSequence)));
        }

        [TestCase(1, AnsiMode.ApplicationCursorKeys)]
        [TestCase(2, AnsiMode.DECANM)]
        [TestCase(3, AnsiMode.DECCOLM)]
        [TestCase(4, AnsiMode.SmoothScroll)]
        [TestCase(5, AnsiMode.ReverseVideo)]
        [TestCase(6, AnsiMode.Origin)]
        [TestCase(7, AnsiMode.AutoWrap)]
        [TestCase(8, AnsiMode.AutoRepeatKeys)]
        [TestCase(9, AnsiMode.SendMouseXY)]
        [TestCase(10, AnsiMode.ShowToolbar)]
        [TestCase(12, AnsiMode.BlinkingCursor)]
        [TestCase(13, AnsiMode.StartBlinkingCursor)]
        [TestCase(14, AnsiMode.XORBlinkingCursor)]
        [TestCase(18, AnsiMode.PrintFormFeed)]
        [TestCase(19, AnsiMode.PrintExtentFullScreen)]
        [TestCase(25, AnsiMode.ShowCursor)]
        [TestCase(30, AnsiMode.ShowScrollbar)]
        [TestCase(35, AnsiMode.EnableFontShiftingFunctions)]
        [TestCase(38, AnsiMode.Tektronix)]
        [TestCase(40, AnsiMode.Display80_132)]
        [TestCase(41, AnsiMode.More_Fix)]
        [TestCase(42, AnsiMode.NationalReplacementCharacters)]
        [TestCase(43, AnsiMode.GraphicExtendedPrint)]
        [TestCase(44, AnsiMode.MarginBell)]
        [TestCase(45, AnsiMode.ReverseWrapAround)]
        [TestCase(46, AnsiMode.XTLogging)]
        [TestCase(47, AnsiMode.AlternateScreenBuffer)]
        [TestCase(66, AnsiMode.ApplicationKeypad)]
        [TestCase(67, AnsiMode.BackArrowKeySendsBackspace)]
        [TestCase(69, AnsiMode.LeftAndRightMargin)]
        [TestCase(80, AnsiMode.SixelDisplayMode)]
        [TestCase(95, AnsiMode.DoNotClearScreenWhenDeccolm)]
        [TestCase(1000, AnsiMode.SendMouseXYOnButtonPressAndRelease)]
        [TestCase(1001, AnsiMode.UseHiliteMouseTracking)]
        [TestCase(1002, AnsiMode.UseCellMotionMouseTracking)]
        [TestCase(1003, AnsiMode.UseAllMotionMouseTracking)]
        [TestCase(1004, AnsiMode.SendFocusIn_FocusOutEvents)]
        [TestCase(1005, AnsiMode.EnableUTF_8Mouse)]
        [TestCase(1006, AnsiMode.EnableSGRMouse)]
        [TestCase(1007, AnsiMode.EnableAlternateScroll)]
        [TestCase(1010, AnsiMode.ScrollToBottomOnTtyOutput)]
        [TestCase(1011, AnsiMode.ScrollToBottomOnKeyPress)]
        [TestCase(1014, AnsiMode.FastScroll)]
        [TestCase(1015, AnsiMode.UrxvtMouse)]
        [TestCase(1016, AnsiMode.SGRMousePixelMode)]
        [TestCase(1034, AnsiMode.InterpretMetaKey)]
        [TestCase(1035, AnsiMode.SpecialModifiersAltAndNumLockKeys)]
        [TestCase(1036, AnsiMode.MetaSendsEscape)]
        [TestCase(1037, AnsiMode.SendDEL_EditingKeypadDelete)]
        [TestCase(1039, AnsiMode.AltSendsEscape)]
        [TestCase(1040, AnsiMode.KeepSelection)]
        [TestCase(1041, AnsiMode.SelectToClipBoard)]
        [TestCase(1042, AnsiMode.BellsUrgent)]
        [TestCase(1043, AnsiMode.PopOnBell)]
        [TestCase(1044, AnsiMode.KeepClipBoard)]
        [TestCase(1045, AnsiMode.ExtendedReverseWrapAround)]
        [TestCase(1046, AnsiMode.EnableSwitchingAlternateScreenBuffer)]
        [TestCase(1047, AnsiMode.UseAlternateScreenBuffer)]
        [TestCase(1048, AnsiMode.SaveCursorAsDECSC)]
        [TestCase(1049, AnsiMode.SaveCursorAsDECSC_AfterSwitchAlternateScreen)]
        [TestCase(1050, AnsiMode.FunctionKey)]
        [TestCase(1051, AnsiMode.Sun_FunctionKeys)]
        [TestCase(1052, AnsiMode.HP_FunctionKeys)]
        [TestCase(1053, AnsiMode.SCO_FunctionKeys)]
        [TestCase(1060, AnsiMode.LegacyKeyboardEmulation)]
        [TestCase(1061, AnsiMode.VT220KeyboardEmulation)]
        [TestCase(2001, AnsiMode.ReadLineMouseButton_1)]
        [TestCase(2002, AnsiMode.ReadLineMouseButton_2)]
        [TestCase(2003, AnsiMode.ReadLineMouseButton_3)]
        [TestCase(2004, AnsiMode.BracketedPaste)]
        [TestCase(2005, AnsiMode.ReadLineCharacterQuoting)]
        [TestCase(2006, AnsiMode.ReadLineNewLinePasting)]
        public void Private_SetMode_Sets_Correct_AnsiMode(int command, AnsiMode expectedMode)
        {
            SetMode(command);
            Assert.That(Screen.HasMode(expectedMode), Is.True);
        }

        [TestCase(1, AnsiMode.ApplicationCursorKeys)]
        [TestCase(2, AnsiMode.DECANM)]
        [TestCase(3, AnsiMode.DECCOLM)]
        [TestCase(4, AnsiMode.SmoothScroll)]
        [TestCase(5, AnsiMode.ReverseVideo)]
        [TestCase(6, AnsiMode.Origin)]
        [TestCase(7, AnsiMode.AutoWrap)]
        [TestCase(8, AnsiMode.AutoRepeatKeys)]
        [TestCase(9, AnsiMode.SendMouseXY)]
        [TestCase(10, AnsiMode.ShowToolbar)]
        [TestCase(12, AnsiMode.BlinkingCursor)]
        [TestCase(13, AnsiMode.StartBlinkingCursor)]
        [TestCase(14, AnsiMode.XORBlinkingCursor)]
        [TestCase(18, AnsiMode.PrintFormFeed)]
        [TestCase(19, AnsiMode.PrintExtentFullScreen)]
        [TestCase(25, AnsiMode.ShowCursor)]
        [TestCase(30, AnsiMode.ShowScrollbar)]
        [TestCase(35, AnsiMode.EnableFontShiftingFunctions)]
        [TestCase(38, AnsiMode.Tektronix)]
        [TestCase(40, AnsiMode.Display80_132)]
        [TestCase(41, AnsiMode.More_Fix)]
        [TestCase(42, AnsiMode.NationalReplacementCharacters)]
        [TestCase(43, AnsiMode.GraphicExtendedPrint)]
        [TestCase(44, AnsiMode.MarginBell)]
        [TestCase(45, AnsiMode.ReverseWrapAround)]
        [TestCase(46, AnsiMode.XTLogging)]
        [TestCase(47, AnsiMode.AlternateScreenBuffer)]
        [TestCase(66, AnsiMode.ApplicationKeypad)]
        [TestCase(67, AnsiMode.BackArrowKeySendsBackspace)]
        [TestCase(69, AnsiMode.LeftAndRightMargin)]
        [TestCase(80, AnsiMode.SixelDisplayMode)]
        [TestCase(95, AnsiMode.DoNotClearScreenWhenDeccolm)]
        [TestCase(1000, AnsiMode.SendMouseXYOnButtonPressAndRelease)]
        [TestCase(1001, AnsiMode.UseHiliteMouseTracking)]
        [TestCase(1002, AnsiMode.UseCellMotionMouseTracking)]
        [TestCase(1003, AnsiMode.UseAllMotionMouseTracking)]
        [TestCase(1004, AnsiMode.SendFocusIn_FocusOutEvents)]
        [TestCase(1005, AnsiMode.EnableUTF_8Mouse)]
        [TestCase(1006, AnsiMode.EnableSGRMouse)]
        [TestCase(1007, AnsiMode.EnableAlternateScroll)]
        [TestCase(1010, AnsiMode.ScrollToBottomOnTtyOutput)]
        [TestCase(1011, AnsiMode.ScrollToBottomOnKeyPress)]
        [TestCase(1014, AnsiMode.FastScroll)]
        [TestCase(1015, AnsiMode.UrxvtMouse)]
        [TestCase(1016, AnsiMode.SGRMousePixelMode)]
        [TestCase(1034, AnsiMode.InterpretMetaKey)]
        [TestCase(1035, AnsiMode.SpecialModifiersAltAndNumLockKeys)]
        [TestCase(1036, AnsiMode.MetaSendsEscape)]
        [TestCase(1037, AnsiMode.SendDEL_EditingKeypadDelete)]
        [TestCase(1039, AnsiMode.AltSendsEscape)]
        [TestCase(1040, AnsiMode.KeepSelection)]
        [TestCase(1041, AnsiMode.SelectToClipBoard)]
        [TestCase(1042, AnsiMode.BellsUrgent)]
        [TestCase(1043, AnsiMode.PopOnBell)]
        [TestCase(1044, AnsiMode.KeepClipBoard)]
        [TestCase(1045, AnsiMode.ExtendedReverseWrapAround)]
        [TestCase(1046, AnsiMode.EnableSwitchingAlternateScreenBuffer)]
        [TestCase(1047, AnsiMode.UseAlternateScreenBuffer)]
        [TestCase(1048, AnsiMode.SaveCursorAsDECSC)]
        [TestCase(1049, AnsiMode.SaveCursorAsDECSC_AfterSwitchAlternateScreen)]
        [TestCase(1050, AnsiMode.FunctionKey)]
        [TestCase(1051, AnsiMode.Sun_FunctionKeys)]
        [TestCase(1052, AnsiMode.HP_FunctionKeys)]
        [TestCase(1053, AnsiMode.SCO_FunctionKeys)]
        [TestCase(1060, AnsiMode.LegacyKeyboardEmulation)]
        [TestCase(1061, AnsiMode.VT220KeyboardEmulation)]
        [TestCase(2001, AnsiMode.ReadLineMouseButton_1)]
        [TestCase(2002, AnsiMode.ReadLineMouseButton_2)]
        [TestCase(2003, AnsiMode.ReadLineMouseButton_3)]
        [TestCase(2004, AnsiMode.BracketedPaste)]
        [TestCase(2005, AnsiMode.ReadLineCharacterQuoting)]
        [TestCase(2006, AnsiMode.ReadLineNewLinePasting)]
        public void Private_ResetMode_Resets_Correct_AnsiMode(int command, AnsiMode expectedMode)
        {
            SetMode(command);
            Assert.That(Screen.HasMode(expectedMode), Is.True);
            ResetMode(command);
            Assert.That(Screen.HasMode(expectedMode), Is.False);
        }

        private void SetMode(int command)
        {
            Decode($"{Escape}?{command}h");
        }

        private void ResetMode(int command)
        {
            Decode($"{Escape}?{command}l");
        }
    }
}