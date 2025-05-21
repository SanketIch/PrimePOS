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

using POS_Core.Resources;

namespace POS_Core_UI.Layout
{
    public partial class frmTransactionLayout : Form
    {
        #region frmPOSTransaction

        static int factorPOSTransactionW = 800;
        static int factorPOSTransactionH = 600;

        public frmTransactionLayout()
        {
            InitializeComponent();

        }


        public void SetFontSize(Control control, int factorW, int factorH, decimal factor)
        {
            //PrimePOS-2523 Added by Farman Ansari from here on 24 May 2018
            clsUIHelper.setColorSchecme(control);
            string[] Window_BackColor = Configuration.Window_Color.Split(',');
            string[] Window_Button_Color1 = Configuration.Window_Button_Color1.Split(',');
            string[] Window_Button_Color2 = Configuration.Window_Button_Color2.Split(',');
            string[] Window_ForeColor = Configuration.Window_ForeColor.Split(',');
            string[] Window_Button_ForeColor = Configuration.Window_Button_ForeColor.Split(',');
            string[] Active_BackColor = Configuration.Active_BackColor.Split(',');
            string[] Active_ForeColor = Configuration.Active_ForeColor.Split(',');
            string[] Header_ForeColor = Configuration.Header_ForeColor.Split(',');

            Color clTblSubTotal = Color.White;
            Color clSubTotalLableFontColor = Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(75)))), ((int)(((byte)(76)))));
            Color clTblAndLabelTotal = Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(68)))), ((int)(((byte)(97)))));
            Color clTotalLableFontColor = Color.White;
            Color clPaymentBorder = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            Color clPaymentFontColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(83)))), ((int)(((byte)(120)))));
            Color clPaymentBackColor = Color.White;
            Color clPaymentHotBackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(68)))), ((int)(((byte)(97)))));
            Color clTitleFontColor = Color.White;
            Color clTitleBackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(68)))), ((int)(((byte)(97)))));
            Color clTblMoney = Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));
            Color clTblPaymentAndRight = Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(231)))), ((int)(((byte)(233)))));

            if (Configuration.Theme == "Style2")
            {
                clTblSubTotal = Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(180)))), ((int)(((byte)(83)))));
                clSubTotalLableFontColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                clTblAndLabelTotal = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(168)))), ((int)(((byte)(93)))));
                clTotalLableFontColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                clPaymentBorder = Color.White;
                clPaymentFontColor = Color.White;
                clPaymentBackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(68)))), ((int)(((byte)(97)))));
                clPaymentHotBackColor = Color.White;
                clTitleFontColor = Color.White;
                clTitleBackColor = Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(68)))), ((int)(((byte)(97)))));
                clTblMoney = Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(121)))), ((int)(((byte)(130)))));
                clTblPaymentAndRight = Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(147)))), ((int)(((byte)(154)))));
            }
            //PrimePOS-2523 Added by Farman Ansari till here on 24 May 2018
            foreach (Control child in control.Controls)
            {

                Font font = child.Font;
                int frmFactorW = factorW;
                int frmFactorH = factorH;

                if (child.Controls.Count > 0)
                {
                    SetFontSize(child, factorW, factorH, factor);
                    child.Font = GetMetrics(frmFactorW, frmFactorH, font, factor);
                    //child.BackgroundImage.Size.Height = child.BackgroundImage.Size.Height;
                    child.BackgroundImageLayout = ImageLayout.Stretch;

                }
                else
                {
                    child.Font = GetMetrics(frmFactorW, frmFactorH, font, factor);
                    child.BackgroundImageLayout = ImageLayout.Stretch;
                }

                //PrimePOS-2523 Added by Farman Ansari from here on 24 May 2018
                if (child.GetType() == typeof(Infragistics.Win.Misc.UltraGroupBox))
                {
                    if (Configuration.convertNullToString(child.Tag) == "TAGFORMHEADER")
                    {
                        ((Infragistics.Win.Misc.UltraGroupBox)child).HeaderAppearance.BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));

                    }
                    else if (Configuration.convertNullToString(child.Tag) == "TAGTITLE")
                    {
                        ((Infragistics.Win.Misc.UltraGroupBox)child).HeaderAppearance.BackColor = clTitleBackColor;
                        ((Infragistics.Win.Misc.UltraGroupBox)child).HeaderAppearance.ForeColor = clTitleFontColor;
                    }
                }
                else if (child.GetType() == typeof(System.Windows.Forms.TableLayoutPanel))
                {
                    if (Configuration.convertNullToString(child.Tag) == "TAGSUBTOTAL")
                    {
                        ((System.Windows.Forms.TableLayoutPanel)child).BackColor = clTblSubTotal;
                    }
                    else if (Configuration.convertNullToString(child.Tag) == "TAGTOTAL")
                    {
                        ((System.Windows.Forms.TableLayoutPanel)child).BackColor = clTblAndLabelTotal;
                    }
                    else if (Configuration.convertNullToString(child.Tag) == "TAGFORMHEADER")
                    {
                        ((System.Windows.Forms.TableLayoutPanel)child).BackColor = Color.FromArgb(Convert.ToInt32(Window_BackColor[0]), Convert.ToInt32(Window_BackColor[1]), Convert.ToInt32(Window_BackColor[2]));
                    }
                    else if (Configuration.convertNullToString(child.Tag) == "TAGMONEY")
                    {
                        ((System.Windows.Forms.TableLayoutPanel)child).BackColor = clTblMoney;
                    }
                    else if (Configuration.convertNullToString(child.Tag) == "TAGPAYMENTANDRIGHT")
                    {
                        ((System.Windows.Forms.TableLayoutPanel)child).BackColor = clTblPaymentAndRight;
                    }
                }
                else if (child.GetType() == typeof(Infragistics.Win.Misc.UltraLabel))
                {
                    if (Configuration.convertNullToString(child.Tag) == "TAGSUBTOTAL")
                    {
                        ((Infragistics.Win.Misc.UltraLabel)child).Appearance.ForeColor = clSubTotalLableFontColor;
                    }
                    else if (Configuration.convertNullToString(child.Tag) == "TAGTOTAL")
                    {
                        ((Infragistics.Win.Misc.UltraLabel)child).Appearance.BackColor = clTblAndLabelTotal;
                        ((Infragistics.Win.Misc.UltraLabel)child).Appearance.ForeColor = clTotalLableFontColor;
                    }
                    else if (Configuration.convertNullToString(child.Tag) == "TAGTITLE")
                    {
                        ((Infragistics.Win.Misc.UltraLabel)child).Appearance.BackColor = clTitleBackColor;
                        ((Infragistics.Win.Misc.UltraLabel)child).Appearance.ForeColor = clTitleFontColor;
                    }
                }
                else if (child.GetType() == typeof(Infragistics.Win.Misc.UltraButton))
                {
                    if (Configuration.convertNullToString(child.Tag) == "TAGPAYMENT")
                    {
                        ((Infragistics.Win.Misc.UltraButton)child).Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.None;
                        ((Infragistics.Win.Misc.UltraButton)child).ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
                        ((Infragistics.Win.Misc.UltraButton)child).Appearance.BackColor = clPaymentBackColor;
                        ((Infragistics.Win.Misc.UltraButton)child).Appearance.ForeColor = clPaymentFontColor;
                        ((Infragistics.Win.Misc.UltraButton)child).Appearance.BorderColor = clPaymentBorder;
                        ((Infragistics.Win.Misc.UltraButton)child).Appearance.BorderColor2 = clPaymentBorder;
                        ((Infragistics.Win.Misc.UltraButton)child).HotTrackAppearance.BackColor = clPaymentHotBackColor;
                        ((Infragistics.Win.Misc.UltraButton)child).HotTrackAppearance.ForeColor = clPaymentBackColor;
                    }
                }
                //PrimePOS-2523 Added by Farman Ansari till here on 24 May 2018
                //if (child.Tag == null || child.Tag.ToString() != "NOCOLOR") {

                //    if (child.GetType() == typeof(Infragistics.Win.Misc.UltraLabel)) {
                //        ((Infragistics.Win.Misc.UltraLabel)child).Appearance.ForeColor = MasterLayout.lableForeColor;
                //    }
                //    if (child.GetType() == typeof(Infragistics.Win.Misc.UltraButton)) {
                //        ((Infragistics.Win.Misc.UltraButton)child).Appearance.ForeColor = MasterLayout.btnForeColor;
                //    }
                //}

            }
        }

        public Font GetMetrics(int mainComponentValueW, int mainComponentValueH, Font ComponentValue, decimal factor)
        {
            //float value;

            //float WidthMultiplicationFactor = factorPOSTransactionW * (float)factor;
            //WidthMultiplicationFactor = Screen.PrimaryScreen.Bounds.Width / WidthMultiplicationFactor;

            //float heightMultiplicationFactor = factorPOSTransactionH* (float)factor;
            //heightMultiplicationFactor = Screen.PrimaryScreen.Bounds.Height / heightMultiplicationFactor;

            //value = ComponentValue.Size * (float)WidthMultiplicationFactor;

            Font font = new Font(ComponentValue.FontFamily, ComponentValue.SizeInPoints * GetMultiplicationFator(), ComponentValue.Style);

            //decimal heightMultiplicationFactor = factorPOSTransactionH* factor;
            //heightMultiplicationFactor = mainComponentValueH / heightMultiplicationFactor;

            //value = value * (float)heightMultiplicationFactor;

            return font;
        }


        private float GetMultiplicationFator()
        {
            float percentageValue = 1;

            string resolution = Screen.PrimaryScreen.Bounds.Width.ToString() + "*" + Screen.PrimaryScreen.Bounds.Height.ToString();

            switch (resolution)
            {
                case "800*600":
                    percentageValue = 1;
                    break;
                case "1024*768":
                    percentageValue = 1.03f;
                    break;
                case "1152*864":
                    percentageValue = 1.15f;
                    break;
                case "1280*600":
                    percentageValue = 1.02f;
                    break;
                case "1280*720":
                    percentageValue = 1.15f;
                    break;
                case "1280*768":
                    percentageValue = 1.15f;
                    break;
                case "1280*800":
                    percentageValue = 1.2f;
                    break;
                case "1280*960":
                    percentageValue = 1.2f;
                    break;
                case "1280*1024":
                    percentageValue = 1.2f;
                    break;
                case "1360*768":
                    percentageValue = 1.25f;
                    break;
                case "1366*768":
                    percentageValue = 1.25f;
                    break;
                case "1400*1050":
                    percentageValue = 1.3f;
                    break;
                case "1440*900":
                    percentageValue = 1.3f;
                    break;
                case "1600*900":
                    percentageValue = 1.5f;
                    break;
                case "1600*1200":
                    percentageValue = 1.6f;
                    break;
                case "1680*1050":
                    percentageValue = 1.6f;
                    break;
                case "1920*1080":
                    percentageValue = 1.8f;
                    break;
                default:
                    percentageValue = 1;
                    break;
            }
            return percentageValue;

        }

        #endregion frmPOSTransaction

    }
}
