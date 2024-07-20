#nullable enable
using System;
using System.Collections.Generic;
using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class GraphicsRenditionSequence : Sequence
    {
        private const int ForegroundTextColor = 38;
        private const int BackgroundTextColor = 48;
        private const int UnderlineColor = 58;
        private const int AnsiColorTableMarker = 5;
        private const int RgbColorMarker = 2;

        public override char Command => 'm';

        public GraphicsRenditionSequence(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Logger.LogWarning("Invalid GraphicsRendition, no parameters given!");
                return;
            }

            string errorMessage = null;
            var arguments = parameters.Split(';') ?? Array.Empty<string>();
            var graphicsRenditions = new List<GraphicsPair>();
            for (int i = 0; i < arguments.Length; i++)
            {
                if (int.TryParse(arguments[i], out var parsedInteger) &&
                    Enum.IsDefined(typeof(GraphicRendition), parsedInteger))
                {
                    if (parsedInteger is ForegroundTextColor or BackgroundTextColor or UnderlineColor)
                    {
                        if (int.TryParse(arguments[++i], out var parsedColorMarker) &&
                            parsedColorMarker is AnsiColorTableMarker)
                        {
                            if (int.TryParse(arguments[++i], out var ansiColorTable))
                            {
                                var rgb = Parse8BitColor(new int?[] { 5, ansiColorTable }, out var ansiColor);
                                if (ansiColor.HasValue)
                                {
                                    var newColorRendition = AnsiColorToGraphicRendition((GraphicRendition)parsedInteger,
                                        ansiColor.Value);
                                    if (newColorRendition.HasValue)
                                        graphicsRenditions.Add(new GraphicsPair
                                            { GraphicRendition = newColorRendition.Value });
                                }
                                else
                                {
                                    graphicsRenditions.Add(new GraphicsPair
                                    {
                                        GraphicRendition = (GraphicRendition)parsedInteger,
                                        Color = rgb
                                    });
                                }
                            }
                            else
                            {
                                errorMessage =
                                    $"Failed to parse GraphicsRendition parameter {arguments[i]} at index {i} from parameters {parameters}. Invalid Color, skipping command...";
                            }
                        }
                        else if (parsedColorMarker is RgbColorMarker)
                        {
                            if (int.TryParse(arguments[++i], out var r) &&
                                int.TryParse(arguments[++i], out var g) &&
                                int.TryParse(arguments[++i], out var b))
                            {
                                graphicsRenditions.Add(new GraphicsPair
                                {
                                    GraphicRendition = (GraphicRendition)parsedInteger, Color = Parse24BitColor(r, g, b)
                                });
                            }
                            else
                            {
                                errorMessage =
                                    $"Failed to parse GraphicsRendition parameter {arguments[i]} at index {i} from parameters {parameters}. Invalid Color, skipping command...";
                            }
                        }
                        else // setting color, yet the marker is not set -> 2/5
                        {
                            errorMessage =
                                $"Failed to parse GraphicsRendition parameter {arguments[i]} at index {i} from parameters {parameters}. Invalid Color, skipping command...";
                        }
                    }
                    else
                    {
                        graphicsRenditions.Add(
                            new GraphicsPair { GraphicRendition = (GraphicRendition)parsedInteger });
                    }
                }
                else
                {
                    Logger?.LogError(
                        $"Failed to parse GraphicsRendition parameter {arguments[i]} at index {i} from parameters {parameters}. Skipping command...");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                Logger?.LogError(errorMessage);
                return;
            }

            screen.SetGraphicsRendition(graphicsRenditions.ToArray());
        }

        private int?[] Parse8BitColor(int?[] customColor, out AnsiColor? defaultColor)
        {
            defaultColor = null;

            int?[] AnsiToRgb(int colorId, out AnsiColor? defaultAnsiColor)
            {
                defaultAnsiColor = null;

                if (colorId > 255)
                {
                    defaultAnsiColor = AnsiColor.Black;
                    return null;
                }

                if (colorId is >= 0 and <= 15)
                {
                    defaultAnsiColor = (AnsiColor)colorId;
                    return null;
                }

                // handle greyscale
                if (colorId >= 232)
                {
                    var c = (colorId - 232) * 10 + 8;
                    return new int?[] { c, c, c };
                }

                colorId -= 16;

                int rem;
                var r = (int)(Math.Floor(colorId / 36d) / 5d * 255d);
                var g = (int)(Math.Floor((rem = colorId % 36) / 6d) / 5d * 255d);
                var b = (int)(rem % 6d / 5d * 255d);

                return new int?[] { r, g, b };
            }


            if (!customColor[1].HasValue)
                return null;
            var colorId = customColor[1].Value;
            return AnsiToRgb(colorId, out defaultColor);
        }

        private int?[] Parse24BitColor(int r, int g, int b)
        {
            return new int?[]
            {
                Math.Clamp(r, 0, 255),
                Math.Clamp(g, 0, 255),
                Math.Clamp(b, 0, 255)
            };
        }

        private GraphicRendition? AnsiColorToGraphicRendition(GraphicRendition oldRendition, AnsiColor color)
        {
            // See the GraphicsRendition enum numbers for details.
            // Indexes are used to map ansi colors to default graphics rendition commands.
            var colorOffset = oldRendition == GraphicRendition.ForegroundColor ? 0 : 10;
            const int foregroundColorStartIndex = 30;
            const int backgroundColorStartIndex = 82;

            return (int)color switch
            {
                >= 0 and <= 7 => (GraphicRendition)(foregroundColorStartIndex + color + colorOffset),
                // ReSharper disable once PatternIsRedundant
                >= 8 and <= 15 => (GraphicRendition)(backgroundColorStartIndex + color + colorOffset),
                _ => null
            };
        }
    }
}