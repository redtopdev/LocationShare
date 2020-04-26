using Engaze.Core.DataContract;
using ShareLocation.CacheManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ShareLocation.Service
{
    public class LocationManager : ILocationManager
    {
        private ILocationCacheManager cacheManager;

        private ConcurrentDictionary<Guid, List<Guid>> eventUserList = new ConcurrentDictionary<Guid, List<Guid>>();       

        public LocationManager(ILocationCacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        public void SetLocation(Guid userId, Location location)
        {

            cacheManager.SaveLocation(userId, location);
        }

        public Dictionary<Guid, Location> GetLocations(Guid userId,  Guid eventId)
        {
            if(eventUserList[eventId]?.Any(eventUserid=> eventUserid == eventId) ?? false)
            {
                return null;
            }

            return cacheManager.GetLocations(eventUserList[eventId]);            
        }

        public void ClearEventAndUserLocations(Guid eventId)
        {
            List<Guid> userIds;
            if(!eventUserList.TryRemove(eventId, out userIds))
            {
                return;
            }

            List<Guid> userIdsToRemove = new List<Guid>();
            userIds.ForEach(userId =>
            {
                if(!eventUserList.Values.Any(userList=> userList.Contains(userId)))
                {
                    userIdsToRemove.Add(userId);
                }
            });

            cacheManager.RemoveLocations(userIdsToRemove);
        }

        public void ClearUserLocations(Guid userId, Guid eventId)
        {
            if (eventUserList[eventId].Remove(userId))
            {
                if (!eventUserList.Values.Any(userList => userList.Contains(userId)))
                {
                    cacheManager.RemoveLocation(userId);
                }               
            }
        }       
    }
}
