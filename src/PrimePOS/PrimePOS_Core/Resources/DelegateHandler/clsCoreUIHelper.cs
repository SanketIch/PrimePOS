using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using Infragistics.Win.UltraWinGrid;

namespace POS_Core.Resources.DelegateHandler
{
    public class clsCoreUIHelper
    {

        public delegate void DelErrorMsg(string msg);
        public static DelErrorMsg ShowErrorMsg;

        public delegate int DelNextNumber(DataSet dtTransData,string TransDetailID);
        public static DelNextNumber GetNextNumber ;

        public delegate bool DelNumeric(string val);
        public static DelNumeric isNumeric ;

        public delegate void DelColorScheme(Control scheme);
        public static DelColorScheme setColorSchecme;

        public delegate void DelWarnErrorMsg(string msg, string msg2);
        public static DelWarnErrorMsg ShowWarningMsg;

        public delegate void DelWarnErrorMsg2(string msg);
        public static DelWarnErrorMsg2 ShowWarningMsg2;

        public static string DefaultValue;

        public delegate void DelButtonErrorMsg(string msg , string msg2,MessageBoxButtons mbb);
        public static DelButtonErrorMsg ShowBtnErrorMsg;

        public delegate DialogResult DelButtonErrorMsg_DialogResult(string msg, string msg2, MessageBoxButtons mbb);
        public static DelButtonErrorMsg_DialogResult ShowBtnErrorMsg_DialogResult;

        public delegate void DelBtnIconMsg(string msg,string msg2, MessageBoxButtons mbb, MessageBoxIcon mbi);
        public static DelBtnIconMsg ShowBtnIconMsg;

        public delegate string DelHostip();
        public static DelHostip GetLocalHostIP;

        public delegate DialogResult DelBtnIconDMsg(string msg, string msg2, MessageBoxButtons mbb, MessageBoxIcon mbi,MessageBoxDefaultButton mbdb);
        public static DelBtnIconDMsg ShowLoginErrorMessage;

        public delegate Bitmap DelGetSignature(string signData , string signtype);
        public static DelGetSignature GetSignature;

        public delegate Bitmap DelGetSignaturePAX(string points, out string sError, string sSigType, out byte[] BSignData);  //PRIMEPOS-2952
        public static DelGetSignaturePAX GetSignaturePAX;

        public delegate void DelOKMsg(string msg);
        public static DelOKMsg ShowOKMsg;

        public delegate void DelAfterEnterEditMode(object sender, System.EventArgs e);
        public static DelAfterEnterEditMode AfterEnterEditMode;

        public delegate void DelAfterExitEditMode(object sender, System.EventArgs e);
        public static DelAfterExitEditMode AfterExitEditMode;

        public delegate void DelSetKeyActionMappings(UltraGrid oUGrid);
        public static DelSetKeyActionMappings SetKeyActionMappings;

        public delegate void DelSetAppearance(UltraGrid oUGrid);
        public static DelSetAppearance SetAppearance;

        public delegate Form DelFrmMain();
        public static DelFrmMain GetFrmMain;

        public delegate void DelCheckPOSInstance();
        public static DelCheckPOSInstance CheckPOSInstance;

        //PRIMEPOS-2636 ADDED BY ARVIND 
        public delegate Bitmap DelConvertPoints(byte[] data);
        public static DelConvertPoints ConvertPoints;
        //

        #region PRIMEPOS-2761
        public delegate int DelGetRandomNumber();
        public static DelGetRandomNumber GetRandomNo;
        #endregion

        public delegate DialogResult DelDisplayYesNo(string message, string caption, MessageBoxButtons buttons);
        public static DelDisplayYesNo DisplayYesNo;
    }
}
