using System;
using System.Collections.Generic;

namespace Who.Auth.Entities.DTO
{
    public class UserListDto
    {

        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public bool HasPassword { get; set; }

        public bool Active { get; set; }

        public List<string> Logins { get; set; } = new();

    }
}
