using System;
using System.Net;

namespace ReallySimpleDynamo.RequestCreation
{
    public interface ICreateSignedRequests
    {
        HttpWebRequest CreateRequest(ClientConfiguration configuration, string awsService, DateTime? timestamp = null);
    }
}