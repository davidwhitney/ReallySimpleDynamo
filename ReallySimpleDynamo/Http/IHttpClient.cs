using System.Net;

namespace ReallySimpleDynamo.Http
{
    public interface IHttpClient
    {
        Response Send(HttpWebRequest request, string body = null);
    }
}