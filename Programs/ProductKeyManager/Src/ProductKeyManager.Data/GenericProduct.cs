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
            _Keys = new ObservableCollection<GenericKey>();
            _Keys.CollectionChanged += Keys_CollectionChanged;
        }

        /// <summary>
        /// Event for when the Keys collection has changed
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Arguments for this event</param>
        private void Keys_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var key = item as GenericKey;
                        if (key != null)
                        {
                            key.OnMarkForDeletion += Key_OnMarkForDeletion;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var key = item as GenericKey;
                        if (key != null)
                        {
                            key.OnMarkForDeletion -= Key_OnMarkForDeletion;
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Event that occurs when a Key has been marked for deletion
        /// </summary>
        /// <param name="obj">Key that has been marked for deletion</param>
        private void Key_OnMarkForDeletion(NotifiableBase obj)
        {
            var key = obj as GenericKey;
            if (key != null)
            {
                _Keys.Remove(key);
            }
        }

        /// <summary>
        /// Mark the current object as clean (aka not dirty)
        /// </summary>
        public override void MarkNotDirty()
        {
            foreach (var k in Keys)
            {
                k.MarkNotDirty();
            }
            base.MarkNotDirty();
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
            get { return _Keys; }
            set
            {
                if (_Keys != value)
                {
                    if (_Keys != null)
                    {
                        Keys.CollectionChanged -= Keys_CollectionChanged;
                    }

                    _Keys = value;
                    NotifyPropertyChanged(KeysPropertyName);

                    if (_Keys != null)
                    {
                        _Keys.CollectionChanged += Keys_CollectionChanged;

                        foreach (var key in _Keys)
                        {
                            key.OnMarkForDeletion += Key_OnMarkForDeletion;
                        }
                    }
                }
            }
        }
        #endregion
    }
}