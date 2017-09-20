using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpKokoDemo.Config;

namespace OpKokoDemo.Services
{
    public class TestTokenService : ITokenService
    {
        private static X509Certificate2 _signingCertificate;
        private readonly string _signingCertificateSubjectDistinguishedName;
        private readonly string _issuer;
        private readonly string _audience;

        public TestTokenService(IOptions<JwtServiceOptions> options)
        {
            _signingCertificateSubjectDistinguishedName = options.Value.SigningCertificateSubjectDistinguishedName;
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;

        }
        public string GetToken()
        {
            return CreateJwtAccessToken(
                new List<Claim> {new Claim("productService", "read,write", ClaimValueTypes.String, "TestTokenService")},
                TimeSpan.FromMinutes(20));
        }

        public string CreateJwtAccessToken(ICollection<Claim> claims, TimeSpan tokenLifeTime)
        {
            var signingCertificate = GetSigningCertifacate();
            var signingCredentials = new SigningCredentials(
                new X509SecurityKey(signingCertificate),
                SecurityAlgorithms.RsaSha512Signature);

            var now = DateTime.UtcNow;
            //DateTimeOffset dto = new DateTimeOffset(now);
            //var claimList = new List<Claim>(claims)
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, claims.Single(x => x.Type == "username").Value),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim(JwtRegisteredClaimNames.Iat, dto.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            //};

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                now,
                now.Add(tokenLifeTime),
                signingCredentials);

            //.NET 4.5 fix, for more see i e https://github.com/IdentityServer/IdentityServer3/issues/431
            //token.Header.Remove("kid");
            //token.Header.Add("x5t", Base64UrlEncoder.Encode(_signingCertificate.GetCertHash()));

            string accessToken;
            try
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return accessToken;
        }

        private X509Certificate2 GetSigningCertifacate()
        {
            if (_signingCertificate == null)
            {
                //TODO
                //Azure uses the cert store of the current user and there is no easy way to add a self-signed cert to trusted people...
                //thus validOnly needs to be set to false when unsing a self-signed cert.
                //https://azure.microsoft.com/en-us/blog/using-certificates-in-azure-websites-applications/
                //Maybe somthing like this could solve the problem
                //http://leastprivilege.com/2011/03/01/adding-a-certificate-to-the-root-certificate-store-from-the-command-line-e-g-as-an-azure-startup-task/

                //var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                _signingCertificate = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, _signingCertificateSubjectDistinguishedName, false)[0];

            }

            return _signingCertificate;
        }
    }
}
