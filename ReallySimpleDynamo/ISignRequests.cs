using System.Net;

namespace ReallySimpleDynamo
{
    public interface ISignRequests
    {
        void Sign(HttpWebRequest request, ClientConfiguration configuration, string body);
    }
}