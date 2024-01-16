using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Encoding;
using NUnit.Framework;
using UnityEngine;

namespace HamerSoft.PuniTY.Tests.Editor
{
    [TestFixture]
    public class ArgumentsTests : TestBase
    {
        [Test]
        public void StartArguments_Are_Invalid_With_Invalid_IP()
        {
            var arguments = new StartArguments("some invalid ip", 2);
            Assert.That(arguments.IsValid(out _), Is.False);
            Assert.That(arguments.Ip, Is.Null);
        }

        [Test]
        public void StartArguments_Are_valid_With_Valid_IP()
        {
            var arguments = new StartArguments("127.0.0.1", 2);
            Assert.That(arguments.IsValid(out _), Is.True);
            Assert.That(arguments.Ip, Is.Not.Null);
        }

        [Test]
        public void ClientArguments_Are_Invalid_With_Invalid_App()
        {
            var clientArguments = new ClientArguments("127.0.0.1", 1337, "invalid app name", new AnsiEncoder(),
                Application.dataPath);

            Assert.That(clientArguments.IsValid(out _), Is.False);
        }
        
        [Test]
        public void ClientArguments_Are_Invalid_When_Encodier_IsNull()
        {
            var clientArguments = new ClientArguments("127.0.0.1", 1337, GetValidAppName(), null,
                Application.dataPath);

            Assert.That(clientArguments.IsValid(out _), Is.False);
        }

        [Test]
        public void ClientArguments_Are_Invalid_With_Invalid_WorkingDirectory()
        {
            var clientArguments = new ClientArguments("127.0.0.1", 1337, GetValidAppName(), new AnsiEncoder(),
                "invalid directory");

            Assert.That(clientArguments.IsValid(out _), Is.False);
        }

     
    }
}