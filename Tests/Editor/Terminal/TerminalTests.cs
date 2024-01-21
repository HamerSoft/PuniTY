using System;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor
{
    [TestFixture]
    public class TerminalTests : TestBase
    {
        private MockServer _server;
        private MockClient _client;

        [SetUp]
        public void SetUp()
        {
            _server = new MockServer(new EditorLogger());
            _client = new MockClient(Guid.NewGuid(), _server);
        }

        [Test]
        public async Task When_Terminal_Is_Started_It_Receives_Responses()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), null);
            string receivedMessage = null;
            const string messageToBeSend = "HamerSoft";

            void ResponseReceived(string message)
            {
                receivedMessage = message;
            }

            terminal.ResponseReceived += ResponseReceived;
            _client.ForceResponse(messageToBeSend);

            await WaitUntil(() => receivedMessage != null);
            terminal.ResponseReceived -= ResponseReceived;
            Assert.That(receivedMessage, Is.EqualTo(messageToBeSend));
            _server.Stop();
        }

        [Test]
        public async Task When_Terminal_Is_StartedAsync_It_Receives_Responses()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            await terminal.StartAsync(GetValidClientArguments(), null);
            string receivedMessage = null;
            const string messageToBeSend = "HamerSoft";

            void ResponseReceived(string message)
            {
                receivedMessage = message;
            }

            terminal.ResponseReceived += ResponseReceived;
            _client.ForceResponse(messageToBeSend);

            await WaitUntil(() => receivedMessage != null);
            terminal.ResponseReceived -= ResponseReceived;
            Assert.That(receivedMessage, Is.EqualTo(messageToBeSend));
            _server.Stop();
        }

        [Test]
        public async Task When_Server_Stops_Terminal_Is_Stopped()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), null);
            var isStopped = false;

            void Stopped()
            {
                isStopped = true;
            }

            terminal.Stopped += Stopped;
            _server.Stop();
            await WaitUntil(() => isStopped);
            terminal.Stopped -= Stopped;
            Assert.IsTrue(isStopped);
        }

        [Test]
        public async Task When_Client_Exits_Terminal_Is_Stopped()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), null);
            var isStopped = false;

            void Stopped()
            {
                isStopped = true;
            }

            terminal.Stopped += Stopped;
            _client.Stop();
            await WaitUntil(() => isStopped);
            terminal.Stopped -= Stopped;
            Assert.IsTrue(isStopped);
            _server.Stop();
        }

        [Test]
        public async Task When_Server_ConnectionLost_Terminal_IsStopped()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), null);
            var lostConnection = false;

            void LostConnection()
            {
                lostConnection = true;
            }

            terminal.Stopped += LostConnection;
            _server.ForceLoseConnection(_client);
            await WaitUntil(() => lostConnection);
            terminal.Stopped -= LostConnection;
            Assert.IsTrue(lostConnection);
            _server.Stop();
        }

        [Test]
        public void When_Server_Is_Stopped_Terminal_Is_No_Longer_Running()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), null);
            Assert.IsTrue(terminal.IsRunning);
            _server.Stop();
            Assert.IsFalse(terminal.IsRunning);
        }

        [Test]
        public async Task When_Writing_To_Terminal_Client_Receives_Message()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), null);
            await terminal.Write("foo");
            Assert.That(_client.WrittenText, Is.EqualTo("foo"));
        }

        [Test]
        public async Task When_WritingLine_To_Terminal_Client_Receives_Message()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), null);
            await terminal.WriteLine("foo-bar");
            Assert.That(_client.WrittenText, Is.EqualTo("foo-bar"));
        }

        [Test]
        public async Task When_WritingBytes_To_Terminal_Client_Receives_Message()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), null);
            await terminal.Write(System.Text.Encoding.ASCII.GetBytes("foo-bar"));
            Assert.That(_client.WrittenText, Is.EqualTo("foo-bar"));
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Stop();
            _client = null;
            _server?.Stop();
            _server = null;
        }
    }
}