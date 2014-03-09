using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Models;

namespace Data
{
    public class NewspaperRepository : IRepository<Newspaper, Guid>
    {
        #region Instance fields

        private readonly NewspaperCache _cache;
        private readonly ISerializer<NewspaperCache> _serializer;

        #endregion

        #region Constructors

        public NewspaperRepository(NewspaperCache cache, ISerializer<NewspaperCache> serializer)
        {
            _cache = cache;
            _serializer = serializer;
        }

        #endregion

        #region IRepository<Newspaper,Guid> Properties and Members

        public void Delete(Newspaper entity)
        {
            if (_cache.ContainsKey(entity.UKey))
                _cache.Remove(entity.UKey);
        }

        public IEnumerable<Newspaper> Find(Func<Newspaper, bool> predicate)
        {
            var papers = (_cache.Values ?? new List<Newspaper>()).ToList();

            if (predicate != null)
                return papers.Where<Newspaper>(predicate).ToList();

            return (IEnumerable<Newspaper>) null;
        }

        public Newspaper Get(Guid id)
        {
            return _cache[id];
        }

        public void Save(Newspaper entity)
        {
            if (entity.DbStatus == DbModificationState.Unchanged)
                return;

            if(entity.DbStatus != DbModificationState.Deleted)
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