using System;
using Who.Auth.Entities;

namespace Who.Auth
{
    internal static class IdpDefaultIdentifier
    {
        public static Guid IdpClient { get; } = new Guid("00000001-0001-0000-0000-000000000000");

        public static Guid Role_IdentityServer_Administrators { get; } = new Guid("00000002-0001-0000-0000-000000000000");



        public static Guid Scope_OpenID_Id { get; } = new Guid("00000003-0001-0000-0000-000000000000");
        public static Guid Scope_Roles_Id { get; } = new Guid("00000003-0002-0000-0000-000000000000");
        public static Guid Scope_MiddlerAppApi_Id { get; } = new Guid("00000003-0003-0000-0000-000000000000");

        public static Guid Resource_MiddlerApi_Id { get; } = new Guid("00000004-0000-0000-0000-000000000000");
        public static Guid Resource_IdpApi_Id { get; } = new Guid("00000004-0001-0000-0000-000000000000");

    }

    internal static class IdpDefaultResources
    {

        public static Role Role_Idp_Administrator { get; } = new Role()
        {
            Id = IdpDefaultIdentifier.Role_IdentityServer_Administrators,
            //BuiltIn = true,
            Name = "Administrators",
            //Description = "BUILTIN Administrator Role",
            //DisplayName = "who Administrators",

        };


       

    }
}
