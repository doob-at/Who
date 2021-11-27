using System;

namespace Who.Auth.Entities.DTO
{
    public class ClientRedirectUriDto
    {
        public Guid? Id { get; set; }
        public string RedirectUri { get; set; }

    }
}
