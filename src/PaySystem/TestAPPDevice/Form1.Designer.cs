namespace TestAPPDevice
{
    partial class Form1
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnShowF1 = new System.Windows.Forms.Button();
            this.btnShowF2 = new System.Windows.Forms.Button();
            this.btnCurrentS = new System.Windows.Forms.Button();
            this.btnShutDown = new System.Windows.Forms.Button();
            this.btnActivate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLineDisp = new System.Windows.Forms.Button();
            this.signBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.signBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(83, 37);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click_1);
            // 
            // btnShowF1
            // 
            this.btnShowF1.Enabled = false;
            this.btnShowF1.Location = new System.Drawing.Point(103, 12);
            this.btnShowF1.Name = "btnShowF1";
            this.btnShowF1.Size = new System.Drawing.Size(93, 37);
            this.btnShowF1.TabIndex = 1;
            this.btnShowF1.Text = "Show Form 1";
            this.btnShowF1.UseVisualStyleBackColor = true;
            this.btnShowF1.Click += new System.EventHandler(this.btnShowF1_Click);
            // 
            // btnShowF2
            // 
            this.btnShowF2.Enabled = false;
            this.btnShowF2.Location = new System.Drawing.Point(203, 12);
            this.btnShowF2.Name = "btnShowF2";
            this.btnShowF2.Size = new System.Drawing.Size(105, 37);
            this.btnShowF2.TabIndex = 2;
            this.btnShowF2.Text = "Show Form 2";
            this.btnShowF2.UseVisualStyleBackColor = true;
            this.btnShowF2.Click += new System.EventHandler(this.btnShowF2_Click);
            // 
            // btnCurrentS
            // 
            this.btnCurrentS.Enabled = false;
            this.btnCurrentS.Location = new System.Drawing.Point(12, 55);
            this.btnCurrentS.Name = "btnCurrentS";
            this.btnCurrentS.Size = new System.Drawing.Size(83, 35);
            this.btnCurrentS.TabIndex = 3;
            this.btnCurrentS.Text = "Show Current Screen";
            this.btnCurrentS.UseVisualStyleBackColor = true;
            this.btnCurrentS.Click += new System.EventHandler(this.btnCurrentS_Click);
            // 
            // btnShutDown
            // 
            this.btnShutDown.Enabled = false;
            this.btnShutDown.Location = new System.Drawing.Point(203, 55);
            this.btnShutDown.Name = "btnShutDown";
            this.btnShutDown.Size = new System.Drawing.Size(105, 35);
            this.btnShutDown.TabIndex = 4;
            this.btnShutDown.Text = "Shut Down";
            this.btnShutDown.UseVisualStyleBackColor = true;
            this.btnShutDown.Click += new System.EventHandler(this.btnShutDown_Click);
            // 
            // btnActivate
            // 
            this.btnActivate.Enabled = false;
            this.btnActivate.Location = new System.Drawing.Point(103, 55);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(93, 35);
            this.btnActivate.TabIndex = 5;
            this.btnActivate.Text = "Activate Device";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(12, 100);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 35);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel Transaction";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnLineDisp
            // 
            this.btnLineDisp.Enabled = false;
            this.btnLineDisp.Location = new System.Drawing.Point(103, 100);
            this.btnLineDisp.Name = "btnLineDisp";
            this.btnLineDisp.Size = new System.Drawing.Size(75, 35);
            this.btnLineDisp.TabIndex = 8;
            this.btnLineDisp.Text = "Delete Line";
            this.btnLineDisp.UseVisualStyleBackColor = true;
            this.btnLineDisp.Click += new System.EventHandler(this.btnLineDisp_Click);
            // 
            // signBox
            // 
            this.signBox.Location = new System.Drawing.Point(12, 169);
            this.signBox.Name = "signBox";
            this.signBox.Size = new System.Drawing.Size(290, 144);
            this.signBox.TabIndex = 7;
            this.signBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 319);
            this.Controls.Add(this.btnLineDisp);
            this.Controls.Add(this.signBox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnActivate);
            this.Controls.Add(this.btnShutDown);
            this.Controls.Add(this.btnCurrentS);
            this.Controls.Add(this.btnShowF2);
            this.Controls.Add(this.btnShowF1);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.signBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnShowF1;
        private System.Windows.Forms.Button btnShowF2;
        private System.Windows.Forms.Button btnCurrentS;
        private System.Windows.Forms.Button btnShutDown;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox signBox;
        private System.Windows.Forms.Button btnLineDisp;
    }
}

