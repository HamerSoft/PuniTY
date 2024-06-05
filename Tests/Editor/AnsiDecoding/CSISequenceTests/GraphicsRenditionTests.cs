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
        public void When_SGR_ForeGroundColor_Attributes_Are_Setting_AnsiColours(GraphicRendition graphicRendition,
            AnsiColor color)
        {
            Decode($"{Escape}{(int)graphicRendition}m");
            Assert.That(GetGraphicsAttributes(),
                Is.EqualTo(new GraphicAttributes(color, AnsiColor.Black)));
        }

        [TestCase(196)] // red
        public void When_SGR_38_Attributes_Is_Setting_CustomColor(int colorId)
        {
            Decode($"{Escape}38;5;{colorId}m");
            Assert.That(GetGraphicsAttributes().Foreground, Is.EqualTo(AnsiColor.Rgb));
            Assert.That(GetGraphicsAttributes().ForegroundRGBColor, Is.EqualTo(new RgbColor(255, 0, 0)));
        }

        private GraphicAttributes GetGraphicsAttributes(int columnNumber = 1)
        {
            Screen.AddCharacter('a');
            return Screen.GetCharacter(new Position(1, columnNumber)).GraphicAttributes;
        }
    }
}