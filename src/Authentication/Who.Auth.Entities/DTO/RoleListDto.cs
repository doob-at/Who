using System;

namespace Who.Auth.Entities.DTO
{
    public class RoleListDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

    }
}
