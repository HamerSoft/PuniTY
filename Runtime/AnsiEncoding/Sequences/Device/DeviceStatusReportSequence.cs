namespace HamerSoft.PuniTY.AnsiEncoding.Device
{
    public class DeviceStatusReportSequence : TransmitSequence
    {
        /// <summary>
        /// This command has a hardcoded 6 as parameter, other integers are invalid.
        /// </summary>
        private const int Six = 6;

        public override char Command => 'n';

        public override void Execute(IScreen screen, string parameters)
        {
            if (int.TryParse(parameters, out var six) && six == Six)
            {
                screen.Transmit(ToBytes($"{Escape}{screen.Cursor.Position.Row};{screen.Cursor.Position.Column};R"));
            }
            else
            {
                Logger.Warning(
                    $"Failed to get DeviceStatusReport, parameter: {parameters} is not an integer, or not equal to 6");
            }
        }
    }
}