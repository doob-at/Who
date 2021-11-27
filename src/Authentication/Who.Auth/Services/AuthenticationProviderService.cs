using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doob.Who.Events;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Who.Auth.Context;
using Who.Auth.Entities;
using Who.Auth.Entities.DTO;
using Who.Auth.ExtensionMethods;


namespace Who.Auth.Services
{
    public class AuthenticationProviderService : IAuthenticationProviderService
    {
        private readonly IMapper _mapper;
        private AuthDbContext DbContext { get; }

        public DataEventDispatcher EventDispatcher { get; }

        public AuthenticationProviderService(AuthDbContext dbContext, DataEventDispatcher eventDispatcher, IMapper mapper)
        {
            _mapper = mapper;
            DbContext = dbContext;
            EventDispatcher = eventDispatcher;
        }

        public async Task<List<AuthenticationProviderListDto>> GetAllListDtos()
        {
            var providers = _mapper.From(DbContext.AuthenticationProviders).ProjectToType<AuthenticationProviderListDto>();

            return await providers.ToListAsync();
        }

        public Task<List<AuthenticationProvider>> GetAll()
        {
            return DbContext.AuthenticationProviders.ToListAsync();
        }

        public Task<AuthenticationProvider> GetSingleAsync(Guid id)
        {
            return DbContext.AuthenticationProviders.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<AuthenticationProvider> GetByNameAsync(string name)
        {
            return DbContext.AuthenticationProviders.FirstOrDefaultAsync(p => p.Name== name);
        }

        public async Task Create(AuthenticationProvider authenticationProvider)
        {
            await DbContext.AuthenticationProviders.AddAsync(authenticationProvider);
            await DbContext.SaveChangesAsync();

            EventDispatcher.DispatchCreatedEvent("AuthProviders", authenticationProvider);
        }

        public async Task Delete(params Guid[] id)
        {
            var providers = await DbContext.AuthenticationProviders.Where(p => id.Contains(p.Id)).ToListAsync();
            DbContext.AuthenticationProviders.RemoveRange(providers);
            await DbContext.SaveChangesAsync();
            EventDispatcher.DispatchDeletedEvent("AuthProviders", providers.Select(r => r.Id));
        }

        public async Task Update(AuthenticationProvider authenticationProvider)
        {
            await DbContext.SaveChangesAsync();
            EventDispatcher.DispatchUpdatedEvent("AuthProviders", authenticationProvider);
        }
    }
}
