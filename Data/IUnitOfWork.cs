namespace Data
{
    internal interface IUnitOfWork
    {
        #region Class Members

        void SaveChanges();

        #endregion
    }
}