using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Neis.ProductKeyManager.Data
{
    /// <summary>
    /// Generic class for key files
    /// </summary>
    public class GenericKeyFile : NotifiableBase
    {
        /// <summary>
        /// Constructor for the GenericKeyFile class
        /// </summary>
        public GenericKeyFile()
        {
            _Products = new ObservableCollection<GenericProduct>();
            _Products.CollectionChanged += _Products_CollectionChanged;
        }

        private void _Products_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.NotifyPropertyChanged(ProductsPropertyName);
        }

        #region Products
        /// <summary>
        /// Property name for the Products property
        /// </summary>
        public const string ProductsPropertyName = "Products";

        private ObservableCollection<GenericProduct> _Products;

        /// <summary>
        /// Gets or sets the list of <see cref="GenericProduct"/> objects
        /// </summary>
        [System.Xml.Serialization.XmlElement("Product", Type = typeof(GenericProduct))]
        public ObservableCollection<GenericProduct> Products
        {
            get 
            {
                if (_Products == null)
                {
                    _Products = new ObservableCollection<GenericProduct>();
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