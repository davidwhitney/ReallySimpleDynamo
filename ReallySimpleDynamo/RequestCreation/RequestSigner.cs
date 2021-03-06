using System;
using System.Net;
using ReallySimpleDynamo.RequestCreation.SignatureVersion4;

namespace ReallySimpleDynamo.RequestCreation
{
    public class RequestSigner : ISignRequests
    {
        private readonly IHashingAlogirthm _hashingAlogirthm;

        public RequestSigner(IHashingAlogirthm hashingAlogirthm)
        {
            _hashingAlogirthm = hashingAlogirthm;
        }

        public void Sign(HttpWebRequest request, ClientConfiguration configuration, DateTime timestamp)
        {
            var signature = _hashingAlogirthm.ComputeSignature(configuration.AwsSecretAccessKey, configuration.AvailabilityZone, timestamp, request.Headers["X-Amz-Target"]);
            var hmacCredential = new AuthorizationCredential(configuration.AwsSecretAccessKey, CredentialType.DynamoDb, timestamp, configuration.AvailabilityZone);
            
            //var contentSha256 = "";
            //request.Headers.Add("X-Amz-Content-SHA256", contentSha256); - this might be optional, not mentioned in docs?

            request.Headers.Add("Authorization", AuthHeader(_hashingAlogirthm, hmacCredential, signature));
        }

        private static string AuthHeader(IHashingAlogirthm algo, AuthorizationCredential credential, string signature)
        {
            return
                String.Format(
                    "{0} Credential={1}, SignedHeaders=content-type;host;user-agent;x-amz-content-sha256;x-amz-date;x-amz-target, Signature={2}",
                    algo.Name, credential, signature);
        }
    }
}