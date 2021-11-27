using System;
using System.Collections.Generic;

namespace Who.Auth.Entities.DTO
{
    public class CreateClientDto
    {
        //public Guid Id { get; set; }
        public string ClientId { get; set; }
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool RequirePkce { get; set; }

        public List<ClientRedirectUriDto> RedirectUris { get; set; }
        public List<string> PostLogoutRedirectUris { get; set; }
    }
}
