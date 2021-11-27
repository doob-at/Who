using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doob.Who.Events;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Who.Auth.Context;
using Who.Auth.Entities;
using Who.Auth.Entities.DTO;


namespace who.Auth.Services
{
    public class RolesService : IRolesService
    {

        private AuthDbContext DbContext { get; }
        public DataEventDispatcher EventDispatcher { get; }
        private IMapper _mapper;


        public RolesService(AuthDbContext dbContext, DataEventDispatcher eventDispatcher, IMapper mapper)
        {
            DbContext = dbContext;
            EventDispatcher = eventDispatcher;
            _mapper = mapper;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await DbContext.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleAsync(Guid id)
        {
            return await DbContext.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task CreateRoleAsync(Role role)
        {

            await DbContext.Roles.AddAsync(role);
            await DbContext.SaveChangesAsync();

            EventDispatcher.DispatchCreatedEvent("IDPRoles", role);
        }

        public async Task CreateRoleAsync(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            DbContext.Attach(role);
            await CreateRoleAsync(role);
        }

        public async Task UpdateRoleAsync(RoleDto roleDto)
        {
            var roleInDb = await DbContext.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == roleDto.Id);
            if (roleInDb == null)
            {
                return;
            }

            _mapper.From(roleDto).EntityFromContext(DbContext).AdaptTo(roleInDb);

            await DbContext.SaveChangesAsync();
        }


        public async Task DeleteRole(params Guid[] id)
        {
            var roles = await DbContext.Roles.AsQueryable().Where(u => id.Contains(u.Id)).ToListAsync();
            DbContext.Roles.RemoveRange(roles);
            await DbContext.SaveChangesAsync();

            EventDispatcher.DispatchDeletedEvent("IDPRoles", roles.Select(r => r.Id));
        }

        //public async Task UpdateRoleAsync(MRole updated)
        //{
        //    //var roleModel = _mapper.Map<MRole>(updated);
        //    var userIds = updated.Users.Select(ur => ur.Id).ToList();
        //    var availableUsers = DbContext.Users.AsQueryable().Where(r => userIds.Contains(r.Id)).Select(r => r.Id).ToList();

        //    updated.Users = updated.Users.Where(ur => availableUsers.Contains(ur.Id)).ToList();

        //    await DbContext.SaveChangesAsync();

        //    EventDispatcher.DispatchUpdatedEvent("IDPRoles", _mapper.Map<MRoleListDto>(updated));
        //}

        public async Task UpdateRoleAsync(Role updated)
        {
            
            //var roleInDb = await GetRoleAsync(updated.Id);
            //_mapper.Map(updated, roleInDb);
            ////var roleModel = _mapper.Map<MRole>(updated);
            //var userIds = updated.Users.Select(ur => ur.Id).ToList();
            //var availableUsers = DbContext.Users.Where(r => userIds.Contains(r.Id)).ToList();

            //roleInDb.Users = availableUsers;


            DbContext.Attach(updated);

            await DbContext.SaveChangesAsync();

            EventDispatcher.DispatchUpdatedEvent("IDPRoles", updated);
        }

        

    }
}