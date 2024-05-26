using System;

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
            var graphicsParameters = new GraphicRendition[arguments.Length];
            for (int i = 0; i < arguments.Length; i++)
            {
                if (int.TryParse(arguments[i], out var parsedInteger) &&
                    Enum.IsDefined(typeof(GraphicRendition), parsedInteger))
                    graphicsParameters[i] = (GraphicRendition)parsedInteger;
                else
                {
                    Logger.Error(
                        $"Failed to parse GraphicsRendition parameter {arguments[i]} at index {i} from parameters {parameters}. Skipping command...");
                    return;
                }
            }

            screen.SetGraphicsRendition(graphicsParameters);
        }
    }
}