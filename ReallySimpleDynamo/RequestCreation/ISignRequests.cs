using System.Net;

namespace ReallySimpleDynamo.RequestCreation
{
    public interface ISignRequests
    {
        void Sign(HttpWebRequest request, ClientConfiguration configuration, string body);
    }
}