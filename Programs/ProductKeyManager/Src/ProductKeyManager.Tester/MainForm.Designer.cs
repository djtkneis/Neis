namespace Neis.ProductKeyManager.Tester
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._tcMain = new System.Windows.Forms.TabControl();
            this._tabMSDNKeys = new System.Windows.Forms.TabPage();
            this._btnExit = new System.Windows.Forms.Button();
            this._msdnKeysControl = new Neis.ProductKeyManager.Tester.Controls.MSDNKeysControl();
            this._tcMain.SuspendLayout();
            this._tabMSDNKeys.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tcMain
            // 
            this._tcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tcMain.Controls.Add(this._tabMSDNKeys);
            this._tcMain.Location = new System.Drawing.Point(13, 13);
            this._tcMain.Name = "_tcMain";
            this._tcMain.SelectedIndex = 0;
            this._tcMain.Size = new System.Drawing.Size(514, 337);
            this._tcMain.TabIndex = 0;
            // 
            // _tabMSDNKeys
            // 
            this._tabMSDNKeys.Controls.Add(this._msdnKeysControl);
            this._tabMSDNKeys.Location = new System.Drawing.Point(4, 22);
            this._tabMSDNKeys.Name = "_tabMSDNKeys";
            this._tabMSDNKeys.Padding = new System.Windows.Forms.Padding(3);
            this._tabMSDNKeys.Size = new System.Drawing.Size(506, 311);
            this._tabMSDNKeys.TabIndex = 0;
            this._tabMSDNKeys.Text = "MSDN Keys";
            this._tabMSDNKeys.UseVisualStyleBackColor = true;
            // 
            // _btnExit
            // 
            this._btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnExit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnExit.Location = new System.Drawing.Point(452, 356);
            this._btnExit.Name = "_btnExit";
            this._btnExit.Size = new System.Drawing.Size(75, 23);
            this._btnExit.TabIndex = 1;
            this._btnExit.Text = "E&xit";
            this._btnExit.UseVisualStyleBackColor = true;
            this._btnExit.Click += new System.EventHandler(this._btnExit_Click);
            // 
            // _msdnKeysControl
            // 
            this._msdnKeysControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._msdnKeysControl.Location = new System.Drawing.Point(0, 0);
            this._msdnKeysControl.Margin = new System.Windows.Forms.Padding(0);
            this._msdnKeysControl.Name = "_msdnKeysControl";
            this._msdnKeysControl.Size = new System.Drawing.Size(506, 311);
            this._msdnKeysControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 391);
            this.Controls.Add(this._btnExit);
            this.Controls.Add(this._tcMain);
            this.Name = "MainForm";
            this.Text = "Product Key Manager - Tester";
            this._tcMain.ResumeLayout(false);
            this._tabMSDNKeys.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl _tcMain;
        private System.Windows.Forms.TabPage _tabMSDNKeys;
        private System.Windows.Forms.Button _btnExit;
        private Controls.MSDNKeysControl _msdnKeysControl;
    }
}

