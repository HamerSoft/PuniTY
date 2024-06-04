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

        private GraphicAttributes GetGraphicsAttributes()
        {
            Screen.AddCharacter('a');
            return Screen.GetCharacter(new Position(1, 1)).GraphicAttributes;
        }
    }
}