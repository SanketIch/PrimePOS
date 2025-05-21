using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS_Core_UI
{
    public partial class usrCustomDateTime : UserControl
    {
        //*************************************************************************************
        //*************************************************************************************
        // Created by: Nadeem Asif
        // Date Created: December 28, 2012

        //*************************************************************************************
        //*************************************************************************************

        #region Private Variables

        private string _sDateSselected = "-1";
        private bool bIsSettingControlValue = false;

        #endregion

        #region Constructors

        public usrCustomDateTime()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns Dates selected or no of days to be added or subtracted in current date.
        /// </summary>
        public string DateSelected
        {
            get { return _sDateSselected; }
            set 
            { 
                _sDateSselected = value;
                bIsSettingControlValue = true;
                SetControlValues();
                bIsSettingControlValue = false;

            }
        }

        #endregion

        #region Event Handlers

        private void cmbDateOptions_ValueChanged(object sender, EventArgs e)
        {
            if (bIsSettingControlValue)
                return;
            if (cmbDateOptions.SelectedItem.DataValue == "1")
            {
                _sDateSselected = "0";
                label1.Text = DateTime.Now.Date.ToString("d");// ("dd/MM/yyyy");
                label1.Visible = true;
                dtpDate.Visible = false;
                lblDays.Visible = false;
                ndDays.Visible = false;
                dtpDate.Visible = false;
            }
            else if (cmbDateOptions.SelectedItem.DataValue == "2")
            {
                _sDateSselected = (ndDays.Value * -1).ToString();
                ndDays.Visible = true;
                lblDays.Visible = true;
                label1.Visible = false;
                dtpDate.Visible = false;
                dtpDate.Visible = false;
            }
            else if (cmbDateOptions.SelectedItem.DataValue == "3")
            {
                _sDateSselected = ndDays.Value.ToString();
                ndDays.Visible = true;
                lblDays.Visible = true;
                label1.Visible = false;
                dtpDate.Visible = false;
                dtpDate.Visible = false;
            }
            else if (cmbDateOptions.SelectedItem.DataValue == "4")
            {
                _sDateSselected = dtpDate.Value.Date.ToString();// ("dd/MM/yyyy");
                //DateSelected = _sDateSselected;
                dtpDate.Visible = true;
                label1.Visible = false;
                lblDays.Visible = false;
                ndDays.Visible = false;
            }
        }

        private void UserControl2_Load(object sender, EventArgs e)
        {
            if (_sDateSselected.Equals("-1"))
                cmbDateOptions.SelectedIndex = 0;
            else
            {
                SetControlValues();
                bIsSettingControlValue = false;
            }
        }

        private void ndDays_ValueChanged(object sender, EventArgs e)
        {
            if (cmbDateOptions.SelectedItem.DataValue.ToString().Equals("2"))
                DateSelected = "-" + ndDays.Value.ToString();
            else if (cmbDateOptions.SelectedItem.DataValue.ToString().Equals("3"))
                DateSelected =  ndDays.Value.ToString();
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            DateSelected = dtpDate.Value.Date.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets values of controls as previously saved by user.
        /// </summary>
        private void SetControlValues()
        {
            bIsSettingControlValue = true;
            if (DateSelected.Equals("0"))
            {
                cmbDateOptions.SelectedIndex = 0;
                label1.Text = DateTime.Now.Date.ToString("d");// ("dd/MM/yyyy");
                label1.Visible = true;
                dtpDate.Visible = false;
                lblDays.Visible = false;
                ndDays.Visible = false;
                dtpDate.Visible = false;
            }
            else if (DateSelected.Contains("/"))
            {
                cmbDateOptions.SelectedIndex = 3;
                bIsSettingControlValue = false;
                dtpDate.Visible = true;
                dtpDate.Value = Convert.ToDateTime(DateSelected);
                dtpDate.Focus();
                label1.Visible = false;
                lblDays.Visible = false;
                ndDays.Visible = false;
            }
            else if (DateSelected.StartsWith("-"))
            {
                cmbDateOptions.SelectedIndex = 1;
                ndDays.Value = Convert.ToInt32(DateSelected) * -1;
                ndDays.Focus();
                ndDays.Visible = true;
                lblDays.Visible = true;
                label1.Visible = false;
                dtpDate.Visible = false;
                dtpDate.Visible = false;
            }
            else
            {
                cmbDateOptions.SelectedIndex = 2;
                ndDays.Value = Convert.ToInt32(DateSelected);
                ndDays.Focus();
                ndDays.Visible = true;
                lblDays.Visible = true;
                label1.Visible = false;
                dtpDate.Visible = false;
                dtpDate.Visible = false;
            }
        }

        #endregion

    }
}
