using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Models;

namespace ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constructors

        private EntityBase _model;
        protected ViewModelBase(EntityBase model)
        {
            _model = model;
            model.PropertyChanging += modelBase_PropertyChanging;
            model.PropertyChanged += modelBase_PropertyChanged;
        }

        #endregion

        public bool IsEntityValid
        {
            get { return _model.IsEntityValid; }
        }

        #region INotifyPropertyChanged Properties and Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region INotifyPropertyChanging Properties and Members

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Class Members

        protected void OnPropertyChanged<T>(Expression<Func<T>> exp)
        {
            var memberExpression = (MemberExpression) exp.Body;
            var propertyName = memberExpression.Member.Name;

            OnPropertyChangedCaller(propertyName);
        }

        protected void OnPropertyChanging<T>(Expression<Func<T>> exp)
        {
            var memberExpression = (MemberExpression) exp.Body;
            var propertyName = memberExpression.Member.Name;

            OnPropertyChangingCaller(propertyName);
        }

        private void OnPropertyChangedCaller(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPropertyChangingCaller(string propertyName)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        private void modelBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChangedCaller(e.PropertyName);
        }

        private void modelBase_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            OnPropertyChangingCaller(e.PropertyName);
        }

        #endregion
    }
}