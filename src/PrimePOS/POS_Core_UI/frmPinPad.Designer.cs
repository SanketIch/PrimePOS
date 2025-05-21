namespace POS_Core_UI
{
    partial class frmPinPad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPinPad));
            this.PinPadDevice = new AxDevice.AxOCXDevice();
            ((System.ComponentModel.ISupportInitialize)(this.PinPadDevice)).BeginInit();
            this.SuspendLayout();
            // 
            // PinPadDevice
            // 
            this.PinPadDevice.Enabled = true;
            this.PinPadDevice.Location = new System.Drawing.Point(134, 48);
            this.PinPadDevice.Name = "PinPadDevice";
            this.PinPadDevice.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("PinPadDevice.OcxState")));
            this.PinPadDevice.Size = new System.Drawing.Size(75, 23);
            this.PinPadDevice.TabIndex = 0;
            // 
            // frmPinPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 117);
            this.Controls.Add(this.PinPadDevice);
            this.Name = "frmPinPad";
            this.Text = "frmPinPad";
            ((System.ComponentModel.ISupportInitialize)(this.PinPadDevice)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxDevice.AxOCXDevice PinPadDevice;

  
    }
}