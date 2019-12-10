
namespace Neis.ProductKeyManager.Data
{
    /// <summary>
    /// Base class for keys
    /// </summary>
    public abstract class BaseKey : NotifiableBase
    {
        #region Value
        /// <summary>
        /// Property name for the Value property
        /// </summary>
        public const string ValuePropertyName = "Value";

        protected string _Value;

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public virtual string Value
        {
            get { return _Value; }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    NotifyPropertyChanged(ValuePropertyName);
                }
            }
        }
        #endregion
    }
}