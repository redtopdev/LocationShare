using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ShareLocation.CacheManager
{
    public class Cache<T, U>
    {
        private ConcurrentDictionary<T, CacheItem<U>> cacheItems = new ConcurrentDictionary<T, CacheItem<U>>();

        public void Add(T key, U value)
        {
            cacheItems[key] = new CacheItem<U>(value);
        }        

        public bool TryGetValue(T key, out U value)
        {
            value = default(U);
            CacheItem<U> item;

            bool itemFound = cacheItems.TryGetValue(key, out item);

            if (itemFound)
            {
                value = item.Value;
            }
            return itemFound;
        }

        public Dictionary<T, U> GetValues(IEnumerable<T> keys)
        {
            Dictionary<T, U> dictValues = new Dictionary<T, U>();

            keys.ToList().
                ForEach(key =>
                {
                    U item;

                    if (TryGetValue(key, out item))
                    {
                        dictValues[key] = item;
                    }
                });

            return dictValues;
        }

        public bool Remove(T key)
        {
            return cacheItems.TryRemove(key, out CacheItem<U> item);
        }

        public void Remove(IEnumerable<T> keys)
        {
            keys.ToList().ForEach(key => cacheItems.TryRemove(key, out CacheItem<U> item));           
        }

        public void RemoveTillDate(DateTime timeFromInUtc)
        {
            cacheItems.Keys.Where(key => cacheItems[key].CreatedOn <= timeFromInUtc)?.ToList().
                ForEach(key => cacheItems.TryRemove(key, out var value));
        }
    }
}
