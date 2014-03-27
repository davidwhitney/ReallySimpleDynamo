using System;

namespace ReallySimpleDynamo.RequestCreation.SignatureVersion4
{
    public class AuthorizationCredential
    {
        private readonly string _credentialValue;
        private readonly CredentialType _credType;
        private readonly DateTime _date;
        private readonly string _availabilityZone;

        private const string AwsSignatureVersion4Terminator = "aws4_request";

        public AuthorizationCredential(string credentialValue, CredentialType credType, DateTime date, string availabilityZone)
        {
            _credentialValue = credentialValue;
            _credType = credType;
            _date = date;
            _availabilityZone = availabilityZone;
        }

        public override string ToString()
        {
            var credentialSegments = new []
            {
                _credentialValue, 
                _date.ToString("yyyyMMdd"), 
                _availabilityZone.ToLower(), 
                _credType.ToString().ToLower(), 
                AwsSignatureVersion4Terminator
            };

            return string.Join("/", credentialSegments);
        }
    }
}