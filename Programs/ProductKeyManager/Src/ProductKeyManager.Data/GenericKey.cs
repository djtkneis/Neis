
namespace Neis.ProductKeyManager.Data
{
    /// <summary>
    /// Generic class for keys
    /// </summary>
    public class GenericKey : NotifiableBase
    {
        /// <summary>
        /// Constructor for the GenericKey class
        /// </summary>
        public GenericKey()
        {

        }

        #region Value
        /// <summary>
        /// Property name for the Value property
        /// </summary>
        public const string ValuePropertyName = "Value";

        protected string _Value;

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        [System.Xml.Serialization.XmlText]
        public string Value
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

        #region ExtraData
        /// <summary>
        /// Property name for the ExtraData property
        /// </summary>
        public const string ExtraDataPropertyName = "ExtraData";

        protected ExtraData _ExtraData;

        /// <summary>
        /// Gets or sets ExtraData
        /// </summary>
        [System.Xml.Serialization.XmlElement(ElementName="ExtraData", IsNullable=true)]
        public ExtraData ExtraData
        {
            get { return _ExtraData; }
            set
            {
                if (_ExtraData != value)
                {
                    _ExtraData = value;
                    NotifyPropertyChanged(ExtraDataPropertyName);
                }
            }
        }
        #endregion
    }
}