using System;
using System.Linq;
using System.Net.Http;
using Moq;
using NUnit.Framework;

namespace ReallySimpleDynamo.Test.Unit.DynamoClient
{
    [TestFixture]
    public class DynamoClientTests_WhenGetTIsCalled
    {
        private ReallySimpleDynamo.DynamoClient _client;
        private Mock<IHttpClient> _mockHttp;
        private HttpRequestMessage _httpResponse;
        private Key _key;

        [SetUp]
        public void SetUp()
        {
            _mockHttp = new Mock<IHttpClient>();
            _mockHttp.Setup(x => x.Send(It.IsAny<HttpRequestMessage>()))
                .Returns(new HttpResponseMessage())
                .Callback(new Action<HttpRequestMessage>(req => _httpResponse = req));

            _key = new Key
            {
                {"ColumnName", new AttributeValue {S = "some-s-value"}}
            };

            _client = new ReallySimpleDynamo.DynamoClient(new ClientConfiguration(), _mockHttp.Object);
        }
        
        [Test]
        public void TableNameNotSupplied_ThrowsArgNull()
        {
            Assert.Throws<ArgumentNullException>(()=>_client.Get<MyDto>("", new Key()));
        }

        [Test]
        public void KeyNotSupplied_ThrowsArgNull()
        {
            Assert.Throws<ArgumentNullException>(()=>_client.Get<MyDto>("tableName", null));
        }

        [Test]
        public void ValidRequest_RequestIsMadeWithCorrect_ContainsAmazonTargetHeader()
        {
            _client.Get<MyDto>("tableName", _key);

            Assert.That(_httpResponse.Headers.Contains("X-Amz-Target"));
            Assert.That(_httpResponse.Headers.GetValues("X-Amz-Target").First(), Is.EqualTo("DynamoDB_20120810.GetItem"));
        }
    }
}