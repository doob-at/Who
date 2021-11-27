using System;
using System.Collections.Generic;

namespace Who.Auth.Entities.DTO
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public string ClientId { get; set; }
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool RequirePkce { get; set; }

        public List<ClientRedirectUriDto> RedirectUris { get; set; }
        public List<ClientPostLogoutRedirectUriDto> PostLogoutRedirectUris { get; set; }
    }
}
