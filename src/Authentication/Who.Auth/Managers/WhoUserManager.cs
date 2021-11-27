//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using doob.Reflectensions.Common;
//using doob.Who.Events;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Who.Auth.Context;
//using Who.Auth.Entities;
//using Who.Auth.Stores;

//namespace Who.Auth.Managers
//{
//    public class WhoUserManager
//    {
//        public AuthDbContext Context { get; }
//        public IdentityErrorDescriber ErrorDescriber { get; }
//        private readonly DataEventDispatcher _dataEventDispatcher;
//        private bool _disposed;

//        public ILogger Logger { get; set; }
//        public IPasswordHasher<User> PasswordHasher { get; set; }
//        public IList<IPasswordValidator<User>> PasswordValidators { get; } = new List<IPasswordValidator<User>>();

//        public WhoUserManager(AuthDbContext context, DataEventDispatcher dataEventDispatcher, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber describer, IServiceProvider services, ILogger<WhoUserManager> logger)
//        {
//            Context = context;
//            ErrorDescriber = describer;
//            _dataEventDispatcher = dataEventDispatcher;
//            PasswordHasher = passwordHasher;
//            Logger = logger;

//            if (passwordValidators != null)
//            {
//                foreach (var v in passwordValidators)
//                {
//                    PasswordValidators.Add(v);
//                }
//            }
//        }

//        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            Context.Add(user);
//            await SaveChanges(cancellationToken);
//            _dataEventDispatcher.DispatchCreatedEvent("User", user);

//            return IdentityResult.Success;
//        }

//        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = default)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            Context.Attach(user);
//            user.ConcurrencyStamp = Guid.NewGuid().ToString();
//            Context.Update(user);
//            try
//            {
//                await SaveChanges(cancellationToken);
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
//            }

//            _dataEventDispatcher.DispatchUpdatedEvent("User", user);
//            return IdentityResult.Success;
//        }

//        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = default)
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            Context.Remove(user);
//            try
//            {
//                await SaveChanges(cancellationToken);
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
//            }
//            _dataEventDispatcher.DispatchDeletedEvent("User", user.Id);
//            return IdentityResult.Success;
//        }


//        protected Task SaveChanges(CancellationToken cancellationToken)
//        {
//            return Context.SaveChangesAsync(cancellationToken);
//        }


//        //public async Task<List<WhoRole>> GetRolesWithDetailsAsync(User user, CancellationToken cancellationToken = default)
//        //{
//        //    cancellationToken.ThrowIfCancellationRequested();
//        //    ThrowIfDisposed();
//        //    if (user == null)
//        //    {
//        //        throw new ArgumentNullException(nameof(user));
//        //    }
//        //    var userId = user.Id;
//        //    var query = from userRole in Context.UserRoles
//        //                join role in Context.Roles on userRole.RoleId equals role.Id
//        //                where userRole.UserId.Equals(userId)
//        //                select role;

//        //    return await query.ToListAsync(cancellationToken);
//        //}

//        //public virtual async Task<bool> CheckPasswordAsync(User user, string password)
//        //{
//        //    ThrowIfDisposed();
//        //    if (user == null)
//        //    {
//        //        return false;
//        //    }

//        //    var result = await VerifyPasswordAsync(user, password);
//        //    if (result == PasswordVerificationResult.SuccessRehashNeeded)
//        //    {
//        //        await UpdatePasswordHash(user, password, validatePassword: false);
//        //        await UpdateUserAsync(user);
//        //    }

//        //    var success = result != PasswordVerificationResult.Failed;
//        //    if (!success)
//        //    {
//        //        Logger.LogWarning(0, "Invalid password for user.");
//        //    }
//        //    return success;
//        //}

//        //private async Task<IdentityResult> UpdatePasswordHash(User user, string newPassword, bool validatePassword = true)
//        //{
//        //    if (validatePassword)
//        //    {
//        //        var validate = await ValidatePasswordAsync(user, newPassword);
//        //        if (!validate.Succeeded)
//        //        {
//        //            return validate;
//        //        }
//        //    }
//        //    var hash = newPassword != null ? PasswordHasher.HashPassword(user, newPassword) : null;
//        //    await SetPasswordHashAsync(user, hash, CancellationToken);
//        //    await UpdateSecurityStampInternal(user);
//        //    return IdentityResult.Success;
//        //}

//        //protected async Task<IdentityResult> ValidatePasswordAsync(User user, string password)
//        //{
//        //    var errors = new List<IdentityError>();
//        //    var isValid = true;
//        //    foreach (var v in PasswordValidators)
//        //    {
//        //        var result = await v.ValidateAsync(this, user, password);
//        //        if (!result.Succeeded)
//        //        {
//        //            if (result.Errors.Any())
//        //            {
//        //                errors.AddRange(result.Errors);
//        //            }

//        //            isValid = false;
//        //        }
//        //    }
//        //    if (!isValid)
//        //    {
//        //        Logger.LogWarning(14, "User password validation failed: {errors}.", string.Join(";", errors.Select(e => e.Code)));
//        //        return IdentityResult.Failed(errors.ToArray());
//        //    }
//        //    return IdentityResult.Success;
//        //}

//        //protected virtual async Task<PasswordVerificationResult> VerifyPasswordAsync(User user, string password)
//        //{
//        //    var hash = await store.GetPasswordHashAsync(user, CancellationToken);
//        //    if (hash == null)
//        //    {
//        //        return PasswordVerificationResult.Failed;
//        //    }
//        //    return PasswordHasher.VerifyHashedPassword(user, hash, password);
//        //}

//        protected void ThrowIfDisposed()
//        {
//            if (_disposed)
//            {
//                throw new ObjectDisposedException(GetType().Name);
//            }
//        }

//        public void Dispose()
//        {
//            _disposed = true;
//        }

//        public async Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
//        {
//            return await Context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email, cancellationToken);
//        }
//    }
//}
