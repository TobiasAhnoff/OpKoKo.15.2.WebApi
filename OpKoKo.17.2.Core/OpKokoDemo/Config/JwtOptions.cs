using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpKokoDemo.Config
{
    public class JwtOptions
    {
        public string SigningCertificateSubjectDistinguishedName { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
