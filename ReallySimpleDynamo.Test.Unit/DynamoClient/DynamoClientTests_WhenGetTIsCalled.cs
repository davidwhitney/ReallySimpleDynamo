using System;
using System.Net;
using Moq;
using NUnit.Framework;
using ReallySimpleDynamo.Http;
using ReallySimpleDynamo.Model;
using System.Linq;

namespace ReallySimpleDynamo.Test.Unit.DynamoClient
{
    [TestFixture]
    public class DynamoClientTests_WhenGetTIsCalled
    {
        private ReallySimpleDynamo.DynamoClient _client;
        private Mock<IHttpClient> _mockHttp;
        private HttpWebRequest _constructedRequest;
        private Key _key;

        [SetUp]
        public void SetUp()
        {
            _mockHttp = new Mock<IHttpClient>();
            _mockHttp.Setup(x => x.Send(It.IsAny<HttpWebRequest>(), It.IsAny<string>()))
                .Returns(new Response())
                .Callback(new Action<HttpWebRequest, string>((req, body) => _constructedRequest = req));

            _key = new Key
            {
                {"ColumnName", new AttributeValue {S = "some-s-value"}}
            };

            _client = new ReallySimpleDynamo.DynamoClient(new ClientConfiguration{AvailabilityZone = "testzone"}, _mockHttp.Object);
        }
        
        [Test]
        public void TableNameNotSupplied_ThrowsArgNull()
        {
            Assert.Throws<ArgumentNullException>(()=>_client.Get<SomeDto>("", _key));
        }

        [Test]
        public void KeyNotSupplied_ThrowsArgNull()
        {
            Assert.Throws<ArgumentNullException>(()=>_client.Get<SomeDto>("tableName", null));
        }

        [Test]
        public void CreateRequestTemplate_ContainsAmazonTargetHeader()
        {
            _client.Get<SomeDto>("tableName", _key);

            Assert.That(_constructedRequest.Headers.AllKeys.Contains("X-Amz-Target"));
            Assert.That(_constructedRequest.Headers["X-Amz-Target"], Is.EqualTo("DynamoDB_20120810.GetItem"));
        }

        public class SomeDto {}
    }
}