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
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Who.Auth.Context;
//using Who.Auth.Entities;

//namespace Who.Auth.Stores
//{
//    public class WhoRoleStore: RoleStore<WhoRole, AuthDbContext>
//    {
//        private readonly DataEventDispatcher _dataEventDispatcher;

//        public WhoRoleStore(AuthDbContext context, DataEventDispatcher dataEventDispatcher, IdentityErrorDescriber describer = null) : base(context, describer)
//        {
//            _dataEventDispatcher = dataEventDispatcher;
//        }

//        public override async Task<IdentityResult> CreateAsync(WhoRole role, CancellationToken cancellationToken = new CancellationToken())
//        {
//            var result = await base.CreateAsync(role, cancellationToken);
//            if (result.Succeeded)
//            {
//                _dataEventDispatcher.DispatchCreatedEvent("WhoRole", role);
//            }

//            return result;
//        }

//        public override async Task<IdentityResult> UpdateAsync(WhoRole role, CancellationToken cancellationToken = new CancellationToken())
//        {
//            var result = await base.UpdateAsync(role, cancellationToken);
//            if (result.Succeeded)
//            {
//                _dataEventDispatcher.DispatchUpdatedEvent("WhoRole", role);
//            }

//            return result;
//        }

//        public override async Task<IdentityResult> DeleteAsync(WhoRole role, CancellationToken cancellationToken = new CancellationToken())
//        {
//            var result = await base.DeleteAsync(role, cancellationToken);
//            if (result.Succeeded)
//            {
//                _dataEventDispatcher.DispatchDeletedEvent("WhoRole", role.Id);
//            }

//            return result;
//        }


//        //public override Task<string> GetRoleNameAsync(WhoRole role, CancellationToken cancellationToken = new CancellationToken())
//        //{
//        //    cancellationToken.ThrowIfCancellationRequested();
//        //    ThrowIfDisposed();
//        //    if (role == null)
//        //    {
//        //        throw new ArgumentNullException(nameof(role));
//        //    }

//        //    var clientId = role.ClientId.ToNull() ?? Guid.Empty.ToString();
//        //    return Task.FromResult($"{clientId}|{role.Name}");
//        //}
//    }
//}
