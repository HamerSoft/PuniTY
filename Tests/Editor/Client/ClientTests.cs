using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace HamerSoft.PuniTY.Tests.Editor
{
    [TestFixture]
    public class ClientTests : TestBase
    {
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
        public void Client_Cannot_Connect_When_Not_Started()
        {
            var client = new PunityClient(Guid.NewGuid(), new EditorLogger());
            var stream = new MemoryStream();
            client.Connect(stream);
            LogAssert.Expect(LogType.Error, new Regex(""));
            CloseStream(stream);
        }

        [Test]
        public void Client_Cannot_Connect_Already_Exited()
        {
            var client = new PunityClient(Guid.NewGuid(), new EditorLogger());
            var stream = new MemoryStream();
            client.Start(GetValidClientArguments());
            client.Stop();
            client.Connect(stream);
            LogAssert.Expect(LogType.Error, new Regex(""));
            CloseStream(stream);
        }

        [Test]
        public void Client_Cannot_Connect_Already_Connected()
        {
            var client = new PunityClient(Guid.NewGuid(), new EditorLogger());
            var stream = new MemoryStream();
            client.Start(GetValidClientArguments());
            client.Connect(stream);
            client.Connect(stream);
            LogAssert.Expect(LogType.Warning, new Regex(""));
            CloseStream(stream);
        }

        [Test]
        public void Client_Cannot_Connect_When_Stream_Is_Null()
        {
            var client = new PunityClient(Guid.NewGuid(), new EditorLogger());
            client.Start(GetValidClientArguments());
            client.Connect(null);
            LogAssert.Expect(LogType.Warning, new Regex(""));
        }

        [Test]
        public void Client_Raises_Event_Upon_Exit()
        {
            var client = new PunityClient(Guid.NewGuid(), new EditorLogger());
            bool hasExit = false;

            void Exitted()
            {
                hasExit = true;
                client.Exited -= Exitted;
            }

            client.Exited += Exitted;
            client.Stop();
            Assert.That(hasExit && client.HasExited, Is.True);
        }

        [Test]
        public async Task When_Client_Is_Connected_Can_Write_To_And_Receive_From_Stream()
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

            await WaitUntil(() => response != null);


            client.Stop();
            client.ResponseReceived -= Responded;
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.EqualTo(guid.ToString()));
        }
    }
}