using System;
using doob.Who.Ldap.Helpers;

namespace doob.Who.Ldap
{
    
        public static class ActionExtensions
        {
            public static T InvokeAction<T>(this Action<T> action, T instance = default) => ActionHelpers.InvokeAction(action, instance);
        }
    
}
