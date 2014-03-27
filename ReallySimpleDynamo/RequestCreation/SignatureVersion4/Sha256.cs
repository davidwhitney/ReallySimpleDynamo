using System;
using System.Security.Cryptography;
using System.Text;

namespace ReallySimpleDynamo.RequestCreation.SignatureVersion4
{
    public class Sha256 : IHashingAlogirthm
    {
        public string Name { get { return "AWS4-HMAC-SHA256"; } }

        public string ComputeSignature(string awsSecretAccessKey, string region, DateTime signedAt, string service)
        {
            var kSecret = awsSecretAccessKey;
            var kDate = Hmac("AWS4" + kSecret, signedAt.ToString("yyyyMMdd"));
            var kRegion = Hmac(kDate, region);
            var kService = Hmac(kRegion, service);
            return Hmac(kService, "aws4_request");
        }

        private static string Hmac(string data, string key)
        {
            var binaryData = Encoding.UTF8.GetBytes(data);

            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException("key", "Please specify a Secret Signing Key.");
            if (binaryData == null || binaryData.Length == 0) throw new ArgumentNullException("data", "Please specify data to sign.");

            var algorithm = KeyedHashAlgorithm.Create("HMACSHA256");

            try
            {
                algorithm.Key = Encoding.UTF8.GetBytes(key);
                var bytes = algorithm.ComputeHash(binaryData);
                return Convert.ToBase64String(bytes);
            }
            finally
            {
                algorithm.Clear();
            }
        }
    }
}