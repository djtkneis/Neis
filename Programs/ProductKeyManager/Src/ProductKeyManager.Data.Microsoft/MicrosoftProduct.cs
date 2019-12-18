using System.Collections.Generic;
using Neis.ProductKeyManager.Data;
using System.Collections.ObjectModel;

namespace Neis.ProductKeyManager.Data.Microsoft
{
    /// <summary>
    /// Represents a single instance of a Product
    /// </summary>
    /// <remarks><see cref="MicrosoftProduct"/> is formatted as: 
    /// <![CDATA[
    /// <Product_Key Name="">
    ///     <Key ID="" Type="" ClaimedDate="">XXXXXXXXXXXXX</Key>
    ///     <Key ID="" Type="" ClaimedDate="">XXXXXXXXXXXXX</Key>
    /// </Product_Key>
    /// ]]>
    /// </remarks>
    public class MicrosoftProduct : NotifiableBase
    {
        public MicrosoftProduct()
        {
            Keys = new ObservableCollection<MicrosoftKey>();
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

        protected ObservableCollection<MicrosoftKey> _Keys;

        /// <summary>
        /// Gets or sets Keys
        /// </summary>
        [System.Xml.Serialization.XmlElement("Key", Type = typeof(MicrosoftKey))]
        public ObservableCollection<MicrosoftKey> Keys
        {
            get
            {
                if (_Keys == null)
                {
                    _Keys = new ObservableCollection<MicrosoftKey>();
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