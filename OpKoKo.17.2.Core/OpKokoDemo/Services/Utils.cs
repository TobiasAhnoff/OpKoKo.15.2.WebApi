using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;

namespace OpKokoDemo.Services
{
    public static class Utils
    {
        private static X509Certificate2 _signingCertificate;

        public static TokenValidationParameters GetTokenValidationParameters(
            string signingCertificateSubjectDistinguishedName, string issuer, string audience) {
            var signingCert = Utils.GetCertFromCertStore(signingCertificateSubjectDistinguishedName);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new X509SecurityKey(signingCert),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
            return tokenValidationParameters;
        }

        public static X509Certificate2 GetCertFromCertStore(string certificateSubjectDistinguishedName)
        {
            if (_signingCertificate == null)
            {

                //TODO
                //Azure uses the cert store of the current user and there is no easy way to add a self-signed cert to trusted people...
                //thus validOnly needs to be set to false when unsing a self-signed cert.
                //https://azure.microsoft.com/en-us/blog/using-certificates-in-azure-websites-applications/
                //Maybe somthing like this could solve the problem
                //http://leastprivilege.com/2011/03/01/adding-a-certificate-to-the-root-certificate-store-from-the-command-line-e-g-as-an-azure-startup-task/

                //Create signign cert with
                //https://brockallen.com/2015/06/01/makecert-and-creating-ssl-or-signing-certificates/

                //var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                _signingCertificate = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName,
                    certificateSubjectDistinguishedName, false)[0];
            }

            return _signingCertificate;
        }
    }
}