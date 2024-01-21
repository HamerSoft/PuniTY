using System;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor
{
    [TestFixture]
    public class UiTests : TestBase
    {
        private MockServer _server;
        private MockClient _client;
        private MockUi _ui;

        [SetUp]
        public void SetUp()
        {
            _server = new MockServer(new EditorLogger());
            _client = new MockClient(Guid.NewGuid(), _server);
            _ui = new MockUi();
        }

        [Test]
        public void When_Receiving_On_Client_UI_Receives_Message()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());

            terminal.Start(GetValidClientArguments(), _ui);
            _client.ForceResponse("foo");
            Assert.That(_ui.WrittenText, Is.EqualTo("foo"));
        }

        [Test]
        public void When_Writing_To_UI_Makes_Terminal_Forwards_To_Client()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), _ui);
            _ui.Write("foo");
            Assert.That(_client.WrittenText, Is.EqualTo("foo"));
        }

        [Test]
        public void When_WritingLine_To_UI_Makes_Terminal_Forwards_To_Client()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), _ui);
            _ui.WriteLine("foo-bar");
            Assert.That(_client.WrittenText, Is.EqualTo($"{Environment.NewLine}foo-bar"));
        }

        [Test]
        public void When_WritingBytes_To_UI_Makes_Terminal_Forwards_To_Client()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), _ui);
            _ui.Write(System.Text.Encoding.ASCII.GetBytes("foo-bar"));
            Assert.That(_client.WrittenText, Is.EqualTo("foo-bar"));
        }

        [Test]
        public void When_UI_Closed_Closes_Terminal()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), _ui);
            _ui.Close();
            Assert.That(_ui.IsClosed, Is.True);
            Assert.That(terminal.IsRunning, Is.False);
        }

        [Test]
        public void When_UI_Closed_Stops_Client()
        {
            var terminal = new PunityTerminal(_server, _client, new EditorLogger());
            terminal.Start(GetValidClientArguments(), _ui);
            _ui.Close();
            Assert.That(_ui.IsClosed, Is.True);
            Assert.That(_client.HasExited, Is.True);
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