using System;
using System.ComponentModel.DataAnnotations;

namespace Who.Auth.Entities
{
    public class ClientRedirectUri
    {
        [Key]
        public Guid Id { get; set; }
        public string RedirectUri { get; set; }

        public Guid ClientId { get; set; }
    }
}
