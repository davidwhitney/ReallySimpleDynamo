using System;
using System.Net;
using ReallySimpleDynamo.Http;

namespace ReallySimpleDynamo.CredentialDetection.Providers
{
    public class InstanceMetadataCredentialProvider : IProvideCredentials
    {
        private readonly IHttpClient _client;

        public InstanceMetadataCredentialProvider(IHttpClient client)
        {
            _client = client;
        }

        public Credentials Retrieve()
        {
            var req = WebRequest.CreateHttp("http://169.254.169.254/latest/meta-data/iam/security-credentials/s3access");
            var response = _client.Send(req);

            return new Credentials();
        }
    }


    public class AmazonClientException : Exception
    {
        public AmazonClientException(string message) 
            : base(message)
        {
        }
    }

    public class AmazonServiceException : Exception
    {
        public AmazonServiceException(string message)
            : base(message)
        {
        }
    }

    public class CredentialsRefreshState
    {
        public Credentials Credentials { get; set; }
        public DateTime Expiration { get; set; }
    }
}