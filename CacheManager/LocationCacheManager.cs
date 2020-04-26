using Engaze.Core.DataContract;
using System;
using System.Collections.Generic;

namespace ShareLocation.CacheManager
{
    public class LocationCacheManager : ILocationCacheManager
    {
        private Cache<Guid, Location> locationCache = new Cache<Guid, Location>();

        public void SaveLocation(Guid userId, Location location)
        {
            locationCache.Add(userId, location);
        }

        public Dictionary<Guid, Location> GetLocations(IEnumerable<Guid> userIds)
        {
            return locationCache.GetValues(userIds);
        }

        public void RemoveLocation(Guid userId)
        {
            locationCache.Remove(userId);
        }

        public void RemoveLocations(IEnumerable<Guid> userIds)
        {
            locationCache.Remove(userIds);
        }

        public void RemoveTillDate(DateTime dateInUtc)
        {
            locationCache.RemoveTillDate(dateInUtc);
        }
    }
}
