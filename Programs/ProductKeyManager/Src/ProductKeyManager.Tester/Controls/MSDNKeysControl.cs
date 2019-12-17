using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neis.ProductKeyManager.Tester.Controls
{
    /// <summary>
    /// Control used for displaying MSDN Keys
    /// </summary>
    public partial class MSDNKeysControl : UserControl
    {
        /// <summary>
        /// Constructor for the <see cref="MSDNKeysControl"/> class
        /// </summary>
        public MSDNKeysControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when the Load button is clicked
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Arguments for this event</param>
        private void _btnLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectFileControl.FileName))
            {
                MessageBox.Show(this.FindForm(), "You must select a file to load first!", "Cannot Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Data.Microsoft.MicrosoftKeyFile keyFile = Data.Microsoft.MicrosoftKeyFile.Load(_selectFileControl.FileName);

            _tvContents.Nodes.Clear();
            foreach (Data.Microsoft.MicrosoftProduct product in keyFile.Products)
            {
                _tvContents.Nodes.Add(new TreeNode_MSDNProduct(product));
            }
        }
        /// <summary>
        /// Occurs afer a <see cref="TreeViewNode"/> has been selected
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Arguments for this event</param>
        private void _tvContents_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var productNode = e.Node as TreeNode_MSDNProduct;
            if (productNode != null)
            {
                _propSelected.SelectedObject = productNode.Product;
            }
            else
            {
                var keyNode = e.Node as TreeNode_MSDNKey;
                if (keyNode != null)
                {
                    _propSelected.SelectedObject = keyNode.Key;
                }
            }
        }
    }
}