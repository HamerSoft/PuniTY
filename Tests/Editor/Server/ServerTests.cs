using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HamerSoft.PuniTY.Tests.Editor
{
    [TestFixture]
    public class ServerTests : TestBase
    {
        private PunityServer _server;

        [SetUp]
        public void Setup()
        {
            _server = new PunityServer(new EditorLogger());
        }

        [Test]
        public void When_StartArguments_Are_Invalid_Server_Does_Not_Start()
        {
            Assert.Throws<ArgumentException>(() => { _server.Start(null); });
        }

        [Test]
        public async Task When_Server_Is_Started_Client_Can_Connect()
        {
            var clientId = Guid.NewGuid();
            Guid connectedId = default;
            var isConnected = false;

            void ClientConnected(Guid id, Stream s)
            {
                connectedId = id;
                isConnected = true;
            }

            _server.ClientConnected += ClientConnected;
            _server.Start(GetValidServerArguments());
            var client = new MockTCPClient(clientId);
            client.Start(GetValidClientArguments());

            await WaitUntil(() => isConnected);

            Assert.That(clientId, Is.EqualTo(connectedId));
            _server.ClientConnected -= ClientConnected;
            client.Stop();
        }

        [Test]
        public void Server_Logs_Error_When_Socket_Already_Taken()
        {
            var args = GetValidServerArguments();
            var tcpListener = new TcpListener(args.Ip, (int)args.Port);
            tcpListener.Start();
            _server.Start(args);
            LogAssert.Expect(LogType.Error, new Regex(""));
            tcpListener.Server.Close();
            tcpListener.Server.Dispose();
            tcpListener.Stop();
        }

        [Test]
        public async Task When_Server_Is_Connected_To_Client_IsConnected_Is_True()
        {
            var clientId = Guid.NewGuid();
            Guid connectedId = default;
            var isConnected = false;

            void ClientConnected(Guid id, Stream s)
            {
                connectedId = id;
                isConnected = true;
            }

            _server.ClientConnected += ClientConnected;
            _server.Start(GetValidServerArguments());
            var client = new MockTCPClient(clientId);
            client.Start(GetValidClientArguments());

            await WaitUntil(() => isConnected);

            Assert.That(_server.IsConnected, Is.True);
            _server.ClientConnected -= ClientConnected;
            client.Stop();
        }

        [Test]
        public void Server_Cannot_Be_Started_When_Already_Started()
        {
            _server.Start(GetValidServerArguments());
            _server.Start(GetValidServerArguments());
            LogAssert.Expect(LogType.Warning, new Regex(""));
        }

        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        public async Task Multiple_Clients_Can_Connect_To_Server(int numberOfClients)
        {
            var connectedClients = new HashSet<Guid>();
            var clientIds = new HashSet<Guid>();
            var clients = new List<IPunityClient>();

            void ClientConnected(Guid id, Stream s)
            {
                connectedClients.Add(id);
            }

            _server.ClientConnected += ClientConnected;
            _server.Start(GetValidServerArguments());

            for (int i = 0; i < numberOfClients; i++)
            {
                var client = new MockTCPClient(Guid.NewGuid());
                clientIds.Add(client.Id);
                clients.Add(client);
                client.Start(GetValidClientArguments());
            }

            await WaitUntil(() => connectedClients.Count == numberOfClients, 15000);
            Assert.IsTrue(connectedClients.SetEquals(clientIds));
            Assert.That(_server.ConnectedClients, Is.EqualTo(numberOfClients));
            clients.ForEach(c => _server.Stop(ref c));
            _server.Stop();
            await Task.Delay(2000);
        }

        [Test]
        public async Task When_Server_Is_Stopped_Event_Is_Raised()
        {
            _server.Start(GetValidServerArguments());
            var isStopped = false;

            void Stopped()
            {
                isStopped = true;
            }

            _server.Stopped += Stopped;
            _server.Stop();
            await WaitUntil(() => isStopped);
            Assert.IsTrue(isStopped);
        }

        [Test]
        public async Task When_Server_Is_Stopped_Client_Connection_Is_Closed()
        {
            Guid lostGuid = default;

            void ConnectionLost(Guid guid)
            {
                lostGuid = guid;
            }

            _server.Start(GetValidServerArguments());

            var client = new MockTCPClient(Guid.NewGuid());
            await client.StartAsync(GetValidClientArguments());

            _server.ConnectionLost += ConnectionLost;
            _server.Stop();
            await WaitUntil(() => lostGuid != default);
            Assert.That(lostGuid, Is.EqualTo(client.Id));
            client.Stop();
        }

        [TearDown]
        public void TearDown()
        {
            _server.Stop();
        }
    }
}