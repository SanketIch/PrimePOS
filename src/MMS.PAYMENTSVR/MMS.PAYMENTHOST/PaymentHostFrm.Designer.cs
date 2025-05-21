namespace MMS.PAYMENTHOST
{
    partial class PaymentHostFrm
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
            KillProcess();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaymentHostFrm));
            this.btnStop = new System.Windows.Forms.Button();
            this.listStatus = new System.Windows.Forms.ListBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.notifyIconHstPmtSrv = new System.Windows.Forms.NotifyIcon(this.components);
            this.contxtMnuHstPmtSrvr = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sTARTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sTOPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cLEARToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mAXIMIZEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mINIMIZEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eXITToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblCopyRignt = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contxtMnuHstPmtSrvr.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Crimson;
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(411, 72);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(127, 34);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // listStatus
            // 
            this.listStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listStatus.FormattingEnabled = true;
            this.listStatus.ItemHeight = 16;
            this.listStatus.Location = new System.Drawing.Point(164, 25);
            this.listStatus.Name = "listStatus";
            this.listStatus.Size = new System.Drawing.Size(241, 148);
            this.listStatus.TabIndex = 3;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(411, 125);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(127, 41);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Orange;
            this.label2.Location = new System.Drawing.Point(161, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Status";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(127, 129);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // notifyIconHstPmtSrv
            // 
            this.notifyIconHstPmtSrv.ContextMenuStrip = this.contxtMnuHstPmtSrvr;
            this.notifyIconHstPmtSrv.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconHstPmtSrv.Icon")));
            this.notifyIconHstPmtSrv.Text = "MMS HOST PAYMENT PROCESSING SERVER";
            this.notifyIconHstPmtSrv.Visible = true;
            // 
            // contxtMnuHstPmtSrvr
            // 
            this.contxtMnuHstPmtSrvr.BackColor = System.Drawing.Color.LemonChiffon;
            this.contxtMnuHstPmtSrvr.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contxtMnuHstPmtSrvr.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sTARTToolStripMenuItem,
            this.sTOPToolStripMenuItem,
            this.cLEARToolStripMenuItem,
            this.mAXIMIZEToolStripMenuItem,
            this.mINIMIZEToolStripMenuItem,
            this.eXITToolStripMenuItem});
            this.contxtMnuHstPmtSrvr.Name = "contxtMnuHstPmtSrvr";
            this.contxtMnuHstPmtSrvr.Size = new System.Drawing.Size(130, 136);
            // 
            // sTARTToolStripMenuItem
            // 
            this.sTARTToolStripMenuItem.Name = "sTARTToolStripMenuItem";
            this.sTARTToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.sTARTToolStripMenuItem.Text = "START";
            this.sTARTToolStripMenuItem.Click += new System.EventHandler(this.sTARTToolStripMenuItem_Click);
            // 
            // sTOPToolStripMenuItem
            // 
            this.sTOPToolStripMenuItem.Name = "sTOPToolStripMenuItem";
            this.sTOPToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.sTOPToolStripMenuItem.Text = "STOP";
            this.sTOPToolStripMenuItem.Click += new System.EventHandler(this.sTOPToolStripMenuItem_Click);
            // 
            // cLEARToolStripMenuItem
            // 
            this.cLEARToolStripMenuItem.Name = "cLEARToolStripMenuItem";
            this.cLEARToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.cLEARToolStripMenuItem.Text = "CLEAR";
            this.cLEARToolStripMenuItem.Click += new System.EventHandler(this.cLEARToolStripMenuItem_Click);
            // 
            // mAXIMIZEToolStripMenuItem
            // 
            this.mAXIMIZEToolStripMenuItem.Name = "mAXIMIZEToolStripMenuItem";
            this.mAXIMIZEToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.mAXIMIZEToolStripMenuItem.Text = "MAXIMIZE";
            this.mAXIMIZEToolStripMenuItem.Click += new System.EventHandler(this.mAXIMIZEToolStripMenuItem_Click);
            // 
            // mINIMIZEToolStripMenuItem
            // 
            this.mINIMIZEToolStripMenuItem.Name = "mINIMIZEToolStripMenuItem";
            this.mINIMIZEToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.mINIMIZEToolStripMenuItem.Text = "MINIMIZE";
            this.mINIMIZEToolStripMenuItem.Click += new System.EventHandler(this.mINIMIZEToolStripMenuItem_Click);
            // 
            // eXITToolStripMenuItem
            // 
            this.eXITToolStripMenuItem.Name = "eXITToolStripMenuItem";
            this.eXITToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.eXITToolStripMenuItem.Text = "EXIT";
            this.eXITToolStripMenuItem.Click += new System.EventHandler(this.eXITToolStripMenuItem_Click);
            // 
            // lblCopyRignt
            // 
            this.lblCopyRignt.AutoSize = true;
            this.lblCopyRignt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyRignt.ForeColor = System.Drawing.Color.Yellow;
            this.lblCopyRignt.Location = new System.Drawing.Point(0, 182);
            this.lblCopyRignt.Name = "lblCopyRignt";
            this.lblCopyRignt.Size = new System.Drawing.Size(253, 16);
            this.lblCopyRignt.TabIndex = 10;
            this.lblCopyRignt.Text = "© Micro Merchant Systems, Inc 2013";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(411, 25);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(127, 32);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // PaymentHostFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(550, 214);
            this.Controls.Add(this.lblCopyRignt);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listStatus);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaymentHostFrm";
            this.Text = "PrimeCharge Interface";
            this.Load += new System.EventHandler(this.PaymentHostFrm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmClosing);
            this.MaximumSizeChanged += new System.EventHandler(this.PmtHstServer_Minimized);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contxtMnuHstPmtSrvr.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ListBox listStatus;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NotifyIcon notifyIconHstPmtSrv;
        private System.Windows.Forms.ContextMenuStrip contxtMnuHstPmtSrvr;
        private System.Windows.Forms.ToolStripMenuItem sTARTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sTOPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cLEARToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mAXIMIZEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mINIMIZEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eXITToolStripMenuItem;
        private System.Windows.Forms.Label lblCopyRignt;
        private System.Windows.Forms.Button btnStart;
    }
}

