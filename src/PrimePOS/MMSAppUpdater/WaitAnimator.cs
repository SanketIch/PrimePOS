using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.Misc;

namespace MMSAppUpdater
{
    // written by adeel shehzad 11-jan-2010
    public partial class WaitAnimator : UserControl
    {
        WaitLabelAnimation oAnim =null;
        WaitLabelProperties oprop1 = null;
        WaitLabelProperties oprop2 = null;
        WaitLabelProperties oprop3 = null;


        public WaitAnimator()
        {
            InitializeComponent();
        }

        private Color mBackcolorStep1 = Color.Tomato;
        private Color mBackcolorStep2 = Color.LightSalmon;
        private Color mBackcolorStep3 = Color.PeachPuff;

        /// <summary>
        /// IMPLEMENTED BY: Adeel Shehzad
        /// DATE: 11-jan-2010
        /// PURPOSE: To set same color to all labels
        /// LOGIC: 
        /// ASSUMPTION: 
        /// LAST MODIFIED: 
        /// LAST MODIFIED BY: 
        /// </summary>
        /// <param name="bc"> back color to be applied on all labels</param>
        private void setLabelColors(Color bc)
        {
            lblWait1.Appearance.BackColor = bc;
            lblWait2.Appearance.BackColor = bc;
            lblWait3.Appearance.BackColor = bc;
            lblWait4.Appearance.BackColor = bc;
            lblWait5.Appearance.BackColor = bc;
            lblWait6.Appearance.BackColor = bc;
        }

        public Color BackcolorStep1
        {
            get { return mBackcolorStep1; }
            set { mBackcolorStep1 = value; setLabelColors(mBackcolorStep1); }
        }

        public Color BackcolorStep2
        {
            get { return mBackcolorStep2; }
            set { mBackcolorStep2 = value; }
        }

        public Color BackcolorStep3
        {
            get { return mBackcolorStep3; }
            set { mBackcolorStep3 = value;  }
        }
        

        public int AnimationDelay
        {

            get { if (oAnim == null) return 110; return oAnim.AnimationDelay; }
            set { if (oAnim != null) oAnim.AnimationDelay = value; }
        }

        private void WaitAnimator_Load(object sender, EventArgs e)
        {
            oprop1 = new WaitLabelProperties(lblWait1.Size, mBackcolorStep1);
            oprop2 = new WaitLabelProperties(new Size(lblWait1.Size.Width, lblWait1.Size.Height + 6), mBackcolorStep2);
            oprop3 = new WaitLabelProperties(new Size(lblWait1.Size.Width, lblWait1.Size.Height + 12), mBackcolorStep3);
            if (oAnim == null) oAnim = new WaitLabelAnimation(oprop1, oprop2, oprop3);
            setLabelColors(mBackcolorStep1);
            oAnim.AddLabel(lblWait1);
            oAnim.AddLabel(lblWait2);
            oAnim.AddLabel(lblWait3);
            oAnim.AddLabel(lblWait4);
            oAnim.AddLabel(lblWait5);
            oAnim.AddLabel(lblWait6);
        }
        /// <summary>
        /// IMPLEMENTED BY: Adeel Shehzad
        /// DATE: 11-jan-2010
        /// PURPOSE: Start Labels Animation
        /// LOGIC: 
        /// ASSUMPTION: 
        /// LAST MODIFIED: 
        /// LAST MODIFIED BY: 
        /// </summary>

        public void startAnimation()
        {
            oAnim.startAnimation();
        }

        /// <summary>
        /// IMPLEMENTED BY: Adeel Shehzad
        /// DATE: 11-jan-2010
        /// PURPOSE: Stop Labels Animation
        /// LOGIC: 
        /// ASSUMPTION: 
        /// LAST MODIFIED: 
        /// LAST MODIFIED BY: 
        /// </summary>

        public void StopAnimation()
        {
            oAnim.StopAnimation();
        }

    }
}
