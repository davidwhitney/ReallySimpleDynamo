using System;
using System.Threading.Tasks;
using ReallySimpleDynamo.Http;
using ReallySimpleDynamo.Model;
using ReallySimpleDynamo.RequestCreation;
using ReallySimpleDynamo.RequestCreation.SignatureVersion4;
using ReallySimpleDynamo.Serialization;

namespace ReallySimpleDynamo
{
    public class DynamoClient
    {
        public ClientConfiguration ClientConfiguration { get; set; }
        public IHttpClient HttpClient { get; set; }
        
        private readonly ICreateSignedRequests _requestTemplater;
        private readonly IJsonSerializer _serializer;

        public DynamoClient(ClientConfiguration clientConfiguration, IHttpClient httpClient = null, ICreateSignedRequests requestCreator = null, IJsonSerializer serializer = null)
        {
            if (clientConfiguration == null) throw new ArgumentNullException("clientConfiguration");

            ClientConfiguration = clientConfiguration;
            HttpClient = httpClient ?? new HttpClientWrapper();

            _requestTemplater = requestCreator ?? new SignedRequestBuilder(new RequestSigner(new Sha256()));
            _serializer = serializer ?? new JsonDotNetSerializer();
        }

        public ResponseEnvelope<T> Get<T>(string tableName, Key key) where T : class
        {
            if (string.IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException("tableName");
            if (key == null) throw new ArgumentNullException("key");

            var body = _serializer.Serialize(key);

            var request = _requestTemplater.CreateRequest(ClientConfiguration, "DynamoDB_20120810.GetItem");
            var response = HttpClient.Send(request, body);

            if (response == null || response.Body == null)
            {
                return new ResponseEnvelope<T>();
            }

            return _serializer.Deserialize<ResponseEnvelope<T>>(response.Body);
        }

        public async Task<ResponseEnvelope<T>> GetAsync<T>(string tableName, Key key) where T : class
        {
            return await Task.Run(() => Get<T>(tableName, key));
        }
    }
}
