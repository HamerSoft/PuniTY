using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace HamerSoft.PuniTY
{
    public class PunityTcpServer : MonoBehaviour
    {
        private Process _myProcess;
        private Server _server;

        private void Awake()
        {
            _server = new Server("127.0.0.1", 13000);
            _server.Start();
            StartClient("127.0.0.1", 13000);
        }

        private class Server
        {
            private readonly string _ip;
            private readonly int _port;
            TcpListener server = null;
            private Thread _listeningThread;
            private NetworkStream _ioStream;
            private readonly UTF8Encoding _encoding;
            private readonly Regex _ansiRegex;
            private TcpClient _client;

            public Server(string ip, int port)
            {
                _ip = ip;
                _port = port;
                _encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
                _ansiRegex = new Regex(
                    @"[\u001B\u009B][[\]()#;?]*(?:(?:(?:[a-zA-Z\d]*(?:;[a-zA-Z\d]*)*)?\u0007)|(?:(?:\d{1,4}(?:;\d{0,4})*)?[\dA-PRZcf-ntqry=><~]))");
            }

            public void Start()
            {
                try
                {
                    IPAddress localAddr = IPAddress.Parse(_ip);

                    // TcpListener server = new TcpListener(port);
                    server = new TcpListener(localAddr, _port);

                    // Start listening for client requests.
                    server.Start();
                    _listeningThread = new Thread(WaitForClient);
                    _listeningThread?.Start();
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
            }

            private void WaitForClient()
            {
                Debug.Log("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                _client = server.AcceptTcpClient();
                Debug.Log("Connected!");
                // Get a stream object for reading and writing
                _ioStream = _client.GetStream();
                StartReading();
            }

            private void StartReading()
            {
                const int readSize = 4096;
                byte[] buffer = new byte[readSize];
                AsyncCallback callback = null;
                callback = ar =>
                {
                    // Call EndRead.
                    int bytesRead = _ioStream.EndRead(ar);

                    if (bytesRead < 0)
                        return;
                    // Process the bytes here.
                    var output = _encoding.GetString(buffer, 0, bytesRead);
                    // result = result.Replace("\r", string.Empty).Replace("\n", string.Empty);
                    output = _ansiRegex.Replace(output, string.Empty);


                    Debug.Log(output);

                    Array.Clear(buffer, 0, buffer.Length);
                    // Read again.  This callback will be called again.
                    _ioStream.BeginRead(buffer, 0, readSize, callback, this);
                };

                // Trigger the initial read.
                _ioStream.BeginRead(buffer, 0, readSize, callback, this);
            }

            public void Stop()
            {
                _listeningThread?.Abort();
                _listeningThread = null;
                server?.Stop();
                server = null;
                _ioStream?.Close();
                _ioStream?.Dispose();
                _ioStream = null;
            }

            public async Task Write(string message)
            {
                var bytes = GetBytes(message);
                await _ioStream.WriteAsync(bytes, 0, bytes.Length);
            }

            private static byte[] GetBytes(string message)
            {
                return System.Text.Encoding.ASCII.GetBytes(message);
            }
        }

        void StartClient(string server, int port)
        {
            _myProcess = new Process();

            _myProcess.StartInfo.UseShellExecute = false;
            _myProcess.StartInfo.Verb = "runas";
            // You can start any process, HelloWorld is a do-nothing example.
            var root = Application.dataPath.Replace("Assets", "");

            // _myProcess.StartInfo.FileName =
            //     Path.Combine(root, "Packages", "com.hamersoft.punity", "Plugins",
            //         $"PunityTCPClient{(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor ? ".exe" : "")}");
            _myProcess.StartInfo.CreateNoWindow = true;
            _myProcess.StartInfo.FileName = Path.Combine(Application.streamingAssetsPath,
                $"PunityTCPClient{(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor ? ".exe" : "")}");
            // _myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            _myProcess.StartInfo.Arguments = $"{server} {port}";
            _myProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            _myProcess.Start();
            // This code assumes the process you are starting will terminate itself.
            // Given that it is started without a window so you cannot terminate it
            // on the desktop, it must terminate itself or you can do it programmatically
            // from this application using the Kill method.
        }

        private void OnDestroy()
        {
            _server?.Stop();
            _myProcess?.Kill();
        }

        public async void Write(string message)
        {
            if (gameObject.activeInHierarchy)
                await _server.Write(message);
        }
    }
}