using System;

namespace Bhbk.Lib.Cryptography.Certificates
{
    //https://github.com/bcgit/bc-csharp/blob/master/crypto/src/crypto/operators/Asn1Signature.cs
    public enum SignatureType
    {
        SHA1WithRSA,
        SHA1WithECDSA,
        SHA256WithRSA,
        SHA256WithECDSA,
        SHA384WithRSA,
        SHA384WithECDSA,
        SHA512WithRSA,
        SHA512WithECDSA,
    }

    public enum RsaKeyLength
    {
        Bits1024 = 1024,
        Bits2048 = 2048,
        Bits4096 = 4096
    }
}
