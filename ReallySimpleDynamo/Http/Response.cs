using System.Net;

namespace ReallySimpleDynamo.Http
{
    public class Response
    {
        public WebResponse ResponseWrapper { get; set; }
        public string Body { get; set; }
    }
}