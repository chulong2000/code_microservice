using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Rijndael256;
using System;
using System.Security.Cryptography.X509Certificates;

namespace GHM.Infrastructure.Helpers
{
    public static class EncryptionHelper
    {
        public static string Encrypt(string plainText, string password)
        {
            return RijndaelEtM.Encrypt(plainText, password, KeySize.Aes256);
        }

        public static string Decrypt(string cipherText, string password)
        {
            return RijndaelEtM.Decrypt(cipherText, password, KeySize.Aes256);
        }

        [Obsolete]
        public static X509Certificate2 GenerateCertificate(string certName)
        {
            var keypairgen = new RsaKeyPairGenerator();
            keypairgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 1024));

            var keypair = keypairgen.GenerateKeyPair();

            var gen = new X509V3CertificateGenerator();

            var CN = new X509Name("CN=" + certName);
            var SN = Org.BouncyCastle.Math.BigInteger.ProbablePrime(120, new Random());

            gen.SetSerialNumber(SN);
            gen.SetSubjectDN(CN);
            gen.SetIssuerDN(CN);
            gen.SetNotAfter(DateTime.MaxValue);
            gen.SetNotBefore(DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)));
            gen.SetSignatureAlgorithm("MD5WithRSA");
            gen.SetPublicKey(keypair.Public);
            var newCert = gen.Generate(keypair.Private);
            return new X509Certificate2(DotNetUtilities.ToX509Certificate(newCert));
        }
    }
}
