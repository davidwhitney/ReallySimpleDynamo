using System.Net;

namespace ReallySimpleDynamo.RequestCreation
{
    public interface ICreateRequestTemplates
    {
        HttpWebRequest CreateRequestTemplate(ClientConfiguration configuration);
    }
}