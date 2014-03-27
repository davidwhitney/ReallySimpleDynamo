using System;
using ReallySimpleDynamo.Http;
using ReallySimpleDynamo.Model;
using ReallySimpleDynamo.RequestCreation;

namespace ReallySimpleDynamo
{
    public class DynamoClient
    {
        public ClientConfiguration ClientConfiguration { get; set; }
        public IHttpClient HttpClient { get; set; }
        
        private readonly ICreateRequestTemplates _requestTemplater;
        private readonly ISignRequests _signer;

        public DynamoClient(ClientConfiguration clientConfiguration, IHttpClient httpClient = null, ICreateRequestTemplates requestTemplater = null, ISignRequests signer = null)
        {
            if (clientConfiguration == null) throw new ArgumentNullException("clientConfiguration");

            ClientConfiguration = clientConfiguration;
            HttpClient = httpClient ?? new HttpClientWrapper();

            _requestTemplater = requestTemplater ?? new RequestTemplater();
            _signer = signer ?? new RequestSigner();
        }

        public T Get<T>(string tableName, Key key) where T : class
        {
            if (string.IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException("tableName");
            if (key == null) throw new ArgumentNullException("key");

            var request = _requestTemplater.CreateRequestTemplate(ClientConfiguration, "DynamoDB_20120810.GetItem");

            var body = ""; // TODO: Serialize request here

            _signer.Sign(request, ClientConfiguration, body);
            
            var response = HttpClient.Send(request, body);
            var dto = default(T); // TODO: Deserialize response.Body here;

            return dto;
        }

    }
}
