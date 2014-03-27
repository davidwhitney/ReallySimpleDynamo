using System;

namespace ReallySimpleDynamo
{
    public class DynamoClient
    {
        public ClientConfiguration ClientConfiguration { get; set; }

        public DynamoClient(ClientConfiguration clientConfiguration)
        {
            if (clientConfiguration == null) throw new ArgumentNullException("clientConfiguration");

            ClientConfiguration = clientConfiguration;
        }

        public void Get<T>(string tableName, string key)
        {
            if (string.IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException("tableName");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");

            throw new NotImplementedException();
        }
    }

    public class ClientConfiguration
    {
    }
}
