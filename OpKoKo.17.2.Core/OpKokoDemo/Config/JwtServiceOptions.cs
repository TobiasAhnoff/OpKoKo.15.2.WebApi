namespace OpKokoDemo.Config
{
    public class JwtServiceOptions
    {
        public string SigningCertificateSubjectDistinguishedName { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string DeveloperSub { get; set; }
        public string DeveloperScope { get; set; }
    }
}
