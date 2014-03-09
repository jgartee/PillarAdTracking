using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Models;
using ViewModels.Commands;

namespace ViewModels
{
    public class AdvertisementViewModel : ViewModelBase
    {
        #region Instance fields

        private readonly IRepository<Advertisement, Guid> _repository;
        private readonly Advertisement _model;

        #endregion

        #region Constructors

        public AdvertisementViewModel(Advertisement model, IRepository<Advertisement, Guid> repository) : base(model)
        {
            _model = model;
            _repository = repository;
            SaveCommand = new RelayCommand(new Action<object>(SaveCommand_Handler));
            DeleteCommand = new RelayCommand(new Action<object>(DeleteCommand_Handler));
        }

        #endregion

        #region Properties

        public ICommand DeleteCommand { get; set; }

        public bool IsUnchanged
        {
            get { return _model.IsUnchanged; }
        }

        public string Name
        {
            get { return _model.Name; }
            set { _model.Name = value; }
        }

        public ObservableCollection<Newspaper> Newspapers
        {
            get { return _model.Newspapers; }
            set { _model.Newspapers = value; }
        }

        public ICommand SaveCommand { get; private set; }

        public string Text
        {
            get { return _model.Text; }
            set { _model.Text = value; }
        }

        #endregion

        #region Class Members

        public void AddNewspaper(Newspaper paperModel)
        {
            if (paperModel == null)
                return;
        }

        private void DeleteCommand_Handler(object parameter)
        {
            var ad = (Advertisement) parameter;

            _repository.Delete(ad);
        }

        private void SaveCommand_Handler(object parameter)
        {
            var ad = (Advertisement) parameter;
            _repository.Save(ad);
            _model.DbStatus = DbModificationState.Unchanged;

        }

        #endregion
    }
}