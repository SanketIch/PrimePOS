namespace PrimeRxPay
{
    partial class HostedPayView
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
            this.tblMessage = new System.Windows.Forms.TableLayoutPanel();
            this.lblStatus = new Infragistics.Win.Misc.UltraLabel();
            this.lblMessage = new Infragistics.Win.Misc.UltraLabel();
            this.tblBrowser = new System.Windows.Forms.TableLayoutPanel();
            this.webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.tblMessage.SuspendLayout();
            this.tblBrowser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView)).BeginInit();
            this.SuspendLayout();
            // 
            // tblMessage
            // 
            this.tblMessage.ColumnCount = 2;
            this.tblMessage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.85507F));
            this.tblMessage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.14493F));
            this.tblMessage.Controls.Add(this.lblStatus, 0, 0);
            this.tblMessage.Controls.Add(this.lblMessage, 1, 0);
            this.tblMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblMessage.Location = new System.Drawing.Point(0, 0);
            this.tblMessage.Margin = new System.Windows.Forms.Padding(4);
            this.tblMessage.Name = "tblMessage";
            this.tblMessage.RowCount = 1;
            this.tblMessage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblMessage.Size = new System.Drawing.Size(1237, 31);
            this.tblMessage.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(4, 4);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(175, 23);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status";
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(187, 4);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(4);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(1046, 23);
            this.lblMessage.TabIndex = 1;
            // 
            // tblBrowser
            // 
            this.tblBrowser.ColumnCount = 1;
            this.tblBrowser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblBrowser.Controls.Add(this.webView, 0, 0);
            this.tblBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblBrowser.Location = new System.Drawing.Point(0, 31);
            this.tblBrowser.Margin = new System.Windows.Forms.Padding(4);
            this.tblBrowser.Name = "tblBrowser";
            this.tblBrowser.RowCount = 1;
            this.tblBrowser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblBrowser.Size = new System.Drawing.Size(1237, 944);
            this.tblBrowser.TabIndex = 2;
            // 
            // webView
            // 
            this.webView.AllowExternalDrop = true;
            this.webView.CreationProperties = null;
            this.webView.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView.Location = new System.Drawing.Point(3, 3);
            this.webView.Name = "webView";
            this.webView.Size = new System.Drawing.Size(1231, 938);
            this.webView.TabIndex = 0;
            this.webView.ZoomFactor = 1D;
            this.webView.CoreWebView2InitializationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs>(this.WebView_CoreWebView2InitializationCompleted);
            this.webView.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.WebView_NavigationCompleted);
            // 
            // HostedPayView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1237, 975);
            this.ControlBox = true; //PRIMEPOS-3533
            this.Controls.Add(this.tblBrowser);
            this.Controls.Add(this.tblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HostedPayView";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PrimeRxPay Card Entry";
            this.Activated += new System.EventHandler(this.HostedPayView_Activated);
            this.Load += new System.EventHandler(this.HostedPayView_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HostedPayView_OnClosed); //PRIMEPOS-3533
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HostedPayView_OnClosing); //PRIMEPOS-3533
            this.tblMessage.ResumeLayout(false);
            this.tblBrowser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.webView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tblMessage;
        private Infragistics.Win.Misc.UltraLabel lblStatus;
        private Infragistics.Win.Misc.UltraLabel lblMessage;
        private System.Windows.Forms.TableLayoutPanel tblBrowser;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
    }
}