using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Encoding;
using UnityEngine;

namespace HamerSoft.PuniTY.Tests.Editor
{
    internal class MockClient : IPunityClient
    {
        private Stream _stream;
        private ClientArguments _startArguments;
        private bool _exitted;
        private TcpClient _tcpClient;
        private NetworkStream _ioStream;

        public Guid Id { get; }
        public bool IsConnected => _stream != null;
        public bool HasExited => _exitted;
        public event Action<string> ResponseReceived;
        public event Action Exited;

        public MockClient(Guid id)
        {
            Id = id;
        }

        public async Task Write(string text)
        {
            var bytes = System.Text.Encoding.ASCII.GetBytes(text);
            await _stream.WriteAsync(bytes, 0, bytes.Length);
        }

        public Task WriteLine(string text)
        {
            throw new NotImplementedException();
        }

        public Task Write(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public void Start(ClientArguments startArguments)
        {
            _startArguments = startArguments;
            _tcpClient = new TcpClient(startArguments.Ip.ToString(), (int)startArguments.Port);
            _ioStream = _tcpClient.GetStream();
            var idBytes = System.Text.Encoding.ASCII.GetBytes(Id.ToString());
            _ioStream.Write(idBytes, 0, idBytes.Length);
        }

        public void Stop()
        {
            _exitted = true;
            CloseStream(_stream);
            CloseStream(_ioStream);
            _stream = null;
            _ioStream = null;
        }

        private void CloseStream(Stream stream)
        {
            stream?.Dispose();
            stream?.Close();
        }

        public void Connect(Stream stream)
        {
            _stream = stream;
        }
    }

    public class TestBase
    {
        protected string GetValidAppName()
        {
            return Application.platform == RuntimePlatform.WindowsEditor
                ? @"C:\Program Files\Git\bin\sh.exe"
                : "/bin/bash/";
        }

        protected ClientArguments GetValidClientArguments(string ip = "127.0.0.1", uint port = 13000)
        {
            return new ClientArguments(ip, port, GetValidAppName(), new AnsiEncoder(), Environment.CurrentDirectory);
        }

        protected async Task WaitUntil(Func<bool> predicate, double timeout = 1000)
        {
            var elapsedTime = 0;
            while (!predicate.Invoke() && elapsedTime < timeout)
            {
                await Task.Delay(100);
            }
        }

        protected static void CloseStream(Stream stream)
        {
            stream.Close();
            stream.Dispose();
            stream = null;
        }
    }
}