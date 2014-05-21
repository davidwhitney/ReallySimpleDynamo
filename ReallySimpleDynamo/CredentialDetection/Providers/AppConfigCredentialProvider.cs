using System.Collections.Specialized;
using System.Configuration;

namespace ReallySimpleDynamo.CredentialDetection.Providers
{
    public class AppConfigCredentialProvider : IProvideCredentials
    {
        private readonly NameValueCollection _appSettings;

        public AppConfigCredentialProvider(): this(ConfigurationManager.AppSettings)
        {
        }

        public AppConfigCredentialProvider(NameValueCollection appSettings)
        {
            _appSettings = appSettings;
        }

        public Credentials Retrieve()
        {
            var creds = new Credentials(_appSettings);
            return creds.AreConfigured() ? creds : null;
        }
    }
}