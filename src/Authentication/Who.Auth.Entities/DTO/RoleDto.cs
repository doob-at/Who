using System;
using System.Collections.Generic;

namespace Who.Auth.Entities.DTO
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public string ClientId { get; set; }

        public ICollection<UserListDto> Users { get; set; } = new List<UserListDto>();
    }
}
