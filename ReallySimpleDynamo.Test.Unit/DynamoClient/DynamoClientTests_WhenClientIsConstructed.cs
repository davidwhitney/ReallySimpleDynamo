using System;
using Moq;
using NUnit.Framework;

namespace ReallySimpleDynamo.Test.Unit.DynamoClient
{
    [TestFixture]
    public class DynamoClientTests_WhenClientIsConstructed
    {
        private Mock<IHttpClient> _mockHttp;

        [SetUp]
        public void SetUp()
        {
            _mockHttp = new Mock<IHttpClient>();
        }

        [Test]
        public void Ctor_NoConfigurationSupplied_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ReallySimpleDynamo.DynamoClient(null, _mockHttp.Object));
        }

        [Test]
        public void Ctor_NoHttpChannelSupplied_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ReallySimpleDynamo.DynamoClient(new ClientConfiguration(), null));
        }

        [Test]
        public void Ctor_WithValidParams_ConstructsAndStoresParams()
        {
            var client = new ReallySimpleDynamo.DynamoClient(new ClientConfiguration(), _mockHttp.Object);

            Assert.That(client.ClientConfiguration, Is.Not.Null);
            Assert.That(client.HttpClient, Is.Not.Null);
        }
    }
}