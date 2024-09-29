using System;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal static class GraphicsRenditionParser
    {
        internal static GraphicAttributes Parse(GraphicAttributes currentGraphicAttributes,
            GraphicsPair[] graphicRenditionPairs,
            ILogger logger)
        {
            foreach (GraphicsPair graphicRendition in graphicRenditionPairs)
            {
                switch (graphicRendition.GraphicRendition)
                {
                    case GraphicRendition.Reset:
                        currentGraphicAttributes.Reset();
                        break;
                    case GraphicRendition.Bold:
                        currentGraphicAttributes.IsBold = true;
                        break;
                    case GraphicRendition.Faint:
                        currentGraphicAttributes.IsFaint = true;
                        break;
                    case GraphicRendition.Italic:
                        currentGraphicAttributes.IsItalic = true;
                        break;
                    case GraphicRendition.Framed:
                        currentGraphicAttributes.IsFramed = true;
                        break;
                    case GraphicRendition.OverLined:
                        currentGraphicAttributes.IsOverLined = true;
                        break;
                    case GraphicRendition.NoOverLined:
                        currentGraphicAttributes.IsOverLined = false;
                        break;
                    case GraphicRendition.Encircled:
                        currentGraphicAttributes.IsEncircled = true;
                        break;
                    case GraphicRendition.NotFramedOrEncircled:
                        currentGraphicAttributes.IsFramed = false;
                        currentGraphicAttributes.IsEncircled = false;
                        break;
                    case GraphicRendition.StrikeThrough:
                        currentGraphicAttributes.IsStrikeThrough = true;
                        break;
                    case GraphicRendition.Underline:
                        currentGraphicAttributes.UnderlineMode = UnderlineMode.Single;
                        break;
                    case GraphicRendition.BlinkSlow:
                        currentGraphicAttributes.BlinkSpeed = BlinkSpeed.Slow;
                        break;
                    case GraphicRendition.BlinkRapid:
                        currentGraphicAttributes.BlinkSpeed = BlinkSpeed.Rapid;
                        break;
                    case GraphicRendition.Positive:
                    case GraphicRendition.Inverse:
                        (currentGraphicAttributes.Foreground, currentGraphicAttributes.Background) = (
                            currentGraphicAttributes.Background, currentGraphicAttributes.Foreground);
                        (currentGraphicAttributes.ForegroundRGBColor, currentGraphicAttributes.BackgroundRGBColor) = (
                            currentGraphicAttributes.BackgroundRGBColor, currentGraphicAttributes.ForegroundRGBColor);
                        currentGraphicAttributes.UnderLineColorRGBColor = currentGraphicAttributes.ForegroundRGBColor;
                        break;
                    case GraphicRendition.Conceal:
                        currentGraphicAttributes.IsConcealed = true;
                        break;
                    case GraphicRendition.UnderlineDouble:
                        currentGraphicAttributes.UnderlineMode = UnderlineMode.Double;
                        break;
                    case GraphicRendition.NormalIntensity:
                        currentGraphicAttributes.IsBold = false;
                        currentGraphicAttributes.IsFaint = false;
                        break;
                    case GraphicRendition.NoItalic:
                        currentGraphicAttributes.IsItalic = false;
                        break;
                    case GraphicRendition.NoUnderline:
                        currentGraphicAttributes.UnderlineMode = UnderlineMode.None;
                        break;
                    case GraphicRendition.NoBlink:
                        currentGraphicAttributes.BlinkSpeed = BlinkSpeed.None;
                        break;
                    case GraphicRendition.Reveal:
                        currentGraphicAttributes.IsConcealed = false;
                        break;
                    case GraphicRendition.NoStrikeThrough:
                        currentGraphicAttributes.IsStrikeThrough = false;
                        break;
                    case GraphicRendition.ProportionalSpacing:
                        currentGraphicAttributes.IsProportionalSpaced = true;
                        break;
                    case GraphicRendition.NoProportionalSpacing:
                        currentGraphicAttributes.IsProportionalSpaced = false;
                        break;
                    case GraphicRendition.ForegroundNormalBlack:
                        currentGraphicAttributes.Foreground = AnsiColor.Black;
                        break;
                    case GraphicRendition.ForegroundNormalRed:
                        currentGraphicAttributes.Foreground = AnsiColor.Red;
                        break;
                    case GraphicRendition.ForegroundNormalGreen:
                        currentGraphicAttributes.Foreground = AnsiColor.Green;
                        break;
                    case GraphicRendition.ForegroundNormalYellow:
                        currentGraphicAttributes.Foreground = AnsiColor.Yellow;
                        break;
                    case GraphicRendition.ForegroundNormalBlue:
                        currentGraphicAttributes.Foreground = AnsiColor.Blue;
                        break;
                    case GraphicRendition.ForegroundNormalMagenta:
                        currentGraphicAttributes.Foreground = AnsiColor.Magenta;
                        break;
                    case GraphicRendition.ForegroundNormalCyan:
                        currentGraphicAttributes.Foreground = AnsiColor.Cyan;
                        break;
                    case GraphicRendition.ForegroundNormalWhite:
                        currentGraphicAttributes.Foreground = AnsiColor.White;
                        break;
                    case GraphicRendition.ForegroundNormalReset:
                        currentGraphicAttributes.Foreground = AnsiColor.White;
                        break;
                    case GraphicRendition.BackgroundNormalBlack:
                        currentGraphicAttributes.Background = AnsiColor.Black;
                        break;
                    case GraphicRendition.BackgroundNormalRed:
                        currentGraphicAttributes.Background = AnsiColor.Red;
                        break;
                    case GraphicRendition.BackgroundNormalGreen:
                        currentGraphicAttributes.Background = AnsiColor.Green;
                        break;
                    case GraphicRendition.BackgroundNormalYellow:
                        currentGraphicAttributes.Background = AnsiColor.Yellow;
                        break;
                    case GraphicRendition.BackgroundNormalBlue:
                        currentGraphicAttributes.Background = AnsiColor.Blue;
                        break;
                    case GraphicRendition.BackgroundNormalMagenta:
                        currentGraphicAttributes.Background = AnsiColor.Magenta;
                        break;
                    case GraphicRendition.BackgroundNormalCyan:
                        currentGraphicAttributes.Background = AnsiColor.Cyan;
                        break;
                    case GraphicRendition.BackgroundNormalWhite:
                        currentGraphicAttributes.Background = AnsiColor.White;
                        break;
                    case GraphicRendition.BackgroundNormalReset:
                        currentGraphicAttributes.Background = AnsiColor.Black;
                        break;
                    case GraphicRendition.ForegroundBrightBlack:
                        currentGraphicAttributes.Foreground = AnsiColor.BrightBlack;
                        break;
                    case GraphicRendition.ForegroundBrightRed:
                        currentGraphicAttributes.Foreground = AnsiColor.BrightRed;
                        break;
                    case GraphicRendition.ForegroundBrightGreen:
                        currentGraphicAttributes.Foreground = AnsiColor.BrightGreen;
                        break;
                    case GraphicRendition.ForegroundBrightYellow:
                        currentGraphicAttributes.Foreground = AnsiColor.BrightYellow;
                        break;
                    case GraphicRendition.ForegroundBrightBlue:
                        currentGraphicAttributes.Foreground = AnsiColor.BrightBlue;
                        break;
                    case GraphicRendition.ForegroundBrightMagenta:
                        currentGraphicAttributes.Foreground = AnsiColor.BrightMagenta;
                        break;
                    case GraphicRendition.ForegroundBrightCyan:
                        currentGraphicAttributes.Foreground = AnsiColor.BrightCyan;
                        break;
                    case GraphicRendition.ForegroundBrightWhite:
                        currentGraphicAttributes.Foreground = AnsiColor.BrightWhite;
                        break;
                    case GraphicRendition.ForegroundBrightReset:
                        currentGraphicAttributes.Foreground = AnsiColor.White;
                        break;
                    case GraphicRendition.BackgroundBrightBlack:
                        currentGraphicAttributes.Background = AnsiColor.BrightBlack;
                        break;
                    case GraphicRendition.BackgroundBrightRed:
                        currentGraphicAttributes.Background = AnsiColor.BrightRed;
                        break;
                    case GraphicRendition.BackgroundBrightGreen:
                        currentGraphicAttributes.Background = AnsiColor.BrightGreen;
                        break;
                    case GraphicRendition.BackgroundBrightYellow:
                        currentGraphicAttributes.Background = AnsiColor.BrightYellow;
                        break;
                    case GraphicRendition.BackgroundBrightBlue:
                        currentGraphicAttributes.Background = AnsiColor.BrightBlue;
                        break;
                    case GraphicRendition.BackgroundBrightMagenta:
                        currentGraphicAttributes.Background = AnsiColor.BrightMagenta;
                        break;
                    case GraphicRendition.BackgroundBrightCyan:
                        currentGraphicAttributes.Background = AnsiColor.BrightCyan;
                        break;
                    case GraphicRendition.BackgroundBrightWhite:
                        currentGraphicAttributes.Background = AnsiColor.BrightWhite;
                        break;
                    case GraphicRendition.BackgroundBrightReset:
                        currentGraphicAttributes.Background = AnsiColor.Black;
                        break;
                    case GraphicRendition.ForegroundColor:
                        currentGraphicAttributes.Foreground = AnsiColor.Rgb;
                        var hasCustomUnderline = currentGraphicAttributes.ForegroundRGBColor !=
                                                 currentGraphicAttributes.UnderLineColorRGBColor;
                        currentGraphicAttributes.ForegroundRGBColor = new RgbColor(graphicRendition.Color[0].Value,
                            graphicRendition.Color[1].Value, graphicRendition.Color[2].Value);
                        if (!hasCustomUnderline)
                            currentGraphicAttributes.UnderLineColorRGBColor =
                                currentGraphicAttributes.ForegroundRGBColor;
                        break;
                    case GraphicRendition.BackgroundColor:
                        currentGraphicAttributes.Background = AnsiColor.Rgb;
                        currentGraphicAttributes.BackgroundRGBColor = new RgbColor(graphicRendition.Color[0].Value,
                            graphicRendition.Color[1].Value, graphicRendition.Color[2].Value);
                        break;
                    case GraphicRendition.UnderLineColor:
                        currentGraphicAttributes.UnderLineColorRGBColor = new RgbColor(graphicRendition.Color[0].Value,
                            graphicRendition.Color[1].Value, graphicRendition.Color[2].Value);
                        break;
                    case GraphicRendition.ResetUnderLineColor:
                        currentGraphicAttributes.UnderLineColorRGBColor = currentGraphicAttributes.ForegroundRGBColor;
                        break;
                    case GraphicRendition.SuperScript:
                        currentGraphicAttributes.ScriptMode = ScriptMode.SuperScript;
                        break;
                    case GraphicRendition.Subscript:
                        currentGraphicAttributes.ScriptMode = ScriptMode.SubScript;
                        break;
                    case GraphicRendition.NoSuperOrSubScript:
                        currentGraphicAttributes.ScriptMode = ScriptMode.None;
                        break;
                    case GraphicRendition.AlternativeFont1:
                    case GraphicRendition.AlternativeFont2:
                    case GraphicRendition.AlternativeFont3:
                    case GraphicRendition.AlternativeFont4:
                    case GraphicRendition.AlternativeFont5:
                    case GraphicRendition.AlternativeFont6:
                    case GraphicRendition.AlternativeFont7:
                    case GraphicRendition.AlternativeFont8:
                    case GraphicRendition.AlternativeFont9:
                    case GraphicRendition.Fraktur:
                    case GraphicRendition.Font1:
                        logger.LogWarning($"Trying to set GraphicsRendition Font {graphicRendition} now what!?");
                        break;
                    case GraphicRendition.RightSideLine:
                    case GraphicRendition.DoubleRightSideLine:
                    case GraphicRendition.LeftSideLine:
                    case GraphicRendition.DoubleLeftSideLine:
                    case GraphicRendition.IdeogramStressMarking:
                    case GraphicRendition.NoIdeogram:
                        logger.LogWarning($"SGR command {(int)graphicRendition.GraphicRendition} not supported.");
                        break;
                    default:
                        throw new Exception("Unknown rendition command");
                }
            }

            return currentGraphicAttributes;
        }
    }
}