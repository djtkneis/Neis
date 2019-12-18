using System.Collections.Generic;
using Neis.ProductKeyManager.Data;
using System.Collections.ObjectModel;

namespace Neis.ProductKeyManager.Data.Microsoft
{
    /// <summary>
    /// Represents a single instance of a key file
    /// </summary>
    [System.Xml.Serialization.XmlRoot("YourKey")]
    public class MicrosoftKeyFile : NotifiableBase
    {
        /// <summary>
        /// Constructor for the MicrosoftKeyFile class
        /// </summary>
        public MicrosoftKeyFile()
        {
            Products = new ObservableCollection<MicrosoftProduct>();
        }

        #region Products
        /// <summary>
        /// Property name for the Products property
        /// </summary>
        public const string ProductsPropertyName = "Products";

        private ObservableCollection<MicrosoftProduct> _Products;

        /// <summary>
        /// Gets or sets the list of <see cref="IProduct"/> objects
        /// </summary>
        [System.Xml.Serialization.XmlElement("Product_Key", Type = typeof(MicrosoftProduct))]
        public ObservableCollection<MicrosoftProduct> Products
        {
            get
            {
                if (_Products == null)
                {
                    _Products = new ObservableCollection<MicrosoftProduct>();
                }
                return _Products;
            }
            set
            {
                if (_Products != value)
                {
                    _Products = value;
                    NotifyPropertyChanged(ProductsPropertyName);
                }
            }
        }
        #endregion
    }
}