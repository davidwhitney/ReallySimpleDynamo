using System;

namespace ReallySimpleDynamo.RequestCreation.SignatureVersion4
{
    public interface IHashingAlogirthm
    {
        string Name { get; }

        string ComputeSignature(string awsSecretAccessKey, string region, DateTime signedAt, string service);
    }
}