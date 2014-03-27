using System;
using NUnit.Framework;
using ReallySimpleDynamo.RequestCreation.SignatureVersion4;

namespace ReallySimpleDynamo.Test.Unit.RequestCreation.SignatureVersion4
{
    [TestFixture]
    public class AuthorizationCredentialTests
    {
        [Test]
        public void Ctor_WithParams_ToStringGeneratesAppropriateCredHeader()
        {
            var creds = new AuthorizationCredential("awsCreds", CredentialType.DynamoDb, new DateTime(2005, 01, 02, 03, 05, 06, 00), "testaz");

            Assert.That(creds.ToString(), Is.EqualTo("awsCreds/20050102/testaz/dynamodb/aws4_request"));
        }
    }
}
