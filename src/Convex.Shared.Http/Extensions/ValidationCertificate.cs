using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Convex.Shared.Http.Extensions
{
    public static class ValidationCertificate
    {
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) == 0)
                return false;

            if (chain == null)
                return true;

            // If there are errors in the certificate chain, look at each error to determine the cause.
            foreach (X509ChainStatusFlags status in chain.ChainStatus.Select(x => x.Status))
            {
                if (certificate.Subject == certificate.Issuer && status == X509ChainStatusFlags.UntrustedRoot)
                {
                    // Self-signed certificates with an untrusted root are valid.
                    continue;
                }
                if (status != X509ChainStatusFlags.NoError)
                {
                    // If there are any other errors in the certificate chain, the certificate is invalid,
                    // so the method returns false.
                    return false;
                }
            }

            return false;
        }
    }
}
