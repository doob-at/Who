using System;

namespace Who.Auth.Entities.DTO
{
    public class AuthenticationProviderListDto
    {
        public Guid Id { get; set; }

        public string Type { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
