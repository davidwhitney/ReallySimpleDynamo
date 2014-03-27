using System;
using System.Net;

namespace ReallySimpleDynamo.RequestCreation
{
    public class RequestTemplater : ICreateRequestTemplates
    {
        public HttpWebRequest CreateRequestTemplate(ClientConfiguration configuration)
        {
            var dateBase = DateTime.Now;
            var timestamp = "";

            var req = WebRequest.CreateHttp(configuration.DatabaseUri);
            req.UserAgent = "aws-sdk-dotnet-45/2.0.12.0 .NET Runtime/4.0 .NET Framework/4.0 OS/6.3.9600.0";
            req.ContentType = "application/x-amz-json-1.0";
            req.Accept = "application/json";
            req.Method = "POST";

            req.Headers.Add("X-Amz-Date", timestamp);

            return req;
        }
    }
}