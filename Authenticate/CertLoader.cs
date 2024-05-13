using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CapstoneApi.Authenticate;
public class CertLoader
{
    public static RSA? LoadCertificateFromBase64String(string base64Certificate)
    {
        try
        {
            // Decode the Base64 string into a byte array
            byte[] certBytes = Convert.FromBase64String(base64Certificate);

            // Create an X509Certificate2 object from the decoded byte array
            X509Certificate2 certificate = new(certBytes);

            RSA rsaPublicKey = certificate.GetRSAPublicKey()!;
            return rsaPublicKey;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading private key: {ex.Message}");
            return null;
        }
    }
}