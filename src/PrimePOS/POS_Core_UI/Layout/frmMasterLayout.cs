using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;


namespace POS_Core_UI.Layout
{
    public partial class frmMasterLayout : Form, INotifyPropertyChanged
    {
        #region InotifyProperty Changed Fields
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        public frmMasterLayout()
        {
            InitializeComponent();
            SetProperties();

        }

        private void SetProperties()
        {
            this.AutoScaleMode = MasterLayout.oAutoScaleMode;
            this.Font = MasterLayout.oFont;
            this.BackColor = MasterLayout.oBackColor;
            this.ClientSize = MasterLayout.oClientSize;
            this.ForeColor = MasterLayout.oForeColor;
            this.FormBorderStyle = MasterLayout.oFormBorderStyle;
            this.StartPosition = MasterLayout.oFormStartPosition;
        }

        public void setChildControlProperties(Control formControls)
        {
            //PrimePOS-2523 Added by Farman Ansari from here on 24 May 2018 
            clsUIHelper.setColorSchecme(this);
            //PrimePOS-2523 Added by Farman Ansari till here on 24 May 2018
            //foreach (Control control in formControls.Controls)
            //{

            //    if (control.Controls.Count > 0)
            //    {
            //        setChildControlProperties(control);
            //    }
            //    if (control.GetType() == typeof(Infragistics.Win.Misc.UltraLabel))
            //    {
            //        ((Infragistics.Win.Misc.UltraLabel)control).Appearance.ForeColor = MasterLayout.lableForeColor;
            //    }
            //    if (control.GetType() == typeof(Infragistics.Win.UltraWinEditors.UltraCheckEditor))
            //    {
            //        ((Infragistics.Win.UltraWinEditors.UltraCheckEditor)control).Appearance.ForeColor = MasterLayout.chkBoxForeColor;
            //    }
            //    if (control.GetType() == typeof(Infragistics.Win.Misc.UltraButton))
            //    {
            //        ((Infragistics.Win.Misc.UltraButton)control).Appearance.ForeColor = MasterLayout.btnForeColor;
            //    }

            //}

        }

        //Added logic to allign data in ultragrid 
        public void TextAllignmentForGrid(UltraGrid oUltraGrid)
        {
            foreach (UltraGridColumn oCol in oUltraGrid.DisplayLayout.Bands[0].Columns)
            {
                //oCol.Width = oCol.CalculateAutoResizeWidth(PerformAutoSizeType.VisibleRows, true);
                if (oCol.DataType.Equals(typeof(System.Int32)) || oCol.DataType.Equals(typeof(System.Decimal)) || oCol.DataType.Equals(typeof(System.DateTime)))
                {
                    oCol.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }
            }
        }        
    }
}
