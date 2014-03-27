using System;
using ReallySimpleDynamo.Http;
using ReallySimpleDynamo.Model;
using ReallySimpleDynamo.RequestCreation;
using ReallySimpleDynamo.Serialization;

namespace ReallySimpleDynamo
{
    public class DynamoClient
    {
        public ClientConfiguration ClientConfiguration { get; set; }
        public IHttpClient HttpClient { get; set; }
        
        private readonly ICreateRequestTemplates _requestTemplater;
        private readonly ISignRequests _signer;
        private readonly IJsonSerializer _serializer;

        public DynamoClient(ClientConfiguration clientConfiguration, IHttpClient httpClient = null, ICreateRequestTemplates requestTemplater = null, ISignRequests signer = null, IJsonSerializer serializer = null)
        {
            if (clientConfiguration == null) throw new ArgumentNullException("clientConfiguration");

            ClientConfiguration = clientConfiguration;
            HttpClient = httpClient ?? new HttpClientWrapper();

            _requestTemplater = requestTemplater ?? new RequestTemplater();
            _signer = signer ?? new RequestSigner();
            _serializer = serializer ?? new JsonDotNetSerializer();
        }

        public ResponseEnvelope<T> Get<T>(string tableName, Key key) where T : class
        {
            if (string.IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException("tableName");
            if (key == null) throw new ArgumentNullException("key");

            var body = _serializer.Serialize(key);

            var request = _requestTemplater.CreateRequestTemplate(ClientConfiguration, "DynamoDB_20120810.GetItem");
            _signer.Sign(request, ClientConfiguration, body);
            
            var response = HttpClient.Send(request, body);

            if (response == null || response.Body == null)
            {
                return new ResponseEnvelope<T>();
            }

            return _serializer.Deserialize<ResponseEnvelope<T>>(response.Body);
        }

    }
}
