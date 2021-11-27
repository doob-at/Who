using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Who.Auth.Entities
{
    public class User: IConcurrencyAware
    {
        [Key]
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool Active { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }

        [ConcurrencyCheck]
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public List<UserClaim> Claims { get; set; } = new();
        public List<Role> Roles { get; set; } = new();

        public string SecurityCode { get; set; }

        public DateTime SecurityCodeExpirationDate { get; set; }
        
    }
}
