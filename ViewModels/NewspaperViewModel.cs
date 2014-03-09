using System;
using System.ComponentModel;
using System.Windows.Input;
using Models;
using ViewModels.Commands;

namespace ViewModels
{
    public class NewspaperViewModel : ViewModelBase
    {
        #region Instance fields

        private readonly Newspaper _model;
        private readonly IRepository<Newspaper, Guid> _repository;

        #endregion

        #region Constructors

        public NewspaperViewModel(Newspaper model, IRepository<Newspaper, Guid> repository) : base(model)
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

        public ICommand SaveCommand { get; set; }

        #endregion

        #region Class Members

        private void DeleteCommand_Handler(object parameter)
        {
            var paper = (Newspaper) parameter;
            _repository.Delete(paper);
        }

        private void SaveCommand_Handler(object parameter)
        {
            var paper = (Newspaper) parameter;
            _repository.Save(paper);
            _model.DbStatus = DbModificationState.Unchanged;
        }

        #endregion
    }
}