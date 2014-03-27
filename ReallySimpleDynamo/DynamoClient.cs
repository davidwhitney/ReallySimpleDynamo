using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ReallySimpleDynamo
{
    public class DynamoClient
    {
        public ClientConfiguration ClientConfiguration { get; set; }
        public IHttpClient HttpClient { get; set; }

        public DynamoClient(ClientConfiguration clientConfiguration, IHttpClient httpClient)
        {
            if (clientConfiguration == null) throw new ArgumentNullException("clientConfiguration");
            if (httpClient == null) throw new ArgumentNullException("httpClient");

            ClientConfiguration = clientConfiguration;
            HttpClient = httpClient;
        }

        public T Get<T>(string tableName, Key key) where T : class
        {
            if (string.IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException("tableName");
            if (key == null) throw new ArgumentNullException("key");

            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.google.com");

            request.Headers.Add("X-Amz-Target", "DynamoDB_20120810.GetItem");

            var response = HttpClient.Send(request);

            return response as T;
        }
    }

    public interface IHttpClient
    {
        HttpResponseMessage Send(HttpRequestMessage request);
    }

    public class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _client;

        public HttpClientWrapper() : this(new HttpClient()) { }
        public HttpClientWrapper(HttpClient client)
        {
            _client = client;
        }

        public HttpResponseMessage Send(HttpRequestMessage request)
        {
            return _client.SendAsync(request).Result;
        }
    }

    public class Key : Dictionary<string, AttributeValue>
    {
        
    }

    public class AttributeValue
    {
        public string S { get; set; }
    }

    public class ClientConfiguration
    {
    }
}
