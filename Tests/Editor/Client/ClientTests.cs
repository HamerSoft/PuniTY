using System;
using System.IO;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Core;
using HamerSoft.PuniTY.Core.Logging;
using NUnit.Framework;

namespace HamerSoft.PuniTY.Tests.Editor
{
    [TestFixture]
    public class ClientTests
    {
        private class MockClient : IPunityClient
        {
            public Guid Id { get; }
            public bool IsConnected { get; }
            public bool HasExited { get; }
            public event Action<string> ResponseReceived;
            public event Action Exited;

            public async Task Write(string text)
            {
                throw new NotImplementedException();
            }

            public async Task WriteLine(string text)
            {
                throw new NotImplementedException();
            }

            public async Task Write(byte[] bytes)
            {
                throw new NotImplementedException();
            }

            public void Start(ClientArguments startArguments)
            {
                throw new NotImplementedException();
            }

            public void Stop()
            {
                throw new NotImplementedException();
            }

            public void Connect(Stream stream)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void When_Client_Is_Started_With_Invalid_Arguments_Exception_Is_Thrown()
        {
            Assert.Throws<ArgumentException>(new PunityClient(System.Guid.NewGuid(),new EditorLogger()));
        }

        public async Task WhenClientConnected_CanWriteToStream()
        {
            Assert.False(true);
        }
    }
}