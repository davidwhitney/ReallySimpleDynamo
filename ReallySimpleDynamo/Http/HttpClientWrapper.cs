using System.IO;
using System.Net;
using System.Text;

namespace ReallySimpleDynamo.Http
{
    public class HttpClientWrapper : IHttpClient
    {
        public Response Send(HttpWebRequest request, string body = null)
        {
            if (body != null)
            {
                request.ContentLength = body.Length;

                var encodingUtf8GetBytes = Encoding.UTF8.GetBytes(body);
                using (var sw = request.GetRequestStream())
                {
                    sw.Write(encodingUtf8GetBytes, 0, encodingUtf8GetBytes.Length);
                }
            }

            using (var response = request.GetResponse())
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                var bodyContents = sr.ReadToEnd();
                return new Response {Body = bodyContents, ResponseWrapper = response};
            }
        }
    }
}