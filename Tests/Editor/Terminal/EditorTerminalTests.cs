using System;
using System.Numerics;
using System.Threading.Tasks;
using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.Stubs;
using NUnit.Framework;
using Tests.Editor.AnsiDecoding.Stubs;

namespace HamerSoft.PuniTY.Tests.Editor
{
    [TestFixture]
    public class EditorTerminalTests : TestBase
    {
        private MockServer _server;
        private MockClient _client;
        private AnsiContext _ansiContext;

        [SetUp]
        public void SetUp()
        {
            _server = new MockServer(new EditorLogger());
            _client = new MockClient(Guid.NewGuid(), _server);
            _ansiContext = new AnsiContext(
                new StubInput(new StubPointer(new AlwaysHide(), new Vector2(0, 0), new Rect(0, 0, 100, 100)),
                    new StubKeyboard()), new Screen.DefaultScreenConfiguration(10, 10, 2, new FontDimensions(10, 10)),
                new EditorLogger());
        }

        [Test]
        public async Task When_Terminal_Is_Started_It_Receives_Responses()
        {
            var terminal = PunityFactory.CreateTerminal(_server, _client, _ansiContext);
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
            await terminal.WriteAsync(System.Text.Encoding.ASCII.GetBytes("foo-bar"));
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