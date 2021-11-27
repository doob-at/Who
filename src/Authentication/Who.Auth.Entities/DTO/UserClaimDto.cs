using System;
using System.ComponentModel.DataAnnotations;

namespace Who.Auth.Entities.DTO
{
    public class UserClaimDto
    {
        public Guid? Id { get; set; }

        [MaxLength(250)]
        [Required]
        public string Type { get; set; }

        [MaxLength(250)]
        [Required]
        public string Value { get; set; }


        public string ConcurrencyStamp { get; set; }

    }
}
