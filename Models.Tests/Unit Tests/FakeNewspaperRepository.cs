using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Models;

namespace Model.Tests.UnitTests
{
    public class FakeNewspaperRepository : IRepository<Newspaper, Guid>
    {
        #region IRepository<Newspaper,Guid> Properties and Members

        public void Delete(Newspaper paper)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Newspaper> Find(Func<Newspaper, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Newspaper> Find(Expression<Func<Newspaper, bool>> query)
        {
            throw new NotImplementedException();
        }

        public Newspaper Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(Newspaper entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Class Members

        public Newspaper Read(Guid uKey)
        {
            throw new NotImplementedException();
        }

        public void Update(Newspaper paper)
        {
            throw new NotImplementedException();
        }

        public void Write(Newspaper paper)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}