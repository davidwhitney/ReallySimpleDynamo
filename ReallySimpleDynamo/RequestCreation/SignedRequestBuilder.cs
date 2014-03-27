using System;
using System.Net;

namespace ReallySimpleDynamo.RequestCreation
{
    public class SignedRequestBuilder : ICreateSignedRequests
    {
        public const string Iso8601BasicDateTimeFormat = "yyyyMMddTHHmmssZ";
        
        /// <summary>
        /// Reference: http://docs.aws.amazon.com/amazondynamodb/latest/developerguide/MakingHTTPRequests.html
        /// </summary>
        public HttpWebRequest CreateRequest(ClientConfiguration configuration, string awsService, DateTime? timestamp = null)
        {
            var dateBase = timestamp.HasValue ? timestamp.Value : DateTime.Now;

            var req = WebRequest.CreateHttp(configuration.DatabaseUri);
            req.UserAgent = "aws-sdk-dotnet-45/2.0.12.0 .NET Runtime/4.0 .NET Framework/4.0 OS/6.3.9600.0";
            req.ContentType = "application/x-amz-json-1.0";
            req.Accept = "application/json";
            req.Method = "POST";

            req.Headers.Add("X-Amz-Date", dateBase.ToString(Iso8601BasicDateTimeFormat));
            req.Headers.Add("X-Amz-Target", awsService);

            new RequestSigner().Sign(req, configuration);
            return req;
        }
    }
}