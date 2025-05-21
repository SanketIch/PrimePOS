using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinDock;
using System.Collections;
using Infragistics.Win.Misc;
using System.Threading;

namespace MMSAppUpdater
{
    // class Written By adeel 11-jan-2010
    public partial class frmWaitCursor : Form
    {

        public frmWaitCursor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// IMPLEMENTED BY: Adeel Shehzad
        /// DATE: 11-jan-2010
        /// PURPOSE: create WaitLabelAnimation class instance and add labels to be animated to it
        /// LOGIC: 
        /// ASSUMPTION: 
        /// LAST MODIFIED: 
        /// LAST MODIFIED BY: 
        /// </summary>
        private void frmWaitCursor_Load(object sender, EventArgs e)
        {
            WaitLabelProperties oprop1 = new WaitLabelProperties(lblWait1.Size, lblWait1.Appearance.BackColor);
            WaitLabelProperties oprop2 = new WaitLabelProperties(new Size(lblWait1.Size.Width, lblWait1.Size.Height + 6), Color.LightSalmon);
            WaitLabelProperties oprop3 = new WaitLabelProperties(new Size(lblWait1.Size.Width , lblWait1.Size.Height + 12), Color.PeachPuff);
            WaitLabelAnimation oAnim = new WaitLabelAnimation(oprop1, oprop2, oprop3);
            oAnim.AddLabel(lblWait1);
            oAnim.AddLabel(lblWait2);
            oAnim.AddLabel(lblWait3);
            oAnim.AddLabel(lblWait4);
            oAnim.AddLabel(lblWait5);
            oAnim.AddLabel(lblWait6);
            oAnim.startAnimation();
        }

        private void waitAnimator1_Load(object sender, EventArgs e)
        {

        }
    }







    // Written By adeel 13-jan-2010
    public class WaitLabelProperties
    {
        private Size mlabelSize = new Size();
        private Color mLabelColor = new Color();
        
        public Size labelSize
        {
            get { return mlabelSize; }
        }


        public Color LabelColor
        {
            get { return mLabelColor; }
        }

        public WaitLabelProperties(Size olabelSize, Color oLabelColor)
        {
            mlabelSize = olabelSize;
            mLabelColor = oLabelColor;
        }
    }

    // Written By adeel 13-jan-2010
    public class WaitLabelAnimation
    {
        System.Windows.Forms.Timer otmr = new System.Windows.Forms.Timer();
        private int iCurrAnimPosition = 0;
        private WaitLabelProperties lblstep1 = null;
        private WaitLabelProperties lblstep2 = null;
        private WaitLabelProperties lblstep3 = null;
        private List<UltraLabel> colLabels = new List<UltraLabel>();

        public List<UltraLabel> Labels
        {
            get { return colLabels; }
            set { colLabels = value; }
        }

        private int iAnimationDelay = 110;

        public int AnimationDelay
        {
            get { return iAnimationDelay; }
            set { iAnimationDelay = value; }
        }

        public void AddLabel(UltraLabel oItem)
        {
            colLabels.Add(oItem);
        }

        public void RemoveLabelAt(int index)
        {
            colLabels.RemoveAt(index);
        }

        public void RemoveLabel(UltraLabel oItem)
        {
            colLabels.Remove(oItem);
        }

        public WaitLabelAnimation(WaitLabelProperties lblProp1, WaitLabelProperties lblProp2, WaitLabelProperties lblProp3)
        {
            otmr.Interval = iAnimationDelay;
            otmr.Tick += new EventHandler(otmr_Tick);
            iCurrAnimPosition = 0;

            lblstep1 = lblProp1;
            lblstep2 = lblProp2;
            lblstep3 = lblProp3;
        }

        /// <summary>
        /// IMPLEMENTED BY: Adeel Shehzad
        /// DATE: 11-jan-2010
        /// PURPOSE: set properties on label by WaitLabelProperties object
        /// LOGIC: 
        /// ASSUMPTION: 
        /// LAST MODIFIED: 
        /// LAST MODIFIED BY: 
        /// </summary>
        /// <param name="olbl"> Label on which properties to be applied</param>
        /// <param name="oProp"> label Properties (WaitLabelProperties object) </param>
        /// <param name="oloc"> Location of Label</param>

        private void setLabelProperties(UltraLabel olbl, WaitLabelProperties oProp, Point oloc)
        {
            olbl.Location = oloc;
            olbl.Size = oProp.labelSize;
            olbl.Appearance.BackColor = oProp.LabelColor;
        }

        /// <summary>
        /// IMPLEMENTED BY: Adeel Shehzad
        /// DATE: 11-jan-2010
        /// PURPOSE: get location for step3 label (full sized label)
        /// LOGIC: 
        /// ASSUMPTION: 
        /// LAST MODIFIED: 
        /// LAST MODIFIED BY: 
        /// </summary>
        /// <param name="oPoint"> Position of orignal label (in actual size)</param>
        private Point getStep3location(Point oPoint)
        {
            Point opnt = new Point(oPoint.X , oPoint.Y - 6);
            return opnt;
        }

        /// <summary>
        /// IMPLEMENTED BY: Adeel Shehzad
        /// DATE: 11-jan-2010
        /// PURPOSE: get location for step2 label (full sized label)
        /// LOGIC: 
        /// ASSUMPTION: 
        /// LAST MODIFIED: 
        /// LAST MODIFIED BY: 
        /// </summary>
        /// <param name="oPoint"> Position of orignal label (in actual size)</param>

        private Point getStep2location(Point oPoint)
        {
            Point opnt = new Point(oPoint.X , oPoint.Y - 3);
            return opnt;
        }

        /// <summary>
        /// IMPLEMENTED BY: Adeel Shehzad
        /// DATE: 11-jan-2010
        /// PURPOSE: Aminate Labels by changing size and position of labels
        /// LOGIC: 
        /// ASSUMPTION: 
        /// LAST MODIFIED: 
        /// LAST MODIFIED BY: 
        /// </summary>

        void otmr_Tick(object sender, EventArgs e)
        {

            if (iCurrAnimPosition == 0)
            {
                setLabelProperties(colLabels[iCurrAnimPosition], lblstep3,getStep3location((Point)colLabels[iCurrAnimPosition].Tag));
            }
            else if (iCurrAnimPosition == 1)
            {
                setLabelProperties(colLabels[iCurrAnimPosition], lblstep3, getStep3location((Point)colLabels[iCurrAnimPosition].Tag));
                setLabelProperties(colLabels[iCurrAnimPosition - 1], lblstep2, getStep2location((Point)colLabels[iCurrAnimPosition-1].Tag));
            }
            else if (iCurrAnimPosition == colLabels.Count)
            {

                setLabelProperties(colLabels[iCurrAnimPosition - 1], lblstep2, getStep2location((Point)colLabels[iCurrAnimPosition-1].Tag));
                setLabelProperties(colLabels[iCurrAnimPosition - 2], lblstep1, (Point)colLabels[iCurrAnimPosition-2].Tag);
            }
            else if (iCurrAnimPosition == colLabels.Count + 1)
            {
                setLabelProperties(colLabels[iCurrAnimPosition - 2], lblstep1, (Point)colLabels[iCurrAnimPosition-2].Tag);
            }
            else
            {
                setLabelProperties(colLabels[iCurrAnimPosition], lblstep3, getStep3location((Point)colLabels[iCurrAnimPosition].Tag));
                setLabelProperties(colLabels[iCurrAnimPosition - 1], lblstep2, getStep2location((Point)colLabels[iCurrAnimPosition-1].Tag));
                setLabelProperties(colLabels[iCurrAnimPosition - 2], lblstep1, (Point)colLabels[iCurrAnimPosition-2].Tag);
            }


            iCurrAnimPosition++;

            if (iCurrAnimPosition == colLabels.Count + 2)
            {
                Thread.Sleep(500);
                iCurrAnimPosition = 0;
            }
        }

        /// <summary>
        /// IMPLEMENTED BY: Adeel Shehzad
        /// DATE: 11-jan-2010
        /// PURPOSE: Save Actual position of labels in tags Properties (as we will change the size and position of label when animation starts)
        /// LOGIC: 
        /// ASSUMPTION: 
        /// LAST MODIFIED: 
        /// LAST MODIFIED BY: 
        /// </summary>
        private void SetTags()
        {
            foreach(UltraLabel olbl in colLabels)
            {
                olbl.Tag = olbl.Location;
            }
        }

        /// <summary>
        /// IMPLEMENTED BY: Adeel Shehzad
        /// DATE: 11-jan-2010
        /// PURPOSE: Start the animation timer 
        /// LOGIC: 
        /// ASSUMPTION: 
        /// LAST MODIFIED: 
        /// LAST MODIFIED BY: 
        /// </summary>

        public void startAnimation()
        {
            SetTags();
            otmr.Start();
        }

        /// <summary>
        /// IMPLEMENTED BY: Adeel Shehzad
        /// DATE: 11-jan-2010
        /// PURPOSE: Stop Animation Timer and set all labels in actual size and position 
        /// LOGIC: 
        /// ASSUMPTION: 
        /// LAST MODIFIED: 
        /// LAST MODIFIED BY: 
        /// </summary>

        public void StopAnimation()
        {
            foreach (UltraLabel olbl in colLabels)
            {
                setLabelProperties(olbl, lblstep1, (Point) olbl.Tag);
            }
            otmr.Stop();
        }

    }
 
}