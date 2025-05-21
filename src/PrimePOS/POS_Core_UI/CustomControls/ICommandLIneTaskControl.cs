using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace POS_Core_UI
{
    interface ICommandLIneTaskControl 
    {
         
         bool CheckTags();//1
         bool SaveTaskParameters(DataTable dt , int TaskId);//2
         bool SetControlParameters(int TaskId);
         bool RunTask(int TaskId, ref string filePath, bool bSendPrint,ref string sNoOfRecordAffect);//3
         void GetTaskParameters(ref DataTable dt, int TaskId);
         Control GetParameterControl();
         bool checkValidation();

    }
}
