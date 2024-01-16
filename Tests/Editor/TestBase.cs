using System;
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
    }
}