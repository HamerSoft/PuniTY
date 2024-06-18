using System.Text.RegularExpressions;
using HamerSoft.PuniTY.AnsiEncoding;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.CSISequenceTests
{
    public class GraphicsRenditionTests : AnsiDecoderTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Screen = new MockScreen(10, 10);
            AnsiDecoder = new AnsiDecoder(Screen,
                EscapeCharacterDecoder,
                new GraphicsRenditionSequence());
        }

        [Test]
        public void When_SGR_0_All_Attributes_Are_Reset()
        {
            Decode($"{Escape}0m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)));
        }

        [Test]
        public void When_SGR_1_Attributes_Are_Bold()
        {
            Decode($"{Escape}1m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsBold = true }));
        }

        [Test]
        public void When_SGR_2_Attributes_Are_Faint()
        {
            Decode($"{Escape}2m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsFaint = true }));
        }

        [Test]
        public void When_SGR_3_Attributes_Are_Italic()
        {
            Decode($"{Escape}3m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsItalic = true }));
        }

        [Test]
        public void When_SGR_4_Attributes_Are_UnderLined()
        {
            Decode($"{Escape}4m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)
                    { UnderlineMode = UnderlineMode.Single }));
        }

        [Test]
        public void When_SGR_5_Attributes_Are_SlowBlink()
        {
            Decode($"{Escape}5m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { BlinkSpeed = BlinkSpeed.Slow }));
        }

        [Test]
        public void When_SGR_6_Attributes_Are_RapidBlink()
        {
            Decode($"{Escape}6m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { BlinkSpeed = BlinkSpeed.Rapid }));
        }

        [Test]
        public void When_SGR_7_Attributes_Are_InvertColors()
        {
            Decode($"{Escape}7m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.Black, AnsiColor.White))); // <-- inverted colors
        }

        [Test]
        public void When_SGR_7_Attributes_Are_InvertColors_Including_Custom_RGB_Colors()
        {
            int[] rgb = { 255, 0, 0 };
            Decode($"{Escape}38;5;{196}m"); // <- 8bit red color
            var attributes = GetGraphicsAttributes();
            Assert.That(attributes.Foreground, Is.EqualTo(AnsiColor.Rgb));
            Assert.That(attributes.ForegroundRGBColor,
                Is.EqualTo(new RgbColor(rgb[0], rgb[1], rgb[2])));
            Assert.That(attributes.UnderLineColorRGBColor,
                Is.EqualTo(new RgbColor(rgb[0], rgb[1], rgb[2])));
            Assert.That(attributes.Background, Is.EqualTo(AnsiColor.Black));

            Decode($"{Escape}7m");
            attributes = GetGraphicsAttributes(2);
            Assert.That(attributes.Background, Is.EqualTo(AnsiColor.Rgb));
            Assert.That(attributes.BackgroundRGBColor,
                Is.EqualTo(new RgbColor(rgb[0], rgb[1], rgb[2])));
            Assert.That(attributes.Foreground, Is.EqualTo(AnsiColor.Black));
            Assert.That(attributes.ForegroundRGBColor, Is.EqualTo((RgbColor)default));
            Assert.That(attributes.UnderLineColorRGBColor, Is.EqualTo((RgbColor)default));
        }

        [Test]
        public void When_SGR_8_Attributes_Are_Concealed()
        {
            Decode($"{Escape}8m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsConcealed = true }));
        }

        [Test]
        public void When_SGR_9_Attributes_Are_StrikeThrough()
        {
            Decode($"{Escape}9m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsStrikeThrough = true }));
        }

        [Test]
        public void When_SGR_10_Attributes_Are_NormalFont()
        {
            Debug.Log("SGR 10 Test - Font -> what to do here?");
            Decode($"{Escape}10m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)));
        }

        [TestCase(GraphicRendition.AlternativeFont1)]
        [TestCase(GraphicRendition.AlternativeFont2)]
        [TestCase(GraphicRendition.AlternativeFont3)]
        [TestCase(GraphicRendition.AlternativeFont4)]
        [TestCase(GraphicRendition.AlternativeFont5)]
        [TestCase(GraphicRendition.AlternativeFont6)]
        [TestCase(GraphicRendition.AlternativeFont7)]
        [TestCase(GraphicRendition.AlternativeFont8)]
        [TestCase(GraphicRendition.AlternativeFont9)]
        public void When_SGR_Alternative_Fonts_Are_NotImplemented(GraphicRendition rendition)
        {
            Decode($"{Escape}{((int)rendition)}m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)));
            LogAssert.Expect(LogType.Warning, new Regex(""));
        }

        [Test]
        public void When_SGR_Fraktur_Font_Is_NotImplemented()
        {
            Decode($"{Escape}{20}m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)));
            LogAssert.Expect(LogType.Warning, new Regex(""));
        }

        [Test]
        public void When_SGR_21_Attributes_Are_DoubleUnderlined()
        {
            Decode($"{Escape}21m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)
                    { UnderlineMode = UnderlineMode.Double }));
        }

        [Test]
        public void When_SGR_22_Attributes_Are_Resetting_Bold()
        {
            Decode($"{Escape}1m");
            Assert.That(GetGraphicsAttributes(1),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsBold = true }));
            Decode($"{Escape}22m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsBold = false }));
        }

        [Test]
        public void When_SGR_22_Attributes_Are_Resetting_Faint()
        {
            Decode($"{Escape}2m");
            Assert.That(GetGraphicsAttributes(1),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsFaint = true }));
            Decode($"{Escape}22m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsFaint = false }));
        }

        [Test]
        public void When_SGR_23_Attributes_Are_Resetting_Italic()
        {
            Decode($"{Escape}3m");
            Assert.That(GetGraphicsAttributes(1),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsItalic = true }));
            Decode($"{Escape}23m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsItalic = false }));
        }

        [Test]
        public void When_SGR_24_Attributes_Are_Resetting_Underline()
        {
            Decode($"{Escape}4m");
            Assert.That(GetGraphicsAttributes(1),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)
                    { UnderlineMode = UnderlineMode.Single }));
            Decode($"{Escape}24m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)
                    { UnderlineMode = UnderlineMode.None }));
        }

        [Test]
        public void When_SGR_25_Attributes_Are_Resetting_Blink()
        {
            Decode($"{Escape}5m");
            Assert.That(GetGraphicsAttributes(1),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { BlinkSpeed = BlinkSpeed.Slow }));
            Decode($"{Escape}25m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { BlinkSpeed = BlinkSpeed.None }));
        }

        [Test]
        public void When_SGR_26_Attributes_Are_ProportionallySpaced()
        {
            Decode($"{Escape}26m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)
                    { IsProportionalSpaced = true }));
        }

        [Test]
        public void When_SGR_27_Attributes_Are_Resetting_Inverse()
        {
            Decode($"{Escape}7m");
            Assert.That(GetGraphicsAttributes(1),
                Is.EqualTo(new GraphicAttributes(AnsiColor.Black, AnsiColor.White)));
            Decode($"{Escape}27m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)));
        }

        [Test]
        public void When_SGR_28_Attributes_Are_Resetting_Concealed()
        {
            Decode($"{Escape}8m");
            Assert.That(GetGraphicsAttributes(1),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsConcealed = true }));
            Decode($"{Escape}28m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsConcealed = false }));
        }

        [Test]
        public void When_SGR_29_Attributes_Are_Resetting_Strikethrough()
        {
            Decode($"{Escape}9m");
            Assert.That(GetGraphicsAttributes(1),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsStrikeThrough = true }));
            Decode($"{Escape}29m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black) { IsStrikeThrough = false }));
        }

        [TestCase(GraphicRendition.ForegroundNormalBlack, AnsiColor.Black)]
        [TestCase(GraphicRendition.ForegroundNormalRed, AnsiColor.Red)]
        [TestCase(GraphicRendition.ForegroundNormalGreen, AnsiColor.Green)]
        [TestCase(GraphicRendition.ForegroundNormalYellow, AnsiColor.Yellow)]
        [TestCase(GraphicRendition.ForegroundNormalBlue, AnsiColor.Blue)]
        [TestCase(GraphicRendition.ForegroundNormalMagenta, AnsiColor.Magenta)]
        [TestCase(GraphicRendition.ForegroundNormalCyan, AnsiColor.Cyan)]
        [TestCase(GraphicRendition.ForegroundNormalWhite, AnsiColor.White)]
        [TestCase(GraphicRendition.ForegroundBrightBlack, AnsiColor.BrightBlack)]
        [TestCase(GraphicRendition.ForegroundBrightRed, AnsiColor.BrightRed)]
        [TestCase(GraphicRendition.ForegroundBrightGreen, AnsiColor.BrightGreen)]
        [TestCase(GraphicRendition.ForegroundBrightYellow, AnsiColor.BrightYellow)]
        [TestCase(GraphicRendition.ForegroundBrightBlue, AnsiColor.BrightBlue)]
        [TestCase(GraphicRendition.ForegroundBrightMagenta, AnsiColor.BrightMagenta)]
        [TestCase(GraphicRendition.ForegroundBrightCyan, AnsiColor.BrightCyan)]
        [TestCase(GraphicRendition.ForegroundBrightWhite, AnsiColor.BrightWhite)]
        public void When_SGR_ForeGroundColor_Attributes_Are_Setting_AnsiColours(GraphicRendition graphicRendition,
            AnsiColor color)
        {
            Decode($"{Escape}{(int)graphicRendition}m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(color, AnsiColor.Black)));
        }

        [TestCase(true, 196, new[] { 255, 0, 0 })] // red
        [TestCase(true, 21, new[] { 0, 0, 255 })] // blue
        [TestCase(true, 46, new[] { 0, 255, 0 })] // blue
        [TestCase(true, 133, new[] { 153, 51, 153 })] // pink'ish color
        [TestCase(true, 16, new[] { 0, 0, 0 })] // black
        [TestCase(true, 231, new[] { 255, 255, 255 })] // white
        [TestCase(true, 232, new[] { 8, 8, 8 })] // dark gray scale
        [TestCase(true, 244, new[] { 128, 128, 128 })] // mid gray scale
        [TestCase(true, 255, new[] { 238, 238, 238 })] // light gray scale
        [TestCase(true, -1, new[] { 0, 0, 0 })] // underflow
        [TestCase(true, 10000, new[] { 0, 0, 0 })] //overflow
        [TestCase(false, 196, new[] { 255, 0, 0 })] // red
        [TestCase(false, 21, new[] { 0, 0, 255 })] // blue
        [TestCase(false, 46, new[] { 0, 255, 0 })] // blue
        [TestCase(false, 133, new[] { 153, 51, 153 })] // pink'ish color
        [TestCase(false, 16, new[] { 0, 0, 0 })] // black
        [TestCase(false, 231, new[] { 255, 255, 255 })] // white
        [TestCase(false, 232, new[] { 8, 8, 8 })] // dark gray scale
        [TestCase(false, 244, new[] { 128, 128, 128 })] // mid gray scale
        [TestCase(false, 255, new[] { 238, 238, 238 })] // light gray scale
        [TestCase(false, -1, new[] { 0, 0, 0 })] // underflow
        [TestCase(false, 10000, new[] { 0, 0, 0 })] //overflow
        public void When_SGR_38_Or_48_Attributes_Is_Setting_CustomColor(bool isForeground, int colorId, int[] rgb)
        {
            Decode($"{Escape}{(isForeground ? "38" : "48")};5;{colorId}m");
            if (isForeground)
            {
                Assert.That(GetGraphicsAttributes().Foreground, Is.EqualTo(AnsiColor.Rgb));
                Assert.That(GetGraphicsAttributes().ForegroundRGBColor,
                    Is.EqualTo(new RgbColor(rgb[0], rgb[1], rgb[2])));
            }
            else
            {
                Assert.That(GetGraphicsAttributes().Background, Is.EqualTo(AnsiColor.Rgb));
                Assert.That(GetGraphicsAttributes().BackgroundRGBColor,
                    Is.EqualTo(new RgbColor(rgb[0], rgb[1], rgb[2])));
            }
        }


        [TestCase(true, 0, AnsiColor.Black)] // default black
        [TestCase(true, 7, AnsiColor.White)] // default white
        [TestCase(true, 8, AnsiColor.BrightBlack)] // default white
        [TestCase(true, 15, AnsiColor.BrightWhite)] // default white
        [TestCase(true, -1, AnsiColor.Black)] // underflow
        [TestCase(true, 10000, AnsiColor.Black)] //overflow
        [TestCase(false, 0, AnsiColor.Black)] // default black
        [TestCase(false, 7, AnsiColor.White)] // default white
        [TestCase(false, 8, AnsiColor.BrightBlack)] // default white
        [TestCase(false, 15, AnsiColor.BrightWhite)] // default white
        [TestCase(false, -1, AnsiColor.Black)] // underflow
        [TestCase(false, 10000, AnsiColor.Black)] //overflow
        public void When_SGR_38_Attributes_Is_Setting_DefaultColors_When_In_DefaultRange_0_15(bool isForeground,
            int colorId, AnsiColor color)
        {
            Decode($"{Escape}{(isForeground ? "38" : "48")};5;{colorId}m");
            if (isForeground)
            {
                Assert.That(GetGraphicsAttributes().Foreground, Is.EqualTo(color));
                Assert.That(GetGraphicsAttributes().ForegroundRGBColor, Is.EqualTo((RgbColor)default));
            }
            else
            {
                Assert.That(GetGraphicsAttributes().Background, Is.EqualTo(color));
                Assert.That(GetGraphicsAttributes().BackgroundRGBColor, Is.EqualTo((RgbColor)default));
            }
        }

        [TestCase(true, new[] { 255, 0, 0 })]
        [TestCase(true, new[] { 0, 0, 255 })]
        [TestCase(true, new[] { 0, 255, 0 })]
        [TestCase(true, new[] { 153, 51, 153 })]
        [TestCase(true, new[] { 0, 0, 0 })]
        [TestCase(true, new[] { 255, 255, 255 })]
        [TestCase(true, new[] { 8, 8, 8 })]
        [TestCase(true, new[] { 128, 128, 128 })]
        [TestCase(true, new[] { 238, 238, 238 })]
        [TestCase(false, new[] { 255, 0, 0 })]
        [TestCase(false, new[] { 0, 0, 255 })]
        [TestCase(false, new[] { 0, 255, 0 })]
        [TestCase(false, new[] { 153, 51, 153 })]
        [TestCase(false, new[] { 0, 0, 0 })]
        [TestCase(false, new[] { 255, 255, 255 })]
        [TestCase(false, new[] { 8, 8, 8 })]
        [TestCase(false, new[] { 128, 128, 128 })]
        [TestCase(false, new[] { 238, 238, 238 })]
        public void When_SGR_38_Or_48_Attributes_Is_Setting_Custom_RGB_Color(bool isForeground, int[] rgb)
        {
            Decode($"{Escape}{(isForeground ? "38" : "48")};2;{rgb[0]};{rgb[1]};{rgb[2]}m");
            if (isForeground)
            {
                Assert.That(GetGraphicsAttributes().Foreground, Is.EqualTo(AnsiColor.Rgb));
                Assert.That(GetGraphicsAttributes().ForegroundRGBColor,
                    Is.EqualTo(new RgbColor(rgb[0], rgb[1], rgb[2])));
            }
            else
            {
                Assert.That(GetGraphicsAttributes().Background, Is.EqualTo(AnsiColor.Rgb));
                Assert.That(GetGraphicsAttributes().BackgroundRGBColor,
                    Is.EqualTo(new RgbColor(rgb[0], rgb[1], rgb[2])));
            }
        }

        [Test]
        public void When_SGR_39_ForeGroundColor_Attributes_Are_Setting_DefaultColor()
        {
            Decode($"{Escape}{(int)GraphicRendition.ForegroundBrightBlue}m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.BrightBlue, AnsiColor.Black)));
            Decode($"{Escape}39m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)));
        }

        [TestCase(GraphicRendition.BackgroundNormalBlack, AnsiColor.Black)]
        [TestCase(GraphicRendition.BackgroundNormalRed, AnsiColor.Red)]
        [TestCase(GraphicRendition.BackgroundNormalGreen, AnsiColor.Green)]
        [TestCase(GraphicRendition.BackgroundNormalYellow, AnsiColor.Yellow)]
        [TestCase(GraphicRendition.BackgroundNormalBlue, AnsiColor.Blue)]
        [TestCase(GraphicRendition.BackgroundNormalMagenta, AnsiColor.Magenta)]
        [TestCase(GraphicRendition.BackgroundNormalCyan, AnsiColor.Cyan)]
        [TestCase(GraphicRendition.BackgroundNormalWhite, AnsiColor.White)]
        [TestCase(GraphicRendition.BackgroundBrightBlack, AnsiColor.BrightBlack)]
        [TestCase(GraphicRendition.BackgroundBrightRed, AnsiColor.BrightRed)]
        [TestCase(GraphicRendition.BackgroundBrightGreen, AnsiColor.BrightGreen)]
        [TestCase(GraphicRendition.BackgroundBrightYellow, AnsiColor.BrightYellow)]
        [TestCase(GraphicRendition.BackgroundBrightBlue, AnsiColor.BrightBlue)]
        [TestCase(GraphicRendition.BackgroundBrightMagenta, AnsiColor.BrightMagenta)]
        [TestCase(GraphicRendition.BackgroundBrightCyan, AnsiColor.BrightCyan)]
        [TestCase(GraphicRendition.BackgroundBrightWhite, AnsiColor.BrightWhite)]
        public void When_SGR_BackGroundColor_Attributes_Are_Setting_AnsiColours(GraphicRendition graphicRendition,
            AnsiColor color)
        {
            Decode($"{Escape}{(int)graphicRendition}m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, color)));
        }

        [Test]
        public void When_SGR_49_BackGroundColor_Attributes_Are_Setting_DefaultColor()
        {
            Decode($"{Escape}{(int)GraphicRendition.BackgroundBrightCyan}m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.BrightCyan)));
            Decode($"{Escape}49m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)));
        }

        [Test]
        public void When_SGR_50_Attributes_Are_Disabling_ProportionallySpaced()
        {
            Decode($"{Escape}26m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)
                    { IsProportionalSpaced = true }));

            Decode($"{Escape}50m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)));
        }

        [Test]
        public void When_SGR_51_Attributes_Are_Framed()
        {
            Decode($"{Escape}51m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)
                    { IsFramed = true }));
        }

        [Test]
        public void When_SGR_52_Attributes_Are_Encircled()
        {
            Decode($"{Escape}52m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)
                    { IsEncircled = true }));
        }

        [Test]
        public void When_SGR_53_Attributes_Are_Overlined()
        {
            Decode($"{Escape}53m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)
                    { IsOverLined = true }));
        }

        [Test]
        public void When_SGR_54_Attributes_Are_NotFramedOrEncircled()
        {
            Decode($"{Escape}51m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)
                    { IsFramed = true }));

            Decode($"{Escape}52m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)
                    { IsFramed = true, IsEncircled = true }));

            Decode($"{Escape}54m");
            Assert.That(GetGraphicsAttributes(3),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)));
        }

        [Test]
        public void When_SGR_55_Attributes_Are_NoLonger_Overlined()
        {
            Decode($"{Escape}53m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)
                    { IsOverLined = true }));
            Decode($"{Escape}55m");
            Assert.That(GetGraphicsAttributes(2),
                Is.EqualTo(new GraphicAttributes(AnsiColor.White, AnsiColor.Black)));
        }

        [TestCase(196, new[] { 255, 0, 0 })] // red
        [TestCase(21, new[] { 0, 0, 255 })] // blue
        [TestCase(46, new[] { 0, 255, 0 })] // blue
        [TestCase(133, new[] { 153, 51, 153 })] // pink'ish color
        [TestCase(16, new[] { 0, 0, 0 })] // black
        [TestCase(231, new[] { 255, 255, 255 })] // white
        [TestCase(232, new[] { 8, 8, 8 })] // dark gray scale
        [TestCase(244, new[] { 128, 128, 128 })] // mid gray scale
        [TestCase(255, new[] { 238, 238, 238 })] // light gray scale
        [TestCase(-1, new[] { 0, 0, 0 })] // underflow
        [TestCase(10000, new[] { 0, 0, 0 })] //overflow
        public void When_SGR_58_Attributes_Is_Setting_Custom_UnderLine_Color(int colorId, int[] rgb)
        {
            Decode($"{Escape}58;5;{colorId}m");
            Assert.That(GetGraphicsAttributes().UnderLineColorRGBColor,
                Is.EqualTo(new RgbColor(rgb[0], rgb[1], rgb[2])));
        }


        [TestCase(new[] { 255, 0, 0 }, new[] { 255, 0, 0 })]
        [TestCase(new[] { 0, 0, 255 }, new[] { 0, 0, 255 })]
        [TestCase(new[] { 0, 255, 0 }, new[] { 0, 255, 0 })]
        [TestCase(new[] { 153, 51, 153 }, new[] { 153, 51, 153 })]
        [TestCase(new[] { 0, 0, 0 }, new[] { 0, 0, 0 })]
        [TestCase(new[] { 255, 255, 255 }, new[] { 255, 255, 255 })]
        [TestCase(new[] { 8, 8, 8 }, new[] { 8, 8, 8 })]
        [TestCase(new[] { 128, 128, 128 }, new[] { 128, 128, 128 })]
        [TestCase(new[] { 238, 238, 238 }, new[] { 238, 238, 238 })]
        [TestCase(new[] { 3000, 3000, 3000 }, new[] { 0, 0, 0 })]
        [TestCase(new[] { -1, -1, -1 }, new[] { 0, 0, 0 })]
        public void When_SGR_58_Attributes_Is_Setting_Custom_UnderLine_RGB_Color(int[] rgb, int[] expected)
        {
            Decode($"{Escape}58;2;{rgb[0]};{rgb[1]};{rgb[2]}m");
            Assert.That(GetGraphicsAttributes().UnderLineColorRGBColor,
                Is.EqualTo(new RgbColor(expected[0], expected[1], expected[2])));
        }

        private GraphicAttributes GetGraphicsAttributes(int columnNumber = 1)
        {
            Screen.AddCharacter('a');
            return Screen.GetCharacter(new Position(1, columnNumber)).GraphicAttributes;
        }
    }
}