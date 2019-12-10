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
        /// Raises the <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="property">Name of property that changed</param>
        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}