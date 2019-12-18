using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Neis.ProductKeyManager.Data
{
    /// <summary>
    /// Generic class for products
    /// </summary>
    public class GenericProduct : NotifiableBase
    {
        /// <summary>
        /// Constructor for the GenericProduct class
        /// </summary>
        public GenericProduct()
        {
            Keys = new ObservableCollection<GenericKey>();
        }

        #region Name
        /// <summary>
        /// Property name for the Name property
        /// </summary>
        public const string NamePropertyName = "Name";

        protected string _Name;

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("Name")]
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged(NamePropertyName);
                }
            }
        }
        #endregion

        #region Keys
        /// <summary>
        /// Property name for the Keys property
        /// </summary>
        public const string KeysPropertyName = "Keys";

        protected ObservableCollection<GenericKey> _Keys;

        /// <summary>
        /// Gets or sets Keys
        /// </summary>
        [System.Xml.Serialization.XmlElement("Key", Type = typeof(GenericKey))]
        public ObservableCollection<GenericKey> Keys
        {
            get
            {
                if (_Keys == null)
                {
                    _Keys = new ObservableCollection<GenericKey>();
                }
                return _Keys;
            }
            set
            {
                if (_Keys != value)
                {
                    _Keys = value;
                    NotifyPropertyChanged(KeysPropertyName);
                }
            }
        }
        #endregion
    }
}