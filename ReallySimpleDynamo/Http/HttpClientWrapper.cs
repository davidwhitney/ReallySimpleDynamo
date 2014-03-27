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
            {
                var stream = response.GetResponseStream();

                if (stream == null)
                {
                    return new Response {Body = body, ResponseWrapper = response};
                }

                using (var sr = new StreamReader(stream))
                {
                    return new Response {Body = sr.ReadToEnd(), ResponseWrapper = response};
                }
            }
        }
    }
}