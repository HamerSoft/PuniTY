using System;
using System.IO;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using HamerSoft.PuniTY.Logging;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor
{
    [TestFixture]
    public class ClientTests : TestBase
    {
        private class ConnectionForcedClient : PunityClient
        {
            public ConnectionForcedClient(Guid id, ILogger logger) : base(id, logger)
            {
            }

            public override void Start(ClientArguments startArguments)
            {
                base.Start(startArguments);
                HasExited = false;
            }
        }

        [Test]
        public void When_Client_Is_Started_With_Null_Arguments_Exception_Is_Thrown()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var client = new PunityClient(System.Guid.NewGuid(), new EditorLogger());
                client.Start(null);
            });
        }

        [Test]
        public void When_Client_Is_Started_With_Invalid_Arguments_Exception_Is_Thrown()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var client = new PunityClient(System.Guid.NewGuid(), new EditorLogger());
                client.Start(new ClientArguments("foo", 12, "invalid app", null, "invalid directory"));
            });
        }

        [Test]
        public void When_Client_Is_Started_Process_Is_Spawned()
        {
            var client = new PunityClient(Guid.NewGuid(), new EditorLogger());
            client.Start(GetValidClientArguments());
            Assert.That(client.HasExited, Is.False);
            client.Stop();
        }

        [Test]
        public async Task When_Client_Is_Connected_Can_Write_To_Stream()
        {
            var guid = Guid.NewGuid();
            var client = new PunityClient(guid, new EditorLogger());
            client.Start(GetValidClientArguments());
            var stream = new MemoryStream();
            client.Connect(stream);
            string response = null;

            void Responded(string message)
            {
                if (string.IsNullOrWhiteSpace(message))
                    return;
                response = message;
                client.Stop();
                client.ResponseReceived -= Responded;
            }

            client.ResponseReceived += Responded;
            await client.Write(guid.ToString());
            stream.Position = 0;
            var elapsedTime = 0;
            while (response == null && elapsedTime < 5000)
            {
                await Task.Delay(100);
                elapsedTime += 100;
            }

            client.Stop();
            client.ResponseReceived -= Responded;
            Assert.That(response, Is.Not.Null);
        }
    }
}