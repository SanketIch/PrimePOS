using System.Drawing;

namespace POS_Core.CommonData
{
	public static class PosUiConstants
	{
		public static int POSTransaction_HeightModificationFactor = 120;

		public static int POSTransaction_GBOptionHeight = 110;
		public static int POSTransaction_DistBtwControls = 3;
		public static int POSTransaction_HeigAdjustmentFactor = 5;
		public static int POSTransaction_MinExtraWidth = 55;
		public static int POSTransaction_MaxExtraWidth = 355;
		public static int POSTransaction_MinExtraWidthWithNumPad = 6;
		public static int POSTransaction_MaxExtraWidthWithNumPad = 44;
		public static int POSTransaction_LeftManagementFactory = 296;

		public static Size POSTransaction_GBOptionSize
		{
			get { return new Size(278, 182); }
		}

		public static Point POSTransaction_GBOptionLocation
		{
			get { return new Point(718, 486); }
		}

        public static string lastBatchNo = string.Empty; // PRIMERX-7688 - Added for BatchDelivery - NileshJ - 25-Sept-2019
    }
}