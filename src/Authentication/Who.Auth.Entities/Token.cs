using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace Who.Auth.Entities
{
    public class Token : OpenIddictEntityFrameworkCoreToken<Guid, Client, Authorization> { }
}