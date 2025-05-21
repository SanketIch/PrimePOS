using Elavon.Connection;
using Elavon.Response;
using NLog;
using PossqlData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Elavon
{
    public class ElavonProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        private static ElavonProcessor elavonProcessor = null;
        public bool IsIdleScreenAdded = false;
        private string IPAddress = string.Empty;
        private int Port;
        private DeviceConnection device = null;
        private ElavonResponse elavonResponse = null;
        const char LF = (char)0x0A;
        const char CR = (char)0x0D;
        const char EOT = (char)0x04;
        char FS = (char)0x1C;
        bool IsTitle = false;
        static Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
        bool IsConnected = false;
        const string Description = "Description";
        const string Qty = "Qty";
        const string Price = "Price";
        const string Discount = "Discount";
        private string Title = String.Format("{0,0} {1,6} {2,9} {3,15}", Description.PadRight(15, ' '), Qty, Price, Discount.PadLeft(10, ' '));
        Decimal TotalGrossAmount = 0;
        int Increment = 4;

        private ElavonProcessor(String IPaddress, int Port)
        {
            device = new DeviceConnection();
            device.IPaddress = IPaddress;
            device.Port = Port;
            IsConnected = device.Connect();
            AddKeyValue();
        }

        private void AddKeyValue()
        {
            keyValuePairs.Add(Constant.TRANSTYPE, "0001");
            keyValuePairs.Add(Constant.AMOUNT, "0002");
            keyValuePairs.Add(Constant.REFERENCENUMBER, "0007");
            keyValuePairs.Add(Constant.USERDATA, "0011");
            keyValuePairs.Add(Constant.FORCED, "0012");
            keyValuePairs.Add(Constant.TRANSDATE, "0013");
            keyValuePairs.Add(Constant.TRANSTIME, "0014");
            keyValuePairs.Add(Constant.CASHBACK, "0017");
            keyValuePairs.Add(Constant.TERMINALID, "0109");
            keyValuePairs.Add(Constant.TRANSQUALIFIER, "0115");
            keyValuePairs.Add(Constant.CASHIERID, "0110");
            //keyValuePairs.Add(Constant.TIP, "0201");
            keyValuePairs.Add(Constant.MASKEDACCNUMBER, "1008");
            keyValuePairs.Add(Constant.ONGUARD, "8002");
            keyValuePairs.Add(Constant.CHAINCODE, "8006");
            keyValuePairs.Add(Constant.TAXEXEMPT71, "0071");
            keyValuePairs.Add(Constant.TAXEXEMPT72, "0072");

        }

        public static ElavonProcessor getInstance(String IPAddress, int Port)
        {
            if (elavonProcessor == null)
            {
                elavonProcessor = new ElavonProcessor(IPAddress, Port);
                elavonProcessor.Port = Port;
                elavonProcessor.IPAddress = IPAddress;
            }
            return elavonProcessor;
        }

        public ElavonResponse Sale(Dictionary<string, string> fields)
        {
            elavonResponse = new ElavonResponse();

            logger.Trace("ENTERED IN SALE METHOD");
            try
            {
                ElavonResponse elavonSignatureResponse = new ElavonResponse();
                string referenceNumber = GetNextRefNumber();

                fields.Add(Constant.REFERENCENUMBER, referenceNumber);

                if (fields.ContainsKey("IIASTRANSACTION") && fields["IIASTRANSACTION"].ToUpper() == "TRUE")
                {
                    if (fields.ContainsKey(Constant.IIASRXAMOUNT))
                        fields[Constant.AMOUNT] = fields[Constant.IIASRXAMOUNT];
                    else if (fields.ContainsKey("IIASAUTHORIZEDAMOUNT"))
                    {
                        fields[Constant.AMOUNT] = fields["IIASAUTHORIZEDAMOUNT"];
                        fields.Add(Constant.IIASRXAMOUNT, fields["IIASAUTHORIZEDAMOUNT"]);
                    }
                }
                string expirydate = string.Empty;
                if (fields.ContainsKey("EXPDATE"))
                {
                    expirydate = fields["EXPDATE"];
                }
                //string expirydate = string.Empty;

                #region PRIMEPOS-3244
                string[] tokens = null;
                string accountToken = string.Empty;
                string subSequence = string.Empty;
                if (fields.ContainsKey(Constant.TOKEN))
                {
                    tokens = fields[Constant.TOKEN].Split('|');
                    if (tokens.Length > 0)
                    {
                        accountToken = tokens[0];
                    }
                    if (tokens.Length > 1)
                    {
                        subSequence = tokens[1];
                    }
                }
                #endregion

                string request = string.Format("" + keyValuePairs[Constant.TRANSTYPE] + ",02{0}{1}" +
                    keyValuePairs[Constant.AMOUNT] + "," + fields[Constant.AMOUNT] +
                    (fields.ContainsKey(Constant.TOKEN) ? "{0}{1}0003,ID:" + accountToken + "{0}{1}0047,M;1;1;1;1;0;6;0;0;1;3;C;0;4{0}{1}0054,01" + "{0}{1}0723,U" + "{0}{1}0738," + subSequence + (!string.IsNullOrWhiteSpace(expirydate) ? "{0}{1}0004," + expirydate : expirydate) : "{0}{1}0723,F") + "{0}{1}" +
                keyValuePairs[Constant.REFERENCENUMBER] + "," + referenceNumber + "{0}{1}" + keyValuePairs[Constant.USERDATA] + ",035S1{0}{1}" +
                /*(fields.ContainsKey(Constant.ISALLOWDUP) && fields[Constant.ISALLOWDUP].ToUpper() == "TRUE" ? keyValuePairs[Constant.FORCED] + ",1{0}{1}" : string.Empty)*/
                keyValuePairs[Constant.CASHBACK] + ",0.00{0}{1}" + keyValuePairs[Constant.TERMINALID] + "," + fields[Constant.TERMINALID] + "{0}{1}" +
                keyValuePairs[Constant.TRANSQUALIFIER] + "," + (fields[Constant.TRANSACTIONTYPE].Contains("CREDIT") ? "010" : "030") + "{0}{1}" +
                keyValuePairs[Constant.CASHIERID] + ",205{0}{1}" + (fields.ContainsKey("ISTAX") && fields["ISTAX"].ToUpper() == "TRUE" ? keyValuePairs[Constant.TAXEXEMPT71] + ",1{0}{1}0072," + fields["TAX"] + "{0}{1}" : keyValuePairs[Constant.TAXEXEMPT71] + ",0{0}{1}" + keyValuePairs[Constant.TAXEXEMPT72] + ",0.00{0}{1}") //+ keyValuePairs[Constant.TIP] + ",0.00{0}{1}"
                + keyValuePairs[Constant.MASKEDACCNUMBER] +
                ",ID:{0}{1}" + (fields.ContainsKey(Constant.IIASRXAMOUNT) ? "1074,00;4S;840;C;" + fields[Constant.AMOUNT] + "{0}{1}1075,00;4U;840;C;" + fields[Constant.AMOUNT] + "{0}{1}" : string.Empty) +
                "0060," + fields["StationID"] + "{0}{1}0070," + referenceNumber + "{0}{1}0401," + referenceNumber + "{0}{1}0406,BRUSH{0}{1}0412,23;1;13.42{0}{1}"
                + keyValuePairs[Constant.ONGUARD] + "," + fields[Constant.LOCATIONNAME] + "{0}{1}0647,1{0}{1}" + keyValuePairs[Constant.CHAINCODE] + "," + fields[Constant.CHAINCODE] +
                //+
                "{0}{1}{2}", CR, LF, EOT);
                /*request = string.Format("0001,02{0}{1}0002,217.75{0}{1}0007,7{0}{1}0011,35{0}{1}0013,031022{0}{1}0014,132146{0}{1}0017,0.00{0}{1}0109,RETERM1{0}{1}0115,010{0}{1}0110,205{0}{1}0201,0.00{0}{1}1008,ID:{0}{1}8002,MMS{0}{1}8006,TSTLA3{0}{1}{2}", CR, LF, EOT);*/
                /*request = string.Format("0001,02{0}{1}0002,108.88{0}{1}0723,F{0}{1}0007,584{0}{1}0011,035S1{0}{1}0017,0.00{0}{1}0109,EBTERM1{0}{1}0115,010{0}{1}0110,205{0}{1}0071,1{0}{1}0072,8.88{0}{1}1008,ID:{0}{1}0060,19{0}{1}0070,584{0}{1}0401,584{0}{1}0406,BRUSH{0}{1}0412,23;1;13.42{0}{1}8002,MMS{0}{1}0647,1{0}{1}8006,TSTLA3{0}{1}{2}", CR, LF, EOT);*/



                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);


                string response = string.Empty;

                do
                {
                    device.ReceiveMessage(out response);
                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        elavonResponse.ElavonSaleResponse(response);
                    }
                    else
                    {
                        Thread.Sleep(20000);
                        elavonResponse = elavonProcessor.Inquiry(fields);
                    }
                }
                while (elavonResponse.IsListenAgain);

                elavonResponse.deviceRequest = request;

                if (elavonResponse.Result.Contains(Constant.COMMERROR))
                {
                    elavonResponse = Inquiry(fields);
                }

                if (elavonResponse.IsSignatureRequired)
                {
                    string signResponse = string.Empty;
                    device.ReceiveMessage(out signResponse);
                    /*elavonResponse.SignatureData = */
                    elavonSignatureResponse.ParseSignature(signResponse);
                    elavonResponse.SignatureData = elavonResponse.SignatureString = elavonSignatureResponse.SignatureData;
                }

                if (elavonResponse.EmvReceipt != null)
                {
                    elavonResponse.EmvReceipt.ReferenceNumber = referenceNumber;
                }

                SaveRefNumber(referenceNumber);
                if (fields.ContainsKey("IIASTRANSACTION") && fields["IIASTRANSACTION"].ToUpper() == "TRUE")
                {
                    if (elavonResponse.Result.ToUpper() == "COMPLETE" && !string.IsNullOrWhiteSpace(elavonResponse.AmountApproved))
                    {
                        elavonResponse.IsFSATransaction = "T";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN SALE METHOD" + ex);
            }
            return elavonResponse;
        }
        public ElavonResponse Void(Dictionary<string, string> fields)
        {
            elavonResponse = new ElavonResponse();
            logger.Trace("ENTERED IN VOID METHOD");
            try
            {
                if (fields[Constant.AMOUNT].Contains("-"))
                {
                    fields[Constant.AMOUNT] = fields[Constant.AMOUNT].Remove(0, 1);
                }
                string referenceNumber = GetNextRefNumber();
                if (fields["TRANSACTIONTYPE"].Contains("DEBIT"))
                {
                    elavonResponse = Return(fields);
                    return elavonResponse;
                }
                string expirydate = string.Empty;
                if (fields.ContainsKey("EXPDATE"))
                {
                    expirydate = fields["EXPDATE"];
                }
                string request = string.Format("" + keyValuePairs[Constant.TRANSTYPE] + ",11{0}{1}" +
                    (!string.IsNullOrWhiteSpace(expirydate) ? "0004," + expirydate + "{0}{1}" : expirydate) +
                    keyValuePairs[Constant.AMOUNT] + "," + fields[Constant.AMOUNT] + "{0}{1}0003,ID:" +
                    fields[Constant.TRANSACTIONID].Split('|')[1] + "{0}{1}" + keyValuePairs[Constant.REFERENCENUMBER] + "," +
                    fields[Constant.TRANSACTIONID].Split('|')[0] + "{0}{1}" + keyValuePairs[Constant.USERDATA] +
                    ",35{0}{1}0043,015341{0}{1}" + keyValuePairs[Constant.TERMINALID] + ","
                    + fields[Constant.TERMINALID] + "{0}{1}" + keyValuePairs[Constant.CASHIERID] + ",205{0}{1}" +
                    "0060," + fields["StationID"] + "{0}{1}0070," + referenceNumber /*+ "{0}{1}0401," + referenceNumber + "{0}{1}0406,BRUSH{0}{1}0412,23;1;13.42{0}{1}"*/
                    + keyValuePairs[Constant.TRANSQUALIFIER] + ",010{0}{1}" + keyValuePairs[Constant.ONGUARD] + ","
                    + fields[Constant.LOCATIONNAME] + "{0}{1}0047,M;1;1;1;1;0;6;0;0;1;3;C;0;4{0}{1}0054,01" + "{0}{1}" + keyValuePairs[Constant.CHAINCODE] + "," +
                    fields[Constant.CHAINCODE] + "{0}{1}", CR, LF, EOT);

                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);

                string response = string.Empty;

                do
                {
                    device.ReceiveMessage(out response);
                    elavonResponse.ElavonVoidResponse(response);
                }
                while (elavonResponse.IsListenAgain);

                elavonResponse.deviceRequest = request;
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN VOID " + ex);
            }
            return elavonResponse;
        }
        public ElavonResponse VoidReturn(Dictionary<string, string> fields)
        {
            elavonResponse = new ElavonResponse();

            logger.Trace("ENTERED IN VOIDRETURN METHOD");

            try
            {
                if (fields[Constant.AMOUNT].Contains("-"))
                {
                    fields[Constant.AMOUNT] = fields[Constant.AMOUNT].Remove(0, 1);
                }
                string expirydate = string.Empty;
                if (fields.ContainsKey("EXPDATE"))
                {
                    expirydate = fields["EXPDATE"];
                }
                string referenceNumber = GetNextRefNumber();
                string request = string.Format("" + keyValuePairs[Constant.TRANSTYPE] + ",17{0}{1}" + keyValuePairs[Constant.AMOUNT]
                    + "," + fields[Constant.AMOUNT] + "{0}{1}0003,ID:" + fields[Constant.TRANSACTIONID].Split('|')[1] + "{0}{1}" + keyValuePairs[Constant.REFERENCENUMBER]
                    + "," + fields[Constant.TRANSACTIONID].Split('|')[0] + "{0}{1}" + keyValuePairs[Constant.USERDATA] +
                    ",35{0}{1}0043,015341{0}{1}" + keyValuePairs[Constant.TERMINALID] + "," +
                    /*fields[Constant.TERMINALID]*/fields[Constant.TERMINALID] + "{0}{1}" +
                    (!string.IsNullOrWhiteSpace(expirydate) ? "0004," + expirydate + "{0}{1}" : expirydate) +
                    keyValuePairs[Constant.CASHIERID] + ",205{0}{1}" +
                    keyValuePairs[Constant.TRANSQUALIFIER] + ",010{0}{1}" +
                    keyValuePairs[Constant.ONGUARD] + "," + /*fields[Constant.LOCATIONNAME]*/fields[Constant.LOCATIONNAME] +
                    "{0}{1}0060," + fields["StationID"] + "{0}{1}0070," + referenceNumber /*+ "{0}{1}0401," + referenceNumber + "{0}{1}0406,BRUSH{0}{1}0412,23;1;13.42"*/ +
                    "{0}{1}0047,M;1;1;1;1;0;6;0;0;1;3;C;0;4{0}{1}0054,01" + "{0}{1}" + keyValuePairs[Constant.CHAINCODE] + "," +
                    /*fields[Constant.CHAINCODE]*/fields[Constant.CHAINCODE] + "{0}{1}", CR, LF, EOT);
                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);

                string response = string.Empty;

                do
                {
                    device.ReceiveMessage(out response);
                    elavonResponse.ElavonVoidResponse(response);
                }
                while (elavonResponse.IsListenAgain);

                elavonResponse.deviceRequest = request;

            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN VOIDRETURN" + ex);
            }
            return elavonResponse;
        }
        public ElavonResponse Return(Dictionary<string, string> fields)
        {
            elavonResponse = new ElavonResponse();

            logger.Trace("ENTERED IN RETURN METHOD");

            try
            {
                if (fields.ContainsKey(Constant.TRANSDATE))
                {
                    if (!fields["TRANSACTIONTYPE"].Contains("DEBIT"))
                    {
                        DateTime dtnow = DateTime.Now;
                        DateTime dtTransDate = Convert.ToDateTime(fields[Constant.TRANSDATE]);

                        double days = (dtnow - dtTransDate).TotalDays;
                        if (days < 1)
                        {
                            elavonResponse = Void(fields);
                            return elavonResponse;
                        }
                    }
                }

                string referenceNumber = GetNextRefNumber();

                fields.Add(Constant.REFERENCENUMBER, referenceNumber);

                if (fields[Constant.AMOUNT].Contains("-"))
                {
                    fields[Constant.AMOUNT] = fields[Constant.AMOUNT].Remove(0, 1);
                }
                string text = string.Empty;

                if (fields["TRANSACTIONTYPE"].Contains("DEBIT") && (fields.ContainsKey("TRANSACTIONID") && fields["TRANSACTIONID"].Contains("|")) && !fields.ContainsKey(Constant.TRANSDATE))
                {
                    text = text = "0003,ID:" +
                    fields[Constant.TRANSACTIONID].Split('|')[1] + "{0}{1}" + keyValuePairs[Constant.REFERENCENUMBER] + "," +
                    fields[Constant.TRANSACTIONID].Split('|')[0] + "{0}{1}0047,M;1;1;1;1;0;6;0;0;1;3;C;0;4{0}{1}0054,01{0}{1}";
                }
                else if (fields.ContainsKey("TRANSACTIONID") && fields["TRANSACTIONID"].Contains("|") && fields["TRANSACTIONTYPE"].Contains("CREDIT"))
                {
                    text = text = "0003,ID:" +
                    fields[Constant.TRANSACTIONID].Split('|')[1] + "{0}{1}" + keyValuePairs[Constant.REFERENCENUMBER] + "," +
                    fields[Constant.TRANSACTIONID].Split('|')[0] + "{0}{1}0047,M;1;1;1;1;0;6;0;0;1;3;C;0;4{0}{1}0054,01{0}{1}";
                }
                else
                    text = keyValuePairs[Constant.REFERENCENUMBER] + "," + referenceNumber + "{0}{1}";
                string requiredFields = string.Empty;
                if (!fields["TRANSACTIONTYPE"].Contains("EBT"))
                {
                    text += "0060," + fields["StationID"] + "{0}{1}0070," + referenceNumber + "{0}{1}0401," + referenceNumber + "{0}{1}0406,BRUSH{0}{1}0412,23;1;13.42{0}{1}";
                }

                string request = string.Format("" + keyValuePairs[Constant.TRANSTYPE] + ",09{0}{1}" + keyValuePairs[Constant.AMOUNT] +
                    "," + fields[Constant.AMOUNT] + "{0}{1}"
                    + text +
                    keyValuePairs[Constant.USERDATA] + ",35{0}{1}"
                    + keyValuePairs[Constant.TERMINALID] + "," + fields[Constant.TERMINALID] +
                    "{0}{1}" + keyValuePairs[Constant.TRANSQUALIFIER] + ",010{0}{1}" +
                    keyValuePairs[Constant.CASHIERID] + ",205{0}{1}" + keyValuePairs[Constant.MASKEDACCNUMBER] + ",ID:{0}{1}"
                    + keyValuePairs[Constant.ONGUARD] + "," + fields[Constant.LOCATIONNAME] +
                    //"{0}{1}0060," + fields["StationID"] + "{0}{1}0070," + referenceNumber + "{0}{1}0401," + referenceNumber + "{0}{1}0406,BRUSH{0}{1}0412,23;1;13.42" +
                    "{0}{1}" + keyValuePairs[Constant.CHAINCODE] + "," + fields[Constant.CHAINCODE] + "{0}{1}", CR, LF, EOT);

                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);

                string response = string.Empty;

                do
                {
                    device.ReceiveMessage(out response);
                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        elavonResponse.ElavonSaleResponse(response);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(20000);
                        elavonResponse = Inquiry(fields);
                    }
                }
                while (elavonResponse.IsListenAgain);

                if (elavonResponse.Result.Contains(Constant.COMMERROR))
                {
                    elavonResponse = Inquiry(fields);
                }

                if (elavonResponse.EmvReceipt != null)
                {
                    elavonResponse.EmvReceipt.ReferenceNumber = referenceNumber;
                }

                SaveRefNumber(referenceNumber);

                elavonResponse.deviceRequest = request;
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN RETURN METHOD" + ex);
            }
            return elavonResponse;
        }
        public ElavonResponse EBT(Dictionary<string, string> fields)
        {
            elavonResponse = new ElavonResponse();

            logger.Trace("ENTERED IN EBT METHOD");

            try
            {
                //bool isconn = device.Connect();

                if (fields[Constant.AMOUNT].Contains("-"))
                {
                    fields[Constant.AMOUNT] = fields[Constant.AMOUNT].Remove(0, 1);

                }

                string TransactionType = string.Empty;
                if (fields.ContainsKey(Constant.TRANSACTIONTYPE))
                {
                    if (fields[Constant.TRANSACTIONTYPE].Contains("RETURN"))
                    {
                        if (fields.ContainsKey("ISEBTVOID") && fields["ISEBTVOID"].ToUpper() == "TRUE")
                        {
                            TransactionType = "11";
                        }
                        else
                            TransactionType = "09";
                        if (fields.ContainsKey(Constant.TRANSDATE))
                        {
                            if (!fields["TRANSACTIONTYPE"].Contains("DEBIT"))
                            {
                                DateTime dtnow = DateTime.Now;
                                DateTime dtTransDate = Convert.ToDateTime(fields[Constant.TRANSDATE]);

                                double days = (dtnow - dtTransDate).TotalDays;
                                if (days < 1)
                                {
                                    TransactionType = "11";
                                }
                            }
                        }
                    }
                    else
                    {
                        TransactionType = "02";
                    }
                }

                string referenceNumber = GetNextRefNumber();

                string text = string.Empty;

                string expirydate = string.Empty;
                if (fields.ContainsKey("EXPDATE"))
                {
                    expirydate = fields["EXPDATE"];
                }

                if (TransactionType == "11" || (fields.ContainsKey("TRANSACTIONID") && fields["TRANSACTIONID"].Contains("|")))
                {
                    text = "0003,ID:" +
                    fields[Constant.TRANSACTIONID].Split('|')[1] + "{0}{1}" + keyValuePairs[Constant.REFERENCENUMBER] + "," +
                    fields[Constant.TRANSACTIONID].Split('|')[0] + "{0}{1}" + "0047,M;1;1;1;1;0;6;0;0;1;3;C;0;4{0}{1}0057,01{0}{1}" + (!string.IsNullOrWhiteSpace(expirydate) ? "0004," + expirydate + "{0}{1}" : expirydate);
                }
                else
                    text = keyValuePairs[Constant.REFERENCENUMBER] + "," + referenceNumber + "{0}{1}";

                string request = string.Format(keyValuePairs[Constant.TRANSTYPE] + "," + TransactionType + "{0}{1}0002," + fields[Constant.AMOUNT] +
                    "{0}{1}" + text + keyValuePairs[Constant.USERDATA] +
                    ",35{0}{1}" + keyValuePairs[Constant.CASHBACK] + ",0.00{0}{1}" + keyValuePairs[Constant.TERMINALID] + "," + fields[Constant.TERMINALID] +
                    "{0}{1}" + keyValuePairs[Constant.TRANSQUALIFIER] + ",040{0}{1}0110,205{0}{1}"/*0201,0.00{0}{1}*/
                    //keyValuePairs[Constant.ONGUARD] + ",ID:" +  "{0}{1}" 
                    + "1008" + ",ID:" +
                    "{0}{1}0119,010{0}{1}1085," + fields[Constant.AMOUNT] +
                    "{0}{1}0060," + fields["StationID"] + "{0}{1}0070," + referenceNumber + "{0}{1}0071,0{0}{1}0072,0.00{0}{1}"/*0401," + referenceNumber + "{0}{1}0406,BRUSH{0}{1}0412,23;1;13.42{0}{1}"*/
                    + keyValuePairs[Constant.ONGUARD] + "," + fields[Constant.LOCATIONNAME] + "{0}{1}8006," + fields[Constant.CHAINCODE] + "{0}{1}{2}", CR, LF, EOT);

                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);

                string response = string.Empty;

                do
                {
                    device.ReceiveMessage(out response);
                    if (TransactionType == "11")
                        elavonResponse.ElavonVoidResponse(response);
                    else
                        elavonResponse.ElavonSaleResponse(response);
                }
                while (elavonResponse.IsListenAgain);

                //device.Disconnect();

                if (elavonResponse.EmvReceipt != null)
                {
                    elavonResponse.EmvReceipt.ReferenceNumber = referenceNumber;
                }

                SaveRefNumber(referenceNumber);

                elavonResponse.deviceRequest = request;
            }
            catch (Exception ex)
            {
                logger.Error("ERROR in EBT " + ex);
            }
            return elavonResponse;
        }
        public ElavonResponse Consent(Dictionary<string, string> fields)
        {
            elavonResponse = new ElavonResponse();

            try
            {
                if (fields[Constant.AMOUNT].Contains("-"))
                {
                    fields[Constant.AMOUNT] = fields[Constant.AMOUNT].Remove(0, 1);
                }

                string referenceNumber = GetNextRefNumber();

                string request = string.Format("" + keyValuePairs[Constant.TRANSTYPE] + ",09{0}{1}" +
                    keyValuePairs[Constant.AMOUNT] + "," + fields[Constant.AMOUNT] +
                    "{0}{1}" + keyValuePairs[Constant.REFERENCENUMBER] + "," + referenceNumber +
                    "{0}{1}" + keyValuePairs[Constant.USERDATA] + ",35{0}{1}" + keyValuePairs[Constant.TERMINALID] +
                    "," + fields[Constant.TERMINALID] + "{0}{1}" + keyValuePairs[Constant.TRANSQUALIFIER] +
                    ",010{0}{1}" + keyValuePairs[Constant.CASHIERID] + ",205{0}{1}" + keyValuePairs[Constant.MASKEDACCNUMBER] +
                    ",ID:{0}{1}" + keyValuePairs[Constant.ONGUARD] + "," + fields[Constant.LOCATIONNAME] + "{0}{1}" +
                    keyValuePairs[Constant.CHAINCODE] + "," + fields[Constant.CHAINCODE] + "{0}{1}", CR, LF, EOT);

                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);

                string response = string.Empty;

                do
                {
                    device.ReceiveMessage(out response);
                    elavonResponse.ElavonSaleResponse(response);
                }
                while (elavonResponse.IsListenAgain);

                SaveRefNumber(referenceNumber);
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN CONSENT METHOD" + ex);
            }
            return elavonResponse;
        }

        private string GetNextRefNumber()
        {
            TransactionReference tr = new TransactionReference();
            try
            {
                var db = new Possql();
                tr = db.TransactionReferences.Where(w => w.Processor == "ELAVON").SingleOrDefault();

                tr.LastTransaction = (Convert.ToInt32(tr.LastTransaction) + 1).ToString();
                if (Convert.ToInt32(tr.LastTransaction) > 99999)
                {
                    tr.LastTransaction = "1";
                    db.TransactionReferences.Attach(tr);
                    db.Entry(tr).Property(p => p.LastTransaction).IsModified = true;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return tr.LastTransaction;
        }
        private void SaveRefNumber(string ReferenceNumber)
        {
            var db = new Possql();
            TransactionReference tr = new TransactionReference();
            tr = db.TransactionReferences.Where(w => w.Processor == "ELAVON").SingleOrDefault();
            tr.LastTransaction = ReferenceNumber;
            db.TransactionReferences.Attach(tr);
            db.Entry(tr).Property(p => p.LastTransaction).IsModified = true;
            db.SaveChanges();
        }

        public ElavonResponse ShowDialogForm(Hashtable Data, string FormType)
        {
            elavonResponse = new ElavonResponse();

            logger.Trace("ENTERED IN SHOWDIALOGFORM");
            try
            {
                IsIdleScreenAdded = true;
                IdleScreen();
                string request = string.Empty;
                IsIdleScreenAdded = false;
                if (FormType == "PATIENTTYPE")
                {
                    request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",14123050300{0}{1}5001,0400740{3}" + //PRIMEPOS-3068
                        "Patient Consent" + "{3}" + Data["Header"] + "{3}{3}" + Data["LABEL1"] + "{3}" + Data["LABEL2"] + "{3}"
                        + Data["LABEL3"] + "{3}" + Data["LABEL4"] + "{0}{1}{2}", CR, LF, EOT, FS);
                }

                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);
                string response = string.Empty;
                Thread.Sleep(3000);
                device.ReceiveMessage(out response);
                if (response.Contains("INVALID") || response == string.Empty)
                {
                    device.ReceiveMessage(out response);
                }
                elavonResponse.ShowDialogForm(response);
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN SHOWDIALOGFORM" + ex);
            }
            return elavonResponse;
        }
        public ElavonResponse ShowTextBox(string Title, string Text, Hashtable hashtable, string ScreenDisplay)
        {
            elavonResponse = new ElavonResponse();

            logger.Trace("ENTERED IN SHOWTEXTBOX METHOD");

            try
            {
                //IdleScreen();
                string request = string.Empty;
                IsIdleScreenAdded = true;
                IdleScreen();
                CancelMessage();
                IsIdleScreenAdded = false;
                if (ScreenDisplay == "HEALTHIXFRM1")
                {
                    request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",14123070300{0}{1}5001,0700561{3}Next{3}2{3}" + Text + "{0}{1}{2}", CR, LF, EOT, FS);//PRIMEPOS-3068
                }
                if (ScreenDisplay == "HEALTHIXFRM2")
                {
                    request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",14123070300{0}{1}5001,0700881{3}All Healthix Participants{3}" + hashtable["PHARMACYNAME"] + "{3}2{3}" + Text + "{0}{1}{2}", CR, LF, EOT, FS);//PRIMEPOS-3068
                }
                if (ScreenDisplay == "HEALTHIXFRM3")
                {
                    //CancelMessage();                    
                    //request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",14123040060{0}{1}5001,04013170{3}" + "Consent" + "{3}" + hashtable["HEADER"] + "{3}{3}Consent{3}Emergency{3}Deny{3}{0}{1}{2}", CR, LF, EOT, FS);
                    #region PRIMEPOS-3068
                    request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] +
                    ",14123050300{0}{1}5001,0400740{3}" + "Patient Consent" + "{3}" + hashtable["HEADER"] +
                    "{3}{3}" + "I AGREE" + "{3}" + "I DENY" + "{3}" + "EMERGENCY ONLY" + "{0}{1}{2}", CR, LF, EOT, FS);
                    #endregion
                }
                if (ScreenDisplay == "RXHEADERINFO" || ScreenDisplay == "HIPPAACK")
                {
                    request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",14123070300{0}{1}5001,0701211{3}I Agree{3}I Do Not Agree{3}2{3}" + Text + "{0}{1}{2}", CR, LF, EOT, FS);//PRIMEPOS-3068
                }
                if (ScreenDisplay == "OTCITEMDESC")
                {
                    request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",14123070300{0}{1}5001,0700561{3}Proceed{3}2{3}" + Text + "{0}{1}{2}", CR, LF, EOT, FS);//PRIMEPOS-3068
                }

                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);

                Thread.Sleep(3000);
                string response = string.Empty;

                device.ReceiveMessage(out response);

                if (response.Contains("INVALID") || response == string.Empty)
                {
                    device.ReceiveMessage(out response);
                }
                elavonResponse.ShowDialogForm(response);
                //CancelMessage();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return elavonResponse;
        }

        public ElavonResponse DisplayMessage(string Text)
        {
            elavonResponse = new ElavonResponse();

            logger.Trace("ENTERED IN DISPLAYMESSAGE METHOD");

            try
            {
                if (Text.ToUpper().Contains("TOTAL"))
                {
                    IsIdleScreenAdded = true;
                    IdleScreen();
                    IsIdleScreenAdded = false;
                }
                string request = string.Empty;
                request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",14123020300{0}{1}5001,0200390" + Text + "{0}{1}{2}", CR, LF, EOT, FS);//PRIMEPOS-3068

                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN DISPLAYMESSAGE" + ex);
            }
            return elavonResponse;
        }
        public ElavonResponse AddItem(Dictionary<int, Hashtable> data, string Action)
        {
            elavonResponse = new ElavonResponse();

            logger.Trace("ENTERED IN ADDITEM METHOD");

            try
            {
                decimal tax = 0;
                string request = string.Empty;


                if (Action == "A")
                {
                    Hashtable hashtable = new Hashtable();
                    data.TryGetValue(data.Keys.Max(), out hashtable);
                    string ItemData = String.Format("{0,0} {1,6} {2,9} {3,15}", hashtable["Name"].ToString().Trim().PadRight(15, ' '), hashtable["Qty"].ToString().Trim(), hashtable["Price"].ToString().Trim(), hashtable["Discount"].ToString().Trim().PadLeft(10, ' '));

                    tax = Convert.ToDecimal(hashtable["Tax"].ToString().Trim());
                    //if (TotalGrossAmount != 0)
                    TotalGrossAmount = TotalGrossAmount + tax + Convert.ToDecimal(hashtable["Total"].ToString().Trim());
                    //else
                    //    TotalGrossAmount = tax + Convert.ToDecimal(hashtable["Total"].ToString().Trim());

                    string TotalAmount = "Total Amount : " + TotalGrossAmount.ToString();

                    if (!IsTitle)
                    {
                        string Total = "Total Amount : 0.00";
                        request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",10020{0}{1}5001,001" + Title.Length.ToString("000") + Title + "002" + Total.Length.ToString("000") + Total + "{0}{1}{2}", CR, LF, EOT, FS);

                        logger.Trace("REQEUST IS :" + request);

                        SendRequest(request);
                        IsTitle = true;
                        //TotalGrossAmount = 0;
                    }

                    request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",10020{0}{1}5001,001" + ItemData.Length.ToString("000") + ItemData + "002" + TotalAmount.Length.ToString("000") + TotalAmount + "{0}{1}{2}", CR, LF, EOT, FS);

                    logger.Trace("REQEUST IS :" + request);

                    SendRequest(request);
                }
                else if ((Action == "R" || Action == "D" || Action == "U") && data.Count > 0)
                {
                    Hashtable hashtable = new Hashtable();
                    data.TryGetValue(data.Keys.Min(), out hashtable);
                    TotalGrossAmount = 0;
                    bool IsStopScroll = false;
                    IsTitle = false;
                    decimal totalAmount = 0;
                    foreach (KeyValuePair<int, Hashtable> keyValuePair in data)
                    {
                        Hashtable items = keyValuePair.Value;
                        tax += Convert.ToDecimal(items["Tax"].ToString().Trim());
                        string ItemData = String.Format("{0,0} {1,6} {2,9} {3,15}", items["Name"].ToString().Trim().PadRight(15, ' '), items["Qty"].ToString().Trim(), items["Price"].ToString().Trim(), items["Discount"].ToString().Trim().PadLeft(10, ' '));
                        totalAmount += Convert.ToDecimal(items["Total"].ToString().Trim());
                        //TotalGrossAmount = 0;
                        //if (TotalGrossAmount != 0)
                        TotalGrossAmount = totalAmount + tax;
                        //else
                        //    TotalGrossAmount = tax + Convert.ToDecimal(hashtable["Total"].ToString().Trim());

                        string TotalAmount = "Total Amount : " + TotalGrossAmount.ToString();

                        if (!IsStopScroll)
                        {
                            request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",11001{0}{1}{2}", CR, LF, EOT, FS);
                            IsStopScroll = true;
                            logger.Trace("REQEUST IS :" + request);
                            SendRequest(request);

                            if (!IsTitle)
                            {
                                string Total = "Total Amount : 0";
                                request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",10020{0}{1}5001,001" + Title.Length.ToString("000") + Title + "002" + Total.Length.ToString("000") + Total + "{0}{1}{2}", CR, LF, EOT, FS);
                                logger.Trace("REQEUST IS :" + request);
                                SendRequest(request);
                                IsTitle = true;
                                //TotalGrossAmount = 0;
                            }
                        }

                        request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",10020{0}{1}5001,001" + ItemData.Length.ToString("000") + ItemData + "002" + TotalAmount.Length.ToString("000") + TotalAmount + "{0}{1}{2}", CR, LF, EOT, FS);
                        logger.Trace("REQEUST IS :" + request);
                        SendRequest(request);
                    }
                }
                else
                {
                    request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",11001{0}{1}{2}", CR, LF, EOT, FS);
                    logger.Trace("REQEUST IS :" + request);
                    SendRequest(request);
                    IsTitle = false;
                    TotalGrossAmount = 0;
                }
            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN ADDITEM METHOD" + ex);
            }
            return elavonResponse;
        }
        public ElavonResponse GetSimpleSignature()
        {
            elavonResponse = new ElavonResponse();

            logger.Trace("ENTERED IN GETSIMPLESIGNATURE METHOD");

            try
            {
                IsIdleScreenAdded = true;
                IdleScreen();
                CancelMessage();
                IsIdleScreenAdded = false;
                string request = string.Empty;
                request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",0100" + Increment.ToString() + "001000{0}{1}5001,001025{0}{1}{2}", CR, LF, EOT);

                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);

                string response = string.Empty;

                Increment++;

                Thread.Sleep(3000);

                device.ReceiveMessage(out response);

                elavonResponse.ParseSignature(response);
            }
            catch (Exception ex)
            {
                logger.Error("Error in GetSimpleSignature method" + ex);
            }
            return elavonResponse;
        }
        #region PRIMEPOS-3260
        public void HealthMessage()
        {
            logger.Trace("ENTERED IN HealthMessage");
            try
            {

                string request = string.Format("0001,73{0}{1}" + keyValuePairs[Constant.USERDATA] + ",0013" + DateTime.Now.ToString("MMddyy") + "{0}{1}{2}", CR, LF, EOT, FS);

                logger.Trace($"HealthMessage Request is :{request}");

                SendRequest(request);

                string response = string.Empty;
                elavonResponse = new ElavonResponse();
                do
                {
                    device.ReceiveMessage(out response);
                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        logger.Trace($"Health Message Response:{response}");
                        elavonResponse.ElavonHealthMessageResponse(response);
                    }
                }
                while (elavonResponse.IsListenAgain);

            }
            catch (Exception ex)
            {
                logger.Error($"Follwong Exception occured on ElavonProcessor==>HealthMessage Method: {ex}");
            }
        }
        #endregion
        public void IdleScreen()
        {
            logger.Trace("ENTERED IN IDLESCREEN");
            try
            {
                if (IsIdleScreenAdded)
                {
                    string request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",11001{0}{1}{2}", CR, LF, EOT, FS);

                    logger.Trace("REQEUST IS :" + request);

                    SendRequest(request);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in IdleScreen Method" + ex);
            }
        }
        public ElavonResponse Inquiry(Dictionary<string, string> fields)
        {
            ElavonResponse elavonResponse = new ElavonResponse();

            logger.Trace("ENTERED IN INQUIRY METHOD");

            try
            {

                if (fields[Constant.AMOUNT].Contains("-"))
                {
                    fields[Constant.AMOUNT] = fields[Constant.AMOUNT].Remove(0, 1);
                }

                //string request = string.Format("0001,22{0}{1}0002," + fields[Constant.AMOUNT] + "{0}{1}0007," +
                //    fields[Constant.REFERENCENUMBER] + "{0}{1}" + keyValuePairs[Constant.USERDATA] +
                //    ",35{0}{1}" + "0013,031621{0}{1}0014,212508{0}{1}" +
                //    keyValuePairs[Constant.TERMINALID] + "," + fields[Constant.TERMINALID] +
                //    "{0}{1}"/*0201,0.00{0}{1}"*/ + keyValuePairs[Constant.ONGUARD] + ",ID:{0}{1}" + keyValuePairs[Constant.ONGUARD] + "," + fields[Constant.LOCATIONNAME] + "{0}{1}8006," + fields[Constant.CHAINCODE] + "{0}{1}{2}", CR, LF, EOT);

                string request = string.Format("0001,22{0}{1}0002," + fields[Constant.AMOUNT] + "{0}{1}0007," +
                    fields[Constant.REFERENCENUMBER] + "{0}{1}" + keyValuePairs[Constant.USERDATA] +
                    ",35{0}{1}" + "0013," + DateTime.Now.ToString("MMddyy") + "{0}{1}0014," + DateTime.Now.ToString("hhmmss") + "{0}{1}" +
                    keyValuePairs[Constant.TERMINALID] + "," + fields[Constant.TERMINALID] +
                    "{0}{1}"/*0201,0.00{0}{1}"*/ + keyValuePairs[Constant.ONGUARD] + ",ID:{0}{1}" + keyValuePairs[Constant.ONGUARD] + "," + fields[Constant.LOCATIONNAME] + "{0}{1}8006," + fields[Constant.CHAINCODE] + "{0}{1}{2}", CR, LF, EOT);

                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);

                string response = string.Empty;

                do
                {
                    device.ReceiveMessage(out response);
                    //elavonResponse.ElavonVoidResponse(response);
                    elavonResponse.ElavonSaleResponse(response);
                }
                while (elavonResponse.IsListenAgain);

            }
            catch (Exception ex)
            {
                logger.Error("ERROR IN INQUIRY METHOD" + ex);
            }
            return elavonResponse;
        }

        private void SendRequest(string request)
        {
            logger.Trace("Entered in SENDREQUEST method");
            try
            {
                if (IsConnected)
                {
                    IsConnected = device.SendMessage(Encoding.Default.GetBytes(request));
                    if (!IsConnected)
                    {
                        logger.Trace("Trying to connect again");
                        IsConnected = device.Connect();
                        IsConnected = device.SendMessage(Encoding.Default.GetBytes(request));
                        System.Threading.Thread.Sleep(500);
                    }
                    //System.Threading.Thread.Sleep(500);
                }
                #region PRIMEPOS-3260
                else
                {
                    logger.Trace("Trying to connect again");
                    IsConnected = device.Connect();
                    IsConnected = device.SendMessage(Encoding.Default.GetBytes(request));
                    //System.Threading.Thread.Sleep(500);
                }
                System.Threading.Thread.Sleep(500);
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error("Error in SendRequest method" + ex);
            }
        }

        public ElavonResponse PatientConsent(Hashtable hashtable)
        {
            elavonResponse = new ElavonResponse();

            logger.Trace("ENTERED IN PATIENTCONSENT METHOD");

            try
            {
                IdleScreen();
                CancelMessage();
                string title = (string)hashtable["TITLE"];
                string text = (string)hashtable["TEXT"];
                string patientName = (string)hashtable["PATIENTNAME"];
                string patientAddress = (string)hashtable["PATIENTADDRESS"];
                string firstradiobtn = (string.IsNullOrWhiteSpace(Convert.ToString(hashtable["FIRSTRDBTN"]))) ? "" : Convert.ToString(hashtable["FIRSTRDBTN"]);
                string secondradiobtn = (string.IsNullOrWhiteSpace(Convert.ToString(hashtable["SECONDRDBTN"]))) ? "" : Convert.ToString(hashtable["SECONDRDBTN"]);
                string thirdradiobtn = (string.IsNullOrWhiteSpace(Convert.ToString(hashtable["THIRDRDBTN"]))) ? "" : Convert.ToString(hashtable["THIRDRDBTN"]);
                string secondbtn = (string.IsNullOrWhiteSpace(Convert.ToString(hashtable["SECONDBTN"]))) ? "" : Convert.ToString(hashtable["SECONDBTN"]);
                string thirdbtn = (string.IsNullOrWhiteSpace(Convert.ToString(hashtable["THIRDBTN"]))) ? "" : Convert.ToString(hashtable["THIRDBTN"]);
                string request = string.Empty;

                request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] + ",14123070300{0}{1}5001,0701211{3}" //PRIMEPOS-3068
                    + secondbtn + "{3}" + thirdbtn + "{3}2{3}" + "Patient Name : " + patientName + "\u001c"
                    + "Address : " + patientAddress + "{0}{1}{2}", CR, LF, EOT, FS);

                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);

                Thread.Sleep(3000);
                string response = string.Empty;
                device.ReceiveMessage(out response);

                if (response.Contains("INVALID") || response == string.Empty)
                {
                    device.ReceiveMessage(out response);
                }

                elavonResponse.FirstButtonResponse = elavonResponse.PatientConsent(response);

                CancelMessage();

                request = string.Format("0001,36{0}{1}" + keyValuePairs[Constant.USERDATA] +
                    ",14123050300{0}{1}5001,0400740{3}" + "Patient Consent" + "{3}" + text +//PRIMEPOS-3068
                    "{3}{3}" + firstradiobtn + "{3}" + secondradiobtn + "{3}" + thirdradiobtn + "{3}"
                    + "Other" + "{0}{1}{2}", CR, LF, EOT, FS);

                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);

                Thread.Sleep(3000);
                response = string.Empty;
                device.ReceiveMessage(out response);

                if (response.Contains("INVALID") || response == string.Empty)
                {
                    device.ReceiveMessage(out response);
                }

                elavonResponse.SecondButtonResponse = elavonResponse.PatientConsent(response);

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return elavonResponse;
        }

        public ElavonResponse OnlyTokenCapture(Dictionary<string, string> fields)
        {
            elavonResponse = new ElavonResponse();

            logger.Trace("ENTERED IN ONLYTOKENCAPTURE METHOD");

            try
            {
                string referenceNumber = GetNextRefNumber();

                string request = string.Format("0001,37{0}{1}0002,0.0{0}{1}0007," + referenceNumber +
                    "{0}{1}0109," + fields[Constant.TERMINALID] + "{0}{1}0115,010{0}{1}" + keyValuePairs[Constant.ONGUARD] + ",ID:{0}{1}" + keyValuePairs[Constant.ONGUARD] + "," + fields[Constant.LOCATIONNAME] + "{0}{1}8006," + fields[Constant.CHAINCODE] + "{0}{1}{2}", CR, LF, EOT, FS);

                logger.Trace("REQEUST IS :" + request);

                SendRequest(request);

                Thread.Sleep(3000);
                string response = string.Empty;

                do
                {
                    device.ReceiveMessage(out response);
                    elavonResponse.ElavonSaleResponse(response);
                }
                while (elavonResponse.IsListenAgain);

                SaveRefNumber(referenceNumber);
            }
            catch (Exception ex)
            {
                logger.Error(" ERROR IN ONLYTOKENCAPTURE()" + ex);
            }

            return elavonResponse;
        }

        private void CancelMessage()
        {
            string referenceNumber = GetNextRefNumber();
            string request = string.Format("0001,80{0}{1}0007," + referenceNumber + "{0}{1}" + "0013,062921{0}{1}0014,111743{0}{1}{2}", CR, LF, EOT);

            SendRequest(request);

            SaveRefNumber(referenceNumber);

            string response = string.Empty;
            device.ReceiveMessage(out response);
        }
    }
}
