namespace HamerSoft.PuniTY.AnsiEncoding
{
    public interface ITabStop
    {
        void ResetTabStops();
        void ClearTabStop(int? column);
        int GetNextTabStop(int column);
        int GetPreviousTabStop(int column);
        int GetCurrentTabStop(int column);
        int TabStopToColumn(int tabStop);
    }
}