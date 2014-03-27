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

        /// <summary>
        /// The string representing Url Encoded Content in HTTP requests
        /// </summary>
        public const string UrlEncodedContent = "application/x-www-form-urlencoded; charset=utf-8";

        /// <summary>
        /// The GMT Date Format string. Used when parsing date objects
        /// </summary>
        public const string GMTDateFormat = "ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T";

        /// <summary>
        /// The ISO8601Date Format string. Used when parsing date objects
        /// </summary>
        public const string ISO8601DateFormat = "yyyy-MM-dd\\THH:mm:ss.fff\\Z";

        /// <summary>
        /// The ISO8601Date Format string. Used when parsing date objects
        /// </summary>
        public const string ISO8601DateFormatNoMS = "yyyy-MM-dd\\THH:mm:ss\\Z";

        /// <summary>
        /// The ISO8601 Basic date/time format string. Used when parsing date objects
        /// </summary>
        public const string ISO8601BasicDateTimeFormat = "yyyyMMddTHHmmssZ";

        /// <summary>
        /// The ISO8601 basic date format. Used during AWS4 signature computation.
        /// </summary>
        public const string ISO8601BasicDateFormat = "yyyyMMdd";

        /// <summary>
        /// The RFC822Date Format string. Used when parsing date objects
        /// </summary>
        public const string RFC822DateFormat = "ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T";
    }
}