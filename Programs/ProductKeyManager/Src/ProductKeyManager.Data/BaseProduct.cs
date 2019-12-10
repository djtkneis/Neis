using System.Collections.Generic;

namespace Neis.ProductKeyManager.Data
{
    /// <summary>
    /// Base class for products
    /// </summary>
    public abstract class BaseProduct : NotifiableBase
    {
        #region Name
        /// <summary>
        /// Property name for the Name property
        /// </summary>
        public const string NamePropertyName = "Name";

        protected string _Name;

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public virtual string Name
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

        protected List<BaseKey> _Keys;

        /// <summary>
        /// Gets or sets Keys
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public virtual List<BaseKey> Keys
        {
            get
            {
                if (_Keys == null)
                {
                    _Keys = new List<BaseKey>();
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