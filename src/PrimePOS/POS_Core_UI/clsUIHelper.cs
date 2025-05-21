using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using POS_Core.CommonData;
using POS_Core_UI.Reports.Reports;
//using POS_Core_UI.Reports.ReportsUI;
//using POS_Core.DataAccess;
using POS_Core.UserManagement;
using POS_Core.ErrorLogging;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using POS_Core_UI.UserManagement;
using POS_Core_UI.Reports.ReportsUI;
using POS_Core.Resources;
using POS_Core.BusinessRules;
using POS_Core.CommonData.Rows;

namespace POS_Core_UI
{
    /// <summary>
    /// This class will work as a base class for all Classes derived from this class.This class implements basic functionality.To implement additional functional just derive a new class from it and implement it.
    /// </summary>
    ///
    public class clsUIHelper
    {
        /// <summary>
        /// constructor for base class
        /// </summary>
        ///

        [DllImport("user32.dll")]
        private static extern int SetParent(IntPtr hwndChild, IntPtr hWindParent);

        [DllImport("user32.dll")]
        private static extern int SetFocus(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern long SetActiveWindow(IntPtr hwnd);

        private static Random oRandom = new Random();
        private static bool isOk = false;

        public static string DefaultValue = string.Empty; //Cahge by SRT (sachin) Dated : 10 Nov 2010
        public static void SetParentWindow(IntPtr hwindChild, IntPtr hwindParent)
        {
            SetParent(hwindChild, hwindParent);
        }

        public static void SETACTIVEWINDOW(IntPtr hwind)
        {
            SetActiveWindow(hwind);
        }

        public static void SetWindowFocus(IntPtr hwind)
        {
            SetFocus(hwind);
        }

        public static bool SetDefaultColor = false;
        public static System.Windows.Forms.Form CurrentForm;
        //private static POS_Core_UI.Reports.ReportsUI.frmReportViewer oRptViewer = new POS_Core_UI.Reports.ReportsUI.frmReportViewer(); //JY 19-Sep-2014 commented as it is not in use
        private static clsLogin m_oLogin=null;

        /// <summary>
        /// Get or Set if F10 is press and SkipF10Sign is check in Preference
        /// </summary>
        public static bool IsF10Trans
        {
            //Added by Manoj 9/27/2013
            get;
            set;
        }

        private clsUIHelper() { }

        public static bool LocakStation()
        {
            Logs.Logger(clsPOSDBConstants.Log_Module_Login, "LocakStation() ","For user"+ Configuration.UserName +"Entering");
            try
            {
                if(m_oLogin == null)
                {
                    m_oLogin = new clsLogin();
                }

                m_oLogin.ConnString = POS_Core.Resources.Configuration.ConnectionString;
                m_oLogin.login(LoginENUM.Lock);
                frmMain.getInstance().ApplyUserPriviliges();
                m_oLogin = null;
                Logs.Logger(clsPOSDBConstants.Log_Module_Login, "LocakStation() ", "For user" + Configuration.UserName + "Exited");
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region JY 19-Sep-2014 commented as it is not in use
        //public static POS_Core_UI.Reports.ReportsUI.frmReportViewer ReportViewer
        //{
        //    get
        //    {
        //        if(oRptViewer == null)
        //            oRptViewer = new POS_Core_UI.Reports.ReportsUI.frmReportViewer();
        //        return oRptViewer;
        //    }
        //}
        #endregion

        public static void setFlowLayout(Control oCCtrl, int topMargin, int leftMargin, int HorizontalGap, int Verticalgap)
        {
            int controlwidth = oCCtrl.Width;
            int controlHeight = oCCtrl.Height;

            Control oCtrl=null;
            int currLeft = leftMargin;
            int currTop = topMargin;

            if(oCCtrl.Controls.Count > 0)
            {
                Control firstCtrl = oCCtrl.Controls[0];
                firstCtrl.Left = currLeft;
                firstCtrl.Top = currTop;

                currLeft = currLeft + firstCtrl.Width + HorizontalGap;
            }

            for(int index = 1; index < oCCtrl.Controls.Count; index++)
            {
                oCtrl = oCCtrl.Controls[index];

                if((currLeft + leftMargin + oCtrl.Width) >= controlwidth)
                {
                    currLeft = leftMargin;
                    currTop = currTop + oCtrl.Height + Verticalgap;
                }

                oCtrl.Left = currLeft;
                oCtrl.Top = currTop;

                currLeft = currLeft + oCtrl.Width + HorizontalGap;

                //if (index<oCCtrl.Controls.Count)
                //oCCtrl.Controls[index];
            }
        }

        public bool IsNumeric(string str)
        {
            try
            {
                Int32.Parse(str);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool IsOK
        {
            set
            {
                isOk = value;
            }
            get
            {
                return isOk;
            }
        }

        public static void ShowWelcomeMessage()
        {
            /*if (Configuration.CPOSSet.PD_MSG.Length>Configuration.CPOSSet.PD_LINELEN)
            {
                String strData;
                String []str=Configuration.CPOSSet.PD_MSG.Split(" ");
                int lineNo=0;
                for (int i=0;i<str.GetUpperBound(0);i++)
                {
                    if (str[i].Length+strData.Length>Configuration.CPOSSet.PD_LINELEN)
                    {
                    }
                }
            }
            */
            frmMain.PoleDisplay.ClearPoleDisplay();
            frmMain.PoleDisplay.WriteToPoleDisplay(Configuration.CPOSSet.PD_MSG);
        }

        public static string Spaces(int count)
        {
            string str;
            str = "";
            for(int i=1; i <= count; i++)
            {
                str += " ";
            }
            return str;
        }

      //  public static void setColorSchecme(Control oCCtrl)
      //  {
      //      //if (oCCtrl != null && oCCtrl.GetType().BaseType == typeof(System.Windows.Forms.Form) && !string.IsNullOrWhiteSpace(oCCtrl.Tag.ToString()) && oCCtrl.Tag.ToString() == "CUSTOM")
      //      //    return;

      //      POS_Core.ErrorLogging.Logs.Logger("Setting UI", "setColorSchecme()", clsPOSDBConstants.Log_Entering);
      //      if(Configuration.Window_Color == null)
      //      {
      //          return;
      //      }

      //      string [] Window_BackColor=Configuration.Window_Color.Split(',');
      //      string [] Window_Button_Color1=Configuration.Window_Button_Color1.Split(',');
      //      string [] Window_Button_Color2= Configuration.Window_Button_Color2.Split(',');
      //      string [] Window_ForeColor=Configuration.Window_ForeColor.Split(',');
      //      string [] Window_Button_ForeColor=Configuration.Window_Button_ForeColor.Split(',');

      //      string [] Active_BackColor=Configuration.Active_BackColor.Split(',');
      //      string [] Active_ForeColor=Configuration.Active_ForeColor.Split(',');

      //      string [] Header_BackColor=Configuration.Header_BackColor.Split(',');
      //      string [] Header_ForeColor=Configuration.Header_ForeColor.Split(',');

      //      try
      //      {
      //          if(oCCtrl.GetType().BaseType == typeof(System.Windows.Forms.Form))
      //          {
      //              if (oCCtrl.Tag!=null  && oCCtrl.Tag.ToString() == "CUSTOM")
      //                  return;

      //              oCCtrl.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));  //sets the form back ground color
      //              oCCtrl.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //              if(oCCtrl.Name == "frmMain")
      //              {
      //                  #region frmMain

      //                  // The following code was commented by AKBAR.1/20/2011
      //                  /*
						//((frmMain)oCCtrl).ultMenuBar.Appearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
						//((frmMain)oCCtrl).ultMenuBar.Appearance.BackColorDisabled=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));

						//((frmMain)oCCtrl).ultMenuBar.DockAreaAppearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
						//((frmMain)oCCtrl).ultMenuBar.DockAreaAppearance.BackColorDisabled=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));

						//((frmMain)oCCtrl).ultMenuBar.Toolbars["POS"].Settings.Appearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
						//((frmMain)oCCtrl).ultMenuBar.Toolbars["POS"].Settings.Appearance.BackColorDisabled=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));

						//Infragistics.Win.UltraWinToolbars.PopupMenuTool oMenu ;

						//oMenu =(Infragistics.Win.UltraWinToolbars.PopupMenuTool) ((frmMain)oCCtrl).ultMenuBar.Tools["POS_Terminal"];
						//oMenu.Settings.Appearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
						//oMenu.Settings.Appearance.BackColorDisabled=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
						//oMenu.Settings.Appearance.ForeColor=Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]),Convert.ToInt32(Window_ForeColor[1]),Convert.ToInt32(Window_ForeColor[2]));
						//oMenu.Settings.Appearance.ForeColorDisabled=Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]),Convert.ToInt32(Window_ForeColor[1]),Convert.ToInt32(Window_ForeColor[2]));
						//oMenu.Settings.IconAreaAppearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]),Convert.ToInt32(Window_Button_Color1[1]),Convert.ToInt32(Window_Button_Color1[2]));

						//oMenu =(Infragistics.Win.UltraWinToolbars.PopupMenuTool) ((frmMain)oCCtrl).ultMenuBar.Tools["Inventory_Management"];
						//oMenu.Settings.Appearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
						//oMenu.Settings.Appearance.BackColorDisabled=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
						//oMenu.Settings.Appearance.ForeColor=Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]),Convert.ToInt32(Window_ForeColor[1]),Convert.ToInt32(Window_ForeColor[2]));
						//oMenu.Settings.Appearance.ForeColorDisabled=Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]),Convert.ToInt32(Window_ForeColor[1]),Convert.ToInt32(Window_ForeColor[2]));
						//oMenu.Settings.IconAreaAppearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]),Convert.ToInt32(Window_Button_Color1[1]),Convert.ToInt32(Window_Button_Color1[2]));

						//oMenu =(Infragistics.Win.UltraWinToolbars.PopupMenuTool) ((frmMain)oCCtrl).ultMenuBar.Tools["Administrative_Functions"];
						//oMenu.Settings.Appearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
						//oMenu.Settings.Appearance.BackColorDisabled=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
						//oMenu.Settings.Appearance.ForeColor=Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]),Convert.ToInt32(Window_ForeColor[1]),Convert.ToInt32(Window_ForeColor[2]));
						//oMenu.Settings.Appearance.ForeColorDisabled=Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]),Convert.ToInt32(Window_ForeColor[1]),Convert.ToInt32(Window_ForeColor[2]));
						//oMenu.Settings.IconAreaAppearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]),Convert.ToInt32(Window_Button_Color1[1]),Convert.ToInt32(Window_Button_Color1[2]));

						//Infragistics.Win.UltraWinToolbars.UltraToolbar oToolbar;
						//oToolbar=((frmMain)oCCtrl).ultMenuBar.Toolbars["tlbPOSTerminal"];
						//oToolbar.Settings.Appearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]),Convert.ToInt32(Window_Button_Color1[1]),Convert.ToInt32(Window_Button_Color1[2]));
						//oToolbar.Settings.Appearance.BackColor2=Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]),Convert.ToInt32(Window_Button_Color2[1]),Convert.ToInt32(Window_Button_Color2[2]));
						//oToolbar.Settings.Appearance.BackColorDisabled=Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]),Convert.ToInt32(Window_Button_Color1[1]),Convert.ToInt32(Window_Button_Color1[2]));
						//oToolbar.Settings.Appearance.BackColorDisabled2=Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]),Convert.ToInt32(Window_Button_Color2[1]),Convert.ToInt32(Window_Button_Color2[2]));
						//oToolbar.Settings.Appearance.BackGradientStyle=GradientStyle.Vertical;

						//oToolbar.Settings.Appearance.ForeColor=Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]),Convert.ToInt32(Window_Button_ForeColor[1]),Convert.ToInt32(Window_Button_ForeColor[2]));
						//oToolbar.Settings.Appearance.ForeColorDisabled=Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]),Convert.ToInt32(Window_Button_ForeColor[1]),Convert.ToInt32(Window_Button_ForeColor[2]));

						//oToolbar=((frmMain)oCCtrl).ultMenuBar.Toolbars["tlbInventoryManagement"];
						//oToolbar.Settings.Appearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]),Convert.ToInt32(Window_Button_Color1[1]),Convert.ToInt32(Window_Button_Color1[2]));
						//oToolbar.Settings.Appearance.BackColor2=Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]),Convert.ToInt32(Window_Button_Color2[1]),Convert.ToInt32(Window_Button_Color2[2]));
						//oToolbar.Settings.Appearance.BackColorDisabled=Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]),Convert.ToInt32(Window_Button_Color1[1]),Convert.ToInt32(Window_Button_Color1[2]));
						//oToolbar.Settings.Appearance.BackColorDisabled2=Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]),Convert.ToInt32(Window_Button_Color2[1]),Convert.ToInt32(Window_Button_Color2[2]));
						//oToolbar.Settings.Appearance.BackGradientStyle=GradientStyle.Vertical;

						//oToolbar.Settings.Appearance.ForeColor=Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]),Convert.ToInt32(Window_Button_ForeColor[1]),Convert.ToInt32(Window_Button_ForeColor[2]));
						//oToolbar.Settings.Appearance.ForeColorDisabled=Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]),Convert.ToInt32(Window_Button_ForeColor[1]),Convert.ToInt32(Window_Button_ForeColor[2]));

						//oToolbar=((frmMain)oCCtrl).ultMenuBar.Toolbars["tlbAdministrativeFunction"];
						//oToolbar.Settings.Appearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]),Convert.ToInt32(Window_Button_Color1[1]),Convert.ToInt32(Window_Button_Color1[2]));
						//oToolbar.Settings.Appearance.BackColor2=Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]),Convert.ToInt32(Window_Button_Color2[1]),Convert.ToInt32(Window_Button_Color2[2]));
						//oToolbar.Settings.Appearance.BackColorDisabled=Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]),Convert.ToInt32(Window_Button_Color1[1]),Convert.ToInt32(Window_Button_Color1[2]));
						//oToolbar.Settings.Appearance.BackColorDisabled2=Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]),Convert.ToInt32(Window_Button_Color2[1]),Convert.ToInt32(Window_Button_Color2[2]));
						//oToolbar.Settings.Appearance.BackGradientStyle=GradientStyle.Vertical;

						//oToolbar.Settings.Appearance.ForeColor=Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]),Convert.ToInt32(Window_Button_ForeColor[1]),Convert.ToInt32(Window_Button_ForeColor[2]));
						//oToolbar.Settings.Appearance.ForeColorDisabled=Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]),Convert.ToInt32(Window_Button_ForeColor[1]),Convert.ToInt32(Window_Button_ForeColor[2]));

						////((frmMain)oCCtrl).ultMenuBar.MenuSettings.HotTrackAppearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
						////((frmMain)oCCtrl).ultMenuBar.MenuSettings.ToolAppearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
						////((frmMain)oCCtrl).ultMenuBar.MenuSettings.ToolAppearance.BackColorDisabled=Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
						////((frmMain)oCCtrl).ultMenuBar.MenuSettings.ToolAppearance.ForeColor=Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]),Convert.ToInt32(Window_ForeColor[1]),Convert.ToInt32(Window_ForeColor[2]));
						////((frmMain)oCCtrl).ultMenuBar.MenuSettings.ToolAppearance.ForeColorDisabled=Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]),Convert.ToInt32(Window_ForeColor[1]),Convert.ToInt32(Window_ForeColor[2]));

						////((frmMain)oCCtrl).ultMenuBar.MenuSettings.IconAreaAppearance.BackColor=Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]),Convert.ToInt32(Window_Button_Color1[1]),Convert.ToInt32(Window_Button_Color1[2]));
						////((frmMain)oCCtrl).ultMenuBar.MenuSettings.IconAreaAppearance.BackColorDisabled=Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]),Convert.ToInt32(Window_Button_Color1[1]),Convert.ToInt32(Window_Button_Color1[2]));
      //                   * */

      //                  #endregion frmMain
      //              }
      //              else
      //              {
      //                  System.Resources.ResourceManager rs=new System.Resources.ResourceManager(typeof(UserManagement.frmLogin));
      //                  ((Form) oCCtrl).Icon = (System.Drawing.Icon) rs.GetObject("$this.Icon");
      //                  //if (((Form) oCCtrl).FormBorderStyle!=FormBorderStyle.None)
      //                  //	((Form) oCCtrl).Icon= new Icon(oCCtrl.GetType(),"prime_pos.ico");
      //              }
      //          }

      //          foreach(Control oCtrl in oCCtrl.Controls)
      //          {
      //              if(oCtrl.Name == "UltraStatusBar")
      //              {
      //              }

      //              if(oCtrl.Name.IndexOf("Version") >= 0)
      //              {
      //              }
      //              if(oCtrl.Tag != "")
      //              {
      //              }

      //              Console.WriteLine(oCtrl.Name);
      //              //panels color
      //              if(oCtrl.GetType() == typeof(System.Windows.Forms.Panel))
      //              {
      //                  #region win panel

      //                  if(oCtrl.Tag != "NOCOLOR")
      //                  {
      //                      ((System.Windows.Forms.Panel) oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((System.Windows.Forms.Panel) oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //                  }

      //                  #endregion win panel
      //              }
      //              else if(oCtrl.GetType() == typeof(Infragistics.Win.Misc.UltraButton))
      //              {
      //                  #region ultra button

      //                  if(oCtrl.Tag != "NOCOLOR")
      //                  {
      //                      ((Infragistics.Win.Misc.UltraButton) oCtrl).SupportThemes = false;
      //                      ((Infragistics.Win.Misc.UltraButton) oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
      //                      ((Infragistics.Win.Misc.UltraButton) oCtrl).Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.Misc.UltraButton) oCtrl).Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
      //                      ((Infragistics.Win.Misc.UltraButton) oCtrl).Appearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.Misc.UltraButton) oCtrl).ButtonStyle = UIElementButtonStyle.Flat;

      //                      if(((Infragistics.Win.Misc.UltraButton) oCtrl).Appearance.BackGradientStyle == GradientStyle.None)
      //                          ((Infragistics.Win.Misc.UltraButton) oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      else
      //                          ((Infragistics.Win.Misc.UltraButton) oCtrl).Appearance.BackGradientStyle = GradientStyle.Vertical;

      //                      ((Infragistics.Win.Misc.UltraButton) oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
      //                      ((Infragistics.Win.Misc.UltraButton) oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));

      //                      ((Infragistics.Win.Misc.UltraButton) oCtrl).HotTrackAppearance.BackColor = Color.FromArgb(188, 193, 207);
      //                      ((Infragistics.Win.Misc.UltraButton) oCtrl).HotTrackAppearance.BackColor2 = Color.FromArgb(188, 193, 207);
      //                      ((Infragistics.Win.Misc.UltraButton) oCtrl).HotTracking = true;
      //                  }

      //                  #endregion ultra button
      //              }
      //              else if(oCtrl.GetType() == typeof(Infragistics.Win.Misc.UltraLabel))
      //              {
      //                  #region ultra label

      //                  if(oCtrl.Tag != "NOCOLOR")
      //                  {
      //                      if (oCtrl.Name == "ultraLabel28")
      //                      {
      //                      }
      //                      ((Infragistics.Win.Misc.UltraLabel) oCtrl).SupportThemes = false;
      //                      if(((Infragistics.Win.Misc.UltraLabel) oCtrl).Appearance.BackColor != Color.Transparent && Convert.ToString(((Infragistics.Win.Misc.UltraLabel) oCtrl).Tag) != "1")
      //                      {
      //                          ((Infragistics.Win.Misc.UltraLabel) oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                          ((Infragistics.Win.Misc.UltraLabel) oCtrl).Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));

      //                          ((Infragistics.Win.Misc.UltraLabel) oCtrl).Appearance.BackGradientStyle = GradientStyle.None;
      //                          ((Infragistics.Win.Misc.UltraLabel) oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //                          ((Infragistics.Win.Misc.UltraLabel) oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //                      }
      //                      //if (Convert.ToString(((Infragistics.Win.Misc.UltraLabel)oCtrl).Tag)=="Header" || Convert.ToString(((Infragistics.Win.Misc.UltraLabel)oCtrl).Tag)=="Header1" )
      //                      /*
      //                      if (Convert.ToString(((Infragistics.Win.Misc.UltraLabel)oCtrl).Name) == "lblTransactionType")
      //                      {
      //                          ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.FontData.Name = "Arial";
      //                          ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.FontData.Bold = DefaultableBoolean.True;
      //                          ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.FontData.Italic = DefaultableBoolean.True;

      //                          ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Header_BackColor[0]), Convert.ToInt32(Header_BackColor[1]), Convert.ToInt32(Header_BackColor[2]));
      //                          ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Header_BackColor[0]), Convert.ToInt32(Header_BackColor[1]), Convert.ToInt32(Header_BackColor[2]));

      //                          ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.BackGradientStyle = GradientStyle.None;
      //                          ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Header_ForeColor[0]), Convert.ToInt32(Header_ForeColor[1]), Convert.ToInt32(Header_ForeColor[2]));
      //                          ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Header_ForeColor[0]), Convert.ToInt32(Header_ForeColor[1]), Convert.ToInt32(Header_ForeColor[2]));

      //                          if (Convert.ToString(((Infragistics.Win.Misc.UltraLabel)oCtrl).Tag) != "Header1")
      //                          {
      //                              ((Infragistics.Win.Misc.UltraLabel)oCtrl).Dock = DockStyle.Top;
      //                              ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.TextVAlign = VAlign.Middle;
      //                              ((Infragistics.Win.Misc.UltraLabel)oCtrl).Height = 45;
      //                          }
      //                      }
      //                       * */
      //                  }

      //                  #endregion ultra label
      //              }
      //              else if(oCtrl.GetType() == typeof(Infragistics.Win.Misc.UltraLabel))
      //              {
      //                  #region ultra textbox

      //                  //				((Infragistics.Win.UltraWinEditors.UltraTextEditor)oCtrl).t= false;

      //                  #endregion ultra textbox
      //              }
      //              else if(oCtrl.GetType() == typeof(Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar))
      //              {
      //                  #region ultra explorer bar

      //                  if(oCtrl.Tag != "NOCOLOR")
      //                  {
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).SupportThemes = false;
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).Appearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).Appearance.BackGradientStyle = GradientStyle.Vertical;

      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).GroupSettings.AppearancesSmall.ItemAreaAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).GroupSettings.AppearancesSmall.ItemAreaAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).GroupSettings.AppearancesSmall.ItemAreaAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).GroupSettings.AppearancesSmall.ItemAreaAppearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).GroupSettings.AppearancesSmall.ItemAreaAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).GroupSettings.AppearancesSmall.ItemAreaAppearance.BackGradientStyle = GradientStyle.None;

      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).ItemSettings.AppearancesSmall.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).ItemSettings.AppearancesSmall.Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).ItemSettings.AppearancesSmall.Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).ItemSettings.AppearancesSmall.Appearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).ItemSettings.AppearancesSmall.Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).ItemSettings.AppearancesSmall.Appearance.BackGradientStyle = GradientStyle.None;

      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
      //                      ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar) oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
      //                  }

      //                  #endregion ultra explorer bar
      //              }
      //              else if(oCtrl.GetType() == typeof(Infragistics.Win.UltraWinStatusBar.UltraStatusBar))
      //              {
      //                  #region ultra status bar

      //                  if(oCtrl.Tag != "NOCOLOR")
      //                  {
      //                      ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar) oCtrl).SupportThemes = false;
      //                      ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar) oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar) oCtrl).PanelAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar) oCtrl).Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar) oCtrl).PanelAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar) oCtrl).PanelAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
      //                      ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar) oCtrl).PanelAppearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
      //                  }

      //                  #endregion ultra status bar
      //              }
      //              else if(oCtrl.GetType() == typeof(System.Windows.Forms.GroupBox))
      //              {
      //                  # region Windows group box
      //                  ((System.Windows.Forms.GroupBox)oCtrl).BackColor =Color.FromArgb(Convert.ToInt32(Window_BackColor[0]),Convert.ToInt32(Window_BackColor[1]),Convert.ToInt32(Window_BackColor[2]));
      //                  ((System.Windows.Forms.GroupBox) oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

      //                  #endregion
      //              }

      //              else if(oCtrl.GetType() == typeof(Infragistics.Win.Misc.UltraGroupBox))
      //              {
      //                  # region ultra group box
      //                  if(oCtrl.Tag != "NOCOLOR")
      //                  {
      //                      ((Infragistics.Win.Misc.UltraGroupBox) oCtrl).ContentAreaAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.Misc.UltraGroupBox) oCtrl).ContentAreaAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //                      ((Infragistics.Win.Misc.UltraGroupBox) oCtrl).ContentAreaAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.Misc.UltraGroupBox) oCtrl).ContentAreaAppearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

      //                      ((Infragistics.Win.Misc.UltraGroupBox) oCtrl).HeaderAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
      //                      ((Infragistics.Win.Misc.UltraGroupBox) oCtrl).HeaderAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.Misc.UltraGroupBox) oCtrl).HeaderAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
      //                      ((Infragistics.Win.Misc.UltraGroupBox) oCtrl).HeaderAppearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));

      //                      ((Infragistics.Win.Misc.UltraGroupBox) oCtrl).HeaderAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
      //                      ((Infragistics.Win.Misc.UltraGroupBox) oCtrl).HeaderAppearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
      //                  }

      //                  #endregion
      //              }
      //              else if(oCtrl.GetType() == typeof(System.Windows.Forms.CheckBox))
      //              {
      //                  # region check box
      //                  if(oCtrl.Tag != "NOCOLOR")
      //                  {
      //                      ((System.Windows.Forms.CheckBox) oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((System.Windows.Forms.CheckBox) oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //                  }

      //                  #endregion
      //              }
      //              else if(oCtrl.GetType() == typeof(Infragistics.Win.UltraWinEditors.UltraCheckEditor))
      //              {
      //                  # region ultra check box
      //                  if(oCtrl.Tag != "NOCOLOR")
      //                  {
      //                      ((Infragistics.Win.UltraWinEditors.UltraCheckEditor) oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinEditors.UltraCheckEditor) oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinEditors.UltraCheckEditor) oCtrl).Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));

      //                      ((Infragistics.Win.UltraWinEditors.UltraCheckEditor) oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //                      ((Infragistics.Win.UltraWinEditors.UltraCheckEditor) oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //                  }

      //                  #endregion
      //              }
      //              else if(oCtrl.GetType() == typeof(System.Windows.Forms.Label))
      //              {
      //                  # region label
      //                  ((System.Windows.Forms.Label) oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                  ((System.Windows.Forms.Label) oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

      //                  #endregion
      //              }
      //              else if(oCtrl.GetType() == typeof(System.Windows.Forms.RadioButton))
      //              {
      //                  # region radio button
      //                  ((System.Windows.Forms.RadioButton) oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                  ((System.Windows.Forms.RadioButton) oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

      //                  #endregion
      //              }
      //              else if(oCtrl.GetType() == typeof(Infragistics.Win.UltraWinEditors.UltraOptionSet))
      //              {
      //                  # region radio button
      //                  ((Infragistics.Win.UltraWinEditors.UltraOptionSet) oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                  ((Infragistics.Win.UltraWinEditors.UltraOptionSet) oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

      //                  #endregion
      //              }
      //              else if(oCtrl.GetType() == typeof(Infragistics.Win.UltraWinTabControl.UltraTabControl))
      //              {
      //                  #region ultra tab control

      //                  if(oCtrl.Tag != "NOCOLOR")
      //                  {
      //                      ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).SupportThemes = false;
      //                      ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Appearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));

      //                      ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));

      //                      ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).ClientAreaAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).ClientAreaAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).ClientAreaAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).ClientAreaAppearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));

      //                      ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //                      ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //                      for(int i = 0; i < ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Tabs.Count; i++)
      //                      {
      //                          ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Tabs[i].Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                          ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Tabs[i].ClientAreaAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                          ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Tabs[i].ClientAreaAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                          ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Tabs[i].Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                          ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Tabs[i].Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

      //                          ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Tabs[i].ActiveAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Active_BackColor[0]), Convert.ToInt32(Active_BackColor[1]), Convert.ToInt32(Active_BackColor[2]));
      //                          ((Infragistics.Win.UltraWinTabControl.UltraTabControl) oCtrl).Tabs[i].ActiveAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Active_ForeColor[0]), Convert.ToInt32(Active_ForeColor[1]), Convert.ToInt32(Active_ForeColor[2]));
      //                      }
      //                  }

      //                  #endregion
      //              }
      //              else if(oCtrl.GetType() == typeof(Infragistics.Win.UltraWinGrid.UltraGrid))
      //              {
      //                  #region ultra grid

      //                  if(oCtrl.Tag != "NOCOLOR")
      //                  {
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).SupportThemes = false;
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.HeaderAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.HeaderAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.HeaderAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.HeaderAppearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));

      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.HeaderAppearance.BackGradientStyle = GradientStyle.Vertical;
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.HeaderAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.HeaderAppearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));

      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.RowSelectorAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.RowSelectorAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.RowSelectorAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.RowSelectorAppearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.RowSelectorAppearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));

      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.SelectedRowAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Active_BackColor[0]), Convert.ToInt32(Active_BackColor[1]), Convert.ToInt32(Active_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Active_BackColor[0]), Convert.ToInt32(Active_BackColor[1]), Convert.ToInt32(Active_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.ActiveRowAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Active_ForeColor[0]), Convert.ToInt32(Active_ForeColor[1]), Convert.ToInt32(Active_ForeColor[2]));

      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.EditCellAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Active_BackColor[0]), Convert.ToInt32(Active_BackColor[1]), Convert.ToInt32(Active_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.Override.EditCellAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Active_ForeColor[0]), Convert.ToInt32(Active_ForeColor[1]), Convert.ToInt32(Active_ForeColor[2]));

      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.ScrollBarLook.ButtonAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.ScrollBarLook.ButtonAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.ScrollBarLook.ButtonAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.ScrollBarLook.ButtonAppearance.BackGradientStyle = GradientStyle.Vertical;

      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.ScrollBarLook.ThumbAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.ScrollBarLook.ThumbAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.ScrollBarLook.ThumbAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));

      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.ScrollBarLook.TrackAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.ScrollBarLook.TrackAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.ScrollBarLook.TrackAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
      //                      ((Infragistics.Win.UltraWinGrid.UltraGrid) oCtrl).DisplayLayout.ScrollBarLook.TrackAppearance.BackGradientStyle = GradientStyle.Vertical;
      //                  }

      //                  #endregion
      //              }

      //              if(oCtrl.Controls.Count > 0)
      //              {
      //                  setColorSchecme(oCtrl);
      //              }
      //          }
      //          POS_Core.ErrorLogging.Logs.Logger("Setting UI", "setColorSchecme()", clsPOSDBConstants.Log_Exiting);
      //      }
      //      catch(Exception exp)
      //      {
      //          ShowErrorMsg(exp.Message);
      //          POS_Core.ErrorLogging.Logs.Logger("Setting UI", "setColorSchecme()", clsPOSDBConstants.Log_Exception_Occured);
      //      }
      //      /*.pnlTransaction.BackColor=Color.FromArgb(10,150,20);

      //          */
      //  }

        #region 27-Dec-2017 JY Added
        public static void setColorSchecme(Control oCCtrl)
        {
            POS_Core.ErrorLogging.Logs.Logger("Setting UI", "setColorSchecme()", clsPOSDBConstants.Log_Entering);
            string GroupBoxBackColor = string.Empty;
            string ButtonHotTrackingColor = string.Empty;
            //PrimePOS-2523 Added by Farman Ansari from here on 24 May 2018
            if (Configuration.Theme == "Style1"  || Configuration.Theme=="" || Configuration.Theme == null)
            {
                Configuration.Window_Color = "255, 255, 255";   //form backcolor
                Configuration.Window_ForeColor = "0,0,0";   //form forecolor       
                Configuration.Window_Button_ForeColor = "0,0,0";   //button forecolor       
                Configuration.Window_Button_Color1 = "255, 255, 255";
                Configuration.Window_Button_Color2 = "255, 255, 255";   //"240,240,240";            
                Configuration.Active_BackColor = "255, 192, 128";
                Configuration.Active_ForeColor = "0,0,0";
                Configuration.Header_ForeColor = "0,0,0";
                GroupBoxBackColor = "240, 240, 240";
                ButtonHotTrackingColor = "188, 193, 207";
            }
           else if (Configuration.Theme == "Style2")
            {
                Configuration.Window_Color = "170, 179, 184";   //form backcolor
                Configuration.Window_ForeColor = "0,0,0";   //form forecolor       
                Configuration.Window_Button_ForeColor = "255, 255, 255";   //button forecolor       
                Configuration.Window_Button_Color1 = "0, 110, 159";
                Configuration.Window_Button_Color2 = "0, 110, 159";   //"240,240,240";            
                Configuration.Active_BackColor = "255, 255, 255";
                Configuration.Active_ForeColor = "0,0,0";
                Configuration.Header_ForeColor = "0,0,0";
                GroupBoxBackColor = "132, 147, 154";
                ButtonHotTrackingColor = "170, 179, 184";
            }
            //PrimePOS-2523 Added by Farman Ansari till here on 24 May 2018

            if (Configuration.Window_Color == null)
            {
                return;
            }

            string[] Window_BackColor = Configuration.Window_Color.Split(',');
            string[] Window_Button_Color1 = Configuration.Window_Button_Color1.Split(',');
            string[] Window_Button_Color2 = Configuration.Window_Button_Color2.Split(',');
            string[] Window_ForeColor = Configuration.Window_ForeColor.Split(',');
            string[] Window_Button_ForeColor = Configuration.Window_Button_ForeColor.Split(',');

            string[] Active_BackColor = Configuration.Active_BackColor.Split(',');
            string[] Active_ForeColor = Configuration.Active_ForeColor.Split(',');

            //string[] Header_BackColor = Configuration.Header_BackColor.Split(',');
            string[] Header_ForeColor = Configuration.Header_ForeColor.Split(',');

            string[] arrGroupBoxBackColor = GroupBoxBackColor.Split(',');
            string[] arrButtonHotTrackingColor = ButtonHotTrackingColor.Split(',');          

            try
            {
                if (oCCtrl.GetType().BaseType == typeof(System.Windows.Forms.Form) || oCCtrl.GetType().BaseType.BaseType == typeof(System.Windows.Forms.Form))
                {
                    if (oCCtrl.Tag != null && oCCtrl.Tag.ToString() == "CUSTOM")
                        return;

                    if (oCCtrl.Name == "frmLogin") {
                        oCCtrl.BackColor = Color.FromArgb(240,240,240);  //sets the form back ground color
                        oCCtrl.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                    } else {
                        oCCtrl.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));  //sets the form back ground color
                        oCCtrl.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                    }

                    if (oCCtrl.Name == "frmMain")
                    {
                    }
                    else
                    {
                        System.Resources.ResourceManager rs = new System.Resources.ResourceManager(typeof(UserManagement.frmLogin));
                        ((Form)oCCtrl).Icon = (System.Drawing.Icon)rs.GetObject("$this.Icon");
                    }
                }

                foreach (Control oCtrl in oCCtrl.Controls)
                {
                    if (oCtrl.GetType() == typeof(System.Windows.Forms.Panel))
                    {
                        #region win panel
                        if (Configuration.convertNullToString(oCtrl.Tag) != "NOCOLOR")
                        {
                            ((System.Windows.Forms.Panel)oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((System.Windows.Forms.Panel)oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                        }
                        #endregion win panel
                    }

                    else if (oCtrl.GetType() == typeof(Infragistics.Win.Misc.UltraPanel))
                    {
                        //PrimePOS-2523 Added by Farman Ansari from here on 24 May 2018
                        if (Configuration.convertNullToString(oCtrl.Tag) == "CRITERIAPANEL") 
                        {
                            ((Infragistics.Win.Misc.UltraPanel)oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));                          
                        }
                        //PrimePOS-2523 Added by Farman Ansari till here on 24 May 2018
                    }
                    else if (oCtrl.GetType() == typeof(System.Windows.Forms.GroupBox))
                    {
                        #region Windows group box
                        ((System.Windows.Forms.GroupBox)oCtrl).FlatStyle = FlatStyle.Flat;
                        ((System.Windows.Forms.GroupBox)oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                        ((System.Windows.Forms.GroupBox)oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                        //((System.Windows.Forms.GroupBox)oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                        //((System.Windows.Forms.GroupBox)oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                        #endregion
                    }
                    else if (oCtrl.GetType() == typeof(Infragistics.Win.Misc.UltraGroupBox))
                    {
                        #region ultra group box
                    
                        if (Configuration.convertNullToString(oCtrl.Tag) != "NOCOLOR")
                        {
                            ((Infragistics.Win.Misc.UltraGroupBox)oCtrl).ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Default;
                            ((Infragistics.Win.Misc.UltraGroupBox)oCtrl).ContentAreaAppearance.BackColor = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                            ((Infragistics.Win.Misc.UltraGroupBox)oCtrl).ContentAreaAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            ((Infragistics.Win.Misc.UltraGroupBox)oCtrl).ContentAreaAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                            ((Infragistics.Win.Misc.UltraGroupBox)oCtrl).ContentAreaAppearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

                            ((Infragistics.Win.Misc.UltraGroupBox)oCtrl).HeaderAppearance.BackColor = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                            ((Infragistics.Win.Misc.UltraGroupBox)oCtrl).HeaderAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                            ((Infragistics.Win.Misc.UltraGroupBox)oCtrl).HeaderAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                            ((Infragistics.Win.Misc.UltraGroupBox)oCtrl).HeaderAppearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));

                            ((Infragistics.Win.Misc.UltraGroupBox)oCtrl).HeaderAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
                            ((Infragistics.Win.Misc.UltraGroupBox)oCtrl).HeaderAppearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
                        }
                                        
                            #endregion
                        }
                    else if (oCtrl.GetType() == typeof(System.Windows.Forms.Label))
                    {
                        # region label forecolor/backcolor same as windows  
                        ((System.Windows.Forms.Label)oCtrl).BackColor = Color.Transparent;  //Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                        ((System.Windows.Forms.Label)oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                        #endregion
                    }
                    else if (oCtrl.GetType() == typeof(Infragistics.Win.Misc.UltraLabel))
                    {
                        #region ultra label
                        if (Configuration.convertNullToString(oCtrl.Tag) != "NOCOLOR")
                        {
                            ((Infragistics.Win.Misc.UltraLabel)oCtrl).SupportThemes = false;
                            if (((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.BackColor != Color.Transparent && Convert.ToString(((Infragistics.Win.Misc.UltraLabel)oCtrl).Tag) != "1")
                            {
                                ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.BackColor = Color.Transparent;  // Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                                ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.BackColorDisabled = Color.Transparent;  //Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));

                                ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.BackGradientStyle = GradientStyle.None;
                                ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                                ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            }
                        }
                        #endregion ultra label
                    }
                    else if (oCtrl.GetType() == typeof(System.Windows.Forms.Button))
                    {
                        ((System.Windows.Forms.Button)oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                        ((System.Windows.Forms.Button)oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                    }
                    else if (oCtrl.GetType() == typeof(Infragistics.Win.Misc.UltraButton))
                    {
                        #region ultra button
                        if (Configuration.convertNullToString(oCtrl.Tag) != "NOCOLOR")
                        {
                            ((Infragistics.Win.Misc.UltraButton)oCtrl).SupportThemes = false;
                            ((Infragistics.Win.Misc.UltraButton)oCtrl).UseFlatMode = DefaultableBoolean.Default;
                            ((Infragistics.Win.Misc.UltraButton)oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
                            ((Infragistics.Win.Misc.UltraButton)oCtrl).Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
                            ((Infragistics.Win.Misc.UltraButton)oCtrl).Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
                            ((Infragistics.Win.Misc.UltraButton)oCtrl).Appearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
                            ((Infragistics.Win.Misc.UltraButton)oCtrl).ButtonStyle = UIElementButtonStyle.Flat;
                            ((Infragistics.Win.Misc.UltraButton)oCtrl).Appearance.BackGradientStyle = GradientStyle.Vertical;

                            //if (((Infragistics.Win.Misc.UltraButton)oCtrl).Appearance.BackGradientStyle == GradientStyle.None)
                            //    ((Infragistics.Win.Misc.UltraButton)oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
                            //else
                            //    ((Infragistics.Win.Misc.UltraButton)oCtrl).Appearance.BackGradientStyle = GradientStyle.Vertical;

                            ((Infragistics.Win.Misc.UltraButton)oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
                            ((Infragistics.Win.Misc.UltraButton)oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));

                            ((Infragistics.Win.Misc.UltraButton)oCtrl).HotTrackAppearance.BackColor = Color.FromArgb(Convert.ToInt32(arrButtonHotTrackingColor[0]), Convert.ToInt32(arrButtonHotTrackingColor[1]), Convert.ToInt32(arrButtonHotTrackingColor[2]));
                            ((Infragistics.Win.Misc.UltraButton)oCtrl).HotTrackAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(arrButtonHotTrackingColor[0]), Convert.ToInt32(arrButtonHotTrackingColor[1]), Convert.ToInt32(arrButtonHotTrackingColor[2]));
                            ((Infragistics.Win.Misc.UltraButton)oCtrl).HotTracking = true;
                        }
                        #endregion ultra button
                    }
                    else if (oCtrl.GetType() == typeof(System.Windows.Forms.CheckBox))
                    {
                        #region check box
                        if (Configuration.convertNullToString(oCtrl.Tag) != "NOCOLOR")
                        {
                            ((System.Windows.Forms.CheckBox)oCtrl).BackColor = Color.Transparent; //Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((System.Windows.Forms.CheckBox)oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                        }
                        #endregion
                    }
                    else if (oCtrl.GetType() == typeof(Infragistics.Win.UltraWinEditors.UltraCheckEditor))
                    {
                        #region ultra check box
                        if (Configuration.convertNullToString(oCtrl.Tag) != "NOCOLOR")
                        {
                            //((Infragistics.Win.UltraWinEditors.UltraCheckEditor)oCtrl).BackColor = Color.Transparent;   //Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinEditors.UltraCheckEditor)oCtrl).Appearance.BackColor = Color.Transparent;   //Color.Transparent;   //Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinEditors.UltraCheckEditor)oCtrl).Appearance.BackColor2 = Color.Transparent;   //Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));

                            ((Infragistics.Win.UltraWinEditors.UltraCheckEditor)oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            ((Infragistics.Win.UltraWinEditors.UltraCheckEditor)oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                        }
                        #endregion
                    }
                    else if (oCtrl.GetType() == typeof(System.Windows.Forms.RadioButton))
                    {
                        # region radio button
                        ((System.Windows.Forms.RadioButton)oCtrl).BackColor = Color.Transparent;   //Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                        ((System.Windows.Forms.RadioButton)oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

                        #endregion
                    }
                    else if (oCtrl.GetType() == typeof(Infragistics.Win.UltraWinEditors.UltraOptionSet))
                    {
                        # region radio button
                        ((Infragistics.Win.UltraWinEditors.UltraOptionSet)oCtrl).Appearance.BackColor = Color.Transparent;   //Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                        ((Infragistics.Win.UltraWinEditors.UltraOptionSet)oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

                        #endregion
                    }
                    else if (oCtrl.GetType() == typeof(Infragistics.Win.UltraWinTabControl.UltraTabControl))
                    {
                        #region ultra tab control
                        if (Configuration.convertNullToString(oCtrl.Tag) != "NOCOLOR")
                        {
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Appearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).ClientAreaAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).ClientAreaAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).ClientAreaAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).ClientAreaAppearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

                            ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).SupportThemes = false;
                            ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
                            ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPageFlat;
                            //foreach (Control oTab in oCtrl.Controls)
                            //{
                            //    if (oTab.GetType() == typeof(Infragistics.Win.UltraWinTabControl.UltraTabPageControl))
                            //    {
                            //        ((Infragistics.Win.UltraWinTabControl.UltraTabPageControl)oTab).Tab.ClientAreaAppearance.BackColor = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                            //        ((Infragistics.Win.UltraWinTabControl.UltraTabPageControl)oTab).Tab.ClientAreaAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                            //        ((Infragistics.Win.UltraWinTabControl.UltraTabPageControl)oTab).Tab.SelectedAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //        ((Infragistics.Win.UltraWinTabControl.UltraTabPageControl)oTab).Tab.SelectedAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //        ((Infragistics.Win.UltraWinTabControl.UltraTabPageControl)oTab).Tab.SelectedAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            //        ((Infragistics.Win.UltraWinTabControl.UltraTabPageControl)oTab).Tab.SelectedAppearance.FontData.Bold = DefaultableBoolean.True;
                            //    }
                            //}

                            for (int i = 0; i < ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs.Count; i++)
                            {
                                ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                                ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                                ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].ClientAreaAppearance.BackColor = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                                ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].ClientAreaAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));

                                ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

                                ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].SelectedAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                                ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].SelectedAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                                ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].SelectedAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                                ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].SelectedAppearance.FontData.Bold = DefaultableBoolean.True;

                                ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].ActiveAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Active_BackColor[0]), Convert.ToInt32(Active_BackColor[1]), Convert.ToInt32(Active_BackColor[2]));
                                ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].ActiveAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Active_ForeColor[0]), Convert.ToInt32(Active_ForeColor[1]), Convert.ToInt32(Active_ForeColor[2]));                                
                            }

                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Appearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));

                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));

                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).ClientAreaAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).ClientAreaAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).ClientAreaAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).ClientAreaAppearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));

                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            //((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            //for (int i = 0; i < ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs.Count; i++)
                            //{
                            //    ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //    ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].ClientAreaAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //    ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].ClientAreaAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //    ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            //    ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

                            //    ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].ActiveAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Active_BackColor[0]), Convert.ToInt32(Active_BackColor[1]), Convert.ToInt32(Active_BackColor[2]));
                            //    ((Infragistics.Win.UltraWinTabControl.UltraTabControl)oCtrl).Tabs[i].ActiveAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Active_ForeColor[0]), Convert.ToInt32(Active_ForeColor[1]), Convert.ToInt32(Active_ForeColor[2]));
                            //}
                        }

                        #endregion
                    }
                    else if (oCtrl.GetType() == typeof(System.Windows.Forms.DataGridView))  
                    {

                    }
                    else if (oCtrl.GetType() == typeof(Infragistics.Win.UltraWinGrid.UltraGrid))
                    {
                        #region ultra grid
                        if (Configuration.convertNullToString(oCtrl.Tag) != "NOCOLOR")
                        {
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).SupportThemes = false;
                            #region header appearance
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.HeaderAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2])); 
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.HeaderAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                            //((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.HeaderAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
                            //((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.HeaderAppearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));

                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.HeaderAppearance.BackGradientStyle = GradientStyle.Vertical;
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.HeaderAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            //((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.HeaderAppearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            #endregion

                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.RowSelectorAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.RowSelectorAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.RowSelectorAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.RowSelectorAppearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.RowSelectorAppearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));

                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.SelectedRowAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Active_BackColor[0]), Convert.ToInt32(Active_BackColor[1]), Convert.ToInt32(Active_BackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Active_BackColor[0]), Convert.ToInt32(Active_BackColor[1]), Convert.ToInt32(Active_BackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.ActiveRowAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Active_ForeColor[0]), Convert.ToInt32(Active_ForeColor[1]), Convert.ToInt32(Active_ForeColor[2]));

                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.EditCellAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Active_BackColor[0]), Convert.ToInt32(Active_BackColor[1]), Convert.ToInt32(Active_BackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.Override.EditCellAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Active_ForeColor[0]), Convert.ToInt32(Active_ForeColor[1]), Convert.ToInt32(Active_ForeColor[2]));

                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.ScrollBarLook.ButtonAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.ScrollBarLook.ButtonAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.ScrollBarLook.ButtonAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.ScrollBarLook.ButtonAppearance.BackGradientStyle = GradientStyle.Vertical;

                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.ScrollBarLook.ThumbAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.ScrollBarLook.ThumbAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(arrGroupBoxBackColor[0]), Convert.ToInt32(arrGroupBoxBackColor[1]), Convert.ToInt32(arrGroupBoxBackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.ScrollBarLook.ThumbAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));

                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.ScrollBarLook.TrackAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.ScrollBarLook.TrackAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.ScrollBarLook.TrackAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            ((Infragistics.Win.UltraWinGrid.UltraGrid)oCtrl).DisplayLayout.ScrollBarLook.TrackAppearance.BackGradientStyle = GradientStyle.Vertical;
                        }
                        #endregion
                    }
                    else if (oCtrl.GetType() == typeof(Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar))
                    {
                        #region ultra explorer bar
                        if (Configuration.convertNullToString(oCtrl.Tag) != "NOCOLOR")
                        {
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).SupportThemes = false;
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).Appearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).Appearance.BackGradientStyle = GradientStyle.Vertical;

                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).GroupSettings.AppearancesSmall.ItemAreaAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).GroupSettings.AppearancesSmall.ItemAreaAppearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).GroupSettings.AppearancesSmall.ItemAreaAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).GroupSettings.AppearancesSmall.ItemAreaAppearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).GroupSettings.AppearancesSmall.ItemAreaAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).GroupSettings.AppearancesSmall.ItemAreaAppearance.BackGradientStyle = GradientStyle.None;

                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).ItemSettings.AppearancesSmall.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).ItemSettings.AppearancesSmall.Appearance.BackColor2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).ItemSettings.AppearancesSmall.Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).ItemSettings.AppearancesSmall.Appearance.BackColorDisabled2 = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).ItemSettings.AppearancesSmall.Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).ItemSettings.AppearancesSmall.Appearance.BackGradientStyle = GradientStyle.None;

                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
                            ((Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar)oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
                        }
                        #endregion ultra explorer bar
                    }
                    else if (oCtrl.GetType() == typeof(Infragistics.Win.UltraWinStatusBar.UltraStatusBar))
                    {
                        #region ultra status bar
                        if (Configuration.convertNullToString(oCtrl.Tag) != "NOCOLOR")
                        {
                            ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar)oCtrl).SupportThemes = false;
                            ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar)oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
                            ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar)oCtrl).PanelAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
                            ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar)oCtrl).Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
                            ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar)oCtrl).PanelAppearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color2[0]), Convert.ToInt32(Window_Button_Color2[1]), Convert.ToInt32(Window_Button_Color2[2]));
                            ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar)oCtrl).PanelAppearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
                            ((Infragistics.Win.UltraWinStatusBar.UltraStatusBar)oCtrl).PanelAppearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_ForeColor[0]), Convert.ToInt32(Window_Button_ForeColor[1]), Convert.ToInt32(Window_Button_ForeColor[2]));
                        }
                        #endregion ultra status bar
                    }
                    else if (oCtrl.GetType() == typeof(Infragistics.Win.UltraWinEditors.UltraComboEditor))        //PRIMEPOS-2502 06-Apr-2018 JY Added
                    {
                        #region UltraCombo
                        if (Configuration.convertNullToString(oCtrl.Tag) != "NOCOLOR")
                        {
                            ((Infragistics.Win.UltraWinEditors.UltraComboEditor)oCtrl).UseOsThemes = DefaultableBoolean.False;

                            ((Infragistics.Win.UltraWinEditors.UltraComboEditor)oCtrl).Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
                            ((Infragistics.Win.UltraWinEditors.UltraComboEditor)oCtrl).Appearance.BackColorDisabled = Color.FromArgb(Convert.ToInt32(Window_Button_Color1[0]), Convert.ToInt32(Window_Button_Color1[1]), Convert.ToInt32(Window_Button_Color1[2]));
                            ((Infragistics.Win.UltraWinEditors.UltraComboEditor)oCtrl).Appearance.BackGradientStyle = GradientStyle.None;
                            ((Infragistics.Win.UltraWinEditors.UltraComboEditor)oCtrl).Appearance.ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                            ((Infragistics.Win.UltraWinEditors.UltraComboEditor)oCtrl).Appearance.ForeColorDisabled = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                        }
                        #endregion
                    }
                    //PrimePOS-2523 Added by Farman Ansari from here on 24 May 2018
                    if (oCtrl.GetType() == typeof(System.Windows.Forms.TableLayoutPanel))
                    {
                        #region TableLayoutPanel
                        if (Configuration.convertNullToString(oCtrl.Tag) == "NOCOLOR")
                        {
                            ((System.Windows.Forms.TableLayoutPanel)oCtrl).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));  //sets the form back ground color
                            ((System.Windows.Forms.TableLayoutPanel)oCtrl).ForeColor = Color.FromArgb(Convert.ToInt32(Window_ForeColor[0]), Convert.ToInt32(Window_ForeColor[1]), Convert.ToInt32(Window_ForeColor[2]));
                        }
                        #endregion
                    }
                    //PrimePOS-2523 Added by Farman Ansari till here on 24 May 2018
                    if (oCtrl.Controls.Count > 0)
                    {
                        setColorSchecme(oCtrl);
                    }
                }
                SetHeader(oCCtrl);
                POS_Core.ErrorLogging.Logs.Logger("Setting UI", "setColorSchecme()", clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                ShowErrorMsg(exp.Message);
                POS_Core.ErrorLogging.Logs.Logger("Setting UI", "setColorSchecme()", clsPOSDBConstants.Log_Exception_Occured);
            }
        }
        #endregion

        #region 27-Dec-2017 JY Added
        public static void SetHeader(Control oCCtrl, string strFormName = "")
        {
            if (oCCtrl.GetType().BaseType == typeof(System.Windows.Forms.Form))
            {
                strFormName = oCCtrl.Name;
            }

            foreach (Control oCtrl in oCCtrl.Controls)
            {
                //header label appearance
                if (oCtrl.Name == "lblTransactionType" && (strFormName == "frmAddNewItem"||strFormName == "frmAckIncompleteItems"||strFormName == "frmChangeCLCardPoints"||strFormName == "frmCLCards"||
                    strFormName == "frmCLCouponsView"||strFormName == "frmCLPointsRewardTier"||strFormName == "frmColorSchemeForViewPOSTrans"||strFormName == "frmCompanionItem"||
                    strFormName == "frmComparePrices"||strFormName == "frmCreateTimesheet"||strFormName == "frmCSItems"||strFormName == "frmCustomerImportFromRX"||strFormName == "frmCustomerNotes"||
                    strFormName == "frmCustomerNotesView"||strFormName == "frmCustomers"||strFormName == "frmDeliveryAddress"||strFormName == "frmDepartment"||strFormName == "frmEndOfDay"||
                    strFormName == "frmFunctionKeys"||strFormName == "frmGroupPricing"||strFormName == "frmHouseChargeConfirmation"||strFormName == "frmHouseChargePayment"||
                    strFormName == "frmInventoryRecieved"||strFormName == "frmInvoiceDiscountLogicInfo"||strFormName == "frmInvTransType"||strFormName == "frmItemAdvSearch"||
                    strFormName == "frmItemComboPricing"||strFormName == "frmItemInvHistory"||strFormName == "frmItemMonitorCategory"||strFormName == "frmItemMonitorCategoryDetail"||
                    strFormName == "frmItemPriceLogView"||strFormName == "frmItemPriceValid"||strFormName == "frmItems"||strFormName == "frmItemSecondaryDescription"||
                    strFormName == "frmItemsQuickAdd"||strFormName == "frmLanguageTranslation"||strFormName == "frmNonVendorItems"||strFormName == "frmOrderDetails"||
                    strFormName == "frmOtherLanguageDesc"||strFormName == "frmPatientInfo"||strFormName == "frmPayOut"||strFormName == "frmPayoutCatagory"||
                    strFormName == "frmPayTypes"||strFormName == "frmPhysicalInvView"||strFormName == "frmPhysicalInv"||strFormName == "frmPOOnHold"||
                    strFormName == "frmPOSCashBack"||strFormName == "frmPOSCoupon"||strFormName == "frmPOSProcessCC"||strFormName == "frmPrefrences"||
                    strFormName == "FrmPSEItem"||strFormName == "frmSearchPOAck"||strFormName == "frmSearchPriceUpdate"||strFormName == "frmRptItemDepartment"||
                    strFormName == "frmSetSellingPrice"||strFormName == "frmShowMsgBox"||strFormName == "frmShowOrderedItems"||strFormName == "frmStationClose"||
                    strFormName == "frmStationCloseCash"||strFormName == "frmStationCloseCashDetail"||strFormName == "frmSubDepartment"||strFormName == "frmTaxCodes"||
                    strFormName == "frmTimesheet"||strFormName == "frmVendor"||strFormName == "frmVendorHistory"||strFormName == "frmVendorItem"||strFormName == "frmViewHistory"||
                    strFormName == "frmWarningMessages" ||
                    strFormName == "frmIIASItemByTrans"||strFormName == "frmIIASTransByPayment"||strFormName == "frmRptCloseStationSummary"||strFormName == "frmRptCostAnalysis"||
                    strFormName == "frmRptCustomerList"||strFormName == "frmRptDelivery"||strFormName == "frmRptEODSummary"||strFormName == "frmRptEventLog"||strFormName == "frmRptInventoryReceived"||
                    strFormName == "frmRptItemConsumptionCompare"||strFormName == "frmRptItemFileListing"||strFormName == "frmRptItemLabel"||strFormName == "frmRptItemPriceLog"||
                    strFormName == "frmRptItemPriceLogLable"||strFormName == "frmRptItemReOrder"||strFormName == "frmRptItemSalesPerformance"||strFormName == "frmRptPayoutDetails"||
                    strFormName == "frmRptPhysicalInventoryHistory"||strFormName == "frmRptPriceOverridden"||strFormName == "frmRptProductivityReport"||strFormName == "frmRptPurchaseOrder"||
                    strFormName == "frmRptROATransDetails"||strFormName == "frmRptRxCheckout"||strFormName == "frmRptSaleAnalysisByProduct"||strFormName == "frmRptSalesByCustomer"||
                    strFormName == "frmRptSalesByItemMonitoring"||strFormName == "frmRptSalesByRXIns"||strFormName == "frmRptSalesbyVendor"||strFormName == "frmRptSalesComparisonByDept"||
                    strFormName == "frmRptSalesTax"||strFormName == "frmRptSalesTaxControl"||strFormName == "frmRptStnCloseCash"||strFormName == "frmRptTimesheet"||
                    strFormName == "frmRptTopSellingProducts"||strFormName == "frmRptTransactionTime"||strFormName == "frmRptVendor"||strFormName == "frmSSalesByVendor"||
                    strFormName == "frmViewTransaction" || strFormName == "frmMsgTemplate" || strFormName == "frmTriPOSSettings" || strFormName == "frmSalesPaymentHistory" || 
                    strFormName == "frmTaxBreakDown" || strFormName == "frmSetItemTax" || strFormName == "frmScheduledTasks" || strFormName == "frmScheduledTasksView" ||
                    strFormName == "frmTaxOverrideReport" || strFormName == "frmRptTimesheet" || strFormName == "frmCreditCardProfiles" || strFormName == "frmInvShrinkage" ||
                    strFormName == "frmTransFee"))  //PRIMEPOS-3116 11-Jul-2022 JY Added
                {
                    ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.BackColor = Color.FromArgb(1, 68, 97); //Navy 
                    ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.BackColor2 = Color.FromArgb(1, 68, 97);    //Navy
                    ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.ForeColor = Color.White;
                    ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.ForeColorDisabled = Color.Navy;
                    ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.TextHAlign = HAlign.Left;
                    ((Infragistics.Win.Misc.UltraLabel)oCtrl).Appearance.TextVAlign = VAlign.Middle;
                    oCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    oCtrl.Dock = DockStyle.Top;
                    ((Infragistics.Win.Misc.UltraLabel)oCtrl).AutoSize = true;
                    break;
                }
                if (oCtrl.Controls.Count > 0)
                {
                    SetHeader(oCtrl, strFormName);
                }
            }
        }
        #endregion

        //Following added by Krishna on 9 April 2011
        public static void ShowOKMsg(String exp)
        {
            Resources.Message.Display(exp, "PrimePOS", MessageBoxButtons.OK);
        }

        //till here Added by Krishna

        public static void SetKeyActionMappings(UltraGrid oUGrid)
        {
            /*for(int i=0;i<oUGrid.KeyActionMappings.Count;i++)
            {
                if (oUGrid.KeyActionMappings[i].KeyCode==Keys.Enter || oUGrid.KeyActionMappings[i].KeyCode==Keys.Left )
                {
                    oUGrid.KeyActionMappings.Remove(i);
                }
            }*/

            oUGrid.KeyActionMappings.Clear();
            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Enter, UltraGridAction.NextCellByTab, 0, UltraGridState.InEdit, 0, 0));
            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Right, UltraGridAction.NextCellByTab, 0, UltraGridState.InEdit, 0, 0));
            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Left, UltraGridAction.PrevCellByTab, 0, UltraGridState.InEdit, 0, 0));

            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Up, UltraGridAction.AboveCell, 0, UltraGridState.InEdit, 0, 0));
            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Up, UltraGridAction.EnterEditMode, 0, UltraGridState.InEdit, 0, 0));

            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Down, UltraGridAction.BelowCell, 0, UltraGridState.InEdit, 0, 0));
            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Down, UltraGridAction.EnterEditMode, 0, UltraGridState.InEdit, 0, 0));

            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape, UltraGridAction.UndoCell, 0, UltraGridState.InEdit, 0, 0));
            //oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape,UltraGridAction.ExitEditMode,0,UltraGridState.InEdit,0,0));

            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape, UltraGridAction.UndoRow, 0, UltraGridState.AddRow, 0, 0));
            //oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape,UltraGridAction.LastRowInGrid,0,UltraGridState.InEdit,0,0));
            //oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape,UltraGridAction.FirstCellInRow,0,UltraGridState.InEdit,0,0));
            oUGrid.KeyActionMappings.Add(new GridKeyActionMapping(Keys.Escape, UltraGridAction.EnterEditMode, 0, UltraGridState.InEdit, 0, 0));
            oUGrid.DisplayLayout.TabNavigation = TabNavigation.NextControl;
        }

        public static void SetAppearance(UltraGrid oUGrid)
        {
            //oUGrid.DisplayLayout.Bands[0].Override.SelectedRowAppearance.BackColor= Color.DarkBlue;
            //oUGrid.DisplayLayout.Bands[0].Override.ActiveRowAppearance.BackColor=Color.LightBlue;
            //oUGrid.DisplayLayout.Bands[0].Override.ActiveRowAppearance.ForeColor=Color.Black;
            oUGrid.DisplayLayout.MaxColScrollRegions = 1;
            oUGrid.DisplayLayout.MaxRowScrollRegions = 1;
        }

        public static void ShowErrorMsg(String exp)
        {
            Resources.Message.Display(exp, Configuration.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowErrorMsg(String exp, string messageTitle)
        {
            Resources.Message.Display(exp, messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarningMsg(String exp)
        {
            ShowWarningMsg(exp, Configuration.ApplicationName);
        }

        public static void ShowWarningMsg(String exp, string messageTitle)
        {
            Resources.Message.Display(exp, messageTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowBtnErrorMsg(string msg, string msg2, MessageBoxButtons mbb) // NileshJ- Add
        {
            Resources.Message.Display(msg, msg2, mbb);
        }

        public static void ShowBtnIconMsg(string msg, string msg2, MessageBoxButtons mbb, MessageBoxIcon mbi) // NileshJ- Add
        {
            Resources.Message.Display(msg, msg2, mbb, mbi);
        }

        public static DialogResult ShowLoginErrorMessage(string msg, string msg2, MessageBoxButtons mbb, MessageBoxIcon mbi, MessageBoxDefaultButton mbdb) // NileshJ- Add
        {
            return Resources.Message.Display(msg, msg2, mbb, mbi, mbdb);
        }
        public static void ShowInfoMsg(String message)
        {
            DialogResult diaRes = Resources.Message.Display(message, Configuration.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

            if(diaRes == DialogResult.Yes)
            {
                isOk = true;
            }
            else
            {
                isOk = false;
            }
        }

        //Added By Amit Date 5 Dec 2011
        public static void ShowSuccessMsg(string message, string caption)
        {
            Resources.Message.Display(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //End
        public static bool isNumeric(System.String str)
        {
            try
            {
                System.Decimal num;
                num = Decimal.Parse(str);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public static void AfterEnterEditMode(object sender, System.EventArgs e)
        {
            try
            {
                string strToolTip="Press F4 to view list";
                string [] Active_BackColor=Configuration.Active_BackColor.Split(',');
                string [] Active_ForeColor=Configuration.Active_ForeColor.Split(',');
                sender.GetType().GetProperty("BackColor").SetValue(sender, Color.FromArgb(Convert.ToInt32(Active_BackColor[0]), Convert.ToInt32(Active_BackColor[1]), Convert.ToInt32(Active_BackColor[2])), null);
                sender.GetType().GetProperty("ForeColor").SetValue(sender, Color.FromArgb(Convert.ToInt32(Active_ForeColor[0]), Convert.ToInt32(Active_ForeColor[1]), Convert.ToInt32(Active_ForeColor[2])), null);
                System.Reflection.PropertyInfo oPSelectionStart=null;
                oPSelectionStart = sender.GetType().GetProperty("SelectionStart");
                if(oPSelectionStart != null)
                {
                    try
                    {
                        if(sender.GetType().GetProperty("SelectionStart") != null)
                        {
                            sender.GetType().GetProperty("SelectionStart").SetValue(sender, 0, null);
                            if(sender.GetType().GetProperty("Text") != null)
                            {
                                sender.GetType().GetProperty("SelectionLength").SetValue(sender, sender.GetType().GetProperty("Text").GetValue(sender, null).ToString().Length, null);
                            }
                            else
                            {
                                sender.GetType().GetProperty("SelectionLength").SetValue(sender, sender.GetType().GetProperty("MaxValue").ToString().Length, null);
                            }
                        }
                    }
                    catch(Exception) { }
                }
                if(sender.GetType() == typeof(Infragistics.Win.UltraWinEditors.UltraTextEditor))
                {
                    Infragistics.Win.UltraWinEditors.UltraTextEditor oCtrl=(Infragistics.Win.UltraWinEditors.UltraTextEditor) sender;
                    if(oCtrl.ButtonsRight.Count > 0)
                    {
                        POSToolTip.Show(oCtrl, strToolTip, 200);
                    }
                }
                else if(sender.GetType() == typeof(Infragistics.Win.UltraWinGrid.UltraGrid))
                {
                    try
                    {
                        Infragistics.Win.UltraWinGrid.UltraGrid oGrid=(Infragistics.Win.UltraWinGrid.UltraGrid) sender;
                        if(oGrid.ActiveCell != null)
                        {
                            if(oGrid.ActiveCell.Column.Style == Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton)
                            {
                                POSToolTip.Show(oGrid, strToolTip, 200);
                            }
                        }
                    }
                    catch(Exception) { }
                }
            }
            catch(Exception exp) { clsUIHelper.ShowErrorMsg(exp.Message); }
        }

        public static void SetReadonlyRow(UltraGrid uGrid)
        {
            uGrid.DisplayLayout.Bands[0].Override.SelectTypeCell = SelectType.None;
            uGrid.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.Single;
            uGrid.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.RowSelect;
        }

        public static void SetEditonlyRow(UltraGrid uGrid)
        {
            uGrid.DisplayLayout.Bands[0].Override.SelectTypeCell = SelectType.Default;
            uGrid.DisplayLayout.Bands[0].Override.SelectTypeRow = SelectType.Default;
            uGrid.DisplayLayout.Bands[0].Override.CellClickAction = CellClickAction.Edit;
        }

        public static void AfterExitEditMode(object sender, System.EventArgs e)
        {
            sender.GetType().GetProperty("BackColor").SetValue(sender, Color.White, null);
            sender.GetType().GetProperty("ForeColor").SetValue(sender, Color.Black, null);
            POSToolTip.Hide();
        }

        public static void PrintPurchaseOrder(System.Int32 OrderNO, System.Boolean Print)
        {
            try
            {
                rptPurchaseOrder oRpt = new rptPurchaseOrder();
                string sSQL = " SELECT " +
                    "  PO.OrderNo,PO.UserID,PO.OrderDate,PO.ExptDeliveryDate,  " +
                    " POD.ItemID,POD.Qty,POD.Cost,i.SellingPrice,i.Description,V.VendorName , " +
                    " case " + clsPOSDBConstants.POHeader_Fld_Status + " when 0 then 'Incomplete' when  1 then 'Pending' when 2 then 'Queued' when 3 then 'Submitted' when 4 then 'Canceled' when 5 then 'Acknowledge' when 6 then 'AcknowledgeManually' when 7 then 'MaxAttempt' when 8  then 'Processed' when 9  then 'Expired' when 16 then 'DeliveryReceived' when 17 then 'DirectDelivery'  end  as [PO Status]" +
                    " from purchaseorder PO,purchaseorderdetail POD,Item i, Vendor v " +
                    " where PO.OrderID=POD.OrderID and POD.ItemID=i.ItemID and PO.VendorID=v.VendorID " +
                    " and PO.OrderNo=" + OrderNO;
                clsReports.Preview(Print, sSQL, oRpt);
            }
            catch(Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public static void PrintPurchaseOrder(System.String OrderID, System.Boolean Print, System.Boolean IsEPOVendor)
        {
            try
            {
                rptPurchaseOrder oRpt = new rptPurchaseOrder();

                //Added by SRT(Abhishek) 08 Aug 2009
                //setting property to true for print preview from the
                //create new purchase order form
                clsReports.CreateNewPreView = true;
                string sSQL = "";
                //End 0f Added by SRT(Abhishek)

                //Modified By Amit Date 21 June 2011
                if(IsEPOVendor == false)
                {
                    sSQL = " SELECT PO.OrderNo, PO.UserID, PO.OrderDate, PO.ExptDeliveryDate, POD.ItemID, POD.Qty, POD.Cost, i.SellingPrice, i.Description, V.VendorName, '' as VendorItemCode" +
                        ", case " + clsPOSDBConstants.POHeader_Fld_Status + " when 0 then 'Incomplete' when  1 then 'Pending' when 2 then 'Queued' when 3 then 'Submitted' when 4 then 'Canceled' when 5 then 'Acknowledge' when 6 then 'AcknowledgeManually' when 7 then 'MaxAttempt' when 8  then 'Processed' when 9  then 'Expired' when 10  then 'PartiallyAck' when 11  then 'PartiallyAck-Reorder' when 12 then 'Error' when 13 then 'Overdue' when 14 then 'SubmittedManually' when 17 then 'DeliveryReceived' end  as [PO Status]" +
                        ", i.PCKSIZE, 0.00 AS Tax from purchaseorder PO" +
                        " INNER JOIN purchaseorderdetail POD ON PO.OrderID = POD.OrderID" +
                        " INNER JOIN Item i ON POD.ItemID = i.ItemID" +
                        " INNER JOIN Vendor v ON PO.VendorID = v.VendorID " +
                        " WHERE PO.OrderID = " + OrderID + " order by i.Description";
                }
                else
                {
                    sSQL = " SELECT PO.OrderNo, PO.UserID, PO.OrderDate, PO.ExptDeliveryDate, POD.ItemID, POD.Qty, POD.Cost, i.SellingPrice, i.Description, i.QtyInStock, i.ReorderLevel, i.QtyOnOrder, i.MinOrdQty, V.VendorName, iv.VendorItemID as VendorItemCode" +
                        ", case " + clsPOSDBConstants.POHeader_Fld_Status + " when 0 then 'Incomplete' when  1 then 'Pending' when 2 then 'Queued' when 3 then 'Submitted' when 4 then 'Canceled' when 5 then 'Acknowledge' when 6 then 'AcknowledgeManually' when 7 then 'MaxAttempt' when 8  then 'Processed' when 9  then 'Expired' when 10  then 'PartiallyAck' when 11  then 'PartiallyAck-Reorder' when 12 then 'Error' when 13 then 'Overdue' when 14 then 'SubmittedManually' when 16 then 'DeliveryReceived' when 17 then 'DirectDelivery'  end  as [PO Status]" +
                        ", i.PCKSIZE, 0.00 AS Tax from purchaseorder PO " +
                        " INNER JOIN purchaseorderdetail POD ON PO.OrderID = POD.OrderID " +
                        " INNER JOIN Item i ON POD.ItemID = i.ItemID " +
                        " INNER JOIN Vendor v ON PO.VendorID = v.VendorID " +
                        " INNER JOIN ItemVendor iv ON i.ItemID = iv.ItemId and iv.VendorID = v.VendorID and POD.VENDORITEMCODE = iv.VendorItemID " +
                        " where " +
                        //Added By shitaljit(QuicSolv) on 7 Dec 2011
                        //To fixed POS-231 it was populating particular item for all the vendors instead of populating for selected vendor.
                        //" iv.VendorID = v.VendorID " +
                        //End of aded by shitaljit.
                        " PO.OrderID = " + OrderID + " order by i.Description";
                }

                //Start Added By Amit Date 21 June 2011
                DataSet ds = clsReports.GetReportSource(sSQL);
                ds.Tables[0].Columns.Add("BarcodeImg", System.Type.GetType("System.Byte[]"));

                string ItemID = null;
                string barCodeOf = null;
                int i = 0;

                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    if(frmCreateNewPurchaseOrder.strBarCodeOf == "VendItmCode")
                    {
                        ItemID = dr[clsPOSDBConstants.PODetail_Fld_VendorItemCode].ToString();
                        barCodeOf = "Vendor Item Code";
                    }
                    else if(frmCreateNewPurchaseOrder.strBarCodeOf == "ItemID")
                    {
                        ItemID = dr[clsPOSDBConstants.PODetail_Fld_ItemID].ToString();
                        barCodeOf = "Item Code";
                    }
                    else
                    {
                        ItemID = "";
                        barCodeOf = "None";
                    }

                    try
                    {
                        if(ItemID != "")
                            Configuration.PrintBarcode(ItemID, 0, 0, 20, 200, "CODE128", "H", System.IO.Path.GetTempPath() + "\\" + ItemID + ".bmp");
                    }
                    catch(Exception ex)
                    {
                    }
                    if(ItemID != "")
                        ds.Tables[0].Rows[i]["BarcodeImg"] = GetImageData(System.IO.Path.GetTempPath() + "\\" + ItemID + ".bmp");
                    else
                        ds.Tables[0].Rows[i]["BarcodeImg"] = null;

                    #region PRIMEPOS-3037 13-Dec-2021 JY Added
                    try
                    {
                        dr["Tax"] = CalculateTax(dr[clsPOSDBConstants.PODetail_Fld_ItemID].ToString(), Configuration.convertNullToInt(dr["Qty"]), Configuration.convertNullToDecimal(dr["SellingPrice"]));
                    }
                    catch (Exception Ex)
                    { }
                    #endregion

                    i++;
                }
                clsReports.setCRTextObjectText("BarCode", barCodeOf, oRpt);
                //End

                //Commented By Amit
                //clsReports.Preview(Print, sSQL, oRpt);
                clsReports.Preview(Print, ds, oRpt);
                //Added by SRT(Abhishek) 08 Aug 2009
                clsReports.CreateNewPreView = false;
                //End 0f Added by SRT(Abhishek)
            }
            catch(Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region PRIMEPOS-3274
        public static decimal CalculateTaxPublic(string strItemID, decimal Qty, decimal SellingPrice)
        {
            decimal TaxAmount = 0.00M;
            TaxAmount = CalculateTax(strItemID, Qty, SellingPrice);
            return TaxAmount;
        }
        #endregion

        #region PRIMEPOS-3037 13-Dec-2021 JY Added
        private static decimal CalculateTax(string strItemID, decimal Qty, decimal SellingPrice)
        {
            decimal TaxAmount = 0.00M;
            try
            {
                Item oItem = new Item();
                ItemData oItemData = oItem.Populate(strItemID);
                ItemRow oItemRow = oItemData.Item[0];

                Department oDepartment = new Department();
                DepartmentData oDepartmentData = new DepartmentData();
                string DepartmentID = "";
                bool isDeptTaxable = false;
                oDepartmentData = oDepartment.Populate(oItemData.Item[0].DepartmentID);

                ItemTax oTaxCodes = new ItemTax();
                TaxCodesData oTaxCodesData = new TaxCodesData();
                if (oDepartmentData != null && oDepartmentData.Tables.Count > 0 && oDepartmentData.Department.Rows.Count > 0)
                {
                    DepartmentRow oDepartmentRow = oDepartmentData.Department[0];
                    DepartmentID = Configuration.convertNullToString(oDepartmentRow.DeptID);
                    isDeptTaxable = oDepartmentRow.IsTaxable;
                }

                if (oItemRow.TaxPolicy == "O" || oItemRow.TaxPolicy == String.Empty)    //Dept Setting if dept is Taxable or NULL/blank
                {
                    if (isDeptTaxable)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(DepartmentID, EntityType.Department);
                        if (oTaxCodesData != null && oTaxCodesData.Tables.Count > 0 && oTaxCodesData.TaxCodes.Rows.Count > 0)
                        {
                        }
                        else if (oItemRow.isTaxable == true)
                        {
                            //if department is taxable but no tax record found then we need to consider item tax policy
                            oTaxCodesData = oTaxCodes.PopulateTaxCodeData(oItemRow.ItemID, EntityType.Item);
                        }
                    }
                    else if (oItemRow.isTaxable)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(oItemRow.ItemID, EntityType.Item);
                    }
                }
                else if (oItemRow.TaxPolicy == "D") //Department Tax Setting
                {
                    if (isDeptTaxable == true)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(DepartmentID, EntityType.Department);
                    }
                }
                else if (oItemRow.TaxPolicy == "I") //Item Tax Setting
                {
                    if (oItemRow.isTaxable)
                    {
                        oTaxCodesData = oTaxCodes.PopulateTaxCodeData(oItemRow.ItemID, EntityType.Item);
                    }
                }

                if (oTaxCodesData != null && oTaxCodesData.Tables.Count > 0 && oTaxCodesData.TaxCodes.Rows.Count > 0)
                {
                    POSTransaction oPOSTransaction = new POSTransaction();
                    decimal tmpTaxAmount = 0.00M;
                    decimal tmpTax = 0.00M;
                    foreach (TaxCodesRow row in oTaxCodesData.TaxCodes.Rows)
                    {
                        tmpTax = (Qty * SellingPrice * row.Amount) / 100;
                        tmpTaxAmount += tmpTax;
                        TaxAmount = oPOSTransaction.RoundTaxValue(tmpTaxAmount);
                    }
                }
            }
            catch (Exception Ex)
            {
            }
            return TaxAmount;
        }
        #endregion

        private static byte[] GetImageData(String fileName)
        {
            //'Method to load an image from disk and return it as a bytestream
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            return (br.ReadBytes(Convert.ToInt32(br.BaseStream.Length)));
        }

        //Modified By Amit Added Parameter oRpt
        public static void PrintPurchaseOrder(System.Data.DataSet reportData, System.Boolean Print, string barCodeOf)
        {
            try
            {
                rptPurchaseOrder oRpt = new rptPurchaseOrder();
                clsReports.setCRTextObjectText("BarCode", barCodeOf, oRpt);

                //Added by SRT(Abhishek) 08 Aug 2009
                //setting property to true for print preview from the
                //create new purchase order form
                clsReports.CreateNewPreView = true;
                //End 0f Added by SRT(Abhishek)

                clsReports.Preview(Print, reportData, oRpt);

                //Added by SRT(Abhishek) 08 Aug 2009
                clsReports.CreateNewPreView = false;
                //End 0f Added by SRT(Abhishek)
            }
            catch(Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region commented

        //public static void PrintPurchaseOrder(System.String VendorID,String orderIDS, System.Boolean Print)
        //{
        //    try
        //    {
        //        rptInCompPurchaseOrder oRpt = new rptInCompPurchaseOrder();
        //        string sSQL = " SELECT " +
        //            "  PO.OrderNo,PO.UserID,PO.OrderDate,PO.ExptDeliveryDate,  " +
        //            " POD.ItemID,POD.Qty,POD.Cost,i.Description,V.VendorName , " +
        //            " case " + clsPOSDBConstants.POHeader_Fld_Status + " when 0 then 'Incomplete' when  1 then 'Pending' when 2 then 'Queued' when 3 then 'Submitted' when 4 then 'Canceled' when 5 then 'Acknowledge' when 6 then 'AcknowledgeManually' when 7 then 'MaxAttempt' when 8  then 'Processed' when 9  then 'Expired' end  as [PO Status]" +
        //            " from purchaseorder PO,purchaseorderdetail POD,Item i, Vendor v " +
        //            " where PO.OrderID=POD.OrderID and POD.ItemID=i.ItemID and PO.VendorID=v.VendorID " +
        //            " and PO.VendorID=" + VendorID + " AND PO.OrderID IN" + orderIDS + " Order By PO.OrderID";
        //        clsReports.Preview(Print, sSQL, oRpt);
        //    }
        //    catch (Exception exp)
        //    {
        //        clsUIHelper.ShowErrorMsg(exp.Message);
        //    }
        //}

        #endregion

        //Need to use for different scenarios.
        public static void PrintPurchaseOrder(System.String VendorID, String orderID, String criteria, System.Boolean Print)
        {
            String mainReportWhereClause = String.Empty;
            String groupByClause = String.Empty;
            String mainReportSql = String.Empty;
            String subReportSql = String.Empty;
            try
            {
                rptMainPurchaseOrder oRpt = new rptMainPurchaseOrder();
                mainReportSql = "select v.VendorCode,PO.OrderNO, COUNT(POD.ItemID) as [Total Items],SUM(POD.Qty) as [Total Qty]," +
                " SUM(POD.Cost) [Total Cost] from purchaseorder PO,purchaseorderdetail POD,Item i, Vendor v where PO.OrderID=POD.OrderID " +
                " and POD.ItemID=i.ItemID and PO.VendorID=v.VendorID ";

                subReportSql = " SELECT " +
                    "  PO.OrderNo,PO.UserID,PO.OrderDate,PO.ExptDeliveryDate,  " +
                    " POD.ItemID,POD.Qty,POD.Cost,i.Description,V.VendorName , " +
                    " case " + clsPOSDBConstants.POHeader_Fld_Status + " when 0 then 'Incomplete' when  1 then 'Pending' when 2 then 'Queued' when 3 then 'Submitted' when 4 then 'Canceled' when 5 then 'Acknowledge' when 6 then 'AcknowledgeManually' when 7 then 'MaxAttempt' when 8  then 'Processed' when 9  then 'Expired' when 16 then 'DeliveryReceived' when 17 then 'DirectDelivery' end  as [PO Status]" +
                    " from purchaseorder PO,purchaseorderdetail POD,Item i, Vendor v " +
                    " where PO.OrderID=POD.OrderID and POD.ItemID=i.ItemID and PO.VendorID=v.VendorID " +
                    " and PO.OrderID=" + orderID;

                groupByClause = "group by v.VendorCode,PO.OrderNO ";

                if(criteria == "ONE-ONE")
                {
                    mainReportWhereClause = " PO.VendorID = " + Convert.ToInt32(VendorID) + " and PO.OrderID =" + Convert.ToInt32(orderID) + "";
                }
                else if(criteria == "ONE-MANY")
                {
                    mainReportWhereClause = " PO.VendorID = " + Convert.ToInt32(VendorID);
                }

                mainReportSql += mainReportWhereClause;

                mainReportSql += groupByClause;

                clsReports.Preview(Print, mainReportSql, subReportSql, oRpt);
            }
            catch(Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public static void PrintPurchaseOrder(System.Boolean Print)
        {
            String mainReportWhereClause = String.Empty;
            String groupByClause = String.Empty;
            String mainReportSql = String.Empty;
            String subReportSql = String.Empty;
            try
            {
                rptMainPurchaseOrder oRpt = new rptMainPurchaseOrder();

                clsReports.Preview(Print, oRpt);
            }
            catch(Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        public static int GetRandomNo()
        {
            return oRandom.Next(100000, 9999999);
        }

        public static Int32 GetNextNumber(System.Data.DataSet oDS, string colName)
        {
            if(oDS.Tables[0].Rows.Count == 0)
            {
                return 1;
            }
            else
            {
                Int32 MaxNo=0;
                foreach(DataRow orow in oDS.Tables[0].Rows)
                {
                    if(orow.RowState != DataRowState.Deleted)
                    {
                        if(Configuration.convertNullToInt(orow[colName].ToString()) > MaxNo)
                        {
                            MaxNo = Configuration.convertNullToInt(orow[colName].ToString());
                        }
                    }
                }
                return MaxNo + 1;
            }
        }

        public static string checkPath(String strPath)
        {
            if(System.IO.Directory.Exists(strPath) == true)
                return strPath;
            else
            {
                System.IO.DirectoryInfo oDInfo= System.IO.Directory.CreateDirectory(strPath);
                if(oDInfo.Exists == true)
                {
                    return strPath;
                }
                else
                {
                    throw (new Exception("Invalid path " + strPath + "\n Make sure the path is valid and have propper privileges."));
                }
            }
        }

        public static bool validateIP(string strIPAddressField)
        {
            System.Text.RegularExpressions.Regex regIP = new System.Text.RegularExpressions.Regex(
            @"(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>2[0-4]\d|25"
            + @"[0-5]|[01]?\d\d?)\.(?<Third>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?"
            + @"<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)",
            RegexOptions.IgnoreCase
            | RegexOptions.CultureInvariant
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled
            );
            if(regIP.IsMatch(strIPAddressField))
            {
                //MessageBox.Show("Valid IP Address entered.", "Valid IP Address", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            //MessageBox.Show(strIPAddressField);
            return false;
        }

        public static string GetLocalHostIP()
        {
            string functionReturnValue = null;
            System.Net.IPAddress[] IPAddresses;
            System.Net.IPHostEntry HostInfo;

            try
            {
                HostInfo = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName());
                IPAddresses = HostInfo.AddressList;
                functionReturnValue = IPAddresses[0].ToString();
                //return the first one we found
            }
            catch
            {
                functionReturnValue = "";
            }
            return functionReturnValue;
        }

        //This method converts a string to a byte array
        public static byte[] StrToByteArray(string str) //Added by Prashant 20-sep-2010
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static Bitmap GetSignature(string sSigData, string sSigType)
        {
            // returns the bitmap containing the signature
            // ignores any errors generated

            SigDiplay.SigDisplay oSigDisplay = new SigDiplay.SigDisplay();
            Bitmap oBmpSig = new Bitmap(335, 245);
            string sErr;

            oSigDisplay.DrawSignature(sSigData, ref oBmpSig, out sErr, sSigType);
            return oBmpSig;
        }

        public static Bitmap GetSignature(byte[] sData, out string sError)
        {
            SigDiplay.SigDisplay oSigDisplay = new SigDiplay.SigDisplay();
            Bitmap oBmpSig = new Bitmap(335, 245);
            oSigDisplay.DrawSignatureMX(sData, ref oBmpSig, out sError);
            return oBmpSig;
        }

        public static Bitmap GetSignaturePAX(string points, out string sError, string sSigType, out byte[] BSignData)  //PRIMEPOS-2952
        {

            SigDiplay.SigDisplay oSigDisplay = new SigDiplay.SigDisplay();
            Bitmap oBmpSig = new Bitmap(335, 245); //(999, 450)
            //var g = Graphics.FromImage(oBmpSig);
            //g.SmoothingMode = SmoothingMode.None;
            //g.InterpolationMode = InterpolationMode.NearestNeighbor;
            //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //g.CompositingQuality = CompositingQuality.AssumeLinear;
            //PRIMEPOS-2998
            if (Configuration.CPOSSet.PinPadModel == "HPSPAX_ARIES8")
            {
                oBmpSig = new Bitmap(650, 200);
            }
            else if (Configuration.CPOSSet.PinPadModel == "HPSPAX_A920")//PRIMEPOS-3146
            {
                oBmpSig = new Bitmap(590, 450);
            }
            else
            {
                oBmpSig = new Bitmap(350, 150);
            }
            oSigDisplay.DrawSignaturePAX(points, ref oBmpSig, out sError);
            var imageStream = new MemoryStream();
            using (imageStream)
            {
                // Save bitmap in some format.
                oBmpSig.Save(imageStream, ImageFormat.Jpeg);
                imageStream.Position = 0;
                // Do something with the memory stream. For example:
                BSignData = imageStream.ToArray();
                // Save bytes to the database.
            }
            return oBmpSig;
        }

        //Suraj PAx Specific Signature Function
        //public static Bitmap GetSignature(string points, out string sError,string sSigType, out byte[] BSignData) {  //Commented for Aries8
        //    SigDiplay.SigDisplay oSigDisplay = new SigDiplay.SigDisplay();
        //    Bitmap oBmpSig = new Bitmap(335, 245);
        //    oSigDisplay.DrawSignaturePAX(points, ref oBmpSig, out sError);
        //    var imageStream = new MemoryStream();
        //    using (imageStream)
        //    {
        //        // Save bitmap in some format.
        //        oBmpSig.Save(imageStream, ImageFormat.Jpeg);
        //        imageStream.Position = 0;
        //        // Do something with the memory stream. For example:
        //        BSignData = imageStream.ToArray();
        //        // Save bytes to the database.
        //    }
        //    return oBmpSig;
        //}
        //
        //PRIMEPOS-2636 ADDED BY ARVIND
        public static Bitmap ConvertPoints(byte[] data)
        {

            SigDiplay.Signature oSigDisplay = new SigDiplay.Signature();
            oSigDisplay.SetFormat("PointsLittleEndian");
            oSigDisplay.SetData(data);

            Bitmap oBmpSig = oSigDisplay.GetSignatureBitmap(80);//PRIMEPOS-3063

            return oBmpSig;

        }
        //
        public static Bitmap ConvertPointsFromASCII3Byte(byte[] data)
        {

            SigDiplay.Signature oSigDisplay = new SigDiplay.Signature();
            oSigDisplay.SetFormat("Ascii3Byte");
            oSigDisplay.SetData(data);

            Bitmap oBmpSig = oSigDisplay.GetSignatureBitmap(10);

            return oBmpSig;

        }
        public static string ByteArrayToString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
        public static Bitmap GetSignature(string sSigData, byte[] sSigDataBin, string sSigType, out string sError)
        {
            // returns the bitmap containing the signature
            // ignores any errors generated

            SigDiplay.SigDisplay oSigDisplay = new SigDiplay.SigDisplay();
            Bitmap oBmpSig = new Bitmap(240, 85);
            if(sSigType == "M") //mx
                oSigDisplay.DrawSignatureMX(sSigDataBin, ref oBmpSig, out sError);
            else
                oSigDisplay.DrawSignature(sSigData, ref oBmpSig, out sError, sSigType);

            return oBmpSig;
        }

        public static bool ValidateItemPrice(string sItem, decimal dSellingPrice)
        {
            POS_Core.BusinessRules.ItemPriceValidation oValidation = new POS_Core.BusinessRules.ItemPriceValidation();
            return oValidation.ValidateItem(sItem, dSellingPrice);
        }
    }

    public class POSToolTip
    {
        private static Infragistics.Win.ToolTip tip;

        public POSToolTip()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void Show(System.Windows.Forms.Control ctrl, string text, int delay)
        {
            int Px=0;
            int Py=0;
            bool flg=true;
            tip = new Infragistics.Win.ToolTip(ctrl);
            tip.ToolTipText = text;
            tip.InitialDelay = 200;
            tip.AutoPopDelay = 0;

            Control p=ctrl;
            //Calculate Location of all controls within form
            while(flg)//!(p is System.Windows.Forms.Form))
            {
                if(p.GetType() == typeof(Infragistics.Win.UltraWinGrid.UltraGrid))
                {
                    Rectangle rect= getActiveCellBounds((Infragistics.Win.UltraWinGrid.UltraGrid) p);
                    Px += rect.Location.X;
                    Py += rect.Location.Y;
                }
                Px += p.Location.X;
                Py += p.Location.Y;
                if(!(p.Parent is System.Windows.Forms.Form))
                {
                    p = p.Parent;
                }
                else
                {
                    p = p.Parent;
                    flg = false;
                }
            }
            if(ctrl.GetType() == typeof(Infragistics.Win.UltraWinGrid.UltraGrid))
            {
                Py -= (((Infragistics.Win.UltraWinGrid.UltraGrid) ctrl).ActiveCell.Height + 5);
            }
            else
            {
                Py -= (ctrl.Height + 5);
            }

            System.Drawing.Point pt=new System.Drawing.Point(Px, Py);
            pt = p.PointToScreen(pt);
            tip.Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
            tip.Show(pt, false);
        }

        private static Rectangle getActiveCellBounds(Infragistics.Win.UltraWinGrid.UltraGrid oGrid)
        {
            UltraGridCell cell = oGrid.ActiveCell;
            Rectangle cellBounds=new Rectangle(0, 0, 0, 0);

            object[] contexts = new object[] { cell };

            // Get the ui element associated with the cell.
            CellUIElement cellElem = (CellUIElement) oGrid.DisplayLayout.UIElement.GetDescendant(typeof(CellUIElement), contexts);

            if(null != cellElem)
            {
                cellBounds = cellElem.Rect;
                return cellBounds;
            }
            else
            {
                return cellBounds;
            }
        }

        public static void Hide()
        {
            try
            {
                if(tip != null)
                {
                    tip.Hide();
                }
            }
            catch(Exception) { }
        }
    }
}