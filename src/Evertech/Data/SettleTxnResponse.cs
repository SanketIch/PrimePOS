using Microsoft.VisualBasic;
using MMS.PROCESSOR;
using NLog;
using System;
using System.Linq;

namespace Evertech.Data
{
    public class SettleTxnResponse : PaymentResponse
    {
        string[] Response;
        string[] RespCodeATH1;
        string[] RespCodeATH2;
        string[] RespCodeATH3;
        string[] RespCodeATH4;
        public String Message = string.Empty;
        ILogger logger = LogManager.GetCurrentClassLogger();

        public override int ParseResponse(string xmlResponse, string FilterNode)
        {
            throw new NotImplementedException();
        }

        public string SetResponse(string response)
        {

            try
            {
                if (response.Contains(Strings.Chr(28)))
                {
                    Response = Strings.Split(response, Strings.Chr(28).ToString());
                }
                if (Response[1].Contains("/"))
                {
                    RespCodeATH1 = Strings.Split(Response[1], "/");
                }
                if (Response.Count() > 4)
                {
                    if (Response[3].Contains("/"))
                    {
                        RespCodeATH2 = Strings.Split(Response[3], "/");
                    }
                }
                if (Response.Count() > 6)
                {
                    if (Response[5].Contains("/"))
                    {
                        RespCodeATH3 = Strings.Split(Response[5], "/");
                    }
                }
                if (Response.Count() > 8)
                {
                    if (Response[7].Contains("/"))
                    {
                        RespCodeATH4 = Strings.Split(Response[7], "/");
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            try
            {
                // if all response codes are ZY and native codes are NT then we have an empty batch
                // any time we get all response codes as ZY we don't print receipt
                if (RespCodeATH1 != null && RespCodeATH2 != null && RespCodeATH3 != null && RespCodeATH4 != null)
                {
                    if ((RespCodeATH1[2] == "ZY" & RespCodeATH2[2] == "ZY" & RespCodeATH3[2] == "ZY" & RespCodeATH4[2] == "ZY"))
                    {
                        //Dont know for printing the Receipt of Settled Data
                        //bPrintSettleRcpt = false;

                        if ((RespCodeATH1[0] == "NT" & RespCodeATH2[0] == "NT" & RespCodeATH3[0] == "NT" & RespCodeATH4[0] == "NT"))
                        {
                            logger.Trace("SETTLE");
                            logger.Trace(Constants.vbCrLf + "Empty batch");
                            Message = "EMPTY BATCH IN EVERTEC PROCESSOR";
                            return Message;
                        }
                        else
                        {
                            logger.Trace("SETTLE");
                            logger.Trace(Constants.vbCrLf + "ATH: " + RespCodeATH1[1]);
                            logger.Trace(Constants.vbCrLf + "ATH2: " + RespCodeATH2[1]);
                            logger.Trace(Constants.vbCrLf + "ATH3: " + RespCodeATH3[1]);
                            logger.Trace(Constants.vbCrLf + "ATH4: " + RespCodeATH4[1]);
                            Message = "PARTIALLY FAILED ";
                            return "SETTLEMENT";
                        }
                    }
                    else if ((RespCodeATH1[2] == "ZW" | RespCodeATH2[2] == "ZW" | RespCodeATH3[2] == "ZW" | RespCodeATH4[2] == "ZW"))
                    {
                        logger.Trace("SETTLE");
                        logger.Trace(Constants.vbCrLf + "ATH: " + RespCodeATH1[1]);
                        logger.Trace(Constants.vbCrLf + "ATH2: " + RespCodeATH2[1]);
                        logger.Trace(Constants.vbCrLf + "ATH3: " + RespCodeATH3[1]);
                        logger.Trace(Constants.vbCrLf + "ATH4: " + RespCodeATH4[1]);
                        Message = "PARTIALLY FAILED";
                        return Message;
                    }
                    else if ((RespCodeATH1[2] == "00" | RespCodeATH2[2] == "00" | RespCodeATH3[2] == "00" | RespCodeATH4[2] == "00"))
                    {
                        //bPrintSettleRcpt = true;
                        logger.Trace("SETTLE");
                        logger.Trace(Constants.vbCrLf + "ATH: " + RespCodeATH1[1]);
                        logger.Trace(Constants.vbCrLf + "ATH2: " + RespCodeATH2[1]);
                        logger.Trace(Constants.vbCrLf + "ATH3: " + RespCodeATH3[1]);
                        logger.Trace(Constants.vbCrLf + "ATH4: " + RespCodeATH4[1]);
                        Message = "Successfully Settled Transaction in Evertec Processor";
                        return Message;
                    }
                    else
                    {
                        //bPrintSettleRcpt = true;
                        logger.Trace("SETTLE");
                        logger.Trace(Constants.vbCrLf + "ATH: " + RespCodeATH1[1]);
                        logger.Trace(Constants.vbCrLf + "ATH2: " + RespCodeATH2[1]);
                        logger.Trace(Constants.vbCrLf + "ATH3: " + RespCodeATH3[1]);
                        logger.Trace(Constants.vbCrLf + "ATH4: " + RespCodeATH4[1]);
                        Message = "PARTIALLY FAILED";
                        return "SETTELMENT";
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return Message;
        }
    }
}
