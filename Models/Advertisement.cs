using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Models
{
    [DataContract]
    public class Advertisement : EntityBase, ISerializable
    {
        #region Instance fields

        private bool _isNameValid;
        private bool _isTextValid;
        [DataMember]
        private string _name;
        [DataMember]
        private ObservableCollection<Newspaper> _newspapers = new ObservableCollection<Newspaper>();
        [DataMember]
        private string _text;
        [DataMember]
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
            _uKey = (Guid) info.GetValue("UKey", typeof (Guid));
            Name = (string) info.GetValue("Name", typeof (string));
            Text = (string) info.GetValue("Text", typeof (string));
            DbStatus = DbModificationState.Unchanged;
            IsEntityValid = IsNameValid = IsTextValid = true;
        }

        #endregion

        #region Properties
        [XmlIgnore]

        public bool IsNameValid
        {
            get { return _isNameValid; }
            private set
            {
                if (_isNameValid == value)
                    return;

                OnPropertyChanging(() => IsNameValid);
                _isNameValid = value;
                OnPropertyChanged(() => IsNameValid);

                IsValid = value;
            }
        }

        [XmlIgnore]
        public bool IsTextValid
        {
            get { return _isTextValid; }
            set
            {
                if (_isTextValid == value)
                    return;

                OnPropertyChanging(() => IsTextValid);
                _isTextValid = value;
                OnPropertyChanged(() => IsTextValid);

                IsValid = value;
            }
        }

        [XmlIgnore]
        public bool IsValid
        {
            get { return IsEntityValid; }
            private set
            {
                if (value == IsEntityValid)
                    return;

                if (IsNameValid && IsTextValid)
                {
                    OnPropertyChanging(() => IsValid);
                    IsEntityValid = true;
                    OnPropertyChanged(() => IsValid);
                }
                else if (!value)
                {
                    OnPropertyChanging(() => IsValid);
                    IsEntityValid = false;
                    OnPropertyChanged(() => IsValid);
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                OnPropertyChanging(() => Name);
                _name = value ?? "";
                OnPropertyChanged(() => Name);

                IsNameValid = !string.IsNullOrEmpty(_name);
            }
        }

        public ObservableCollection<Newspaper> Newspapers
        {
            get { return _newspapers; }
            set
            {
                _newspapers.Clear();

                if (value == null)
                    return;

                foreach (var paper in value)
                    AddNewspaper(paper);
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value)
                    return;

                OnPropertyChanging(() => Text);
                _text = value;
                OnPropertyChanged(() => Text);

                IsTextValid = !string.IsNullOrEmpty(_text);
            }
        }

        public Guid UKey
        {
            get { return _uKey; }
            set
            {
                if (_uKey == value)
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
            info.AddValue("UKey", UKey, typeof (Guid));
            info.AddValue("Name", Name, typeof (string));
            info.AddValue("Text", Text, typeof (string));
        }

        #endregion

        #region Class Members

        public void AddNewspaper(Newspaper paper)
        {
            if (paper == null)
                return;

            _newspapers.Add(paper);
        }

        public void AddNewspapers(List<Newspaper> newspapers)
        {
            if (newspapers == null)
                return;

            newspapers.ForEach(AddNewspaper);
        }

        #endregion
    }
}