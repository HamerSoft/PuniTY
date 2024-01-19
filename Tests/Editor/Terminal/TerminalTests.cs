using System;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor
{
    [TestFixture]
    public class TerminalTests : TestBase
    {
        private MockServer _server;
        private MockClient _tcpClient;

        [SetUp]
        public void SetUp()
        {
            _server = new MockServer(new EditorLogger());
            _tcpClient = new MockClient(Guid.NewGuid(), _server);
        }

        [Test]
        public void When_Terminal_Is_Started_It_Subscribes_To_Server()
        {
            var terminal = new PunityTerminal(_server, _tcpClient, new EditorLogger());
            terminal.Start(GetValidClientArguments(), null);
            _server.Stop();
            Assert.IsFalse(terminal.IsRunning);
        }

        [TearDown]
        public void TearDown()
        {
            _tcpClient?.Stop();
            _tcpClient = null;
            _server?.Stop();
            _server = null;
        }
    }
}