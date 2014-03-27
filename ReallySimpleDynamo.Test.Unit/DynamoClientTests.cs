using System;
using NUnit.Framework;

namespace ReallySimpleDynamo.Test.Unit
{
    [TestFixture]
    public class DynamoClientTests
    {
        private DynamoClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new DynamoClient(new ClientConfiguration());
        }

        [Test]
        public void Ctor_NoConfigurationSupplied_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new DynamoClient(null));
        }

        [Test]
        public void Ctor_WithConfiguration_ConstructsAndStoresConfiguration()
        {
            var client = new DynamoClient(new ClientConfiguration());

            Assert.That(client.ClientConfiguration, Is.Not.Null);
        }

        [Test]
        public void GetT_TableNameNotSupplied_ThrowsArgNull()
        {
            Assert.Throws<ArgumentNullException>(()=>_client.Get<MyDto>("", "key"));
        }

        [Test]
        public void GetT_KeyNotSupplied_ThrowsArgNull()
        {
            Assert.Throws<ArgumentNullException>(()=>_client.Get<MyDto>("tableName", ""));
        }
    }

    public class MyDto
    {
    }
}
