namespace Neis.ProductKeyManager.Tester.Controls
{
    partial class MSDNKeysControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._selectFileControl = new Neis.ProductKeyManager.Tester.Controls.SelectFileControl();
            this._btnLoad = new System.Windows.Forms.Button();
            this._tvContents = new System.Windows.Forms.TreeView();
            this._splitMain = new System.Windows.Forms.SplitContainer();
            this._propSelected = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this._splitMain)).BeginInit();
            this._splitMain.Panel1.SuspendLayout();
            this._splitMain.Panel2.SuspendLayout();
            this._splitMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // _selectFileControl
            // 
            this._selectFileControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._selectFileControl.FileName = "";
            this._selectFileControl.Filter = "XML Files(*.xml)|*.xml";
            this._selectFileControl.Location = new System.Drawing.Point(4, 4);
            this._selectFileControl.Name = "_selectFileControl";
            this._selectFileControl.Size = new System.Drawing.Size(551, 28);
            this._selectFileControl.TabIndex = 0;
            // 
            // _btnLoad
            // 
            this._btnLoad.Location = new System.Drawing.Point(4, 33);
            this._btnLoad.Name = "_btnLoad";
            this._btnLoad.Size = new System.Drawing.Size(75, 23);
            this._btnLoad.TabIndex = 2;
            this._btnLoad.Text = "&Load";
            this._btnLoad.UseVisualStyleBackColor = true;
            this._btnLoad.Click += new System.EventHandler(this._btnLoad_Click);
            // 
            // _tvContents
            // 
            this._tvContents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tvContents.Location = new System.Drawing.Point(3, 0);
            this._tvContents.Name = "_tvContents";
            this._tvContents.Size = new System.Drawing.Size(150, 371);
            this._tvContents.TabIndex = 4;
            this._tvContents.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._tvContents_AfterSelect);
            // 
            // _splitMain
            // 
            this._splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._splitMain.Location = new System.Drawing.Point(85, 33);
            this._splitMain.Name = "_splitMain";
            // 
            // _splitMain.Panel1
            // 
            this._splitMain.Panel1.Controls.Add(this._tvContents);
            // 
            // _splitMain.Panel2
            // 
            this._splitMain.Panel2.Controls.Add(this._propSelected);
            this._splitMain.Size = new System.Drawing.Size(470, 374);
            this._splitMain.SplitterDistance = 156;
            this._splitMain.TabIndex = 5;
            // 
            // _propSelected
            // 
            this._propSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._propSelected.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this._propSelected.Location = new System.Drawing.Point(3, 3);
            this._propSelected.Name = "_propSelected";
            this._propSelected.Size = new System.Drawing.Size(304, 368);
            this._propSelected.TabIndex = 0;
            // 
            // MSDNKeysControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._splitMain);
            this.Controls.Add(this._btnLoad);
            this.Controls.Add(this._selectFileControl);
            this.Name = "MSDNKeysControl";
            this.Size = new System.Drawing.Size(558, 410);
            this._splitMain.Panel1.ResumeLayout(false);
            this._splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitMain)).EndInit();
            this._splitMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SelectFileControl _selectFileControl;
        private System.Windows.Forms.Button _btnLoad;
        private System.Windows.Forms.TreeView _tvContents;
        private System.Windows.Forms.SplitContainer _splitMain;
        private System.Windows.Forms.PropertyGrid _propSelected;


    }
}
