using Newtonsoft.Json;

namespace ReallySimpleDynamo.Serialization
{
    class JsonDotNetSerializer : IJsonSerializer
    {
        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}