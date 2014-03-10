using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Models
{
    public class Advertisement : EntityBase, ISerializable
    {
        #region Instance fields

        private readonly ObservableCollection<Newspaper> _newspapers = new ObservableCollection<Newspaper>();
        private string _name;
        private string _text;
        private Guid _uKey;

        #endregion

        #region Constructors

        public Advertisement()
        {
            UKey = Guid.NewGuid();
            DbStatus = DbModificationState.Added;
            Name = Text = "";
        }

        public Advertisement(Guid uKey, string name, string text)
        {
            UKey = uKey;
            _name = name;
            _text = text;
            DbStatus = DbModificationState.Unchanged;
        }

        public Advertisement(SerializationInfo info, StreamingContext context)
        {
            _uKey = (Guid) info.GetValue("UKey", typeof(Guid));
            Name = (string) info.GetValue("Name", typeof(string));
            Text = (string) info.GetValue("Text", typeof(string));
            DbStatus = DbModificationState.Unchanged;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set
            {
                if(_name == value)
                    return;

                OnPropertyChanging(() => Name);
                _name = value ?? "";

                if(string.IsNullOrEmpty(_name))
                    SetError(() => Name, "Advertisement name cannot be empty.");
                else
                    ClearError(() => Name);

                OnPropertyChanged(() => Name);
                NotifyErrorsChanged(() => Name);
            }
        }

        public ObservableCollection<Newspaper> Newspapers
        {
            get { return _newspapers; }
            set
            {
                _newspapers.Clear();

                if(value == null)
                    return;

                foreach(Newspaper paper in value)
                    AddNewspaper(paper);
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if(_text == value)
                    return;

                OnPropertyChanging(() => Text);
                _text = value;

                if(string.IsNullOrEmpty(_text))
                    SetError(() => Text, "The Advertisement text cannot be empty.");
                else
                    ClearError(() => Text);

                OnPropertyChanged(() => Text);
            }
        }

        public Guid UKey
        {
            get { return _uKey; }
            set
            {
                if(_uKey == value)
                    return;

                OnPropertyChanging(() => UKey);
                _uKey = value;
                OnPropertyChanged(() => UKey);
            }
        }

        #endregion

        #region ISerializable Properties and Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UKey", UKey, typeof(Guid));
            info.AddValue("Name", Name, typeof(string));
            info.AddValue("Text", Text, typeof(string));
        }

        #endregion

        #region Class Members

        public void AddNewspaper(Newspaper paper)
        {
            if(paper == null)
                return;

            _newspapers.Add(paper);
        }

        public void AddNewspapers(List<Newspaper> newspapers)
        {
            if(newspapers == null)
                return;

            newspapers.ForEach(AddNewspaper);
        }

        #endregion
    }
}