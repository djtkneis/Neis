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
    public partial class SelectFileControl : UserControl
    {
        /// <summary>
        /// Constructor for the <see cref="SelectFileControl"/> class
        /// </summary>
        public SelectFileControl()
        {
            InitializeComponent();
            Filter = "XML Files(*.xml)|*.xml";
        }

        /// <summary>
        /// Gets or sets the filter
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string FileName
        {
            get { return _tbFileName.Text; }
            set { _tbFileName.Text = value; }
        }

        private void _btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = Filter;

            if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                FileName = dlg.FileName;
            }
        }
    }
}