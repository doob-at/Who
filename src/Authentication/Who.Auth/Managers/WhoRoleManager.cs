//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using Who.Auth.Entities;
//using Who.Auth.Stores;

//namespace Who.Auth.Managers
//{
//    public class WhoRoleManager: RoleManager<WhoRole>
//    {
        
//        private readonly WhoRoleStore _whoRoleStore;

//        public WhoRoleManager(IRoleStore<WhoRole> store, IEnumerable<IRoleValidator<WhoRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<WhoRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
//        {
//            _whoRoleStore = store as WhoRoleStore;
//        }


//        public override async Task<string> GetRoleNameAsync(WhoRole role)
//        {

//            return await base.GetRoleNameAsync(role);
//        }
//    }
//}
