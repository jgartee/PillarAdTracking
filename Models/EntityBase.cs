using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Models
{
    [Flags]
    public enum DbModificationState
    {
        Unchanged = 1,
        Added = 2,
        Modified = 4,
        Deleted = 8
    };

    public abstract class EntityBase : INotifyPropertyChanging, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Instance fields

        private readonly Dictionary<string, List<string>> _errorDictionary =
                new Dictionary<string, List<string>>();

        private DbModificationState _dbStatus;

        #endregion

        #region Constructors

        protected EntityBase()
        {
            DbStatus = DbModificationState.Unchanged;
        }

        #endregion

        #region Properties

        public DbModificationState DbStatus
        {
            get { return _dbStatus; }
            set
            {
                if(_dbStatus == value)
                    return;

                _dbStatus = value;
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

        public bool IsEntityValid
        {
            get { return !HasErrors; }
        }

        public bool IsUnchanged
        {
            get { return DbStatus == DbModificationState.Unchanged; }
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

            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanging<T>(Expression<Func<T>> expression)
        {
            var memberExpression = (MemberExpression) expression.Body;
            string propertyName = memberExpression.Member.Name;

            if(PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        #endregion

        protected void SetError<T>(Expression<Func<T>> propertyExpression, string message)
        {
            List<string> errorCollection;

            _errorDictionary.TryGetValue(ExtractPropertyName(propertyExpression), out errorCollection);

            if(errorCollection != null && errorCollection.Any())
            {
                if(!errorCollection.Contains(message))
                    errorCollection.Add(message);
            }
            else
            {
                errorCollection = new List<string>{message};
                _errorDictionary.Add(ExtractPropertyName(propertyExpression), errorCollection);
            }
        }

        protected void ClearError<T>(Expression<Func<T>> propertyExpression)
        {
            List<string> errorCollection;

            _errorDictionary.TryGetValue(ExtractPropertyName(propertyExpression), out errorCollection);

            if(errorCollection != null && errorCollection.Any())
                errorCollection.Remove(ExtractPropertyName(propertyExpression));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorDictionary[propertyName];
        }

        public bool HasErrors
        {
            get { return _errorDictionary.Values.Any(); }
        }

        protected void NotifyErrorsChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if(ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(ExtractPropertyName(propertyExpression)));
            }
        }

        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("not member access expression.");

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentException("expression not property.");

            var getMethod = property.GetGetMethod(true);
            if (getMethod == null)
                throw new ArgumentException("static expression.");

            return memberExpression.Member.Name;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}