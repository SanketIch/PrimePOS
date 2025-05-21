using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MMSAppUpdater
{
    public partial class frmStationsSettings : Form
    {
        private bool mouse_is_down = false;
        private Point mouse_pos;
        contStationSettings oContStations = null;
        DataView dvStations;
        public frmStationsSettings(clsUpdateManager oUpdateManager)
        {
            InitializeComponent();
            this.oContStations = new contStationSettings(oUpdateManager);
        }

        private void btnWindowClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
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

        private void lblMessage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                Point current_pos = Control.MousePosition;
                current_pos.X = current_pos.X - mouse_pos.X; // .Offset(mouseOffset.X, mouseOffset.Y);
                current_pos.Y = current_pos.Y - mouse_pos.Y;
                this.Location = current_pos;
            }
        }

        private void lblMessage_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_is_down = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmStationsSettings_Load(object sender, EventArgs e)
        {
            this.cbAppFilter.DataSource = oContStations.GetApplicationList();
            this.cbAppFilter.DataBind();
            PapolateGrid();
        }
        void PapolateGrid()
        {
          DataTable tblStations= oContStations.PopulateStationTable();
          dvStations = tblStations.DefaultView;
          dgUpdates.DataSource = dvStations;
          dgUpdates.DisplayLayout.Bands[0].Columns["UpdateType"].Hidden = true;
        }

        private void btnDownloadInstall_Click(object sender, EventArgs e)
        {
            bool Result = false;
            if (txtStationID.Text.Trim().Length != 0)
            {
                if (dgUpdates.Selected != null && dgUpdates.Selected.Rows.Count>0)
                {
                    string ApplicationName = dgUpdates.Selected.Rows[0].Cells["AppName"].Value.ToString();
                    Result = oContStations.UpdateSystemStation(dgUpdates.Selected.Rows[0].Cells["SatationId"].Value.ToString(), txtStationID.Text.Trim(), ApplicationName);
                }
            }
            if (Result)
            {
                MessageBox.Show(this, "Station Updated Sucessfully", "MMS POS APP Updater");
                txtStationID.Text = "";
                PapolateGrid();
            }
        }

        private void SMDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool Result = false;
                if (dgUpdates.Selected != null && dgUpdates.Selected.Rows.Count > 0)
                {
                    string ApplicationName = dgUpdates.Selected.Rows[0].Cells["AppName"].Value.ToString();
                    Result = oContStations.DeleteStation(dgUpdates.Selected.Rows[0].Cells["StationId"].Value.ToString(), txtStationID.Text.Trim(), ApplicationName);
                }

                if (Result)
                {
                    MessageBox.Show(this, "Station Deleted Sucessfully", "MMS POS APP Updater");
                    txtStationID.Text = "";
                    PapolateGrid();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cbAppFilter_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                dvStations.RowFilter = null;
                if(!string.IsNullOrEmpty(cbAppFilter.SelectedText))
                {
                    dvStations.RowFilter = "APPNAME='" + cbAppFilter.SelectedText + "'";
                }
                dgUpdates.DataSource = dvStations;
            }
            catch (Exception ex)
            {

            }
            
        }
    }
}