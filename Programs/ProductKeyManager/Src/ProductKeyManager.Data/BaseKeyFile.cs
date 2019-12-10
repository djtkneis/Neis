using System.Collections.Generic;

namespace Neis.ProductKeyManager.Data
{
    /// <summary>
    /// Base class for key files
    /// </summary>
    public abstract class BaseKeyFile : NotifiableBase
    {
        #region Products
        /// <summary>
        /// Property name for the Products property
        /// </summary>
        public const string ProductsPropertyName = "Products";

        private List<BaseProduct> _Products;

        /// <summary>
        /// Gets or sets the list of <see cref="BaseProduct"/> objects
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public virtual List<BaseProduct> Products
        {
            get 
            {
                if (_Products == null)
                {
                    _Products = new List<BaseProduct>();
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