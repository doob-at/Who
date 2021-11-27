using System;
using System.Reactive.Linq;
using doob.SignalARRR.Common.Attributes;
using doob.SignalARRR.Server;
using doob.Who.Events;
using MapsterMapper;
using Who.Auth.Entities;
using Who.Auth.Entities.DTO;

namespace doob.Who.Hubs.Methods
{
    [MessageName("WhoUser")]
    public class WhoUserServerMethodsHub : ServerMethods<UIHub>
    {
        private readonly IMapper _mapper;
        public DataEventDispatcher EventDispatcher { get; }


        public WhoUserServerMethodsHub(DataEventDispatcher eventDispatcher, IMapper mapper)
        {
            _mapper = mapper;
            EventDispatcher = eventDispatcher;
        }


        public IObservable<DataEvent> Subscribe()
        {
            return EventDispatcher.Notifications.Where(ev => ev.Subject == "WhoUser").Select(ev =>
            {
                if (ev.Payload is User)
                {
                    ev.Payload = _mapper.Map<UserListDto>(ev.Payload);
                }
               
                return ev;
            });
        }
    }
}
