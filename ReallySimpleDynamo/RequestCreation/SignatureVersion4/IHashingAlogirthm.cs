namespace ReallySimpleDynamo.RequestCreation.SignatureVersion4
{
    public interface IHashingAlogirthm
    {
        string Name { get; }

        string ComputeSignature(string awsAccessKeyId, string awsSecretAccessKey, string region, string signedAt, string service, string canonicalizeHeaderNames, string canonicalRequest);
    }
}