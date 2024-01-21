using System;
using System.Collections.Generic;
using System.IO;
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
        [Test]
        public void When_StartArguments_Are_Invalid_Server_Does_Not_Start()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var server = new PunityServer(new EditorLogger());
                server.Start(null);
            });
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

            var server = new PunityServer(new EditorLogger());
            server.ClientConnected += ClientConnected;
            server.Start(GetValidServerArguments());
            var client = new MockTCPClient(clientId);
            client.Start(GetValidClientArguments());

            await WaitUntil(() => isConnected);

            Assert.That(connectedId, Is.EqualTo(clientId));
            server.ClientConnected -= ClientConnected;

            server.Stop();
            client.Stop();
            await Task.Delay(500);
        }

        [Test]
        public async Task Server_Cannot_Be_Started_When_Already_Started()
        {
            var server = new PunityServer(new EditorLogger());
            server.Start(GetValidServerArguments());
            server.Start(GetValidServerArguments());
            LogAssert.Expect(LogType.Warning, new Regex(""));
            server.Stop();
            await Task.Delay(500);
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

            var server = new PunityServer(new EditorLogger());
            server.ClientConnected += ClientConnected;
            server.Start(GetValidServerArguments());

            for (int i = 0; i < numberOfClients; i++)
            {
                var client = new MockTCPClient(Guid.NewGuid());
                clientIds.Add(client.Id);
                clients.Add(client);
                client.Start(GetValidClientArguments());
            }

            await WaitUntil(() => connectedClients.Count == numberOfClients);
            Assert.IsTrue(connectedClients.SetEquals(clientIds));
            server.Stop();
            clients.ForEach(c => c.Stop());
            await Task.Delay(500);
        }

        [Test]
        public async Task When_Server_Is_Stopped_Event_Is_Raised()
        {
            var server = new PunityServer(new EditorLogger());
            server.Start(GetValidServerArguments());
            var isStopped = false;

            void Stopped()
            {
                isStopped = true;
            }

            server.Stopped += Stopped;
            server.Stop();
            await WaitUntil(() => isStopped);
            Assert.IsTrue(isStopped);
            await Task.Delay(500);
        }

        [Test]
        public async Task When_Server_Is_Stopped_Client_Connection_Is_Closed()
        {
            Guid lostGuid = default;

            void ConnectionLost(Guid guid)
            {
                lostGuid = guid;
            }

            var server = new PunityServer(new EditorLogger());
            server.Start(GetValidServerArguments());

            var client = new MockTCPClient(Guid.NewGuid());
            await client.StartAsync(GetValidClientArguments());

            server.ConnectionLost += ConnectionLost;
            server.Stop();
            await WaitUntil(() => lostGuid != default);
            Assert.That(lostGuid, Is.EqualTo(client.Id));
            client.Stop();
            await Task.Delay(500);
        }
    }
}