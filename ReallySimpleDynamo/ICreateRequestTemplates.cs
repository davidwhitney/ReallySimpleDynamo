using System.Net;

namespace ReallySimpleDynamo
{
    public interface ICreateRequestTemplates
    {
        HttpWebRequest CreateRequestTemplate(ClientConfiguration configuration);
    }
}