using System;
using System.Collections.Specialized;

namespace ReallySimpleDynamo.CredentialDetection
{
    public class Credentials
    {
        private DateTime? _expiration;

        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string SessionToken { get; set; }

        public DateTime Expiration
        {
            get { return _expiration ?? default(DateTime); }
            set { _expiration = value; }
        }

        public bool AreConfigured()
        {
            return !string.IsNullOrWhiteSpace(AccessKey) && !string.IsNullOrWhiteSpace(SecretKey);
        }

        public Credentials()
        {
            SessionToken = string.Empty;
        }

        public Credentials(NameValueCollection appSettings)
        {
            AccessKey = appSettings["AWSAccessKey"];
            SecretKey = appSettings["AWSSecretKey"];
        }
    }
}