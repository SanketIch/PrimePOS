namespace POS_Core_UI.UI
{
    partial class frmUrlView
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
            this.tblBrowser = new System.Windows.Forms.TableLayoutPanel();
            this.webView2 = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.tblBrowser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView2)).BeginInit();
            this.SuspendLayout();
            // 
            // tblBrowser
            // 
            this.tblBrowser.ColumnCount = 1;
            this.tblBrowser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblBrowser.Controls.Add(this.webView2, 0, 0);
            this.tblBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblBrowser.Location = new System.Drawing.Point(0, 0);
            this.tblBrowser.Name = "tblBrowser";
            this.tblBrowser.RowCount = 1;
            this.tblBrowser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblBrowser.Size = new System.Drawing.Size(784, 749);
            this.tblBrowser.TabIndex = 2;
            // 
            // webView
            // 
            this.webView2.AllowExternalDrop = true;
            this.webView2.CreationProperties = null;
            this.webView2.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView2.Location = new System.Drawing.Point(2, 2);
            this.webView2.Margin = new System.Windows.Forms.Padding(2);
            this.webView2.Name = "webView";
            this.webView2.Size = new System.Drawing.Size(780, 745);
            this.webView2.TabIndex = 0;
            this.webView2.ZoomFactor = 1D;
            this.webView2.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.WebView_CoreWebView2InitializationCompleted);
            this.webView2.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.WebView_NavigationCompleted);
            // 
            // frmUrlView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(784, 749);
            this.Controls.Add(this.tblBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUrlView";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hyphen View";
            this.Activated += new System.EventHandler(this.frmUrlView_Activated);
            this.Load += new System.EventHandler(this.frmUrlView_Load);
            this.FormClosing += FrmUrlView_FormClosing;
            this.tblBrowser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.webView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tblBrowser;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView2;
    }
}