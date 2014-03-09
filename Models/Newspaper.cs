using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Models
{
    [Serializable,
    DataContract(Name="Newspaper"),
    KnownType(typeof(Advertisement)),
    KnownType(typeof(Newspaper))]
    public class Newspaper : EntityBase, ISerializable
    {
        #region Instance fields
        [DataMember]
        private readonly ObservableCollection<Advertisement> _advertisements = new ObservableCollection<Advertisement>();
        [DataMember]
        private string _name = "";
        [DataMember]
        private Guid _uKey;

        #endregion

        #region Constructors

        public Newspaper()
        {
            Name = "";
            UKey = Guid.NewGuid();
            DbStatus = DbModificationState.Added;
        }

        public Newspaper(Guid uKey, string name)
        {
            UKey = uKey;
            _name = name;
            DbStatus = DbModificationState.Unchanged;
        }

        public Newspaper(SerializationInfo info, StreamingContext context)
        {
            _uKey = (Guid)info.GetValue("UKey", typeof(Guid));
            Name = (string)info.GetValue("Name", typeof(string));
            DbStatus = DbModificationState.Unchanged;
        }

        #endregion

        #region Properties
        [DataMember]
        public ObservableCollection<Advertisement> Advertisements
        {
            get { return _advertisements; }
            set
            {
                _advertisements.Clear();

                if (value == null)
                    return;

                foreach (Advertisement ad in value)
                    _advertisements.Add(ad);
            }
        }
 
        [DataMember]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                OnPropertyChanging(() => Name);
                _name = value ?? "";

                if(string.IsNullOrEmpty(_name))
                    SetError(() => Name, "Newspaper name cannot be empty.");

                OnPropertyChanged(() => Name);

            }
        }
        [DataMember]
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

        #region Class Members

        public void AddAdvertisement(Advertisement advertisement)
        {
            if (advertisement == null)
                return;

            if (!_advertisements.Contains(advertisement))
            {
                advertisement.AddNewspaper(this);
                _advertisements.Add(advertisement);
            }
        }

        public void AddAdvertisements(List<Advertisement> advertisements)
        {
            if (advertisements == null)
                return;

            advertisements.ForEach(AddAdvertisement);
        }

        public void RemoveAdvertisement(Advertisement advertisement)
        {
            if (advertisement == null)
                return;

            _advertisements.Remove(advertisement);
        }

        public void RemoveAdvertisements(List<Advertisement> advertisements)
        {
            if (advertisements == null)
                return;

            advertisements.ForEach(RemoveAdvertisement);
        }

        #endregion

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UKey", UKey, typeof(Guid));
            info.AddValue("Name", Name, typeof(string));
        }
    }
}