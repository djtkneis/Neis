using System.Collections.Generic;

namespace Neis.ProductKeyManager.Data.Microsoft
{
    /// <summary>
    /// Represents a single instance of a key file
    /// </summary>
    [System.Xml.Serialization.XmlRoot("YourKey")]
    public class KeyFile : BaseKeyFile
    {
        /// <summary>
        /// Gets or sets the list of <see cref="Product"/> objects
        /// </summary>
        [System.Xml.Serialization.XmlElement("Product_Key", Type = typeof(Product))]
        public override List<BaseProduct> Products
        {
            get { return base.Products; }
            set { base.Products = value; }
        }

        /// <summary>
        /// Loads the contents of a given file into a new <see cref="KeyFile"/> object
        /// </summary>
        /// <param name="filename">Name of file to load</param>
        /// <returns><see cref="KeyFile"/> object containing contents of the file</returns>
        public static KeyFile Load(string filename)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(KeyFile));
            using (System.IO.FileStream stream = System.IO.File.OpenRead(filename))
            {
                var retVal = serializer.Deserialize(stream);
                return retVal as KeyFile;
            }
        }

    }
}