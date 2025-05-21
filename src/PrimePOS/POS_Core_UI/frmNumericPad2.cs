using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
//using POS_Core.DataAccess;
using System.Data;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinGrid;
using NLog;
using POS_Core.Resources;

namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for frmNumericPad2.
	/// </summary>
	public class frmNumericPad2 : System.Windows.Forms.Form
	{
		private FunctionKeys oFunctionKeys = new FunctionKeys();
		private FunctionKeysData oFunctionKeysData = new FunctionKeysData();
		private System.Windows.Forms.ToolTip toolTip1;

		private System.ComponentModel.IContainer components;
		
		private bool mouse_is_down=false; 
		private Point mouse_pos=new Point(0,0);
		private Point mouse_pos2=new Point(0,0);
		private Infragistics.Win.Misc.UltraGroupBox gbItemPad;
        private static ILogger logger = LogManager.GetCurrentClassLogger();

		public frmNumericPad2(IntPtr hWindParent)
		{
			
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			clsUIHelper.SetParentWindow(this.Handle,hWindParent);

			this.Tag="";
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

        public void AttachParent(IntPtr hWindParent)
        {
            if (hWindParent != null)
                clsUIHelper.SetParentWindow(this.Handle, hWindParent);
            else
                clsUIHelper.SetParentWindow(this.Handle, new IntPtr(0));
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.gbItemPad = new Infragistics.Win.Misc.UltraGroupBox();
			((System.ComponentModel.ISupportInitialize)(this.gbItemPad)).BeginInit();
			this.SuspendLayout();
			// 
			// gbItemPad
			// 
			this.gbItemPad.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.gbItemPad.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.None;
			appearance1.BackColor = System.Drawing.SystemColors.ControlLight;
			appearance1.BorderColor = System.Drawing.Color.Black;
			appearance1.BorderColor3DBase = System.Drawing.Color.Black;
			this.gbItemPad.ContentAreaAppearance = appearance1;
			this.gbItemPad.Dock = System.Windows.Forms.DockStyle.Fill;
			appearance2.BackColor2 = System.Drawing.SystemColors.Control;
			appearance2.BorderColor = System.Drawing.Color.Black;
			appearance2.BorderColor3DBase = System.Drawing.Color.Black;
			this.gbItemPad.HeaderAppearance = appearance2;
			this.gbItemPad.Location = new System.Drawing.Point(0, 0);
			this.gbItemPad.Name = "gbItemPad";
			this.gbItemPad.Size = new System.Drawing.Size(479, 409);
			this.gbItemPad.SupportThemes = false;
			this.gbItemPad.TabIndex = 0;
			this.gbItemPad.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2000;
			this.gbItemPad.Click += new System.EventHandler(this.gbItemPad_Click);
			// 
			// frmNumericPad2
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.SystemColors.ControlLight;
			this.ClientSize = new System.Drawing.Size(479, 409);
			this.Controls.Add(this.gbItemPad);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmNumericPad2";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.TopMost = true;
			this.Resize += new System.EventHandler(this.frmNumericPad2_Resize);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmNumericPad2_Closing);
			this.Load += new System.EventHandler(this.frmNumericPad2_Load);
			this.Activated += new System.EventHandler(this.frmNumericPad2_Activated);
			((System.ComponentModel.ISupportInitialize)(this.gbItemPad)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void frmNumericPad2_Load(object sender, System.EventArgs e)
		{
			oFunctionKeysData = oFunctionKeys.PopulateList("");
			PopulateFormWithButtons();

			FormLocation frmLoc= Configuration.GetPadSetting(0);
			this.Left=frmLoc.Left-2;
			this.Top=frmLoc.Top-20;
			this.Width=frmLoc.Width;
			this.Height=frmLoc.Height;
		}

		private void PopulateFormWithButtons()
		{
			try
			{
				for(int index =1;index<= oFunctionKeysData.FunctionKeys.Rows.Count;index++)
				{
					string butCaption = "";
					FunctionKeysRow oRow = (FunctionKeysRow)oFunctionKeysData.FunctionKeys.Rows[index-1];
					
					butCaption = getItemName(oRow.Operation) + "\n(" + oRow.FunKey + ")"  ;

					Infragistics.Win.Misc.UltraButton btn = new Infragistics.Win.Misc.UltraButton();
					Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
					Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
					Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();

					appearance5.BackColor = System.Drawing.SystemColors.Control;
					appearance5.BackColor2 = System.Drawing.SystemColors.Control;
					appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.None;

					appearance7.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(188)), ((System.Byte)(193)), ((System.Byte)(207)));
					appearance7.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(188)), ((System.Byte)(193)), ((System.Byte)(207)));

					btn.Appearance = appearance5;
					btn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
					btn.Cursor = System.Windows.Forms.Cursors.Arrow;
					btn.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
					btn.Location = new System.Drawing.Point(76, 26);
					appearance6.BackColor = System.Drawing.Color.SteelBlue;
					btn.PressedAppearance = appearance6;
					btn.HotTrackAppearance = appearance7;
					btn.HotTracking  =true;

					btn.Name = "btn" + index;

					btn.TabIndex = index;
					btn.Tag = oRow.FunKey;
					btn.Text = butCaption;
					btn.Size = new System.Drawing.Size(121, 57);
					Size sz=new Size();


					btn.Click += new System.EventHandler(this.btnClick);

					gbItemPad.Controls.Add(btn);

/*					if (index <= 12)
					{
						Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem itm = ebItemPad.Groups[0].Items.Add("Item"+index,butCaption);
						itm.Text = butCaption;
						itm.Tag = oRow.FunKey;
						itm.ToolTipText = oRow.FunKey;
						Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem itm2 = ebItemPad.Groups[0].Items.Add("Seperator"+index,"");
					
						itm2.Settings.Style = Infragistics.Win.UltraWinExplorerBar.ItemStyle.Separator;
						itm2.Settings.Height = 2;
					}
					else
					{
						Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem itm = ebItemPad2.Groups[0].Items.Add("Item"+index,butCaption);
						itm.Text = butCaption;
						itm.Tag = oRow.FunKey;
						itm.ToolTipText = oRow.FunKey;
						Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem itm2 = ebItemPad2.Groups[0].Items.Add("Seperator"+index,"");
					
						itm2.Settings.Style = Infragistics.Win.UltraWinExplorerBar.ItemStyle.Separator;
						itm2.Settings.Height = 2;
					}*/

				}
				clsUIHelper.setColorSchecme(this);
				clsUIHelper.setFlowLayout(gbItemPad,5,5,2,2);
			}
			catch(Exception exp)
			{
                logger.Fatal(exp, "PopulateFormWithButtons()");
                clsUIHelper.ShowErrorMsg(exp.Message);
			}
		}
		private string getItemName(string itemId)
		{
			Item oItem = new Item();
			ItemData oItemData;
			ItemRow oItemRow;
			
			oItemData = oItem.Populate(itemId);
			if (oItemData.Tables[0].Rows.Count > 0 )
				oItemRow = oItemData.Item[0];
			else
				return "";
 
			return oItemRow.Description;
		}

		private void btnClick(object sender, System.EventArgs e)
		{
			Infragistics.Win.Misc.UltraButton btn = (Infragistics.Win.Misc.UltraButton)sender;
			POSNumericKeyPad.NumericKeyPad nkp = new POSNumericKeyPad.NumericKeyPad();
			
			nkp.ItembuttonClick(btn.Tag.ToString());
		}

		private void btnesc_Click(object sender, System.EventArgs e)
		{
			POSNumericKeyPad.NumericKeyPad nkp = new POSNumericKeyPad.NumericKeyPad();
			nkp.buttonClick("btClear");

		}

		private void frmNumericPad2_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
			//Configuration.SavePadSettings(this,0);			
		}


		private void frmNumericPad2_Resize(object sender, System.EventArgs e)
		{
			clsUIHelper.setFlowLayout(gbItemPad,5,5,2,2);
		}

		private void ebItemPad_ItemClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
		{
			POSNumericKeyPad.NumericKeyPad nkp = new POSNumericKeyPad.NumericKeyPad();
			
			nkp.ItembuttonClick(e.Item.Tag.ToString());
		}

		private void ebItemPad_GroupDragging(object sender, Infragistics.Win.UltraWinExplorerBar.CancelableGroupEventArgs e)
		{
			
		}

		private void ebItemPad_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ( e.Y < 30)
			{
				mouse_pos.X = e.X;
				mouse_pos.Y = e.Y;
				mouse_is_down=true; 
			}
		}

		private void ebItemPad_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ( mouse_is_down )
			{
				Point current_pos = Control.MousePosition;
				current_pos.X = current_pos.X - mouse_pos.X; // .Offset(mouseOffset.X, mouseOffset.Y);
				current_pos.Y = current_pos.Y - mouse_pos.Y;
				this.Location = current_pos;
			}
		}

		private void frmNumericPad2_Activated(object sender, System.EventArgs e)
		{
			//clsUIHelper.SETACTIVEWINDOW(99999999);
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void lblResize_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			mouse_pos2.X = e.X;
			mouse_pos2.Y = e.Y;
			mouse_is_down=true; 
		}

		private void lblResize_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			mouse_is_down=false;
		}

		private void lblResize_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				if ( mouse_is_down )
				{
					Size current_Size = this.Size;
					Point current_pos = Control.MousePosition;
					current_Size.Width = current_Size.Width -  (mouse_pos2.X - e.X); // .Offset(mouseOffset.X, mouseOffset.Y);
					current_Size.Height = current_Size.Height - (mouse_pos2.Y - e.Y);
					this.Size = current_Size;
				}
				
			}
			catch(Exception )
			{

			}
		}

		private void gbItemPad_Click(object sender, System.EventArgs e)
		{
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			
		}
	}
}
