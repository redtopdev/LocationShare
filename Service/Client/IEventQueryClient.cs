using Engaze.Core.DataContract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShareLocation.Service.Client
{
    public interface IEventQueryClient
    {
        public Task<IEnumerable<Event>> GetEventsByUserIdAsync(Guid userId);

    }
}
