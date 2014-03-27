using System;
using System.Net;
using NUnit.Framework;
using System.Linq;

namespace ReallySimpleDynamo.Test.Unit
{
    public class RequestTemplaterTests
    {
        private RequestTemplater _client;
        private ClientConfiguration _cfg;
        private HttpWebRequest _template;

        [SetUp]
        public void SetUp()
        {
            _cfg = new ClientConfiguration {AvailabilityZone = "testzone"};
            _client = new RequestTemplater();
            _template = _client.CreateRequestTemplate(_cfg);
        }

        [Test]
        public void CreateRequestTemplate_UriCorrect()
        {
            Assert.That(_template.RequestUri, Is.EqualTo(new Uri("https://dynamodb.testzone.amazonaws.com/")));
        }
        
        [Test]
        public void CreateRequestTemplate_ContainsAmazonJsonContentType()
        {
            Assert.That(_template.ContentType, Is.EqualTo("application/x-amz-json-1.0"));
        }

        [Test]
        public void CreateRequestTemplate_ContainsCorrectAcceptTypeForJson()
        {
            Assert.That(_template.Accept, Is.EqualTo("application/json"));
        }

        [Test]
        public void CreateRequestTemplate_HttpMethodIsPost()
        {
            Assert.That(_template.Method, Is.EqualTo("POST"));
        }

        [Test]
        [Description("This probably needs reviewing, because we could do with a distinct user agent rather than lying.")]
        public void CreateRequestTemplate_ContainsAmazonSdkUA()
        {
            Assert.That(_template.UserAgent, Is.EqualTo("aws-sdk-dotnet-45/2.0.12.0 .NET Runtime/4.0 .NET Framework/4.0 OS/6.3.9600.0"));
        }
    }
}
