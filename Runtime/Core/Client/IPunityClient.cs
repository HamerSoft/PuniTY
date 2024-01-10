using System;

namespace HamerSoft.PuniTY.Core.Client
{
    internal interface IPunityClient
    {
        public event Action Exited;
        public bool HasExited { get; }
        public void Start(StartArguments args);
        public void Stop();
    }
}