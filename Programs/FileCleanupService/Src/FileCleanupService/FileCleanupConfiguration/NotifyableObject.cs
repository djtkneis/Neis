using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Neis.FileCleanup.Configuration
{
    /// <summary>
    /// Base class for all objects that raise the <see cref="PropertyChanged"/> event
    /// </summary>
    [DataContract]
    public abstract class NotifyableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Property name for the <see cref="IsDirty"/> property
        /// </summary>
        public const string IsDirtyPropertyName = "IsDirty";

        private bool _isDirty = false;

        /// <summary>
        /// Gets or sets a value indicating whether or not the current object is dirty
        /// </summary>
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    NotifyPropertyChanged(IsDirtyPropertyName);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="propName">Name of property that has changed</param>
        protected void NotifyPropertyChanged(string propName)
        {
            if (propName != IsDirtyPropertyName)
            {
                IsDirty = true;
            }

            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }

        /// <summary>
        /// Event for when a property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}