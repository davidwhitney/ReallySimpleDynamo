using System;
using System.Net;

namespace ReallySimpleDynamo
{
    public class RequestTemplater : ICreateRequestTemplates
    {
        // TODO: Implement hashing algos lifted from official SDK.
        public HttpWebRequest CreateRequestTemplate(ClientConfiguration configuration)
        {
            var contentSh256 = "";
            var dateBase = DateTime.Now;
            var timestamp = "";
            var signature = "";
            var credentialPrefix = "";
            var shortDate = dateBase.ToString("yyyy-MM-dd");
            var hmacCredential = string.Format("{0}/{1}/{2}/dynamodb/aws4_request", credentialPrefix, shortDate, configuration.AvailabilityZone);

            var authHeader =
                String.Format(
                    "AWS4-HMAC-SHA256 Credential={0}, SignedHeaders=content-type;host;user-agent;x-amz-content-sha256;x-amz-date;x-amz-target, Signature={1}",
                    hmacCredential, signature);

            var req = WebRequest.CreateHttp(configuration.DatabaseUri);
            req.UserAgent = "aws-sdk-dotnet-45/2.0.12.0 .NET Runtime/4.0 .NET Framework/4.0 OS/6.3.9600.0";
            req.ContentType = "application/x-amz-json-1.0";
            req.Accept = "application/json";
            req.Method = "POST";

            req.Headers.Add("X-Amz-Date", timestamp);
            req.Headers.Add("X-Amz-Content-SHA256", contentSh256);
            req.Headers.Add("Authorization", authHeader);

            return req;
        }
    }
}