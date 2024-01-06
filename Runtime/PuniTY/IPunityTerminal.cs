using System;

namespace HamerSoft.PuniTY
{
    public interface IPunityTerminal : IDisposable
    {
        public void Start(string workingDirectory, string app);
        public void Stop();
    }
}