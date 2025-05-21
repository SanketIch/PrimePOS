using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace MMSAppUpdater
{
    internal partial class frmUpdate : Form
    {
        private bool mouse_is_down = false;
        private Point mouse_pos;
      
       public DataTable odt = null;
       public List<clsAppUpdate> oApps = null;
       ContUpdate oContUpdate = null;
        clsLogService log = null;
        Thread oThread;
       public frmUpdate(clsUpdateManager oManagerObject)
        {
            InitializeComponent();
            oContUpdate = new ContUpdate(oManagerObject);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = true;
            waMain.startAnimation();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            waMain.StopAnimation();
        }

        protected override void WndProc(ref Message m)
        {

            base.WndProc(ref m);



            if (m.Msg == 0x84)
            {



                if (m.Result.ToInt32() == 1)

                    m.Result = (IntPtr)2;

            }

        }


        private void button3_Click(object sender, EventArgs e)
        {
        }

    
       
        private void FillUpdatesGrid()
        {
           odt=oContUpdate.PopulateGridTable(); /// get the table 
            //Changes by Qamar Abbas on 29-September-2011
            //displaying a messsage if there is no update available
           if (odt == null || odt.Rows.Count == 0)
           {
               MessageBox.Show("No updates are available", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
               this.Close();
               return;
           }
            dgUpdates.DataSource = odt;
            dgUpdates.DisplayLayout.Bands[0].Columns["Update"].CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            //dgUpdates.DisplayLayout.Bands[0].Columns["Application Path"].EditorControl = txtBrowse;
        }

        private void frmPrimeRxUpdate_Load(object sender, EventArgs e)
        {
            try
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                FillUpdatesGrid();
            }
            catch (Exception exp)
            {
                throw(exp);
            }
        }

        private void btnWindowClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(oThread!=null && oThread.IsAlive)
            oThread.Abort();
            this.Close();
        }

       

        private void SynchronizeChanges()
        {
            oContUpdate.SynchronizeChanges(odt);
        }
     


        private void btnDownloadInstall_Click(object sender, EventArgs e)
        {
            try
            {
                label1.Visible = true;
                waMain.startAnimation();
                SynchronizeChanges();
                ThreadStart oThreadFunction = new ThreadStart(DownloadandInstall);
                oThread = new Thread(oThreadFunction);
                oThread.Start();
               
            }
            catch (Exception exp)
            {
                waMain.StopAnimation();
                throw(exp);
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void lblMessage_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_pos.X = e.X;
            mouse_pos.Y = e.Y;
            mouse_is_down = true;

        }

        private void lblMessage_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_is_down = false;

        }

        private void lblMessage_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_is_down)
            {
                Point current_pos = Control.MousePosition;
                current_pos.X = current_pos.X - mouse_pos.X; // .Offset(mouseOffset.X, mouseOffset.Y);
                current_pos.Y = current_pos.Y - mouse_pos.Y;
                this.Location = current_pos;
            }
        }

        private void DownloadandInstall()
        {

           // if (lblStatus.Tag != null) lblStatus.Text = lblStatus.Tag.ToString();
            if (oContUpdate.DownloadAndInstall(this))
            {
                waMain.StopAnimation();
                MessageBox.Show(this, "Installation Completed Sucessfully", "MMS POS APP Updater");
            }
            else
                MessageBox.Show(this, "Error Occured During Installation", "MMS POS APP Updater");
            
              this.DialogResult = DialogResult.OK;
        }

        //private void txtBrowse_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        //{
        //    OpenFileDialog ObjFileDiag = new OpenFileDialog();
        //    ObjFileDiag.Title = "Please Select the Application File Path";
        //    ObjFileDiag.Filter = "*.exe|*.exe";
        //    if (ObjFileDiag.ShowDialog() == DialogResult.OK)
        //    {

        //        string AppVersionName = dgUpdates.Rows[dgUpdates.ActiveCell.Row.Index].Cells["AppName"].Value.ToString();
        //        string AppVersion = dgUpdates.Rows[dgUpdates.ActiveCell.Row.Index].Cells["Version"].Value.ToString();
        //        if (AppVersion != null && AppVersionName!=null )
        //        {
        //            dgUpdates.ActiveCell.Value = System.IO.Path.GetDirectoryName(ObjFileDiag.FileName);
        //            FileVersionInfo oFileInforamtion = FileVersionInfo.GetVersionInfo(ObjFileDiag.FileName);
        //            if (System.IO.Path.GetFileName(oFileInforamtion.FileName).ToLower() != AppVersionName)
        //            {
        //                MessageBox.Show("Application name does not matche", "MMS APP Updater");
        //            }
        //            else
        //            {
        //                if (oFileInforamtion.FileVersion != AppVersion.ToString())
        //                {
        //                  dgUpdates.Rows[dgUpdates.ActiveCell.Row.Index].Cells["Update"].Value = true;
        //                }
        //                else
        //                {
        //                  dgUpdates.Rows[dgUpdates.ActiveCell.Row.Index].Cells["Update"].Value = false;
        //                }
        //            }
        //        }

               
        //    }
        //}
    }
}