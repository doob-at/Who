using System;
using Who.Auth.Entities;

namespace doob.Who.Providers
{
    public interface IExternalUserFactory
    {
        Guid GetId();

        User BuildUser();

        void UpdateClaims(User existingUser);

    }
}