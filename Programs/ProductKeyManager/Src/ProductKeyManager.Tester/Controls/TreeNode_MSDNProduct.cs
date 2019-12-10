using System.Windows.Forms;

namespace Neis.ProductKeyManager.Tester.Controls
{
    /// <summary>
    /// Used for displaying MSDN Products as a <see cref="TreeNode"/>
    /// </summary>
    public class TreeNode_MSDNProduct : TreeNode
    {
        Data.Microsoft.Product _product;

        /// <summary>
        /// Constructor for the <see cref="TreeNode_MSDNProduct"/> class
        /// </summary>
        /// <param name="product"><see cref="Data.Microsoft.Product"/></param>
        public TreeNode_MSDNProduct(Data.Microsoft.Product product)
        {
            Product = product;
        }

        /// <summary>
        /// Gets or sets the Product
        /// </summary>
        public Data.Microsoft.Product Product
        {
            get { return _product; }
            set 
            { 
                _product = value;
                this.Text = _product.Name;

                this.Nodes.Clear();
                foreach(Data.Microsoft.Key key in _product.Keys)
                {
                    this.Nodes.Add(new TreeNode_MSDNKey(key));
                }
            }
        }
    }
}