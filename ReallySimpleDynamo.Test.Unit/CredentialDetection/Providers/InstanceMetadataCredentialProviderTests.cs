using System.Net;
using Moq;
using NUnit.Framework;
using ReallySimpleDynamo.CredentialDetection.Providers;
using ReallySimpleDynamo.Http;

namespace ReallySimpleDynamo.Test.Unit.CredentialDetection.Providers
{
    [TestFixture]
    public class InstanceMetadataCredentialProviderTests
    {
        private Mock<IHttpClient> _mockHttp;

        [SetUp]
        public void SetUp()
        {
            _mockHttp = new Mock<IHttpClient>();
        }

        [Test]
        public void Retrieve_NoCachedCredentials_FetchesCredentials()
        {
            var provider = new InstanceMetadataCredentialProvider(_mockHttp.Object);

            var creds = provider.Retrieve();

            Assert.That(creds, Is.Not.Null);
        }

        [Test]
        public void Retrieve_NoCachedCredentials_CredentialsMarshalledFromHttpResponse()
        {
            _mockHttp.Setup(x => x.Send(It.IsAny<HttpWebRequest>(), It.IsAny<string>())).Returns(new Response());
            var provider = new InstanceMetadataCredentialProvider(_mockHttp.Object);

            var creds = provider.Retrieve();

            Assert.That(creds, Is.Not.Null);
        }
    }
}
