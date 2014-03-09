﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using Models;

namespace ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanging, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Constructors

        private readonly EntityBase _model;

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
            string propertyName = memberExpression.Member.Name;

            OnPropertyChangedCaller(propertyName);
        }

        protected void OnPropertyChanging<T>(Expression<Func<T>> exp)
        {
            var memberExpression = (MemberExpression) exp.Body;
            string propertyName = memberExpression.Member.Name;

            OnPropertyChangingCaller(propertyName);
        }

        private void OnPropertyChangedCaller(string propertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPropertyChangingCaller(string propertyName)
        {
            if(PropertyChanging != null)
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

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }

        public bool HasErrors
        {
            get { return IsEntityValid; }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}