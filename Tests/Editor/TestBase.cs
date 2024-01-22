using System;
using System.IO;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Encoding;
using UnityEngine;

namespace HamerSoft.PuniTY.Tests.Editor
{
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

        protected ClientArguments GetValidClientArguments(StartArguments startArguments)
        {
            return new ClientArguments(startArguments, GetValidAppName(), new AnsiEncoder(),
                Environment.CurrentDirectory);
        }

        protected StartArguments GetValidServerArguments(string ip = "127.0.0.1", uint port = 13000)
        {
            return new StartArguments(ip, port);
        }

        protected async Task WaitUntil(Func<bool> predicate, double timeout = 1000)
        {
            var elapsedTime = 0;
            while (!predicate.Invoke() && elapsedTime < timeout)
            {
                await Task.Delay(100);
                elapsedTime += 100;
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