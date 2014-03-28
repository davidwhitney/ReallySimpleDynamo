using System;
using Moq;
using NUnit.Framework;
using ReallySimpleDynamo.CredentialDetection;
using ReallySimpleDynamo.CredentialDetection.Providers;

namespace ReallySimpleDynamo.Test.Unit.CredentialDetection
{
    [TestFixture]
    public class CredentialFactoryTests
    {
        [Test]
        public void Ctor_NoProviderChainSupplied_CreatesDefault()
        {
            var factory = new CredentialFactory();
            
            Assert.That(factory.Providers[0], Is.TypeOf<AppConfigCredentialProvider>());
            Assert.That(factory.Providers[1], Is.TypeOf<InstanceMetadataCredentialProvider>());
        }

        [Test]
        public void Retrieve_ReturnsFromFirstProviderThatHandsBackANotNullSetting()
        {
            var returnsNull = new Mock<IProvideCredentials>();
            var returnsAnEntry = new Mock<IProvideCredentials>();
            returnsAnEntry.Setup(x => x.Retrieve()).Returns(new Credentials());
            var factory = new CredentialFactory(returnsNull.Object, returnsAnEntry.Object, returnsNull.Object);

            var creds = factory.Retrieve();

            Assert.That(creds, Is.Not.Null);
        }

        [Test]
        public void Retrieve_WhenNoProvidersReturnAnything_ThrowsException()
        {
            var returnsNull = new Mock<IProvideCredentials>();
            var factory = new CredentialFactory(returnsNull.Object);

            var ex = Assert.Throws<InvalidOperationException>(() => factory.Retrieve());

            Assert.That(ex.Message, Is.EqualTo("No credential providers returned AWS credentials"));
        }
    }
}
