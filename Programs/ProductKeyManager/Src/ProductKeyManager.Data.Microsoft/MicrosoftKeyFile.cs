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
            _Products = new ObservableCollection<MicrosoftProduct>();
            _Products.CollectionChanged += Products_CollectionChanged;
        }

        /// <summary>
        /// Event for when the Products collection has changed
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments for this event</param>
        private void Products_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.NotifyPropertyChanged(ProductsPropertyName);

            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var p = item as NotifiableBase;
                        if (p != null)
                        {
                            p.OnMarkForDeletion += Product_OnMarkForDeletion;
                        }
                    }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var p = item as NotifiableBase;
                        if (p != null)
                        {
                            p.OnMarkForDeletion += Product_OnMarkForDeletion;
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Event for when a product has been marked for deletion
        /// </summary>
        /// <param name="obj">Product that has been marked for deletion</param>
        private void Product_OnMarkForDeletion(NotifiableBase obj)
        {
            var product = obj as MicrosoftProduct;
            if (product != null)
            {
                Products.Remove(product);
            }
        }

        /// <summary>
        /// Mark the current object as clean (aka not dirty)
        /// </summary>
        public override void MarkNotDirty()
        {
            foreach (var p in Products)
            {
                p.MarkNotDirty();
            }
            base.MarkNotDirty();
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
            get { return _Products; }
            set            {
                if (_Products != value)
                {
                    if (_Products != null)
                    {
                        _Products.CollectionChanged -= Products_CollectionChanged;
                    }

                    _Products = value;
                    NotifyPropertyChanged(ProductsPropertyName);

                    if (_Products != null)
                    {
                        _Products.CollectionChanged += Products_CollectionChanged;
                        foreach (var p in _Products)
                        {
                            p.OnMarkForDeletion += Product_OnMarkForDeletion;
                        }
                    }
                }
            }
        }
        #endregion
    }
}