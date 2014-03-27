using System;
using System.Net;

namespace ReallySimpleDynamo.RequestCreation
{
    public interface ICreateRequestTemplates
    {
        HttpWebRequest CreateRequestTemplate(ClientConfiguration configuration, string awsService, DateTime? timestamp = null);
    }
}