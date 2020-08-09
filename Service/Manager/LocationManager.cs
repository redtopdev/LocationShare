using Engaze.Core.DataContract;
using ShareLocation.CacheManager;
using ShareLocation.Service.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareLocation.Service
{
    public class LocationManager : ILocationManager
    {
        private readonly ILocationCacheManager cacheManager;
        private readonly IEventQueryClient eventQueryClient;

        private ConcurrentDictionary<Guid, List<Guid>> eventUserList = new ConcurrentDictionary<Guid, List<Guid>>();

        public LocationManager(ILocationCacheManager cacheManager, IEventQueryClient eventQueryClient)
        {
            this.cacheManager = cacheManager;
            this.eventQueryClient = eventQueryClient;
        }

        public void SetLocation(Guid userId, Location location)
        {

            cacheManager.SaveLocation(userId, location);
        }

        public async Task<string> ValidateLocationRequest(Guid userId, Guid eventId)
        {
            if (!(eventUserList.TryGetValue(eventId, out var userIds) && userIds.ToList().Any(eventUserid => eventUserid == userId)))
            {
                var result = await eventQueryClient.GetEventsByUserIdAsync(userId);
                if (!(result?.Any() ?? false))
                {
                    return $"No running event found for the user {userId}";
                }

                result.ToList().ForEach(evnt =>
                {
                    eventUserList[evnt.EventId] = evnt.Participants.Select(p => p.UserId).ToList();
                });
                //eventUserList[eventId] = result.Select(evnt => evnt.Participants.ToList());
                if (!result.Any(evnt => evnt.EventId == eventId))
                {
                    return $"user {userId} is not an active participant for the event {eventId} ";
                }
            }

            return string.Empty;
        }

        public Dictionary<Guid, Location> GetLocations(Guid eventId)
        {
            return cacheManager.GetLocations(eventUserList[eventId]);
        }

        public void ClearEventAndUserLocations(Guid eventId)
        {
            List<Guid> userIds;
            if (!eventUserList.TryRemove(eventId, out userIds))
            {
                return;
            }

            List<Guid> userIdsToRemove = new List<Guid>();
            userIds.ForEach(userId =>
            {
                if (!eventUserList.Values.Any(userList => userList.Contains(userId)))
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