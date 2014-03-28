using System;

namespace ReallySimpleDynamo.CredentialDetection
{
    public class Credentials
    {
        private DateTime? _expiration;

        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string SessionToken { get; set; }

        public DateTime Expiration
        {
            get { return _expiration ?? default(DateTime); }
            set { _expiration = value; }
        }
    }
}