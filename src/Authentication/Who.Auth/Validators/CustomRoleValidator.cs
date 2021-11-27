
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Who.Auth.Entities;

namespace Who.Auth.Validators
{

    public class CustomRoleValidator : RoleValidator<Role>
    {

        public CustomRoleValidator(IdentityErrorDescriber errors = null)
        {
            Describer = errors ?? new IdentityErrorDescriber();
        }

        private IdentityErrorDescriber Describer { get; set; }


        public virtual async Task<IdentityResult> ValidateAsync(RoleManager<Role> manager, Role role)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            var errors = new List<IdentityError>();
            await ValidateRoleName(manager, role, errors);
            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }

        private async Task ValidateRoleName(RoleManager<Role> manager, Role role,
            ICollection<IdentityError> errors)
        {
            var roleName = await manager.GetRoleNameAsync(role);
            if (string.IsNullOrWhiteSpace(roleName))
            {
                errors.Add(Describer.InvalidRoleName(roleName));
            }
            else
            {
                var owner = await manager.Roles.FirstOrDefaultAsync(r => r.Name == role.Name && r.ClientId == role.ClientId);
                //var owner = await manager.FindByNameAsync(roleName);
                if (owner != null && !string.Equals(await manager.GetRoleIdAsync(owner), await manager.GetRoleIdAsync(role)))
                {
                    errors.Add(Describer.DuplicateRoleName(roleName));
                }
            }
        }
    }
}