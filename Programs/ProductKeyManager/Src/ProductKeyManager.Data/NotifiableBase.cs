using System;
using System.ComponentModel;

namespace Neis.ProductKeyManager.Data
{
    /// <summary>
    /// Base class for any class that raises the <see cref="PropertyChanged"/> event
    /// </summary>
    public class NotifiableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Event for when a property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Event for when the current object has been marked for deletion
        /// </summary>
        public event Action<NotifiableBase> OnMarkForDeletion;
        /// <summary>
        /// Event for when the current object is marked as dirty
        /// </summary>
        public event Action<NotifiableBase> OnIsDirty;

        /// <summary>
        /// Mark the current object as dirty
        /// </summary>
        protected void MarkDirty()
        {
            _IsDirty = true;
            if (OnIsDirty != null)
            {
                OnIsDirty.Invoke(this);
            }
        }
        /// <summary>
        /// Mark the current object as clean (aka not dirty)
        /// </summary>
        public virtual void MarkNotDirty()
        {
            _IsDirty = false;
        }
        /// <summary>
        /// Marks the current object for deletion
        /// </summary>
        public void MarkForDeletion()
        {
            if (OnMarkForDeletion != null)
            {
                OnMarkForDeletion.Invoke(this);
            }
        }
        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="property">Name of property that changed</param>
        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
            }
            MarkDirty();
        }

        
        #region IsDirty
        /// <summary>
        /// Property name for the IsDirty property
        /// </summary>
        public const string IsDirtyPropertyName = "IsDirty";

        private bool _IsDirty;

        /// <summary>
        /// Gets or sets IsDirty
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public bool IsDirty
        {
            get { return _IsDirty; }
        }
        #endregion

        #region IsMarkedDeleted
        /// <summary>
        /// Property name for the IsMarkedDeleted property
        /// </summary>
        public const string IsMarkedDeletedPropertyName = "IsMarkedDeleted";

        private bool _IsMarkedDeleted;

        /// <summary>
        /// Gets or sets IsMarkedDeleted
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public bool IsMarkedDeleted
        {
            get { return _IsMarkedDeleted; }
            set
            {
                if (_IsMarkedDeleted != value)
                {
                    _IsMarkedDeleted = value;
                    NotifyPropertyChanged(IsMarkedDeletedPropertyName);

                    if (value)
                    {
                        MarkForDeletion();
                    }
                }
            }
        }
        #endregion
                
    }
}