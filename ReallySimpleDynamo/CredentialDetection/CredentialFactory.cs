using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleDynamo.CredentialDetection.Providers;
using ReallySimpleDynamo.Http;

namespace ReallySimpleDynamo.CredentialDetection
{
    public class CredentialFactory : IProvideCredentials
    {
        public IList<IProvideCredentials> Providers { get; private set; }

        public CredentialFactory(params IProvideCredentials[] providers)
        {
            Providers = providers == null || !providers.Any() ? DefaultCredentialLookupChain() : providers;
        }

        private static IProvideCredentials[] DefaultCredentialLookupChain()
        {
            return new IProvideCredentials[]
            {
                new AppConfigCredentialProvider(),
                new InstanceMetadataCredentialProvider(new HttpClientWrapper())
            };
        }

        public Credentials Retrieve()
        {
            foreach (var creds in Providers.Select(provider => provider.Retrieve()).Where(creds => creds != null))
            {
                return creds;
            }

            throw new InvalidOperationException("No credential providers returned AWS credentials");

            // throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Access Key or Secret Key could not be found.  Add an appsetting to your App.config with the name {0} with a value of your access key.", ACCESSKEY));
            // throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Secret Key could not be found.  Add an appsetting to your App.config with the name {0} with a value of your secret key.", SECRETKEY));
        }
    }
}