using System;
using System.Collections.Generic;
using System.Linq;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class GraphicsRenditionSequence : Sequence
    {
        public override char Command => 'm';

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Logger.Warning("Invalid GraphicsRendition, no parameters given!");
                return;
            }

            var arguments = parameters.Split(';');
            var graphicsParameters = new List<GraphicRendition>();
            var customColor = new int?[4];
            var customColorIndex = 0;
            var isCustomColor = false;
            for (int i = 0; i < arguments.Length; i++)
            {
                if (!isCustomColor && int.TryParse(arguments[i], out var parsedInteger) &&
                    Enum.IsDefined(typeof(GraphicRendition), parsedInteger))
                {
                    graphicsParameters.Add((GraphicRendition)parsedInteger);
                    isCustomColor = parsedInteger is 38 or 48;
                }
                else if (isCustomColor && int.TryParse(arguments[i], out var colorInt))
                {
                    customColor[customColorIndex] = colorInt;
                    customColorIndex++;
                }
                else
                {
                    Logger.Error(
                        $"Failed to parse GraphicsRendition parameter {arguments[i]} at index {i} from parameters {parameters}. Skipping command...");
                    return;
                }
            }

            if (isCustomColor)
            {
                AnsiColor? parsedColor = null;
                if (customColor[0] == 2)
                    customColor = Parse24BitColor(customColor);
                else if (customColor[0] == 5)
                {
                    customColor = Parse8BitColor(customColor, out parsedColor);
                    if (parsedColor.HasValue)
                    {
                        GraphicRendition coloring = graphicsParameters.Last();
                        graphicsParameters.Remove(coloring);
                        var newColorRendition = AnsiColorToGraphicRendition(coloring, parsedColor.Value);
                        if (newColorRendition.HasValue)
                            graphicsParameters.Add(newColorRendition.Value);
                        else
                        {
                            parsedColor = null;
                        }
                    }
                }

                if (customColor == null && parsedColor == null)
                {
                    Logger.Error(
                        $"Failed to parse GraphicsRendition parameters {parameters}, invalid custom color {string.Join(';', customColor)}. Skipping command...");
                    return;
                }
            }

            screen.SetGraphicsRendition(customColor, graphicsParameters.ToArray());
        }

        private int?[] Parse8BitColor(int?[] customColor, out AnsiColor? defaultColor)
        {
            defaultColor = null;

            int?[] AnsiToRgb(int colorId, out AnsiColor? defaultAnsiColor)
            {
                defaultAnsiColor = null;
                if (colorId is >= 0 and <= 15)
                {
                    defaultAnsiColor = (AnsiColor)colorId;
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

        private int?[] Parse24BitColor(int?[] customColor)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < customColor.Length; i++)
                if (!customColor[i].HasValue)
                    return null;

            return new int?[]
            {
                Math.Clamp(customColor[1].Value, 0, 255),
                Math.Clamp(customColor[2].Value, 0, 255),
                Math.Clamp(customColor[3].Value, 0, 255)
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
                >= 8 and <= 15 => (GraphicRendition)(backgroundColorStartIndex + color + colorOffset),
                _ => null
            };
        }
    }
}