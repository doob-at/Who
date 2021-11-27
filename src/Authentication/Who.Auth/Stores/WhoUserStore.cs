//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using doob.Who.Events;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using Who.Auth.Context;
//using Who.Auth.Entities;

//namespace Who.Auth.Stores
//{
//    public class WhoUserStore //: UserStore<WhoUser, WhoRole, AuthDbContext>
//    {
//        public AuthDbContext Context { get; }
//        public IdentityErrorDescriber ErrorDescriber { get; }
//        private readonly DataEventDispatcher _dataEventDispatcher;
//        private bool _disposed;

//        public WhoUserStore(AuthDbContext context, DataEventDispatcher dataEventDispatcher, IdentityErrorDescriber describer = null)
//        {
//            Context = context;
//            ErrorDescriber = describer;
//            _dataEventDispatcher = dataEventDispatcher;
//        }

//        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            Context.Add(user);
//            await SaveChanges(cancellationToken);
//            _dataEventDispatcher.DispatchCreatedEvent("WhoUser", user);

//            return IdentityResult.Success;
//        }

//        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
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

//            _dataEventDispatcher.DispatchUpdatedEvent("WhoUser", user);
//            return IdentityResult.Success;
//        }
        
//        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
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
//            _dataEventDispatcher.DispatchDeletedEvent("WhoUser", user.Id);
//            return IdentityResult.Success;
//        }


//        protected Task SaveChanges(CancellationToken cancellationToken)
//        {
//            return Context.SaveChangesAsync(cancellationToken);
//        }

       
//        public async Task<List<WhoRole>> GetRolesWithDetailsAsync(User user, CancellationToken cancellationToken = new CancellationToken())
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            var userId = user.Id;
//            var query = from userRole in Context.UserRoles
//                join role in Context.Roles on userRole.RoleId equals role.Id
//                where userRole.UserId.Equals(userId)
//                select role;

//            return await query.ToListAsync(cancellationToken);
//        }

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
//    }
//}
