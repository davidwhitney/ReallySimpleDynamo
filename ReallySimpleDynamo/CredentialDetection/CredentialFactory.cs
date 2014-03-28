using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleDynamo.CredentialDetection.Providers;

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
                new InstanceMetadataCredentialProvider()
            };
        }

        public Credentials Retrieve()
        {
            foreach (var creds in Providers.Select(provider => provider.Retrieve()).Where(creds => creds != null))
            {
                return creds;
            }

            throw new InvalidOperationException("No credential providers returned AWS credentials");
        }
    }
}