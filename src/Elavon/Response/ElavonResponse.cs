using MMS.PROCESSOR;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elavon.Response
{
    public class ElavonResponse : PaymentResponse
    {
        public bool IsListenAgain = true;
        public int FirstButtonResponse;
        public int SecondButtonResponse;
        public string deviceRequest;
        public string deviceResponse;
        ILogger logger = LogManager.GetCurrentClassLogger();

        public string SignatureData = string.Empty;
        public int buttonResponse;
        public bool IsSignatureRequired = false;
        public ElavonResponse()
        {

        }
        public override int ParseResponse(string xmlResponse, string FilterNode)
        {
            throw new NotImplementedException();
        }
        public void ElavonSaleResponse(string response)
        {
            try
            {
                logger.Trace("RESPONSE IS " + response);
                deviceResponse = response;
                if (!string.IsNullOrWhiteSpace(response))
                {
                    Dictionary<string, string> keyvaluePair = ParseKeyValuePair(response);

                    if (keyvaluePair.Keys.Contains(Constant.ResponseCode))
                    {
                        IsListenAgain = false;
                        if (keyvaluePair[Constant.ResponseCode].ToUpper() == Constant.Complete.ToUpper())
                        {
                            base.ResultDescription = base.Result = keyvaluePair[Constant.ResponseCode].ToUpper();
                        }
                        else
                        {
                            base.ResultDescription = base.Result = keyvaluePair[Constant.ResponseCode].ToUpper();
                        }
                        base.EmvReceipt = new EmvReceiptTags();
                        if (keyvaluePair.ContainsKey("1326") && keyvaluePair.ContainsKey("1300") && keyvaluePair.ContainsKey("1305"))
                            base.EmvReceipt.ApplicationLabel = keyvaluePair["1326"] + "|" +keyvaluePair["1300"] + "|" + keyvaluePair["1305"];
                        if (keyvaluePair.ContainsKey("1307"))
                            base.EmvReceipt.TerminalVerficationResult = keyvaluePair["1307"];
                        if (keyvaluePair.ContainsKey("1314"))
                            base.EmvReceipt.AppIndentifer = keyvaluePair["1314"];
                        if (keyvaluePair.ContainsKey("1380"))
                            base.EmvReceipt.EntryLegend = keyvaluePair["1380"];
                        if (keyvaluePair.ContainsKey("0006"))
                            base.EmvReceipt.ApprovalCode = keyvaluePair["0006"];

                        if (keyvaluePair.ContainsKey(Constant.AmountApproved))
                            base.AmountApproved = keyvaluePair[Constant.AmountApproved];
                        if (keyvaluePair.ContainsKey(Constant.CardLogo))
                            base.EmvReceipt.CardLogo = base.CardType = keyvaluePair[Constant.CardLogo];
                        if (keyvaluePair.ContainsKey(Constant.Expiration))
                            base.Expiration = keyvaluePair[Constant.Expiration];
                        if (keyvaluePair.ContainsKey(Constant.AccountNo))
                            base.AccountNo = base.MaskedCardNo = keyvaluePair[Constant.AccountNo];
                        if (keyvaluePair.ContainsKey(Constant.MerchantID))
                            base.EmvReceipt.MerchantID = keyvaluePair[Constant.MerchantID];
                        if (keyvaluePair.ContainsKey(Constant.TransactionID))
                            base.EmvReceipt.TransactionID = base.EmvReceipt.TransID = base.TransactionNo = keyvaluePair[Constant.TransactionID];
                        if (keyvaluePair.ContainsKey(Constant.AccountMaskedNumber) && keyvaluePair[Constant.AccountMaskedNumber].Contains(":"))
                        {
                            //base.EmvReceipt.ApplicationLabel = keyvaluePair[Constant.ApplicationLabel].Split(':')[1];
                            base.ProfiledID = keyvaluePair[Constant.AccountMaskedNumber].Split(':')[1];
                            if (keyvaluePair.ContainsKey("0738"))
                                base.ProfiledID += "|" + keyvaluePair["0738"];
                            base.EmvReceipt.TransactionID = base.EmvReceipt.TransID = base.TransactionNo += "|" + keyvaluePair[Constant.AccountMaskedNumber].Split(':')[1];
                        }                        
                        if (keyvaluePair.ContainsKey(Constant.DialogForm) && keyvaluePair[Constant.DialogForm] == Constant.SignatureRequired)
                        {
                            IsSignatureRequired = true;
                        }
                        if (keyvaluePair.ContainsKey("1002"))
                        {
                            base.EmvReceipt.CcName = keyvaluePair["1002"];
                        }
                    }
                    else
                    {
                        IsListenAgain = true;
                    }
                }
                else
                {
                    IsListenAgain = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        public void ElavonVoidResponse(string response)
        {
            try
            {
                logger.Trace("RESPONSE IS " + response);
                deviceResponse = response;
                if (!string.IsNullOrWhiteSpace(response))
                {
                    Dictionary<string, string> keyvaluePair = ParseKeyValuePair(response);
                    if (keyvaluePair.Keys.Contains(Constant.ResponseCode))
                    {
                        IsListenAgain = false;
                        if (keyvaluePair[Constant.ResponseCode].ToUpper() == Constant.Complete.ToUpper())
                        {
                            base.ResultDescription = base.Result = Constant.SUCCESS;
                        }
                        else
                        {
                            base.ResultDescription = base.Result = keyvaluePair[Constant.ResponseCode].ToUpper();
                        }
                        base.EmvReceipt = new EmvReceiptTags();
                        if (keyvaluePair.ContainsKey("1300"))
                            base.EmvReceipt.ApplicationLabel = keyvaluePair["1300"];
                        if (keyvaluePair.ContainsKey("1314"))
                            base.EmvReceipt.TerminalVerficationResult = keyvaluePair["1314"];
                        if (keyvaluePair.ContainsKey("1305"))
                            base.EmvReceipt.AppIndentifer = keyvaluePair["1305"];
                        if (keyvaluePair.ContainsKey("1380"))
                            base.EmvReceipt.EntryLegend = keyvaluePair["1380"];
                        if (keyvaluePair.ContainsKey("0006"))
                            base.EmvReceipt.ApprovalCode = keyvaluePair["0006"];

                        base.AmountApproved = keyvaluePair[Constant.AmountApproved];
                        base.EmvReceipt.CardLogo = base.CardType = keyvaluePair[Constant.CardLogo];
                        if (keyvaluePair.ContainsKey(Constant.Expiration))
                            base.Expiration = keyvaluePair[Constant.Expiration];
                        if (keyvaluePair.ContainsKey(Constant.AccountNo))
                            base.AccountNo = base.MaskedCardNo = keyvaluePair[Constant.AccountNo];
                        if (keyvaluePair.ContainsKey(Constant.MerchantID))
                            base.EmvReceipt.MerchantID = keyvaluePair[Constant.MerchantID];
                        if (keyvaluePair.ContainsKey(Constant.TransactionID))
                            base.EmvReceipt.TransactionID = base.EmvReceipt.TransID = base.TransactionNo = keyvaluePair[Constant.TransactionID];
                        if (keyvaluePair.ContainsKey(Constant.AccountMaskedNumber) && keyvaluePair[Constant.AccountMaskedNumber].Contains(":"))
                        {
                            base.ProfiledID = keyvaluePair[Constant.AccountMaskedNumber].Split(':')[1];
                            base.EmvReceipt.TransactionID = base.EmvReceipt.TransID = base.TransactionNo += "|" + keyvaluePair[Constant.AccountMaskedNumber].Split(':')[1];
                        }
                    }
                    else
                    {
                        IsListenAgain = true;
                    }
                }
                else
                {
                    IsListenAgain = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void ShowDialogForm(string response)
        {
            logger.Trace("RESPONSE IS " + response);
            deviceResponse = response;
            if (!string.IsNullOrWhiteSpace(response))
            {
                Dictionary<string, string> keyvaluePair = ParseKeyValuePair(response);
                if (keyvaluePair.ContainsKey(Constant.DialogForm))
                {
                    char FS = (char)0x1C;
                    string[] strArray = keyvaluePair[Constant.DialogForm].Split(FS);
                    try
                    {
                        buttonResponse = Convert.ToInt32(strArray[1]);
                    }
                    catch (Exception ex)
                    {
                        buttonResponse = 0;
                    }
                }
                else
                    buttonResponse = 0;
            }
        }
        public int PatientConsent(string response)
        {
            logger.Trace("RESPONSE IS " + response);
            deviceResponse = response;
            if (!string.IsNullOrWhiteSpace(response))
            {
                Dictionary<string, string> keyvaluePair = ParseKeyValuePair(response);
                if (keyvaluePair.ContainsKey(Constant.DialogForm))
                {
                    char FS = (char)0x1C;
                    string[] strArray = keyvaluePair[Constant.DialogForm].Split(FS);
                    try
                    {
                        buttonResponse = Convert.ToInt32(strArray[1]);
                    }
                    catch (Exception ex)
                    {
                        buttonResponse = 0;
                    }
                }
                else
                    buttonResponse = 0;

            }
            else
            {
                buttonResponse = 0;
            }
            return buttonResponse;
        }
        public void ParseSignature(string response)
        {
            logger.Trace("RESPONSE IS " + response);
            deviceResponse = response;
            if (!string.IsNullOrWhiteSpace(response))
            {
                Dictionary<string, string> keyvaluePair = ParseKeyValuePairForSign(response);
                if (keyvaluePair.ContainsKey(Constant.Signature))
                {
                    SignatureData = keyvaluePair[Constant.Signature];
                    //SignatureData = @"b7X(_ )_ A_ A_ :_ +_ *_ 3_ :_ :_ ;_ B_ -_ F_!P !D !@ !P!!*!!:!!J!!8!!H! O! E! 5! 3! *! *! !  X!   _V __!_% _\ _\ _\!_' _[_[_Z _S _R _S _S _J _D _C ^ W _8 ^ '_^W_^.__;__3__5__,^_]^_^^_W^ @^ X^ Y^ R^ [^ S_ $_ ,_ +_ =_ ;_ =_ D_!8_!@ !)  ?  F  W!! !!8! O! =! <! L! S! [' 1' 9' 8'_?'_6'_.'_-'_-!_\'_%!_T!_U!_M!_T!_D!_=!_- _^ _] _V _M _N _U _F^_Z_ (^_O^_?^ 0^_G^ 0^_W^ P_  _ 0_ 9_ A_ A_ B  & !X  ]! %! N' %' $' #' !! Y!_W!_G!_>!_.!_5 _U _] _D _M _J _0__J__C__3__#^_J^_C^_;^_C^_D^_]__&__.__?_ 8_ 9_!P ![  U  ^! -! =! M! K! B! 9!_7!_7!_' _ ^ _N _U _E__,^ _] ^ _E ^ _.]_E]_F]_ =]_6]_ %]_V]_ =]_$]_ %]_]]_]]_F]_ ^^ _'^ 0^ 8^ B^ J^ R_ +_ #_ #_ 3_ C_ C_ ;_ =_ W !A  V! '!6!O' ''.' 5' 3' ;' 9' 8'_ / '_&'_ % '_$!_\!_L!_[!_[!_R!_K!_L!_<!_4!_& _U _V _G _F _F _G_ (_ !^ 3] K] K] K^ #] J] +] *] 9] (]_']_?]_6]_6]_6]_=]_-]_=]_6]_E]_E]_N^_&]_^]_M]_V^_.^_.^_.^_>^_F^_V__'__?__?__?__7_ @  >  \! G'!8' W' G' F' E' E' ]' L' L! [' <' T' T' ; ' '!R' 2'[' A! Q! Q'(' H'_ ? '_/'_ / '_6!_N!_N!_=!_5 _^ _] _U _U _M _R _Bp";
                }
                else
                {
                    SignatureData = null;
                }
            }
        }

        public static Dictionary<string, string> ParseKeyValuePair(string response)
        {
            string[] Splitedstr = response.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            Dictionary<string, string> keyvaluePair = new Dictionary<string, string>();
            for (int i = 0; i < Splitedstr.Length; i++)
            {
                if (Splitedstr[i].Contains(','))
                {
                    Splitedstr[i] = Splitedstr[i].Replace(@"\u0004", string.Empty);
                    if (!keyvaluePair.Keys.Contains(Splitedstr[i].Split(',')[0]))
                        keyvaluePair.Add(Splitedstr[i].Split(',')[0], Splitedstr[i].Split(',')[1]);
                }
            }
            return keyvaluePair;
        }
        public static Dictionary<string, string> ParseKeyValuePairForSign(string response)
        {
            string[] Splitedstr = response.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            Dictionary<string, string> keyvaluePair = new Dictionary<string, string>();
            for (int i = 0; i < Splitedstr.Length; i++)
            {
                if (Splitedstr[i].Contains(','))
                {
                    Splitedstr[i] = Splitedstr[i].Replace(@"\u0004", string.Empty);
                    if (!keyvaluePair.Keys.Contains(Splitedstr[i].Split(',')[0]))
                        keyvaluePair.Add(Splitedstr[i].Split(',')[0], Splitedstr[i].Split(new[] { ',' }, 2)[1]);
                }
            }
            return keyvaluePair;
        }
        #region PRIMEPOS-3260
        public void ElavonHealthMessageResponse(string response)
        {
            try
            {
                logger.Trace($"Health Message Response:{response} ");
                if (!string.IsNullOrWhiteSpace(response))
                {
                    Dictionary<string, string> keyvaluePair = ParseKeyValuePair(response);

                    if (keyvaluePair.Keys.Contains(Constant.HEARTBEAT))
                    {
                        IsListenAgain = false;
                    }
                    else
                    {
                        IsListenAgain = true;
                    }
                }
                else
                {
                    IsListenAgain = false;
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Following Exception occuced Elavon.ElavonResponse=>ElavonHealthMessageResponse=>{ex}");
            }
        }
        #endregion
    }
}
