using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Core_UI.Layout
{
    public static class MasterLayout
    {
        #region form properties
        public static AutoScaleMode oAutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        public static System.Drawing.Font oFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        public static System.Drawing.Color oBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
        public static System.Drawing.Color oForeColor = System.Drawing.Color.Black;
        public static System.Drawing.Size oClientSize = new System.Drawing.Size(800, 600);
        public static FormBorderStyle oFormBorderStyle = FormBorderStyle.FixedSingle;
        public static FormStartPosition oFormStartPosition = FormStartPosition.CenterScreen;
        //public static System.Drawing.Size atScaleBaseSize = new System.Drawing.Size(6, 14);
        #endregion

        #region ChildControl properties
        public static System.Drawing.Color lableForeColor = System.Drawing.Color.Black;
        public static System.Drawing.Color chkBoxForeColor = System.Drawing.Color.Black;
        public static System.Drawing.Color btnForeColor = System.Drawing.Color.Black;
        
        #endregion

    }
}
