using Engaze.Core.DataContract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShareLocation.Service
{
    public interface ILocationManager
    {
        void SetLocation(Guid userId, Location location);

        Task<Dictionary<Guid, Location>> GetLocations(Guid userId, Guid eventId);

        void ClearEventAndUserLocations(Guid eventId);

        void ClearUserLocations(Guid userId, Guid eventId);        
        
    }
}
