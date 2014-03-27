using System;
using System.Net;
using NUnit.Framework;
using ReallySimpleDynamo.RequestCreation;
using ReallySimpleDynamo.RequestCreation.SignatureVersion4;

namespace ReallySimpleDynamo.Test.Unit.RequestCreation
{
    public class SignedRequestBuilderTests
    {
        private SignedRequestBuilder _builder;
        private ClientConfiguration _cfg;
        private HttpWebRequest _request;

        [SetUp]
        public void SetUp()
        {
            _cfg = new ClientConfiguration {AvailabilityZone = "testzone"};
            _builder = new SignedRequestBuilder(new RequestSigner(new Sha256()));
            _request = _builder.CreateRequest(_cfg, "MethodNameCalled", new DateTime(2013, 03, 15, 09, 20, 54));
        }

        [Test]
        public void CreateRequestTemplate_UriCorrect()
        {
            Assert.That(_request.RequestUri, Is.EqualTo(new Uri("https://dynamodb.testzone.amazonaws.com/")));
        }
        
        [Test]
        public void CreateRequestTemplate_ContainsAmazonJsonContentType()
        {
            Assert.That(_request.ContentType, Is.EqualTo("application/x-amz-json-1.0"));
        }

        [Test]
        public void CreateRequestTemplate_ContainsCorrectAcceptTypeForJson()
        {
            Assert.That(_request.Accept, Is.EqualTo("application/json"));
        }

        [Test]
        public void CreateRequestTemplate_HttpMethodIsPost()
        {
            Assert.That(_request.Method, Is.EqualTo("POST"));
        }

        [Test]
        public void CreateRequestTemplate_AmazonMethodNameHeaderSet()
        {
            Assert.That(_request.Headers["X-Amz-Target"], Is.EqualTo("MethodNameCalled"));
        }

        [Test]
        public void CreateRequestTemplate_AmazonTimeHeadeSet()
        {
            Assert.That(_request.Headers["X-Amz-Date"], Is.EqualTo("20130315T092054Z"));
        }

        [Test]
        [Description("This probably needs reviewing, because we could do with a distinct user agent rather than lying.")]
        public void CreateRequestTemplate_ContainsAmazonSdkUA()
        {
            Assert.That(_request.UserAgent, Is.EqualTo("aws-sdk-dotnet-45/2.0.12.0 .NET Runtime/4.0 .NET Framework/4.0 OS/6.3.9600.0"));
        }
    }
}
