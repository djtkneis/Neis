using System;
using Neis.ProductKeyManager.Data;

namespace Neis.ProductKeyManager.Data.Microsoft
{
    /// <summary>
    /// Represents a single instance of a key
    /// </summary>
    /// <remarks><see cref="MicrosoftKey"/> is formatted as:
    /// <![CDATA[ <Key ID="" Type="" ClaimedDate="">XXXXXXXXXXXXX</Key> ]]> 
    /// </remarks>
    public class MicrosoftKey : NotifiableBase
    {
        /// <summary>
        /// Constructor for the Key class
        /// </summary>
        public MicrosoftKey()
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

        #region ID
        /// <summary>
        /// Property name for the ID property
        /// </summary>
        public const string IDPropertyName = "ID";

        private string _ID;

        /// <summary>
        /// Gets or sets ID
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("ID")]
        public string ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    NotifyPropertyChanged(IDPropertyName);
                }
            }
        }
        #endregion

        #region Type
        /// <summary>
        /// Property name for the Type property
        /// </summary>
        public const string TypePropertyName = "Type";

        private string _Type;

        /// <summary>
        /// Gets or sets Type
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("Type")]
        public string Type
        {
            get { return _Type; }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    NotifyPropertyChanged(TypePropertyName);
                }
            }
        }
        #endregion

        #region ClaimedDate
        /// <summary>
        /// Property name for the ClaimedDate property
        /// </summary>
        public const string ClaimedDatePropertyName = "ClaimedDate";

        private DateTime? _ClaimedDate;

        /// <summary>
        /// Gets or sets ClaimedDate
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public DateTime? ClaimedDate
        {
            get { return _ClaimedDate; }
            set
            {
                if (_ClaimedDate != value)
                {
                    _ClaimedDate = value;
                    NotifyPropertyChanged(ClaimedDatePropertyName);
                }
            }
        }

        /// <summary>
        /// Used for serializing <see cref="ClaimedDate"/> since it is nullable
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("ClaimedDate")]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.ComponentModel.Browsable(false)]
        public string Xml_ClaimedDate 
        {
            get { return ClaimedDate.HasValue ? ClaimedDate.Value.ToString() : null; }
            set 
            {
                DateTime val = new DateTime();
                if (DateTime.TryParse(value, out val))
                {
                    ClaimedDate = val;
                }
            }
        }
        /// <summary>
        /// Used by the <see cref="XmlSerializer"/> to determine whether or not <see cref="ClaimedDate"/> has been specified
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.ComponentModel.Browsable(false)]
        public bool Xml_ClaimedDateSpecified { get { return ClaimedDate.HasValue; } }
        #endregion
    }
}