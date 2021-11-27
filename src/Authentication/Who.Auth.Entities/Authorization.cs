using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace Who.Auth.Entities
{
    public class Authorization : OpenIddictEntityFrameworkCoreAuthorization<Guid, Client, Token> { }
}