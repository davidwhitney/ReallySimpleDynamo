using System;

namespace ReallySimpleDynamo
{
    public class ClientConfiguration
    {
        public string AvailabilityZone { get; set; }
        public string AwsSecretAccessKey { get; set; }

        public Uri DatabaseUri
        {
            get { return new Uri("https://dynamodb." + AvailabilityZone + ".amazonaws.com/"); }
        }

        public ClientConfiguration()
        {
            AwsSecretAccessKey = "";
            AvailabilityZone = "eu-west-1";
        }
    }
}