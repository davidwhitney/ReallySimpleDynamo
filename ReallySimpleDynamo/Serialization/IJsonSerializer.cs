namespace ReallySimpleDynamo.Serialization
{
    public interface IJsonSerializer
    {
        T Deserialize<T>(string data);
        string Serialize(object data);
    }
}
