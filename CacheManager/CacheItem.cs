using System;

namespace ShareLocation.CacheManager
{
    public class CacheItem<U>
    {
        public DateTime CreatedOn => DateTime.Now;
        public U Value { get; private set; }

        public CacheItem(U value)
        {
            this.Value = value;           
        }
    }
}
