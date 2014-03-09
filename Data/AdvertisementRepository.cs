using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Models;

namespace Data
{
    public class AdvertisementRepository : IRepository<Advertisement, Guid>
    {
        #region Instance fields

        private readonly AdvertisementCache _cache;
        private readonly ISerializer<AdvertisementCache> _serializer;

        #endregion

        #region Constructors

        public AdvertisementRepository(AdvertisementCache cache, ISerializer<AdvertisementCache> serializer)
        {
            _cache = cache;
            _serializer = serializer;
        }

        #endregion

        #region IRepository<Newspaper,Guid> Properties and Members

        public void Delete(Advertisement entity)
        {
            if (_cache.ContainsKey(entity.UKey))
                _cache.Remove(entity.UKey);
        }

        public IEnumerable<Advertisement> Find(Func<Advertisement, bool> predicate)
        {
            var papers = (_cache.Values ?? new List<Advertisement>()).ToList();

            if (predicate != null)
                return papers.Where<Advertisement>(predicate).ToList();

            return (IEnumerable<Advertisement>)null;
        }

        public Advertisement Get(Guid id)
        {
            return _cache[id];
        }

        public void Save(Advertisement entity)
        {
            if (entity.DbStatus == DbModificationState.Unchanged)
                return;

            if (entity.DbStatus != DbModificationState.Deleted)
            {
                if (_cache.ContainsKey(entity.UKey))
                    _cache[entity.UKey] = entity;
                else
                    _cache.Add(entity.UKey, entity);
            }
            else
            {
                if (_cache.ContainsKey(entity.UKey))
                    _cache.Remove(entity.UKey);
            }

            _serializer.SaveCache(_cache);
        }

        #endregion


    }
}