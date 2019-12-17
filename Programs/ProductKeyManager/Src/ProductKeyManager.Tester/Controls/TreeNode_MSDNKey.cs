﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neis.ProductKeyManager.Tester.Controls
{
    /// <summary>
    /// Used for displaying an MSDN Key in a <see cref="TreeNode"/>
    /// </summary>
    public class TreeNode_MSDNKey : TreeNode
    {
        Data.Microsoft.MicrosoftKey _key;

        /// <summary>
        /// Constructor for the <see cref="TreeNode_MSDNKey"/> class
        /// </summary>
        /// <param name="key"><see cref="Data.Microsoft.MicrosoftKey"/></param>
        public TreeNode_MSDNKey(Data.Microsoft.MicrosoftKey key)
        {
            Key = key;
        }

        /// <summary>
        /// Gets or sets ths key
        /// </summary>
        public Data.Microsoft.MicrosoftKey Key
        {
            get { return _key; }
            set
            {
                _key = value;
                this.Text = _key.Value;
            }
        }
    }
}