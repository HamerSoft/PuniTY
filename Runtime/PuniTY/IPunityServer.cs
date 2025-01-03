﻿using System;
using System.IO;
using HamerSoft.PuniTY.Configuration;

namespace HamerSoft.PuniTY
{
    internal interface IPunityServer
    {
        public event Action<Guid> ConnectionLost;
        public event Action<Guid, Stream> ClientConnected;
        public event Action Stopped;
        public bool IsConnected { get; }
        public int ConnectedClients { get; }

        public void Start(StartArguments startArguments);
        public void Stop(ref IPunityClient client);
        public void Stop();
    }
}