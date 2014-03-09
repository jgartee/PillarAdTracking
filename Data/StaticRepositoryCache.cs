using System;
using System.Collections.Generic;
using Models;

namespace Data
{
    public static class StaticRepositoryCache
    {
        #region Constants and Fields

        private static Dictionary<Guid, Advertisement> _advertisementsDictionary = new Dictionary<Guid, Advertisement>();
        private static Dictionary<Guid, Newspaper> _newspaperDictionary = new Dictionary<Guid, Newspaper>();

        #endregion

        #region Properties

        public static Dictionary<Guid, Advertisement> Advertisements
        {
            get { return _advertisementsDictionary; }
        }

        public static Dictionary<Guid, Newspaper> Newspapers
        {
            get { return _newspaperDictionary; }
        }

        #endregion

        #region Class Members

        public static void SerializeAllValues()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}