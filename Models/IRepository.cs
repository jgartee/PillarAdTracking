﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Models
{
    public interface IRepository<T, in TKey> where T : class
    {
        #region Class Members

        void Delete(T entity);
        IEnumerable<T> Find(Func<T, bool> predicate);
        T Get(TKey id);
        void Save(T entity);

        #endregion
    }
}