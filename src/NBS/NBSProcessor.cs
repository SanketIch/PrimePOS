
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using NBS.RequestModels;
using System.Security.Cryptography;
using NBS.ResponseModels;
using NLog;

namespace NBS
{
    public class NBSProcessor
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        public string url = string.Empty;
        public string key = string.Empty;

        public NBSProcessor(string URL, string KEY)
        {
            url = URL;
            this.key = KEY;
        }

        #region PRIMEPOS-NBS-GET-TOKEN
        public TokenResponseData GetToken(string npiNo)
        {
            try
            {
                TokenRequest tokenRequest = new TokenRequest()
                {
                    NpiNo = npiNo,
                    AppId = NBSHelper.ApplicationID
                };
                string postBody = JsonConvert.SerializeObject(tokenRequest);
                logger.Trace("The NBS Token Request is " + postBody);
                string result = NBSToken(postBody, url);
                if (!string.IsNullOrEmpty(result))
                {
                    logger.Trace($"NBSProcessor==>GetToken(): The NBS Token Response is :- {result}");
                    return JsonConvert.DeserializeObject<TokenResponseData>(result);
                }
                else
                {
                    logger.Warn("NBSProcessor==>GetToken(): Failed to get NBS Token.");
                    return null;
                }

            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>GetToken(): An Exception Occured" + ex);
                return null;
            }
        }
        public string NBSToken(string postBody, string url)
        {
            try
            {
                string GetUrl = $"{url}{NBSHelper.GetToken}";
                string response = NBSHelper.SendRequestGetToken(postBody, GetUrl);

                if (!string.IsNullOrEmpty(response))
                {
                    TokenApiResponse tokenApiResponse = JsonConvert.DeserializeObject<TokenApiResponse>(response);

                    if (tokenApiResponse?.Data != null)
                    {
                        return tokenApiResponse.Data;
                    }
                }
                else
                {
                    logger.Warn("NBSProcessor==>NBSToken(): Failed to get NBS Token.");
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>NBSToken(): An Exception Occured" + ex);
                return string.Empty;
            }
            return string.Empty;
        }
        #endregion

        #region PRIMEPOS-NBS-GET-BIN-RANGE
        public List<BinRangeData> GetBinRange()
        {
            try
            {
                string result = BinRangeData();
                if (!string.IsNullOrEmpty(result))
                {
                    logger.Trace($"NBSProcessor==>GetBinRange(): The NBS Get Bin Response is :- {result}");
                    return JsonConvert.DeserializeObject<List<BinRangeData>>(result);
                }
                else
                {
                    logger.Warn("NBSProcessor==>GetBinRange(): Failed to get NBS Bin Range.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>GetBinRange(): An Exception Occured " + ex);
                return null;
            }
        }
        public string BinRangeData()
        {
            try
            {
                string GetUrl = $"{url}{NBSHelper.GetNbsBinData}";
                System.Net.HttpStatusCode httpCode = System.Net.HttpStatusCode.ServiceUnavailable;
                string response = NBSHelper.SendRequestGet(GetUrl, out httpCode, key);

                if (!string.IsNullOrEmpty(response))
                {
                    BinRangeResponse binRange = JsonConvert.DeserializeObject<BinRangeResponse>(response);

                    if (binRange?.Data != null)
                    {
                        return binRange.Data;
                    }
                }
                else
                {
                    logger.Warn($"NBSProcessor==>BinRangeData(): Please check the NBS configuration. The API response we are receiving is as follows: {httpCode}");
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>BinRangeData(): An Exception Occured" + ex);
                return string.Empty;
            }
            return string.Empty;
        }
        #endregion

        #region PRIMEPOS-NBS-ANALYZE-REQUEST
        public AnalyseData AnalyzeRequest(List<AnalyzeLineItem> lineItemsdata, AnalyzeMerchant merchant, string uid, string ticketNum, bool isReturn)
        {
            try
            {

                if (lineItemsdata == null || lineItemsdata.Count == 0)
                {
                    return null;
                }

                AnalyzeMember member = new AnalyzeMember
                {
                    UidType = NBSHelper.trackOneId,
                    Uid = uid
                };

                List<AnalyzeLineItem> lineItems = new List<AnalyzeLineItem>();
                lineItems.AddRange(lineItemsdata);

                AnalyzeTransaction transaction = new AnalyzeTransaction
                {
                    MerchantTransactionId = ticketNum,
                    TransactionLocalDateTime = DateTime.UtcNow,
                    TransactionCurrencyCode = NBSHelper.CurrencyCode,
                    LineItems = lineItems
                };

                AnalyseRequest nbsAnalyseRequest = new AnalyseRequest
                {
                    Member = member,
                    Merchant = merchant,
                    Transaction = transaction
                };

                string postBody = JsonConvert.SerializeObject(nbsAnalyseRequest);
                logger.Trace("The NBS Analyze Request is " + postBody);
                string result = NBSAnalyze(postBody, url,key);
                if (!string.IsNullOrEmpty(result))
                {
                    logger.Trace($"NBSProcessor==>AnalyzeRequest(): The NBS Analyze Response is :- {result} ");
                    return JsonConvert.DeserializeObject<AnalyseData>(result);
                }
                else
                {
                    logger.Warn("NBSProcessor==>AnalyzeRequest(): Failed to get NBS Analyze data.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>AnalyzeRequest(): An Exception Occured" + ex);
                return null;
            }
        }

        public string NBSAnalyze(string postBody, string url, string key)
        {
            try
            {
                string GetUrl = $"{url}{NBSHelper.NBSAnalyze}";
                string response = NBSHelper.SendRequestPost(postBody, GetUrl, key);

                if (!string.IsNullOrEmpty(response))
                {
                    AnalyseApiResponse analyseApiResponse = JsonConvert.DeserializeObject<AnalyseApiResponse>(response);

                    if (analyseApiResponse?.Data != null)
                    {
                        return analyseApiResponse.Data;
                    }
                }
                else
                {
                    logger.Warn("NBSProcessor==>NBSAnalyze(): Failed to get Analyze data.");
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>NBSAnalyze(): An Exception Occured" + ex);
                return string.Empty;
            }
            return string.Empty;
        }
        #endregion

        #region PRIMEPOS-NBS-REDEEM-REQUEST
        public RedeemData RedeemTransaction(string nbstransId, string amount)
        {
            try
            {
                RedeemRequest nBSRedeemRequest = new RedeemRequest()
                {
                    NationsBenefitsTransactionId = nbstransId,
                    RedeemedAmount = amount,
                    MerchantDiscretionaryData = NBSHelper.NBSRedeem
                };
                string postBody = JsonConvert.SerializeObject(nBSRedeemRequest);
                logger.Trace("The NBS Redeem Transaction Request is " + postBody);
                string result = NBSRedeem(postBody, url, key);
                if (!string.IsNullOrEmpty(result))
                {
                    logger.Trace($"NBSProcessor==>RedeemTransaction(): The NBS Redeem Response is :- {result}");
                    return JsonConvert.DeserializeObject<RedeemData>(result);
                }
                else
                {
                    logger.Warn("NBSProcessor==>RedeemTransaction(): Failed to get NBS Redeem data.");
                    return null;
                }

            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>RedeemTransaction(): An Exception Occured" + ex);
                return null;
            }
        }

        public string NBSRedeem(string postBody, string url, string key)
        {
            try
            {
                string GetUrl = $"{url}{NBSHelper.NBSRedeem}";
                string response = NBSHelper.SendRequestPost(postBody, GetUrl, key);

                if (!string.IsNullOrEmpty(response))
                {
                    RedeemApiResponse redeemApiResponse = JsonConvert.DeserializeObject<RedeemApiResponse>(response);

                    if (redeemApiResponse?.Data != null)
                    {
                        return redeemApiResponse.Data;
                    }
                }
                else
                {
                    logger.Warn("NBSProcessor==>NBSRedeem(): Failed to get NBS Redeem data.");
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>NBSRedeem(): An Exception Occured" + ex);
                return string.Empty;
            }
            return string.Empty;
        }
        #endregion

        #region PRIMEPOS-NBS-REVERSAL-REQUEST
        public ReversalData ReversalTransaction(string nbstransId)
        {
            try
            {
                ReversalRequest nBSReversalRequest = new ReversalRequest()
                {
                    NationsBenefitsTransactionId = nbstransId,
                    MerchantDiscretionaryData = NBSHelper.NBSReversal
                };
                string postBody = JsonConvert.SerializeObject(nBSReversalRequest);
                logger.Trace("The NBS Reversal Transaction Request is " + postBody);
                string result = NBSReversal(postBody, url, key);
                if (!string.IsNullOrEmpty(result))
                {
                    logger.Trace($"NBSProcessor==>ReversalTransaction(): The NBS Reversal Response is :- {result}");
                    return JsonConvert.DeserializeObject<ReversalData>(result);
                }
                else
                {
                    logger.Warn("NBSProcessor==>ReversalTransaction(): Failed to get NBS Reversal data.");
                    return null;
                }

            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>ReversalTransaction(): An Exception Occured" + ex);
                return null;
            }
        }

        public string NBSReversal(string postBody, string url, string key)
        {
            try
            {
                string GetUrl = $"{url}{NBSHelper.NBSReversal}";
                string response = NBSHelper.SendRequestPost(postBody, GetUrl, key);

                if (!string.IsNullOrEmpty(response))
                {
                    ReversalApiResponse reversalApiResponse = JsonConvert.DeserializeObject<ReversalApiResponse>(response);

                    if (reversalApiResponse.Data != null)
                    {
                        return reversalApiResponse.Data;
                    }
                }
                else
                {
                    logger.Warn("NBSProcessor==>NBSReversal(): Please check the NBS configuration.");
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>NBSReversal(): An Exception Occured" + ex);
                return string.Empty;
            }
            return string.Empty;
        }
        #endregion

        #region OTHER METHODS
        public string ComputeSHA256Hash(string input)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                    byte[] hashBytes = sha256.ComputeHash(inputBytes);

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        builder.Append(hashBytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>ComputeSHA256Hash(): An Exception Occured" + ex);
                return string.Empty;
            }
        }
        #endregion
        #region PRIMEPOS-3373
        public VoidData VoidRequest(string nbstransId)
        {
            try
            {
                VoidRequest nBSVoidRequest = new VoidRequest()
                {
                    NationsBenefitsTransactionId = nbstransId,
                    MerchantDiscretionaryData = NBSHelper.NBSVoid
                };
                string postBody = JsonConvert.SerializeObject(nBSVoidRequest);
                logger.Trace("NBSProcessor==>VoidRequest(): The NBS Void Request is " + postBody);
                String result = NBSVoid(postBody, url, key);
                if (!string.IsNullOrEmpty(result))
                {
                    logger.Trace("NBSProcessor==>VoidRequest(): The NBS Void Response is " + Convert.ToString(result));
                    return JsonConvert.DeserializeObject<VoidData>(result);
                }
                else
                {
                    logger.Warn("NBSProcessor==>VoidRequest(): Failed to get NBS Void data.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>VoidRequest(): An Exception Occured" + ex);
                return null;
            }
        }

        public string NBSVoid(string json, string url, string key)
        {
            try
            {
                string GetUrl = $"{url}{NBSHelper.NBSVoid}";
                string response = NBSHelper.SendRequestPost(json, GetUrl, key);
                if (!string.IsNullOrEmpty(response))
                {
                    VoidApiResponse voidApiResponse = JsonConvert.DeserializeObject<VoidApiResponse>(response);

                    if (voidApiResponse?.Data != null)
                    {
                        return voidApiResponse.Data;
                    }
                }
                else
                {
                    logger.Warn("NBSProcessor==>NBSVoid(): Failed to get NBS Void data.");
                }
            }
            catch (Exception ex)
            {
                logger.Error("NBSProcessor==>NBSVoid(): An Exception Occured" + ex);
                return string.Empty;
            }
            return string.Empty;
        }
        #endregion
    }
}
