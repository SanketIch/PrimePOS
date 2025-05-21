//Author : Ritesh
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make communication with PC Charge Payment Server & provide POS different processors
//External functions:None
//Known Bugs : None
//Start Date : 01 January 2008.
using System;
using System.Collections.Generic;
using System.Text;
using MMS.PROCESSOR;


namespace MMS.PAYMENT
{
    /// <summary>
    /// Author : Ritesh
    /// Functionality Desciption : The purpose of this class is to make communication with PC Charge Payment Server & provide POS different processors
    /// External functions:None
    /// Known Bugs : None
    /// Start Date : 01 January 2008.
    /// </summary>
    public class PaymentServer : IDisposable
    {
        #region variables

        private ICreditProcessor credit = null;
        private IDebitProcessor debit = null;
        private IEbtProcessor ebt = null;
        private INBSProcessor nbs = null; //PRIMEPOS-3372
        //Deleted unnecessary MerchatInfo By SRT(Ritesh) Date : 6 NOV 2008
        private MerchantInfo Merchant = null;
        //End Of Added By SRT(Gaurav)
        private Boolean Disposed = false;
        private String paymentProcessor = null;

        #endregion variables

        #region constant

        private const int MAX_PROCESSORS = 20;

        #endregion constant

        /// <summary>
        /// Author : Prashant
        /// Functionality Desciption : Theis is the Constructor of the PaymentServer
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 01 Jan 2008.
        /// </summary>
        public PaymentServer(String ProcessorName)
        {
            // initialize variables
            Merchant = new MerchantInfo();
            //End Of Added By SRT(Gaurav)
            this.paymentProcessor = ProcessorName;
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method provides User access to CreditProcessor class
        /// External functions:Methods of SocketClient Class.
        /// Known Bugs : None
        /// Start Date : 02 January 2008.
        /// </summary>
        /// <returns>CreditProcessor</returns>
        public ICreditProcessor GetCreditProcessor()
        {
            if (credit == null)
            {
                if (paymentProcessor == "PCCHARGE")
                {
                    credit = (ICreditProcessor)new PCCHARGE.CreditProcessor(Merchant);
                }
                else if (paymentProcessor == "XCHARGE")
                {
                    credit = (ICreditProcessor)new XCHARGE.CreditProcessor(Merchant);
                }
                //Added By SRT(Gaurav) Date: 06 NOV 2008
                else if (paymentProcessor == "XLINK")
                {
                    credit = (ICreditProcessor)new XLINK.CreditProcessor(Merchant);
                }
                //End Of Added By SRT(Gaurav)
                else if (paymentProcessor == "HPS")
                {
                    credit = (ICreditProcessor)new HPS.CreditProcessor(Merchant);
                }
                //Suraj
                else if (paymentProcessor == "HPSPAX")
                {
                    credit = (ICreditProcessor)new HPSPAX.CreditProcessor(Merchant);
                }
                //PRIMEPOS-2664
                else if (paymentProcessor == "EVERTEC")
                {
                    credit = (ICreditProcessor)new EVERTEC.CreditProcessor(Merchant);
                }
                //ADDED BY ARVIND PRIMEPOS-2636 
                else if (paymentProcessor == "VANTIV")
                {
                    credit = (ICreditProcessor)new VANTIV.CreditProcessor(Merchant);
                }
                #region PRIMEPOS-2841
                else if (paymentProcessor == "PRIMERXPAY")
                {
                    credit = (ICreditProcessor)new PrimeRxPay.CreditProcessor(Merchant);
                }
                #endregion
                else if (paymentProcessor == "ELAVON")//2943
                {
                    credit = (ICreditProcessor)new Elavon.CreditProcessor(Merchant);
                }
            }
            return credit;
            //Similarly make methods for Deit and other processors
        }

        public IDebitProcessor GetDebitProcessor()
        {
            if (debit == null)
            {
                if (paymentProcessor == "PCCHARGE")
                {
                    debit = (IDebitProcessor)new PCCHARGE.DebitProcessor(Merchant);
                }
                else if (paymentProcessor == "XCHARGE")
                {
                    debit = (IDebitProcessor)new XCHARGE.DebitProcessor(Merchant);
                }
                //Added By SRT(Gaurav) Date: 06 NOV 2008
                else if (paymentProcessor == "XLINK")
                {
                    debit = (IDebitProcessor)new XLINK.DebitProcessor(Merchant);
                }
                //End Of Added By SRT(Gaurav)
                else if (paymentProcessor == "HPS")
                {
                    debit = (IDebitProcessor)new HPS.DebitProcessor(Merchant);
                }
                ////Suraj
                else if (paymentProcessor == "HPSPAX")
                {
                    debit = (IDebitProcessor)new HPSPAX.DebitProcessor(Merchant);
                }
                ////PRIMEPOS-2664
                else if (paymentProcessor == "EVERTEC")
                {
                    debit = (IDebitProcessor)new EVERTEC.DebitProcessor(Merchant);
                }
                //PRIMEPOS-2636 ADDED BY ARVIND 
                else if (paymentProcessor == "VANTIV")
                {
                    debit = (IDebitProcessor)new VANTIV.DebitProcessor(Merchant);
                }
                else if (paymentProcessor == "ELAVON")//2943
                {
                    debit = (IDebitProcessor)new Elavon.DebitProcessor(Merchant);
                }
            }
            return debit;
            //Similarly make methods for Deit and other processors
        }

        public IEbtProcessor GetEBTProcessor()
        {
            if (ebt == null)
            {
                if (paymentProcessor == "XLINK")
                {
                    ebt = (IEbtProcessor)new XLINK.EBTProcessor(Merchant);
                }
                else if (paymentProcessor == "HPS")
                {
                    ebt = (IEbtProcessor)new HPS.EBTProcessor(Merchant);
                }
                //Suraj
                else if (paymentProcessor == "HPSPAX")
                {
                    ebt = (IEbtProcessor)new HPSPAX.EBTProcessor(Merchant);
                }
                //PRIMEPOS-2664
                else if (paymentProcessor == "EVERTEC")
                {
                    ebt = (IEbtProcessor)new EVERTEC.EBTProcessor(Merchant);
                }
                //PRIMEPOS-2636 ADDED BY ARVIND
                else if (paymentProcessor == "VANTIV")
                {
                    ebt = (IEbtProcessor)new VANTIV.EBTProcessor(Merchant);
                }
                else if (paymentProcessor == "ELAVON")//2943
                {
                    ebt = (IEbtProcessor)new Elavon.EBTProcessor(Merchant);
                }
            }
            return ebt;
            //Similarly make methods for Deit and other processors
        }

        #region PRIMEPOS-3372
        public INBSProcessor GetNBSProcessor()
        {
            if (nbs == null)
            {
                if (paymentProcessor == "VANTIV")
                {
                    nbs = (INBSProcessor)new VANTIV.NBSProcessor(Merchant);
                }
            }
            return nbs;
            //Similarly make methods for Deit and other processors
        }
        #endregion

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method provides infromation about Merchant.
        /// External functions:variables of MerchantInfo Class.
        /// Known Bugs : None
        /// Start Date : 02 January 2008.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="merchantNum"></param>
        /// <param name="processesors"></param>
        public void GetValidCards(out String[] cards)
        {
            //Same old method that is there.
            cards = Merchant.GetValidCards();
        }

        /*
        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method Closes the Batch for settlement.
        /// External functions:variables of MerchantInfo Class.
        /// Known Bugs : BatchProcessor methods
        /// Start Date : 02 January 2008.
        /// </summary>
        /// <param name="processors"></param>
        /// <param name="processesors"></param>
        /// <returns>String[]</returns>
        public String[] CloseBatch(String[] processors)
        {
            //Used for closing the batch
            PaymentResponse pmtResp = null;
            int count = 0;
            String[] response = new String[MAX_PROCESSORS];

            if (batch == null)
                batch = new BatchProcessor(Merchant);

            foreach (String processor in processors)
            {
                response[count] = batch.CloseBatch(processor, out pmtResp);
                count++;
            }
            return response;
        }
         * */

        /// <summary>
        /// Author : Prashant
        /// Functionality Desciption : Theis is the Destructor of the PaymentServer
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 01 Jan 2008.
        /// </summary>
        ~PaymentServer()
        {
            System.Diagnostics.Debug.Print("PaymentServer destructor\n");
            Dispose(false);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                if (credit != null)
                {
                    credit.Dispose();
                    credit = null;
                }
                /*
                if (batch != null)
                {
                    batch.Dispose();
                    batch = null;
                }
                */
                if (Merchant != null)
                {
                    Merchant.Dispose();
                    Merchant = null;
                }
            }

            #endregion IDisposable Members
        }
    }
}