using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Who.Auth.Entities;
using Who.Auth.Entities.DTO;

namespace Who.Auth.Services
{
    public interface ILocalUserService
    {
        //Task<bool> ValidateClearTextCredentialsAsync(
        //    string userName,
        //    string password);
        Task<bool> ValidateCredentialsAsync(
            string userName,
            string password);
        Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(
            string subject);
        Task<User> GetUserByUserNameOrEmailAsync(
            string userName);
        Task<User> GetUserByEmailAsync(
            string userName);
        Task<User> GetUserBySubjectAsync(
            string subject);

        Task<UserDto> AddUserAsync(User userToAdd, string password = null);

        
        Task<bool> IsUserActive(
            string subject);
        Task<bool> ActivateUser(
            string securityCode);
        Task<bool> SaveChangesAsync();
        Task<string> InitiatePasswordResetRequest(string email);

        Task<bool> SetPassword(
            string securityCode,
            string password);

        Task<bool> SetPassword(Guid id, string password);

        Task<User> GetUserByExternalProvider(
            string provider,
            string providerIdentityKey);
        User ProvisionUserFromExternalIdentity(
            string provider,
            string providerIdentityKey,
            IEnumerable<Claim> claims);
        Task AddExternalProviderToUser(
            string subject,
            string provider,
            string providerIdentityKey);
        Task<bool> AddUserSecret(
            string subject,
            string name,
            string secret);
        //Task<UserSecret> GetUserSecret(
        //    string subject,
        //    string name);
        //Task<bool> UserHasRegisteredTotpSecret(
        //    string subject);


        public Task<bool> ClearPassword(Guid id);

        Task<List<User>> GetAllUsersAsync();

        //Task<MUser> GetUserAsync(Guid id);
        //Task<MUser> GetUserAsync(ClaimsPrincipal claimsPrincipal);

        //Task<MUserDto> GetUserDtoAsync(Guid id);


        Task<User> GetUserAsync(ClaimsPrincipal principal);
        Task<bool> CanSignInAsync(User user);
        Task<ClaimsPrincipal> CreateUserPrincipalAsync(User user);
        Task<User> GetUserByIdAsync(Guid id);
        Task UpdateAsync(User user);
        Task UpdateAsync(UserDto user);

        Task DeleteAsync(params Guid[] id);
        Task<UserDto> AddUserAsync(CreateUserDto createUserDto);
    }
}
