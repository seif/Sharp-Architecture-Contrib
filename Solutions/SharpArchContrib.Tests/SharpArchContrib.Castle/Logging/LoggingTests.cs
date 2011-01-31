namespace Tests.SharpArchContrib.Castle.Logging
{
    using System;
    using System.IO;
    using System.Threading;

    using global::Castle.DynamicProxy;

    using global::SharpArchContrib.Castle;

    using Microsoft.Practices.ServiceLocation;

    using NUnit.Framework;

    using SharpArch.Testing.NUnit;

    using global::SharpArchContrib.Castle.Logging;
    using global::SharpArchContrib.Core.Logging;

    [TestFixture]
    public class LoggingTests
    {
        string logPath;

        [SetUp]
        public void Setup()
        {
            this.logPath = Path.GetFullPath(@"TestData/Tests.SharpArchContrib.Castle.Logging.DebugLevelTests.DebugLevel.log");
        }

        [Test]
        public void LoggingDebugEntryWorks()
        {
            this.TryLogging();
            File.Exists(this.logPath).ShouldBeTrue();
            string log = GetLog();
            log.IndexOf("testClass.Method logged").ShouldBeGreaterThan(0);
            log.IndexOf("testClass.VirtualMethod Logged").ShouldBeGreaterThan(0);
            log.IndexOf("testClass.NotLogged should not log").ShouldEqual(-1);
        }

        [Test]
        public void LoggingViaProxyWorks()
        {
            this.TryLoggingViaProxy();
            File.Exists(this.logPath).ShouldBeTrue();
            var log = this.GetLog();
            log.IndexOf("testLogger2.GetMessage not logged").ShouldEqual(-1); // non virtual can't be intercepted.
            log.IndexOf("testLogger2.GetMessageVirtual logged").ShouldBeGreaterThan(0);
            log.IndexOf("testLogger2.GetMessage not logged").ShouldEqual(-1);
        }

        private void TryLogging()
        {
            var testClass = ServiceLocator.Current.GetInstance<ILogTestClass>();
            testClass.Method("testClass.Method logged", 1);
            testClass.VirtualMethod("testClass.VirtualMethod Logged", 2);
            testClass.NotLogged("testClass.NotLogged should not log", 3);
            Assert.Throws<Exception>(() => testClass.ThrowException());
        }

        private void TryLoggingViaProxy()
        {
            var generator = new ProxyGenerator();
            var testLogger2 =
                generator.CreateClassProxy<TestLogger2>(
                    new ProxyGenerationOptions(),
                    ServiceLocator.Current.GetInstance<LogInterceptor>());
            testLogger2.GetMessage("testLogger2.GetMessage not logged");
            testLogger2.GetMessageVirtual("testLogger2.GetMessageVirtual logged");
            testLogger2.GetMessageNotLogged("testLogger2.GetMessage not logged");
        }


        private string GetLog()
        {
            File.Copy(this.logPath, this.logPath + "1");
            var log = File.ReadAllText(this.logPath + "1");
            File.Delete(this.logPath + 1);
            return log;
        }

        public class TestLogger2
        {
            [Log]
            public string GetMessage(string message)
            {
                return message;
            }

            public virtual string GetMessageNotLogged(string message)
            {
                return message;
            }

            [Log(EntryLevel = LoggingLevel.Info)]
            public virtual string GetMessageVirtual(string message)
            {
                return message;
            }
        }
    }
}