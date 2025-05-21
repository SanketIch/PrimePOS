using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace POS_Core_UI.Resources
{
	class CustomMsgBox : System.Windows.Forms.Form
	{
		private bool close=false;
		public System.Windows.Forms.RichTextBox txtPoruka;
		public System.Windows.Forms.Label lblText;
		public System.Windows.Forms.PictureBox picIcon;
		public Infragistics.Win.Misc.UltraButton btnAbort;
		public Infragistics.Win.Misc.UltraButton btnNo;
		public Infragistics.Win.Misc.UltraButton btnYes;
		public System.ComponentModel.IContainer components = null;
        private bool useCustomButtonText=false;
		public CustomMsgBox():this(false)
		{
		}

        public CustomMsgBox(bool useCustomButtonText)
        {
            this.useCustomButtonText = useCustomButtonText;
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }
        
        /// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.txtPoruka = new System.Windows.Forms.RichTextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.btnYes = new Infragistics.Win.Misc.UltraButton();
            this.btnNo = new Infragistics.Win.Misc.UltraButton();
            this.btnAbort = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // picIcon
            // 
            this.picIcon.Location = new System.Drawing.Point(8, 4);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(45, 45);
            this.picIcon.TabIndex = 7;
            this.picIcon.TabStop = false;
            // 
            // txtPoruka
            // 
            this.txtPoruka.BackColor = System.Drawing.SystemColors.Control;
            this.txtPoruka.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPoruka.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPoruka.Location = new System.Drawing.Point(64, 8);
            this.txtPoruka.Name = "txtPoruka";
            this.txtPoruka.ReadOnly = true;
            this.txtPoruka.Size = new System.Drawing.Size(168, 40);
            this.txtPoruka.TabIndex = 9;
            this.txtPoruka.TabStop = false;
            this.txtPoruka.Text = "";
            this.txtPoruka.Visible = false;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.Location = new System.Drawing.Point(56, 8);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(48, 17);
            this.lblText.TabIndex = 10;
            this.lblText.Text = "label1";
            // 
            // btnYes
            // 
            appearance1.FontData.BoldAsString = "True";
            this.btnYes.Appearance = appearance1;
            this.btnYes.AutoSize = true;
            this.btnYes.Location = new System.Drawing.Point(88, 56);
            this.btnYes.MinimumSize = new System.Drawing.Size(75, 0);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 25);
            this.btnYes.TabIndex = 18;
            this.btnYes.Text = "&Ok";
            this.btnYes.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnYes.Click += new System.EventHandler(this.btnDa_Click);
            this.btnYes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomMsgBox_KeyDown);
            // 
            // btnNo
            // 
            appearance2.FontData.BoldAsString = "True";
            this.btnNo.Appearance = appearance2;
            this.btnNo.AutoSize = true;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnNo.Location = new System.Drawing.Point(168, 56);
            this.btnNo.MinimumSize = new System.Drawing.Size(75, 0);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 25);
            this.btnNo.TabIndex = 19;
            this.btnNo.Text = "&No";
            this.btnNo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnNo.Click += new System.EventHandler(this.btnNe_Click);
            this.btnNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomMsgBox_KeyDown);
            // 
            // btnAbort
            // 
            appearance3.FontData.BoldAsString = "True";
            this.btnAbort.Appearance = appearance3;
            this.btnAbort.AutoSize = true;
            this.btnAbort.Location = new System.Drawing.Point(8, 56);
            this.btnAbort.MinimumSize = new System.Drawing.Size(75, 0);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(75, 25);
            this.btnAbort.TabIndex = 20;
            this.btnAbort.Text = "&Abort";
            this.btnAbort.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnAbort.Click += new System.EventHandler(this.btnOdustani_Click);
            this.btnAbort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomMsgBox_KeyDown);
            // 
            // CustomMsgBox
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.btnNo;
            this.ClientSize = new System.Drawing.Size(250, 88);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.picIcon);
            this.Controls.Add(this.txtPoruka);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnAbort);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomMsgBox";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.CustomMsgBox_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.CustomMsgBox_Closing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomMsgBox_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		# region "Apperiance"

		public void ResizeForm()
		{
			this.lblText.Text=this.txtPoruka.Text;
			
			if(lblText.Width>168)
				this.Width=lblText.Width+88;
			
			int sirina=this.lblText.Width;
			this.lblText.AutoSize=false;
			this.lblText.Height=this.txtPoruka.Lines.Length*18;
			this.lblText.Width=sirina;
			
			this.btnAbort.Top=this.lblText.Height<40 ? 56:this.lblText.Bottom + 12;
			this.btnYes.Top=this.lblText.Height<40 ? 56:this.lblText.Bottom + 12;
			this.btnNo.Top=this.lblText.Height<40 ? 56:this.lblText.Bottom + 12;
			this.Height=this.lblText.Height<40 ? 120:this.btnYes.Bottom + 44;

            int increasedSize = (this.btnYes.Width + this.btnNo.Width + this.btnAbort.Width) - (this.btnNo.MinimumSize.Width * 3);
            if (increasedSize > 0)
            {
                this.Width += increasedSize;
            }
			this.Left=(Screen.PrimaryScreen.Bounds.Width-this.Width)/2;
			this.Top=(Screen.PrimaryScreen.Bounds.Height-this.Height)/2;
		}

		public void SetInfoIcon()
		{
			this.picIcon.Image=SystemIcons.Information.ToBitmap();
		}

		public void SetQuestionIcon()
		{
			this.picIcon.Image=SystemIcons.Question.ToBitmap();
		}

		public void SetErrorIcon()
		{
			this.picIcon.Image=SystemIcons.Error.ToBitmap();
		}

		public void SetWarningIcon()
		{
			this.picIcon.Image=SystemIcons.Warning.ToBitmap();
		}

		#endregion

		# region "Buttons"

		public void Yes()
		{
			btnYes.Visible=true;
            if (useCustomButtonText == false)
            {
                btnYes.Text = "&Ok";
            }
			btnNo.Visible=false;
			btnAbort.Visible=false;
			btnYes.Left=(this.Width - this.btnYes.Width)/2;
		}

		public void YesNo()
		{
			btnYes.Visible=true;
            if (useCustomButtonText == false)
            {
                btnYes.Text = "&Yes";
                btnNo.Text = "&No";
            }

			btnNo.Visible=true;
			btnAbort.Visible=false;

			btnYes.Left=this.Width/2 - btnYes.Width - 5;
			btnNo.Left=btnYes.Right + 5;
		}


        public void RetryCancel()
        {
            btnYes.Visible = true;
            if (useCustomButtonText == false)
            {
                btnYes.Text = "&Ok";
                btnNo.Text = "&Continue";
            }

            btnNo.Visible = true;
            btnAbort.Visible = false;

            btnYes.Left = this.Width / 2 - btnYes.Width - 5;
            btnNo.Left = btnYes.Right + 5;
        }
        public void YesNoCancel()
		{
			btnYes.Visible=true;
			btnNo.Visible=true;
			btnAbort.Visible=true;

            if (useCustomButtonText == false)
            {
                btnYes.Text = "&Yes";
                btnNo.Text = "&No";
                btnAbort.Text = "&Abort";
            }

            btnAbort.DialogResult = DialogResult.Cancel;

            int btnWidth = btnYes.Width + btnNo.Width + btnAbort.Width;
            int emptySpace =this.Width-btnWidth-25;
			btnYes.Left=emptySpace/2;
			btnNo.Left=btnYes.Right + 5;
			btnAbort.Left=btnNo.Right + 5;
		}
		# endregion

		# region "Result"
		private void btnNe_Click(object sender, System.EventArgs e)
		{
			close=true;
			this.DialogResult = DialogResult.No;
		}

		private void btnOdustani_Click(object sender, System.EventArgs e)
		{
			close=true;
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnDa_Click(object sender, System.EventArgs e)
		{
			close=true;
			this.DialogResult = DialogResult.Yes;
		}

		private void CustomMsgBox_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel=!close;
		}
		#endregion

		private void CustomMsgBox_Load(object sender, System.EventArgs e)
		{
			clsUIHelper.setColorSchecme(this);

            int currentWidth = 0;
            int gap = 0;
            if (btnYes.Visible == true)
            {
                currentWidth += btnYes.Width;
            }
            if (btnNo.Visible == true)
            {
                gap += 5;
                currentWidth += btnNo.Width;
            }
            if (btnAbort.Visible == true)
            {
                gap += 5;
                currentWidth += btnAbort.Width;
            }

            int left = (this.Width - (currentWidth + gap)) / 2;

            if (btnYes.Visible == true)
            {
                btnYes.Left = left;
                left += btnYes.Width + 5;
            }
            if (btnNo.Visible == true)
            {
                btnNo.Left = left;
                left += btnNo.Width + 5;
            }
            if (btnAbort.Visible == true)
            {
                btnAbort.Left = left;
            }
		}

        private void CustomMsgBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Escape )
            {
                close = true;
                if (btnAbort.Visible == true)
                {
                    this.DialogResult = DialogResult.Cancel;
                }
                else
                {
                    this.DialogResult = DialogResult.No;
                }
            }

            
        }

        

	}

	public class Message
	{
		# region "Methods"
		public static DialogResult Display(string message,string caption,MessageBoxButtons buttons,MessageBoxIcon icon,MessageBoxDefaultButton defaultButton)
		{
			CustomMsgBox p=new CustomMsgBox();
			p.txtPoruka.Text=message;
			p.ResizeForm();
			p.Text=caption;

			switch(buttons)
			{
				case MessageBoxButtons.YesNo:
					p.YesNo();
					break;
				case MessageBoxButtons.OKCancel:
					p.YesNo();
					break;
				case MessageBoxButtons.YesNoCancel:
					p.YesNoCancel();
					break;
                case MessageBoxButtons.RetryCancel:
                    p.RetryCancel();
                    break;
                default:
					p.Yes();
					break;
			}
			switch(icon)
			{
				case MessageBoxIcon.Information:
					p.SetInfoIcon();
					break;
				case MessageBoxIcon.Question:
					p.SetQuestionIcon();
					break;
				case MessageBoxIcon.Exclamation:
					p.SetWarningIcon();
					break;
				case MessageBoxIcon.Error:
					p.SetErrorIcon();
					break;
			}
			switch(defaultButton)
			{
				case MessageBoxDefaultButton.Button1:
					p.btnYes.TabIndex=0;
					break;
				case MessageBoxDefaultButton.Button2:
					p.btnNo.TabIndex=0;
					break;
				case MessageBoxDefaultButton.Button3:
					p.btnAbort.TabIndex=0;
					break;
			}
			return p.ShowDialog();
		}

        public static DialogResult Display(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Display(null,message,caption,buttons,icon);
        }

		public static DialogResult Display(Form parent,string message,string caption,MessageBoxButtons buttons,MessageBoxIcon icon)
		{
			CustomMsgBox p=new CustomMsgBox();
			p.txtPoruka.Text=message;
			p.ResizeForm();
			p.Text=caption;
			switch(buttons)
			{
				case MessageBoxButtons.YesNo:
					p.YesNo();
					break;
				case MessageBoxButtons.OKCancel:
					p.YesNo();
					break;
				case MessageBoxButtons.YesNoCancel:
					p.YesNoCancel();
					break;
				default:
					p.Yes();
					break;
			}
			switch(icon)
			{
				case MessageBoxIcon.Information:
					p.SetInfoIcon();
					break;
				case MessageBoxIcon.Question:
					p.SetQuestionIcon();
					break;
				case MessageBoxIcon.Exclamation:
					p.SetWarningIcon();
					break;
				case MessageBoxIcon.Error:
					p.SetErrorIcon();
					break;
			}
			p.btnYes.Focus();
            if (parent == null)
            {
                p.StartPosition = FormStartPosition.CenterScreen;
                return p.ShowDialog();
            }
            else
            {
                return p.ShowDialog(parent);
            }
		}
       
        
        //Added By shitaljit (QuicSolv) 4 May 2011
        public static DialogResult DisplayDefaultNo(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            CustomMsgBox p = new CustomMsgBox();
            p.txtPoruka.Text = message;
            p.ResizeForm();
            p.Text = caption;
            switch (buttons)
            {
                case MessageBoxButtons.YesNo:
                    p.YesNo();
                    break;
                case MessageBoxButtons.OKCancel:
                    p.YesNo();
                    break;
                case MessageBoxButtons.YesNoCancel:
                    p.YesNoCancel();
                    break;
                default:
                    p.Yes();
                    break;
            }
            switch (icon)
            {
                case MessageBoxIcon.Information:
                    p.SetInfoIcon();
                    break;
                case MessageBoxIcon.Question:
                    p.SetQuestionIcon();
                    break;
                case MessageBoxIcon.Exclamation:
                    p.SetWarningIcon();
                    break;
                case MessageBoxIcon.Error:
                    p.SetErrorIcon();
                    break;
            }
            p.btnNo.Select();
            return p.ShowDialog();
        
        }
        //Till here aded by shitaljit

		public static DialogResult Display(string message,string caption,MessageBoxButtons buttons)
		{
			CustomMsgBox p=new CustomMsgBox();
			p.txtPoruka.Text=message;
			p.ResizeForm();
			p.Text=caption;
			switch(buttons)
			{
				case MessageBoxButtons.YesNo:
					p.YesNo();
					break;
				case MessageBoxButtons.OKCancel:
					p.YesNo();
					break;
				case MessageBoxButtons.YesNoCancel:
					p.YesNoCancel();
					break;
				default:
					p.Yes();
					break;
			}
			p.btnYes.Focus();
			return p.ShowDialog();
		}
		public static DialogResult Display(string message,string caption)
		{
			CustomMsgBox p=new CustomMsgBox();
			p.txtPoruka.Text=message;
			p.ResizeForm();
			p.Text=caption;
			p.Yes();
			p.btnYes.Focus();
			return p.ShowDialog();
		}
		public static DialogResult Display(string message)
		{
			CustomMsgBox p=new CustomMsgBox();
			p.txtPoruka.Text=message;
			p.ResizeForm();
			p.Yes();
			p.btnYes.Focus();
			return p.ShowDialog();
		}

        //Following Code Added by Krishna on 13 October 2011
        public static DialogResult Display(string message, FormStartPosition Position)
        {
            CustomMsgBox p = new CustomMsgBox();
            p.StartPosition = Position;
            p.SetInfoIcon();
            p.txtPoruka.Text = message;
            p.ResizeForm();
            p.Yes();
            p.btnYes.Focus();
            return p.ShowDialog();
        }
        //Till here added by Krishna on 13 October 2011

		public static DialogResult DisplayLocal(string message,string caption,MessageBoxButtons buttons,MessageBoxIcon icon,MessageBoxDefaultButton defaultButton,string yesButtonText,string noButtonText,string abortButtonText)
		{
			CustomMsgBox p=new CustomMsgBox(true);
			p.btnYes.Text=yesButtonText;
			p.btnNo.Text=noButtonText;
			p.btnAbort.Text=abortButtonText;
			p.txtPoruka.Text=message;
			p.ResizeForm();
			p.Text=caption;
			switch(buttons)
			{
				case MessageBoxButtons.YesNo:
					p.YesNo();
					break;
				case MessageBoxButtons.OKCancel:
					p.YesNo();
					break;
				case MessageBoxButtons.YesNoCancel:
					p.YesNoCancel();
					break;
				default:
					p.Yes();
					break;
			}
			switch(icon)
			{
				case MessageBoxIcon.Information:
					p.SetInfoIcon();
					break;
				case MessageBoxIcon.Question:
					p.SetQuestionIcon();
					break;
				case MessageBoxIcon.Exclamation:
					p.SetWarningIcon();
					break;
				case MessageBoxIcon.Error:
					p.SetErrorIcon();
					break;
			}
			switch(defaultButton)
			{
				case MessageBoxDefaultButton.Button1:
					p.btnYes.TabIndex=0;
					break;
				case MessageBoxDefaultButton.Button2:
					p.btnNo.TabIndex=0;
					break;
				case MessageBoxDefaultButton.Button3:
					p.btnAbort.TabIndex=0;
					break;
			}
			return p.ShowDialog();
		}
        //Added By Shitaljit on 12 Sept 2012.
        public static DialogResult Display(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, FormStartPosition Position)
        {
            CustomMsgBox p = new CustomMsgBox();
            p.StartPosition = Position;
            p.txtPoruka.Text = message;
            p.ResizeForm();
            p.Text = caption;
            switch (buttons)
            {
                case MessageBoxButtons.YesNo:
                    p.YesNo();
                    break;
                case MessageBoxButtons.OKCancel:
                    p.YesNo();
                    break;
                case MessageBoxButtons.YesNoCancel:
                    p.YesNoCancel();
                    break;
                default:
                    p.Yes();
                    break;
            }
            switch (icon)
            {
                case MessageBoxIcon.Information:
                    p.SetInfoIcon();
                    break;
                case MessageBoxIcon.Question:
                    p.SetQuestionIcon();
                    break;
                case MessageBoxIcon.Exclamation:
                    p.SetWarningIcon();
                    break;
                case MessageBoxIcon.Error:
                    p.SetErrorIcon();
                    break;
            }
            switch (defaultButton)
            {
                case MessageBoxDefaultButton.Button1:
                    p.btnYes.TabIndex = 0;
                    break;
                case MessageBoxDefaultButton.Button2:
                    p.btnNo.TabIndex = 0;
                    break;
                case MessageBoxDefaultButton.Button3:
                    p.btnAbort.TabIndex = 0;
                    break;
            }
            return p.ShowDialog();
        }
		public static DialogResult DisplayLocal(string message,string caption,MessageBoxButtons buttons,MessageBoxIcon icon,string yesButtonText,string noButtonText,string abortButtonText)
		{
			CustomMsgBox p=new CustomMsgBox(true);
			p.btnYes.Text=yesButtonText;
			p.btnNo.Text=noButtonText;
			p.btnAbort.Text=abortButtonText;
			p.txtPoruka.Text=message;
			p.ResizeForm();
			p.Text=caption;
			switch(buttons)
			{
				case MessageBoxButtons.YesNo:
					p.YesNo();
					break;
				case MessageBoxButtons.OKCancel:
					p.YesNo();
					break;
				case MessageBoxButtons.YesNoCancel:
					p.YesNoCancel();
					break;
				default:
					p.Yes();
					break;
			}
			switch(icon)
			{
				case MessageBoxIcon.Information:
					p.SetInfoIcon();
					break;
				case MessageBoxIcon.Question:
					p.SetQuestionIcon();
					break;
				case MessageBoxIcon.Exclamation:
					p.SetWarningIcon();
					break;
				case MessageBoxIcon.Error:
					p.SetErrorIcon();
					break;
			}
			p.btnYes.Focus();
			return p.ShowDialog();
		}
		public static DialogResult DisplayLocal(string message,string caption,MessageBoxButtons buttons,string yesButtonText,string noButtonText,string abortButtonText)
		{
			CustomMsgBox p=new CustomMsgBox(true);
			p.btnYes.Text=yesButtonText;
			p.btnNo.Text=noButtonText;
			p.btnAbort.Text=abortButtonText;
			p.txtPoruka.Text=message;
			p.ResizeForm();
			p.Text=caption;
			switch(buttons)
			{
				case MessageBoxButtons.YesNo:
					p.YesNo();
					break;
				case MessageBoxButtons.OKCancel:
					p.YesNo();
					break;
				case MessageBoxButtons.YesNoCancel:
					p.YesNoCancel();
					break;
				default:
					p.Yes();
					break;
			}
			p.btnYes.Focus();
			return p.ShowDialog();
		}
		public static DialogResult DisplayLocal(string message,string caption,string yesButtonText)
		{
			CustomMsgBox p=new CustomMsgBox(true);
			p.btnYes.Text=yesButtonText;
			p.txtPoruka.Text=message;
			p.ResizeForm();
			p.Text=caption;
			p.Yes();
			p.btnYes.Focus();
			return p.ShowDialog();
		}
		public static DialogResult DisplayLocal(string message,string yesButtonText)
		{
			CustomMsgBox p=new CustomMsgBox(true);
			p.btnYes.Text=yesButtonText;
			p.txtPoruka.Text=message;
			p.ResizeForm();
			p.Yes();
			p.btnYes.Focus();
			return p.ShowDialog();
		}

	#endregion
	}
}
