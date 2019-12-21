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
                        var p = item as GenericProduct;
                        if (p != null)
                        {
                            p.OnMarkForDeletion += Product_OnMarkForDeletion;
                        }
                    }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var p = item as GenericProduct;
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
            var product = obj as GenericProduct;
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

        private ObservableCollection<GenericProduct> _Products;

        /// <summary>
        /// Gets or sets the list of <see cref="GenericProduct"/> objects
        /// </summary>
        [System.Xml.Serialization.XmlElement("Product", Type = typeof(GenericProduct))]
        public ObservableCollection<GenericProduct> Products
        {
            get { return _Products; }
            set
            {
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