using System;

namespace Who.Auth.Entities.DTO
{
    public class ClientPostLogoutRedirectUriDto
    {
        public Guid Id { get; set; }
        public string PostLogoutRedirectUri { get; set; }

    }
}
