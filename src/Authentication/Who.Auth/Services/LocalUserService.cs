using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using doob.Reflectensions.Common;
using doob.Who.Events;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Who.Auth.Context;
using Who.Auth.Entities;
using Who.Auth.Entities.DTO;

namespace Who.Auth.Services
{
    public class LocalUserService : ILocalUserService
    {
        public DataEventDispatcher EventDispatcher { get; }
        private readonly AuthDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public LocalUserService(
            AuthDbContext context,
            IPasswordHasher<User> passwordHasher, DataEventDispatcher eventDispatcher, IMapper mapper)
        {

            _context = context
                       ?? throw new ArgumentNullException(nameof(context));
            _passwordHasher = passwordHasher
                ?? throw new ArgumentNullException(nameof(passwordHasher));
            EventDispatcher = eventDispatcher;
            _mapper = mapper;
        }


        public async Task<bool> IsUserActive(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                return false;
            }

            var user = await GetUserBySubjectAsync(subject);

            if (user == null)
            {
                return false;
            }

            return user.Active;
        }

        //public async Task<bool> ValidateClearTextCredentialsAsync(string userName,
        //  string password)
        //{
        //    if (string.IsNullOrWhiteSpace(userName) ||
        //        string.IsNullOrWhiteSpace(password))
        //    {
        //        return false;
        //    }

        //    var user = await GetUserByUserNameOrEmailAsync(userName);

        //    if (user == null)
        //    {
        //        return false;
        //    }

        //    if (!user.Active)
        //    {
        //        return false;
        //    }

        //    // Validate credentials
        //    return (user.Password == password);
        //}

        public async Task<bool> ValidateCredentialsAsync(string userName,
            string password)
        {
            if (string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var user = await GetUserByUserNameOrEmailAsync(userName);



            if (user == null)
            {
                return false;
            }

            if (!user.Active)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                return false;
            }

            // Validate credentials
            var verificationResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                password);
            return (verificationResult == PasswordVerificationResult.Success);
        }

        public async Task<User> GetUserByEmailAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            return await _context.Users
                    .Include(u => u.Roles)
                    .Include(u => u.Claims)
                    //.Include(u => u.ExternalClaims)
                    .FirstOrDefaultAsync(u => u.Email == userName);

        }

        public async Task<User> GetUserByUserNameOrEmailAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (userName.IsValidEmailAddress())
            {
                return await GetUserByEmailAsync(userName);
            }
            else
            {
                return await _context.Users
                    .Include(u => u.Roles)
                    .Include(u => u.Claims)
                    //.Include(u => u.ExternalClaims)
                    .FirstOrDefaultAsync(u => u.UserName == userName);
            }


        }


        public async Task<IEnumerable<UserClaim>> GetUserClaimsBySubjectAsync(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            var user = await GetUserBySubjectAsync(subject);

            return await _context.UserClaims.AsQueryable().Where(u => u.UserId == user.Id).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {

            return await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Claims)
                //.Include(u => u.ExternalClaims)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserBySubjectAsync(string subject)
        {

            return await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Claims)
                //.Include(u => u.ExternalClaims)
                .FirstOrDefaultAsync(u => u.Subject == subject);
        }

        public async Task<UserDto> AddUserAsync(User userToAdd, string password = null)
        {
            if (userToAdd == null)
            {
                throw new ArgumentNullException(nameof(userToAdd));
            }

            if (_context.Users.Any(u => u.UserName == userToAdd.UserName))
            {
                // in a real-life scenario you'll probably want to 
                // return this as a validation issue
                throw new Exception("Username must be unique");
            }

            if (_context.Users.Any(u => u.Email == userToAdd.Email))
            {
                // in a real-life scenario you'll probably want to 
                // return this a a validation issue
                throw new Exception("Email must be unique");
            }

            if (String.IsNullOrWhiteSpace(userToAdd.Subject))
            {
                userToAdd.Subject = Guid.NewGuid().ToString();
            }

            if (!String.IsNullOrWhiteSpace(password))
            {
                using (var randomNumberGenerator = new RNGCryptoServiceProvider())
                {
                    var securityCodeData = new byte[128];
                    randomNumberGenerator.GetBytes(securityCodeData);
                    userToAdd.SecurityCode = Convert.ToBase64String(securityCodeData);
                }

                userToAdd.SecurityCodeExpirationDate = DateTime.UtcNow.AddHours(1);

                userToAdd.Password = _passwordHasher.HashPassword(userToAdd, password);
            }

            _context.Attach(userToAdd);
            await _context.Users.AddAsync(userToAdd);
            await _context.SaveChangesAsync();
            EventDispatcher.DispatchCreatedEvent("WhoUser", userToAdd);

            return _mapper.Map<UserDto>(userToAdd);
        }

        public Task<UserDto> AddUserAsync(CreateUserDto createUserDto)
        {

            //var usermodel = _mapper.From(createUserDto).EntityFromContext(_context).AdaptToType<User>();
            var userModel = _mapper.Map<User>(createUserDto);
            _context.Attach(userModel);
            return AddUserAsync(userModel);
        }

        //public void AddUser(MUser userToAdd, string password)
        //{
        //    if (userToAdd == null)
        //    {
        //        throw new ArgumentNullException(nameof(userToAdd));
        //    }

        //    if (string.IsNullOrWhiteSpace(password))
        //    {
        //        throw new ArgumentNullException(nameof(password));
        //    }

        //    if (_context.Users.Any(u => u.UserName == userToAdd.UserName))
        //    {
        //        // in a real-life scenario you'll probably want to 
        //        // return this as a validation issue
        //        throw new Exception("Username must be unique");
        //    }

        //    if (_context.Users.Any(u => u.Email == userToAdd.Email))
        //    {
        //        // in a real-life scenario you'll probably want to 
        //        // return this a a validation issue
        //        throw new Exception("Email must be unique");
        //    }

        //    using (var randomNumberGenerator = new RNGCryptoServiceProvider())
        //    {
        //        var securityCodeData = new byte[128];
        //        randomNumberGenerator.GetBytes(securityCodeData);
        //        userToAdd.SecurityCode = Convert.ToBase64String(securityCodeData);
        //    }

        //    userToAdd.SecurityCodeExpirationDate = DateTime.UtcNow.AddHours(1);

        //    userToAdd.Password = _passwordHasher.HashPassword(userToAdd, password);
        //    _context.Users.Add(userToAdd);
        //    _context.SaveChanges();
        //    EventDispatcher.DispatchCreatedEvent("IDPUsers", _mapper.Map<MUserDto>(userToAdd));
        //}

        //public void AddUser(MUser userToAdd, string password)
        //{
        //    if (userToAdd == null)
        //    {
        //        throw new ArgumentNullException(nameof(userToAdd));
        //    }

        //    if (string.IsNullOrWhiteSpace(password))
        //    {
        //        throw new ArgumentNullException(nameof(password));
        //    }

        //    if (_context.Users.Any(u => u.UserName == userToAdd.UserName))
        //    {
        //        // in a real-life scenario you'll probably want to 
        //        // return this a a validation issue
        //        throw new Exception("Username must be unique");
        //    }

        //    if (_context.Users.Any(u => u.Email == userToAdd.Email))
        //    {
        //        // in a real-life scenario you'll probably want to 
        //        // return this a a validation issue
        //        throw new Exception("Email must be unique");
        //    }

        //    // hash & salt the password
        //    userToAdd.Password = _passwordHasher.HashPassword(userToAdd, password);

        //    using (var randomNumberGenerator = new RNGCryptoServiceProvider())
        //    {
        //        var securityCodeData = new byte[128];
        //        randomNumberGenerator.GetBytes(securityCodeData);
        //        userToAdd.SecurityCode = Convert.ToBase64String(securityCodeData);
        //    }
        //    userToAdd.SecurityCodeExpirationDate = DateTime.UtcNow.AddHours(1);
        //    _context.Users.Add(userToAdd);
        //}

        public async Task<bool> ActivateUser(string securityCode)
        {
            if (string.IsNullOrWhiteSpace(securityCode))
            {
                throw new ArgumentNullException(nameof(securityCode));
            }

            // find an user with this security code as an active security code.  
            var user = await _context.Users.AsQueryable().FirstOrDefaultAsync(u =>
                u.SecurityCode == securityCode &&
                u.SecurityCodeExpirationDate >= DateTime.UtcNow);

            if (user == null)
            {
                return false;
            }

            user.Active = true;
            user.SecurityCode = null;
            return true;
        }

        public async Task<bool> AddUserSecret(string subject, string name, string secret)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new ArgumentNullException(nameof(secret));
            }

            var user = await GetUserBySubjectAsync(subject);

            if (user == null)
            {
                return false;
            }

            ///TODO: Enable when Secrets implemented
            //user.Secrets.Add(new MUserSecret() { Name = name, Secret = secret });
            return true;
        }

        //public async Task<bool> UserHasRegisteredTotpSecret(string subject)
        //{
        //    if (string.IsNullOrWhiteSpace(subject))
        //    {
        //        throw new ArgumentNullException(nameof(subject));
        //    }

        //    return await _context.UserSecrets.AsQueryable().AnyAsync(u =>
        //    u.User.Subject == subject && u.Name == "TOTP");
        //}






        //public async Task<MUserSecret> GetUserSecret(string subject, string name)
        //{
        //    if (string.IsNullOrWhiteSpace(subject))
        //    {
        //        throw new ArgumentNullException(nameof(subject));
        //    }

        //    if (string.IsNullOrWhiteSpace(name))
        //    {
        //        throw new ArgumentNullException(nameof(name));
        //    }

        //    return await _context.UserSecrets.AsQueryable()
        //        .FirstOrDefaultAsync(u => u.User.Subject == subject && u.Name == name);
        //}

        public async Task<string> InitiatePasswordResetRequest(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var user = await _context.Users.AsQueryable().FirstOrDefaultAsync(u =>
              u.Email == email);

            if (user == null)
            {
                throw new Exception($"MUser with email address {email} can't be found.");
            }

            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var securityCodeData = new byte[128];
                randomNumberGenerator.GetBytes(securityCodeData);
                user.SecurityCode = Convert.ToBase64String(securityCodeData);
            }

            user.SecurityCodeExpirationDate = DateTime.UtcNow.AddHours(1);
            return user.SecurityCode;
        }

        public async Task<bool> SetPassword(string securityCode, string password)
        {
            if (string.IsNullOrWhiteSpace(securityCode))
            {
                throw new ArgumentNullException(nameof(securityCode));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var user = await _context.Users.AsQueryable().FirstOrDefaultAsync(u =>
            u.SecurityCode == securityCode &&
            u.SecurityCodeExpirationDate >= DateTime.UtcNow);

            if (user == null)
            {
                return false;
            }

            user.SecurityCode = null;
            // hash & salt the password
            user.Password = _passwordHasher.HashPassword(user, password);
            return true;
        }

        public async Task<bool> SetPassword(Guid id, string password)
        {

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var user = await _context.Users.AsQueryable().FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return false;
            }

            user.SecurityCode = null;
            // hash & salt the password
            user.Password = _passwordHasher.HashPassword(user, password);
            await _context.SaveChangesAsync();
            EventDispatcher.DispatchUpdatedEvent("WhoUser", user);
            return true;
        }

        public async Task<bool> ClearPassword(Guid id)
        {
            var user = await _context.Users.AsQueryable().FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return false;
            }

            user.SecurityCode = null;
            user.Password = null;
            await _context.SaveChangesAsync();
            EventDispatcher.DispatchUpdatedEvent("WhoUser", user);
            return true;
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByExternalProvider(
            string provider,
            string providerIdentityKey)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(providerIdentityKey))
            {
                throw new ArgumentNullException(nameof(providerIdentityKey));
            }

            //var userLogin = await _context.UserLogins.Include(ul => ul.User)
            //    .FirstOrDefaultAsync(ul => ul.Provider == provider
            //    && ul.ProviderIdentityKey == providerIdentityKey);

            return null; // userLogin?.User;
        }

        public async Task AddExternalProviderToUser(
            string subject,
            string provider,
            string providerIdentityKey)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(providerIdentityKey))
            {
                throw new ArgumentNullException(nameof(providerIdentityKey));
            }

            var user = await GetUserBySubjectAsync(subject);

            ///TODO: Enable when External Logins implemented
            //user.Logins.Add(new MUserLogin()
            //{
            //    Provider = provider,
            //    ProviderIdentityKey = providerIdentityKey
            //});
        }

        public User ProvisionUserFromExternalIdentity(
            string provider,
            string providerIdentityKey,
            IEnumerable<Claim> claims)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(providerIdentityKey))
            {
                throw new ArgumentNullException(nameof(providerIdentityKey));
            }

            var user = new User()
            {
                Active = true,
                Subject = Guid.NewGuid().ToString()
            };
            foreach (var claim in claims)
            {
                user.Claims.Add(new UserClaim()
                {
                    Type = claim.Type,
                    Value = claim.Value
                });
            }

            ///TODO: Enable when External Logins implemented
            //user.Logins.Add(new MUserLogin()
            //{
            //    Provider = provider,
            //    ProviderIdentityKey = providerIdentityKey
            //});

            _context.Users.Add(user);

            return user;
        }



        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }


        //public async Task<List<MUserListDto>> GetAllUserListDtosAsync()
        //{
        //    var users = await _context.Users
        //        .Include(u => u.Logins)
        //        .AsNoTracking().ToListAsync();
        //    return _mapper.Map<List<MUserListDto>>(users);
        //}

        public async Task<User> GetUserAsync(Guid id)
        {
            var query = _context.Users
                .Include(u => u.Claims)
                //.Include(u => u.Logins)
                //.Include(u => u.Secrets)
                //.Include(u => u.ExternalClaims)
                .Include(u => u.Roles)
                .AsQueryable();

            return await query.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            var query = _context.Users
                .Include(u => u.Claims)
                //.Include(u => u.Logins)
                //.Include(u => u.Secrets)
                //.Include(u => u.ExternalClaims)
                .Include(u => u.Roles)
                .AsQueryable();

            return await query.FirstOrDefaultAsync(u => u.Subject == GetUserId(claimsPrincipal));
        }

        public async Task<bool> CanSignInAsync(User user)
        {
            if (!user.Active)
            {
                return false;
            }

            return true;
        }

        public async Task<ClaimsPrincipal> CreateUserPrincipalAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var id = await GenerateClaimsAsync(user);
            return new ClaimsPrincipal(id);
        }

        

        protected virtual async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {

            var userName = user.UserName;
            var id = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, // REVIEW: Used to match Application scheme
                OpenIddictConstants.Claims.Name,
                OpenIddictConstants.Claims.Role);
            id.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, user.Subject));
            id.AddClaim(new Claim(OpenIddictConstants.Claims.Name, userName));
            var email = user.Email;
            if (!string.IsNullOrEmpty(email))
            {
                id.AddClaim(new Claim(OpenIddictConstants.Claims.Email, email));
            }


            /// TODO: add Userclaims when user class has claims property
            //if (UserManager.SupportsUserClaim)
            //{
            //    id.AddClaims(await UserManager.GetClaimsAsync(user));
            //}

            return id;
        }

        //public async Task<MUserDto> GetUserDtoAsync(Guid id)
        //{
        //    var user = await GetUserAsync(id);
        //    return _mapper.Map<MUserDto>(user);
        //}

        public async Task UpdateAsync(User userModel)
        {
            //var att = _context.Users.Attach(userModel);
            //att.State = EntityState.Modified;

            //var roleIds = userModel.Roles.Select(ur => ur.Id).ToList();
            //var availableRoles = DbContext.Roles.AsNoTracking().Where(r => roleIds.Contains(r.Id)).Select(r => r.Id).ToList();

            //userModel.Roles = userModel.Roles.Where(ur => availableRoles.Contains(ur.Id)).ToList();

            await _context.SaveChangesAsync();
            EventDispatcher.DispatchUpdatedEvent("WhoUser", userModel);
        }

        public async Task UpdateAsync(UserDto user)
        {

            var userInDb = await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Claims)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            var nuser = _mapper.From(user).EntityFromContext(_context).AdaptTo(userInDb);

            await _context.SaveChangesAsync();
            EventDispatcher.DispatchUpdatedEvent("WhoUser", userInDb);

        }

        //public async Task UpdateUserAsync(MUserDto userDto)
        //{

        //    var userInDB = await GetUserAsync(userDto.Id);
        //    //userDto.Logins = userInDB.Logins;
        //    _mapper.Map(userDto, userInDB);

        //    var roleIds = userDto.Roles.Select(ur => ur.Id).ToList();
        //    var availableRoles = _context.Roles.Where(r => roleIds.Contains(r.Id)).ToList();

        //    userInDB.Roles = availableRoles;

        //    await _context.SaveChangesAsync();
        //    EventDispatcher.DispatchUpdatedEvent("IDPUsers", _mapper.Map<MUserDto>(userInDB));
        //}

        public async Task DeleteAsync(params Guid[] id)
        {
            var users = await _context.Users.Where(u => id.Contains(u.Id)).ToListAsync();
            _context.Users.RemoveRange(users);
            await _context.SaveChangesAsync();
            EventDispatcher.DispatchDeletedEvent("WhoUser", users.Select(r => r.Id));
        }

       

        public string GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException(nameof(claimsPrincipal));
            }

            return claimsPrincipal.FindFirstValue(OpenIddictConstants.Claims.Subject);
        }


    }
}
