using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neis.ProductKeyManager.Data
{
    /// <summary>
    /// Generic class for extra data content
    /// </summary>
    public class ExtraData : NotifiableBase
    {
        #region Value
        /// <summary>
        /// Property name for the Value property
        /// </summary>
        public const string TypePropertyName = "Type";

        protected string _Type;

        /// <summary>
        /// Gets or sets Type
        /// </summary>
        [System.Xml.Serialization.XmlAttribute]
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

        #region Data
        /// <summary>
        /// Property name for the Data property
        /// </summary>
        public const string DataPropertyName = "Data";

        protected string _Data;

        /// <summary>
        /// Gets or sets Data
        /// </summary>
        [System.Xml.Serialization.XmlText]
        public string Data
        {
            get { return _Data; }
            set
            {
                if (_Data != value)
                {
                    _Data = value;
                    NotifyPropertyChanged(DataPropertyName);
                }
            }
        }
        #endregion
    }
}
