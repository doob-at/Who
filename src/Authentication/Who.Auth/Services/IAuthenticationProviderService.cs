using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using doob.Who.Events;
using Who.Auth.Entities;
using Who.Auth.Entities.DTO;


namespace Who.Auth.Services
{
    public interface IAuthenticationProviderService
    {
        DataEventDispatcher EventDispatcher { get; }
        Task<List<AuthenticationProviderListDto>> GetAllListDtos();
        Task<List<AuthenticationProvider>> GetAll();
        Task<AuthenticationProvider> GetSingleAsync(Guid id);

        Task<AuthenticationProvider> GetByNameAsync(string name);
        Task Create(AuthenticationProvider authenticationProvider);
        Task Delete(params Guid[] id);
        Task Update(AuthenticationProvider authenticationProvider);
    }
}