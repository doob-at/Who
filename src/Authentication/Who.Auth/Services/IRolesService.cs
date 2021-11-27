using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Who.Auth.Entities;
using Who.Auth.Entities.DTO;

namespace who.Auth.Services
{
    public interface IRolesService
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleAsync(Guid id);


        Task CreateRoleAsync(Role roleDto);

        Task DeleteRole(params Guid[] ids);
        Task UpdateRoleAsync(Role updated);
        Task CreateRoleAsync(RoleDto roleDto);
        Task UpdateRoleAsync(RoleDto roleDto);
    }
}
