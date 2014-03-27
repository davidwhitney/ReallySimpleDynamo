using System;
using System.Net;
using ReallySimpleDynamo.RequestCreation.SignatureVersion4;

namespace ReallySimpleDynamo.RequestCreation
{
    public class RequestSigner : ISignRequests
    {
        // TODO: Implement hashing algos lifted from official SDK.
        public void Sign(HttpWebRequest request, ClientConfiguration configuration, string body, DateTime? timestamp = null)
        {
            var dateBase = timestamp.HasValue ? timestamp.Value : DateTime.Now;

            var hashAlgo = new Sha256();

            var contentSh256 = "";
            var signature = "";
            var credentialValue = "";

            var hmacCredential = new AuthorizationCredential(credentialValue, CredentialType.DynamoDb, dateBase, configuration.AvailabilityZone);
            
            request.Headers.Add("X-Amz-Content-SHA256", contentSh256);
            request.Headers.Add("Authorization", AuthHeader(hashAlgo, hmacCredential, signature));
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