using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using POS_Core.Resources;
//using POS_Core.DataAccess;

namespace POS_Core_UI
{
    
    public partial class ctrlExcludeData : UserControl
    {
        public delegate void AddDataHandler();    
        public event AddDataHandler AddDataEvent;

        private BindingList<IdDescriptionData> _data=new BindingList<IdDescriptionData>();
        private List<String> _deletedData = new List<String>();
        private bool _isChanged = false;

        public ctrlExcludeData()
        {
            InitializeComponent();
            this.grdData.DataSource = _data;
            clsUIHelper.SetReadonlyRow(this.grdData);
            clsUIHelper.SetAppearance(this.grdData);
            try
            {
                this.grdData.DisplayLayout.Bands[0].Columns["IsChecked"].Header.Caption = "Exclude Coupon Payment";
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private void CheckUncheckGridRow(UltraGridCell oCell)
        {
            if ((bool)oCell.Value == false)
            {
                oCell.Value = true;
            }
            else
            {
                oCell.Value = false;
            }
            oCell.Row.Update();
        }
        public void AddData(string id, string description,bool exlfromCPay)
        {
            if ( _data.SingleOrDefault(s => s.Id == id)!=null) return;

            _data.Add(new IdDescriptionData(id, description, exlfromCPay));
            _deletedData.Remove(id);
        }

        public ExcludeData GetData()
        {
            ExcludeData IdsData = new ExcludeData();
            IdsData.Data.AddRange( from data in _data select data.Id);
            IdsData.IsDataChanged = _isChanged;
            return IdsData;
        }

        public ExcludeData GetSelectedData()
        {
            ExcludeData IdsData = new ExcludeData();
            IdsData.Data.AddRange(from data in _data where data.isChecked == true select data.Id);
            IdsData.IsDataChanged = _isChanged;
            return IdsData;
        }

        public List<string> GetDeletedData()
        {
            return _deletedData;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (AddDataEvent != null)
            {
                int items = _data.Count;
                AddDataEvent();
                if (_data.Count > items)
                {
                    _isChanged = true;
                }
            }
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (this.grdData.Selected.Rows.Count > 0)
            {
                foreach (UltraGridRow row in grdData.Selected.Rows)
                {
                    _deletedData.Add(row.Cells["id"].Text);   
                }
                grdData.DeleteSelectedRows(false);
                _isChanged = true;
            }
        }

        private void ctrlExcludeData_Resize(object sender, EventArgs e)
        {
            this.groupBox1.Width= this.grdData.Width = this.Width;
        }

        public void ShowIdColumn()
        {
            this.grdData.DisplayLayout.Bands[0].Columns["id"].Hidden = false;
        }

        public void SetIdColumnCaption(string caption)
        {
            this.grdData.DisplayLayout.Bands[0].Columns["id"].Header.Caption = caption;
        }

        public void SetDescriptionColumnCaption(string caption)
        {
            this.grdData.DisplayLayout.Bands[0].Columns["description"].Header.Caption = caption;
        }

        private void grdData_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

        }

        private void grdData_ClickCell(object sender, ClickCellEventArgs e)
        {
            if (e.Cell.Column.Key.ToUpper() == "ISCHECKED")
            {
                CheckUncheckGridRow(e.Cell);
            }
        }

        private void grdData_MouseClick(object sender, MouseEventArgs e)
        {
           
        }

    }

    public class IdDescriptionData
    {
        public IdDescriptionData(string key, string value,bool isSelect)
        {
            Id = key;
            Description = value;
            isChecked = isSelect;
        }

        public string Id { get; set; }
        public string Description { get; set; }
        public bool isChecked { get; set; }
    }
}
