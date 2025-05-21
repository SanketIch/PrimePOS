using System;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using POS_Core.CommonData;
using POS_Core.BusinessRules;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Rows;
using Infragistics.Win.UltraWinMaskedEdit;
using System.Data;
//using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
namespace POS_Core_UI
{
	/// <summary>
	/// Summary description for clsEPurchaseOrder.
	/// </summary>
	public  class ItemAckStatus
	{
		public static String itemAccepted="IA";
		public static String itemBackordered="IB";
		public static String itemAcceptedQuantityChanged="IQ";
		public static String itemRejected="IR";
		public static String itemAcceptedSubstitutionMade="IS";
		public static String itemOnHoldWaiverRequired="IW";
		public static String itemAcceptedPartialShipment="BP";
	}

	public class POAckType
	{
		public static String AcknowledgeWithDetailChange="AC";
		public static String AcknowledgeWithDetailNoChange="AD";
		public static String AcknowledgeWithExceptionDetailOnly="AE";
		public static String RejectedNoDetail="RJ";
	}
}
