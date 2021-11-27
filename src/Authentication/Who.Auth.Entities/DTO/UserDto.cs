using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Who.Auth.Entities.DTO
{
    public class UserDto
    {

        public Guid Id { get; set; }

        [MaxLength(200)]
        public string UserName { get; set; }

        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }

        public DateTime? ExpiresOn { get; set; }

        public bool HasPassword { get; set; }

        public bool Active { get; set; }

        public string ConcurrencyStamp { get; set; }

        public List<UserClaimDto> Claims { get; set; } = new();
        public List<ExternalClaimDto> ExternalClaims { get; set; } = new();
        //public ICollection<MUserLogin> Logins { get; set; } = new List<MUserLogin>();
        //public ICollection<MUserSecret> Secrets { get; set; } = new List<MUserSecret>();

        public List<RoleListDto> Roles { get; set; } = new();

    }
}
