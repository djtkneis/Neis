using System.Collections.Generic;

namespace Neis.ProductKeyManager.Data.Microsoft
{
    /// <summary>
    /// Represents a single instance of a Product
    /// </summary>
    /// <remarks><see cref="Product"/> is formatted as: 
    /// <![CDATA[
    /// <Product_Key Name="">
    ///     <Key ID="" Type="" ClaimedDate="">XXXXXXXXXXXXX</Key>
    ///     <Key ID="" Type="" ClaimedDate="">XXXXXXXXXXXXX</Key>
    /// </Product_Key>
    /// ]]>
    /// </remarks>
    public class Product : BaseProduct
    {
        #region Name
        /// <summary>
        /// Gets or sets Name
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("Name")]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
        #endregion

        #region Keys
        /// <summary>
        /// Gets or sets the list of <see cref="Key"/> objects
        /// </summary>
        [System.Xml.Serialization.XmlElement("Key", Type = typeof(Key))]
        public override List<BaseKey> Keys
        {
            get { return base.Keys; }
            set { base.Keys = value; }
        }
        #endregion
    }
}