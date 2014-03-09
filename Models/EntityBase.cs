using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Models
{
    [Flags]
    public enum DbModificationState
    {
        Unchanged=1,
        Added=2,
        Modified=4,
        Deleted=8
    };

    public abstract class EntityBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Instance fields

        private DbModificationState _dbStatus;
        private bool _isEntityValid;

        #endregion

        #region Constructors

        protected EntityBase()
        {
            DbStatus = DbModificationState.Unchanged;
        }

        #endregion

        #region Properties

        public bool IsEntityValid
        {
            get { return _isEntityValid; }
            set
            {
                if (_isEntityValid == value)
                    return;

                OnPropertyChanging(()=>IsEntityValid);
                _isEntityValid = value;
                OnPropertyChanged(()=>IsEntityValid);
            }
        }

        public bool IsAdded
        {
            get { return DbStatus == DbModificationState.Added; }
        }

        public bool IsChanged
        {
            get { return DbStatus == DbModificationState.Modified; }
        }

        public bool IsDeleted
        {
            get { return DbStatus == DbModificationState.Deleted; }
        }

        public bool IsUnchanged
        {
            get { return DbStatus == DbModificationState.Unchanged; }
        }

        public DbModificationState DbStatus
        {
            get { return _dbStatus; }
            set
            {
                if (_dbStatus == value)
                    return;

                _dbStatus = value;
            }
        }

        #endregion

        #region INotifyPropertyChanged Properties and Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region INotifyPropertyChanging Properties and Members

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Class Members


        protected void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            var memberExpression = (MemberExpression) expression.Body;
            string propertyName = memberExpression.Member.Name;

            DbStatus = DbStatus == DbModificationState.Unchanged ? DbModificationState.Modified : DbStatus;

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanging<T>(Expression<Func<T>> expression)
        {
            var memberExpression = (MemberExpression) expression.Body;
            string propertyName = memberExpression.Member.Name;

            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        #endregion
    }
}