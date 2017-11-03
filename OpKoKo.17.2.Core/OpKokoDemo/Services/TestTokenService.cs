using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpKokoDemo.Config;

namespace OpKokoDemo.Services
{
    public class TestTokenService : ITokenService
    {
        private readonly string _signingCertificateSubjectDistinguishedName;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _devloperSub;
        private readonly string _devloperScope;

        public TestTokenService(IOptions<JwtServiceOptions> options)
        {
            _signingCertificateSubjectDistinguishedName = options.Value.SigningCertificateSubjectDistinguishedName;
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
            _devloperSub = options.Value.DeveloperSub;
            _devloperScope = options.Value.DeveloperScope;

        }
        public string GetToken()
        {
            //Note that normaly you would authenticate the client and/or the user and then add 
            //acecss token claims, but for test implementation we just hard code some claims
            var accessTokenClaims = new List<Claim>
            {
                new Claim("sub", _devloperSub, ClaimValueTypes.String, "TestTokenService"),
                new Claim("scope", _devloperScope, ClaimValueTypes.String, "TestTokenService")
            };

            return CreateJwtAccessToken(accessTokenClaims, TimeSpan.FromMinutes(20));
        }

        public string CreateJwtAccessToken(ICollection<Claim> claims, TimeSpan tokenLifeTime)
        {
            var signingCertificate = Utils.GetCertFromCertStore(_signingCertificateSubjectDistinguishedName);
            var signingCredentials = new SigningCredentials(
                new X509SecurityKey(signingCertificate),
                SecurityAlgorithms.RsaSha512Signature);

            var now = DateTime.UtcNow;
            //Optional JWT claims
            //DateTimeOffset dto = new DateTimeOffset(now);
            //var claimList = new List<Claim>(claims)
            //{
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
    }
}
