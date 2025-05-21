using System;
using System.Data.Entity.Spatial;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PossqlData
{
    [Table("CCTransmission_Log")]
    public partial class CCTransmission_Log
    {
        [Key,Column("TransNo")]
        public long TransNo { set; get; }

        [Column("TransDateTime")]
        public DateTime TransDateTime { set; get; }

        [Column("TransAmount")]
        public decimal TransAmount { set; get; }

        [Column("TransDataStr")]
        public string TransDataStr { set; get; }

        [Column("RecDataStr")]
        public string RecDataStr { set; get; }


        #region PRIMEPOS-2761 - VoidSummary

        [Column("StationID")]
        public string StationID { set; get; }

        [Column("UserID")]
        public string UserID { set; get; }

        [Column("PaymentProcessor")]
        public string PaymentProcessor { set; get; }

        [Column("TicketNo")]
        public string TicketNo { set; get; }

        [Column("TransmissionStatus")]
        public string TransmissionStatus { set; get; }

        [Column("HostTransID")]
        public string HostTransID { set; get; }

        [Column("POSTransID")]
        public string POSTransID { set; get; }

        [Column("POSPaymentID")]
        public string POSPaymentID { set; get; }

        [Column("TransType")]
        public string TransType { set; get; }

        [Column("IsReversed")]
        public bool IsReversed { set; get; }

        [Column("AmtApproved")]
        public decimal AmtApproved { set; get; }

        [Column("TerminalRefNumber")]
        public string TerminalRefNumber { set; get; }

        [Column("OrgTransNo")]
        public long OrgTransNo { set; get; }

        [Column("ResponseMessage")]
        public string ResponseMessage { set; get; }


        #endregion

        //Nilesh,Sajid PRIMEPOS-2854
        [Column("RecoveryFlag")]
        public string RecoveryFlag { get; set; }

        //PRIMEPOS-2990
        [Column("IsExpressTrans")]
        public bool IsExpressTrans { get; set; }

        //PRIMEPOS-3182
        [Column("last4")]
        public string last4 { get; set; }
    }
}
