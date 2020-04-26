using Engaze.Core.DataContract;
using System;
using System.Collections.Generic;

namespace ShareLocation.CacheManager
{
    public interface ILocationCacheManager
    {
        void SaveLocation(Guid userId, Location location);

        Dictionary<Guid, Location> GetLocations(IEnumerable<Guid> userIds);

        void RemoveLocation(Guid userId);

        void RemoveLocations(IEnumerable<Guid> userIds);

        void RemoveTillDate(DateTime dateInUtc);

    }
}
