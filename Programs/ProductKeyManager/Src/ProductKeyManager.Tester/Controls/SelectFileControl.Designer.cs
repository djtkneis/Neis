namespace Neis.ProductKeyManager.Tester.Controls
{
    partial class SelectFileControl
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
            this._layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this._tbFileName = new System.Windows.Forms.TextBox();
            this._lblText = new System.Windows.Forms.Label();
            this._btnBrowse = new System.Windows.Forms.Button();
            this._layoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // _layoutMain
            // 
            this._layoutMain.ColumnCount = 3;
            this._layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this._layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._layoutMain.Controls.Add(this._tbFileName, 1, 0);
            this._layoutMain.Controls.Add(this._lblText, 0, 0);
            this._layoutMain.Controls.Add(this._btnBrowse, 2, 0);
            this._layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this._layoutMain.Location = new System.Drawing.Point(0, 0);
            this._layoutMain.Name = "_layoutMain";
            this._layoutMain.RowCount = 1;
            this._layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._layoutMain.Size = new System.Drawing.Size(349, 28);
            this._layoutMain.TabIndex = 0;
            // 
            // _tbFileName
            // 
            this._tbFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbFileName.Location = new System.Drawing.Point(68, 3);
            this._tbFileName.Name = "_tbFileName";
            this._tbFileName.ReadOnly = true;
            this._tbFileName.Size = new System.Drawing.Size(197, 20);
            this._tbFileName.TabIndex = 1;
            // 
            // _lblText
            // 
            this._lblText.AutoSize = true;
            this._lblText.Location = new System.Drawing.Point(3, 5);
            this._lblText.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this._lblText.Name = "_lblText";
            this._lblText.Size = new System.Drawing.Size(59, 13);
            this._lblText.TabIndex = 0;
            this._lblText.Text = "Select File:";
            // 
            // _btnBrowse
            // 
            this._btnBrowse.Location = new System.Drawing.Point(271, 3);
            this._btnBrowse.Name = "_btnBrowse";
            this._btnBrowse.Size = new System.Drawing.Size(75, 22);
            this._btnBrowse.TabIndex = 2;
            this._btnBrowse.Text = "Browse...";
            this._btnBrowse.UseVisualStyleBackColor = true;
            this._btnBrowse.Click += new System.EventHandler(this._btnBrowse_Click);
            // 
            // SelectFileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._layoutMain);
            this.Name = "SelectFileControl";
            this.Size = new System.Drawing.Size(349, 28);
            this._layoutMain.ResumeLayout(false);
            this._layoutMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _layoutMain;
        private System.Windows.Forms.TextBox _tbFileName;
        private System.Windows.Forms.Label _lblText;
        private System.Windows.Forms.Button _btnBrowse;
    }
}
