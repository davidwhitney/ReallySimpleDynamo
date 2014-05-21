using System.Collections.Specialized;
using NUnit.Framework;
using ReallySimpleDynamo.CredentialDetection.Providers;

namespace ReallySimpleDynamo.Test.Unit.CredentialDetection.Providers
{
    [TestFixture]
    public class AppConfigCredentialProviderTests
    {
        [Test]
        public void Retreive_HasSettingsInAppConfig_ReturnsSettings()
        {
            var provider = new AppConfigCredentialProvider(new NameValueCollection
            {
                {"AWSAccessKey", "access"},
                {"AWSSecretKey", "secret"}
            });

            var creds = provider.Retrieve();

            Assert.That(creds.AccessKey, Is.EqualTo("access"));
            Assert.That(creds.SecretKey, Is.EqualTo("secret"));
        }

        [Test]
        public void Retreive_NoSettingsInAppConfig_ReturnsNull()
        {
            var provider = new AppConfigCredentialProvider(new NameValueCollection());

            var creds = provider.Retrieve();

            Assert.That(creds, Is.Null);
        }
    }
}
