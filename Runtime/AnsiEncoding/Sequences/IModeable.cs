using System;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public interface IMode
    {
        /// <summary>
        /// Called when enabling the mode
        /// </summary>
        public void Enable(IScreen screen);

        /// <summary>
        /// Called when disabling the mode
        /// </summary>
        public void Disable(IScreen screen);

        /// <summary>
        /// Apply Mode if needs to be done repeatedly
        /// </summary>
        public void Apply(IScreen screen);
    }

    public interface IModeable
    {
        /// <summary>
        /// Event raised when mode changed
        /// <remarks>Data is on format [mode, enabled]</remarks> 
        /// </summary>
        public event Action<AnsiMode, bool> ModeChanged;

        /// <summary>
        /// Set a mode
        /// </summary>
        /// <param name="mode">The mode that needs to be set</param>
        public void SetMode(AnsiMode mode);

        /// <summary>
        /// Reset a mode
        /// </summary>
        /// <param name="mode">The mode that needs to be reset</param>
        public void ResetMode(AnsiMode mode);

        /// <summary>
        /// Check whether a mode is active
        /// </summary>
        /// <param name="mode">The mode to check</param>
        /// <returns>true when the mode is active</returns>
        public bool HasMode(params AnsiMode[] mode);
    }
}