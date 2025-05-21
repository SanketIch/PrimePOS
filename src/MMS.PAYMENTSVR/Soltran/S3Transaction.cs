using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutran
{
    
    public class S3Transaction
    {

        [JsonProperty("VerType")]
        public string VerType
        {
            get;
            set;
        }

        [JsonProperty("MessType")]
        public string MessType
        {
            get;
            set;
        }

        [JsonProperty("LocalDateTime")]
        public long LocalDateTime // INT
        {
            get;
            set;
        }

        [JsonProperty("BusDate")]
        public int BusDate
        {
            get;
            set;
        }

        [JsonProperty("S3MerchID")]
        public string S3MerchID
        {
            get;
            set;
        }

        [JsonProperty("RetaiLoc")]
        public string RetaiLoc
        {
            get;
            set;
        }

        [JsonProperty("TermID")]
        public string TermID
        {
            get;
            set;
        }

        [JsonProperty("TermTranID")]
        public string TermTranID
        {
            get;
            set;
        }

        [JsonProperty("OrderID")]
        public string OrderID
        {
            get;
            set;
        }

        [JsonProperty("APLVer")]
        public string APLVer
        {
            get;
            set;
        }

        [JsonProperty("CurrCde")]
        public string CurrCde
        {
            get;
            set;
        }

        [JsonProperty("S3CardCt")]
        public string S3CardCt
        {
            get;
            set;
        }

        [JsonProperty("S3RecMess")]
        public string S3RecMess
        {
            get;
            set;
        }
        [JsonProperty("S3ProdCt")]
        public string S3ProdCt
        {
            get;
            set;
        }
        [JsonProperty("S3PurAmt")]
        public string S3PurAmt // Decimal 
        {
            get;
            set;
        }
        [JsonProperty("TotalTranAmt")]
        public string TotalTranAmt // Decimal
        {
            get;
            set;
        }
        [JsonProperty("TotalTaxAmt")]
        public string TotalTaxAmt // decimal
        {
            get;
            set;
        }
        [JsonProperty("TotalTranDisc")]
        public string TotalTranDisc // Decimal
        {
            get;
            set;
        }
        [JsonProperty("S3TranID")]
        public string S3TranID
        {
            get;
            set;
        } // INT
        [JsonProperty("ActionCode")]
        public string ActionCode
        {
            get;
            set;
        }

        [JsonProperty("CardInfo")]
        public CardInfo[] CardInfo
        {
            get;
            set;
        }

        [JsonProperty("ProductInfo")]
        public ProductInfo[] ProductInfo
        {
            get;
            set;
        }

        [JsonProperty("DiscountInfo")]
        public DiscountInfo[] DiscountInfo
        {
            get;
            set;
        }

        [JsonProperty("RecptInfo")]
        public RecptInfo[] RecptInfo
        {
            get;
            set;
        }

        [JsonProperty("BalanceInfo")]
        public BalanceInfo[] BalanceInfo
        {
            get;
            set;
        }

        [JsonProperty("DiscItems")]
        public DiscItems[] DiscItems
        {
            get;
            set;
        }

    }

    public class  CardInfo
    {
        [JsonProperty("BarcodeData")]
        public string BarcodeData
        {
            get;
            set;
        }
        [JsonProperty("POSDataCode")]
        public string POSDataCode
        {
            get;
            set;
        }
        [JsonProperty("CVV")]
        public string CVV
        {
            get;
            set;
        }

    }

    public class ProductInfo
    {
        [JsonProperty("ProdCode")]
        public string ProdCode
        {
            get;
            set;
        }

        [JsonProperty("ProdLevel")]
        public string ProdLevel
        {
            get;
            set;
        }

        [JsonProperty("Depart")]
        public string Depart
        {
            get;
            set;
        }

        [JsonProperty("OrdinalNum")]
        public string OrdinalNum
        {
            get;
            set;
        }

        [JsonProperty("PurchAmt")]
        public string PurchAmt
        {
            get;
            set;
        }

        [JsonProperty("NonS3DiscAmt")]
        public string NonS3DiscAmt
        {
            get;
            set;
        }

        [JsonProperty("TaxAmt")]
        public string TaxAmt
        {
            get;
            set;
        }

        [JsonProperty("QuantityType")]
        public string QuantityType
        {
            get;
            set;
        }

        [JsonProperty("PurQuantity")]
        public string PurQuantity
        {
            get;
            set;
        }
    }

    public class DiscountInfo
    {
        [JsonProperty("ProdCode")]
        public string ProdCode
        {
            get;
            set;
        }

        [JsonProperty("DeptNr")]
        public string DeptNr
        {
            get;
            set;
        }

        [JsonProperty("DiscLevel")]
        public string DiscLevel
        {
            get;
            set;
        }
        [JsonProperty("AppDiscAmt")]
        public string AppDiscAmt
        {
            get;
            set;
        }
        [JsonProperty("DiscDescr")]
        public string DiscDescr
        {
            get;
            set;
        }
    }

    public class RecptInfo
    {
        [JsonProperty("MessOrdinalNum")]
        public string MessOrdinalNum
        {
            get;
            set;
        }

        [JsonProperty("S3RecptMess")]
        public string S3RecptMess
        {
            get;
            set;
        }
    }

    public class BalanceInfo
    {
        [JsonProperty("OrdinalNum")]
        public string OrdinalNum
        {
            get;
            set;
        }

        [JsonProperty("BarCode")]
        public string BarCode
        {
            get;
            set;
        }

        [JsonProperty("ProgCode")]
        public string ProgCode
        {
            get;
            set;
        }
        [JsonProperty("PromoCode")]
        public string PromoCode
        {
            get;
            set;
        }
        [JsonProperty("BalUnit")]
        public string BalUnit
        {
            get;
            set;
        }

        [JsonProperty("BalVal")]
        public string BalVal
        {
            get;
            set;
        }
        [JsonProperty("PromoShort")]
        public string PromoShort
        {
            get;
            set;
        }
    }

    // temp
    public class DiscItems
    {
        [JsonProperty("DiscOrdinalNum")]
        public string DiscOrdinalNum
        {
            get;
            set;
        }
    }
}
