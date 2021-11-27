using System;
using System.Reactive.Linq;
using doob.SignalARRR.Common.Attributes;
using doob.SignalARRR.Server;
using doob.Who.Events;

namespace doob.Who.Hubs.Methods
{
    [MessageName("AuthProviders")]
    public class AuthProviderServerMethodsHub : ServerMethods<UIHub>
    {
        public DataEventDispatcher EventDispatcher { get; }



        public AuthProviderServerMethodsHub(DataEventDispatcher eventDispatcher)
        {
            EventDispatcher = eventDispatcher;

        }


        public IObservable<DataEvent> Subscribe()
        {
            return EventDispatcher.Notifications.Where(ev => ev.Subject == "AuthProviders");
        }
    }
}
