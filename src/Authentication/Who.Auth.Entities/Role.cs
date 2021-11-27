using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Who.Auth.Entities
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public bool BuiltIn { get; set; }

        public List<User> Users { get; set; } = new();
        public Guid ClientId { get; set; }
    }
}
