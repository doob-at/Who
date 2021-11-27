using System;
using System.Collections.Generic;
using System.Security.Claims;
using OpenIddict.Abstractions;

namespace doob.Who.Models
{
    public class LoginResultModel
    {
        public string ReturnUrl { get; set; }
        public Status Status { get; set; }
        public List<LoginError> Errors { get; set; } = new List<LoginError>();
       

        public LoginResultModel WithStatus(Status status)
        {
            this.Status = status;
            return this;
        }

        public LoginResultModel WithError(string message, object data = null)
        {
            var err = new LoginError();
            err.Message = message;
            Status = Status.Error;
            return WithError(err);
        }

        public LoginResultModel WithError(LoginError error)
        {
            if (Errors == null)
            {
                Errors = new List<LoginError>();
            }
            Errors.Add(error);
            return this;
        }

        public LoginResultModel WithError(IEnumerable<LoginError> errors)
        {
            foreach (var loginError in errors)
            {
                WithError(loginError);
            }
            return this;
        }

    }

    public enum Status
    {
        Error,
        MustConfirm,
        Confirmed,
        Ok,
        IsLockedOut,
        RequiresTwoFactor
    }

}
