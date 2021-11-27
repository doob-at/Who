using System;

namespace Who.Auth.Entities.DTO
{
    public class ExternalClaimDto
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public string Issuer { get; set; }
        
    }
}
