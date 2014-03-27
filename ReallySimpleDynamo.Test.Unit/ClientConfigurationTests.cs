using System;
using NUnit.Framework;

namespace ReallySimpleDynamo.Test.Unit
{
    [TestFixture]
    public class ClientConfigurationTests
    {
        [Test]
        public void Az_DefaultsToEuWest1()
        {
            var cfg = new ClientConfiguration();

            Assert.That(cfg.AvailabilityZone, Is.EqualTo("eu-west-1"));
        }

        [TestCase("eu-west-1")]
        [TestCase("something-else")]
        public void DatabaseUri_DerivedFromAvailabilityZone(string az)
        {
            var cfg = new ClientConfiguration {AvailabilityZone = az};

            Assert.That(cfg.DatabaseUri, Is.EqualTo(new Uri("https://dynamodb." + az + ".amazonaws.com/")));
        }
    }
}
