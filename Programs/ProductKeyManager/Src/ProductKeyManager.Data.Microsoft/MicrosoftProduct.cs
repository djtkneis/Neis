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
            _Keys = new ObservableCollection<MicrosoftKey>();
            _Keys.CollectionChanged += Keys_CollectionChanged;
        }

        /// <summary>
        /// Event for when the Keys collection has changed
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Arguments for this event</param>
        private void Keys_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var key = item as NotifiableBase;
                        if (key != null)
                        {
                            key.OnMarkForDeletion += Key_OnMarkForDeletion;
                        }
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var key = item as NotifiableBase;
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
            var key = obj as MicrosoftKey;
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

        protected ObservableCollection<MicrosoftKey> _Keys;

        /// <summary>
        /// Gets or sets Keys
        /// </summary>
        [System.Xml.Serialization.XmlElement("Key", Type = typeof(MicrosoftKey))]
        public ObservableCollection<MicrosoftKey> Keys
        {
            get { return _Keys; }
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