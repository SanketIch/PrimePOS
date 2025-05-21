using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gateway
{
    public class EMV:IDisposable
    {
        #region "Private Data"


        private int emvflagField=0;

        private string cryptogramField;

        private string cryptoidField;

        private string issuerdataField;

        private string transcounterField;

        private string interchangeprofileField;

        private string randomnumberField;

        private string termverresultsField;

        private string languagepreferenceField;

        private string terminaltransdateField;

        private string transactionstatusField;

        private string terminalversionnoField;

        private string issueractiondefaultField;

        private string issueractiondenialField;

        private string issueractiononlineField;

        private string additionaltermcapabilityField;

        private string iccdynamicnumberField;

        private string cryptotranstypeField;

        private string terminalcountrycodeField;

        private string panseqnumberField;

        private string terminalcapprofileField;

        private string cvresultsField;

        private string terminalserialnumField;

        private string applicationidField;

        private string terminaltypeField;

        private string dedicatedfilenameField;

        private string transseqcounterField;

        private string transrefcurrencycodeField;

        private string appusagecontrolField;

        private string aidterminalField;

        private string issuerscriptresultField;

        private string secondarypinblockField;

        private string formfactorindField;

        private string customerexcdataField;

        private string transcatcodeField;

        private string customerid;

        private string cashbackamount;

        private List<Tuple<string, byte[],string>> _SecureDataList;

        #endregion


        /// <summary>
        /// LIst That Contains the Encrypted Data to be passed on to the Gateway
        /// </summary>
        public List<Tuple<string, byte[],string>> SecureDataList
        {
            get { return _SecureDataList; }
            set { _SecureDataList = value; }
        }


        public void LoadSecureData(DataEncryptionKey Params)
        {
            foreach (var tmpSecureData in SecureDataList)
            {
                string tag = tmpSecureData.Item1;
                byte[] data = tmpSecureData.Item2;
                string oType = tmpSecureData.Item3;
                string decryptData = string.Empty;
                //decryptData = AppGlobal.Decryption(data, Params).ToUpper();
                if (oType.ToUpper() == "EMV")
                    {
                        this.emvflagField = 1;
                    }
                switch (tag.ToUpper())
                {

                    case "82":
                        this.interchangeprofileField = AppGlobal.Decryption(data, Params);
                        break;
                    case "84":
                        this.dedicatedfilenameField = AppGlobal.Decryption(data, Params);
                        break;
                    case "95":
                        this.termverresultsField = AppGlobal.Decryption(data, Params);
                        break;
                    case "0C0B":
                        this.secondarypinblockField = AppGlobal.Decryption(data, Params);
                        break;
                    case "4F":
                        this.applicationidField = AppGlobal.Decryption(data, Params);
                        break;
                    case "5F2D":
                        this.languagepreferenceField = AppGlobal.Decryption(data, Params);
                        break;
                    case "5F34":
                        this.panseqnumberField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9A":
                        this.terminaltransdateField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9B":
                        this.transactionstatusField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9C":
                        this.cryptotranstypeField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F06":
                        this.aidterminalField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F07":
                        this.appusagecontrolField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F09":
                        this.terminalversionnoField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F0D":
                        this.issueractiondefaultField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F0E":
                        this.issueractiondenialField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F0F":
                        this.issueractiononlineField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F10":
                        this.issuerdataField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F1A":
                        this.terminalcountrycodeField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F1E":
                        this.terminalserialnumField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F26":
                        this.cryptogramField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F27":
                        this.cryptoidField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F33":
                        this.terminalcapprofileField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F34":
                        this.cvresultsField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F35":
                        this.terminaltypeField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F36":
                        this.transcounterField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F37":
                        this.randomnumberField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F3C":
                        this.transrefcurrencycodeField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F40":
                        this.additionaltermcapabilityField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F41":
                        this.transseqcounterField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F4C":
                        this.iccdynamicnumberField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F53":
                        this.transcatcodeField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F5B":
                        this.issuerscriptresultField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F6E":
                        this.formfactorindField = AppGlobal.Decryption(data, Params);
                        break;
                    case "9F7C":
                        this.customerexcdataField = AppGlobal.Decryption(data, Params);                        
                        break;
                    default:
                        break;


                }
            }
        }

        public PrismPay.EMVData LoadEMV(out PrismPay.EMVData oEMVData)
        {
            oEMVData = new PrismPay.EMVData();

            oEMVData.emvflag = emvflagField;
            if(! string.IsNullOrEmpty(this.cryptogramField))
            {
                oEMVData.cryptogram =  this.cryptogramField;
            }
            if (!string.IsNullOrEmpty(this.cryptoidField))
            {
                oEMVData.cryptoid = this.cryptoidField;
            }
            if (!string.IsNullOrEmpty(this.issuerdataField))
            {
                oEMVData.issuerdata = this.issuerdataField;
            }
            if (!string.IsNullOrEmpty(this.transcounterField))
            {
                oEMVData.transcounter = this.transcounterField;
            }
            if (!string.IsNullOrEmpty(this.interchangeprofileField))
            {
                oEMVData.interchangeprofile = this.interchangeprofileField;
            }
            if (!string.IsNullOrEmpty(this.randomnumberField))
            {
                oEMVData.randomnumber = this.randomnumberField;
            }
            if (!string.IsNullOrEmpty(this.termverresultsField))
            {
                oEMVData.termverresults = this.termverresultsField;
            }
            if (!string.IsNullOrEmpty(this.languagepreferenceField))
            {
                oEMVData.languagepreference = this.languagepreferenceField;
            }
            if (!string.IsNullOrEmpty(this.terminaltransdateField))
            {
                oEMVData.terminaltransdate = this.terminaltransdateField;
            }
            if (!string.IsNullOrEmpty(this.transactionstatusField))
            {
                oEMVData.transactionstatus = this.transactionstatusField;
            }
            if (!string.IsNullOrEmpty(this.terminalversionnoField))
            {
                oEMVData.terminalversionno = this.terminalversionnoField;
            }
            if (!string.IsNullOrEmpty(this.issueractiondefaultField))
            {
                oEMVData.issueractiondefault = this.issueractiondefaultField;
            }
            if (!string.IsNullOrEmpty(this.issueractiondenialField))
            {
                oEMVData.issueractiondenial = this.issueractiondenialField;
            }
            if (!string.IsNullOrEmpty(this.issueractiononlineField))
            {
                oEMVData.issueractiononline = this.issueractiononlineField;
            }
            if (!string.IsNullOrEmpty(this.additionaltermcapabilityField))
            {
                oEMVData.additionaltermcapability = this.additionaltermcapabilityField;
            }
            if (!string.IsNullOrEmpty(this.cryptotranstypeField))
            {
                oEMVData.cryptotranstype = this.cryptotranstypeField;
            }
            if (!string.IsNullOrEmpty(this.terminalcountrycodeField))
            {
                oEMVData.terminalcountrycode = this.terminalcountrycodeField;
            }
            if (!string.IsNullOrEmpty(this.panseqnumberField))
            {
                oEMVData.panseqnumber = this.panseqnumberField;
            }
            if (!string.IsNullOrEmpty(this.terminalcapprofileField))
            {
                oEMVData.terminalcapprofile = this.terminalcapprofileField;
            }
            if (!string.IsNullOrEmpty(this.cvresultsField))
            {
                oEMVData.cvresults = this.cvresultsField;
            }
            if (!string.IsNullOrEmpty(this.terminalserialnumField))
            {
                oEMVData.terminalserialnum = this.terminalserialnumField;
            }
            if (!string.IsNullOrEmpty(this.applicationidField))
            {
                oEMVData.applicationid = this.applicationidField;
            }
            if (!string.IsNullOrEmpty(this.terminaltypeField))
            {
                oEMVData.cryptogram = this.terminaltypeField;
            }
            if (!string.IsNullOrEmpty(this.dedicatedfilenameField))
            {
                oEMVData.dedicatedfilename = this.dedicatedfilenameField;
            }
            if (!string.IsNullOrEmpty(this.transseqcounterField))
            {
                oEMVData.transseqcounter = this.transseqcounterField;
            }
            if (!string.IsNullOrEmpty(this.transrefcurrencycodeField))
            {
                oEMVData.transrefcurrencycode = this.transrefcurrencycodeField;
            }
            if (!string.IsNullOrEmpty(this.appusagecontrolField))
            {
                oEMVData.appusagecontrol = this.appusagecontrolField;
            }
            if (!string.IsNullOrEmpty(this.aidterminalField))
            {
                oEMVData.aidterminal = this.aidterminalField;
            }
            if (!string.IsNullOrEmpty(this.issuerscriptresultField))
            {
                oEMVData.issuerscriptresult = this.issuerscriptresultField;
            }
            if (!string.IsNullOrEmpty(this.secondarypinblockField))
            {
                oEMVData.secondarypinblock = this.secondarypinblockField;
            }
            if (!string.IsNullOrEmpty(this.formfactorindField))
            {
                oEMVData.formfactorind = this.formfactorindField;
            }
            if (!string.IsNullOrEmpty(this.customerexcdataField))
            {
                oEMVData.customerexcdata = this.customerexcdataField;
            }
            if (!string.IsNullOrEmpty(this.transcatcodeField))
            {
                oEMVData.transcatcode = this.transcatcodeField;
            }




            return oEMVData;
        }

        public WorldPay.EMVData LoadEMV(out WorldPay.EMVData oEMVData)
        {
            oEMVData = new WorldPay.EMVData();

            oEMVData.emvflag = emvflagField;
            if (!string.IsNullOrEmpty(this.cryptogramField))
            {
                oEMVData.cryptogram = this.cryptogramField;
            }
            if (!string.IsNullOrEmpty(this.cryptoidField))
            {
                oEMVData.cryptoid = this.cryptoidField;
            }
            if (!string.IsNullOrEmpty(this.issuerdataField))
            {
                oEMVData.issuerdata = this.issuerdataField;
            }
            if (!string.IsNullOrEmpty(this.transcounterField))
            {
                oEMVData.transcounter = this.transcounterField;
            }
            if (!string.IsNullOrEmpty(this.interchangeprofileField))
            {
                oEMVData.interchangeprofile = this.interchangeprofileField;
            }
            if (!string.IsNullOrEmpty(this.randomnumberField))
            {
                oEMVData.randomnumber = this.randomnumberField;
            }
            if (!string.IsNullOrEmpty(this.termverresultsField))
            {
                oEMVData.termverresults = this.termverresultsField;
            }
            if (!string.IsNullOrEmpty(this.languagepreferenceField))
            {
                oEMVData.languagepreference = this.languagepreferenceField;
            }
            if (!string.IsNullOrEmpty(this.terminaltransdateField))
            {
                oEMVData.terminaltransdate = this.terminaltransdateField;
            }
            if (!string.IsNullOrEmpty(this.transactionstatusField))
            {
                oEMVData.transactionstatus = this.transactionstatusField;
            }
            if (!string.IsNullOrEmpty(this.terminalversionnoField))
            {
                oEMVData.terminalversionno = this.terminalversionnoField;
            }
            if (!string.IsNullOrEmpty(this.issueractiondefaultField))
            {
                oEMVData.issueractiondefault = this.issueractiondefaultField;
            }
            if (!string.IsNullOrEmpty(this.issueractiondenialField))
            {
                oEMVData.issueractiondenial = this.issueractiondenialField;
            }
            if (!string.IsNullOrEmpty(this.issueractiononlineField))
            {
                oEMVData.issueractiononline = this.issueractiononlineField;
            }
            if (!string.IsNullOrEmpty(this.additionaltermcapabilityField))
            {
                oEMVData.additionaltermcapability = this.additionaltermcapabilityField;
            }
            if (!string.IsNullOrEmpty(this.cryptotranstypeField))
            {
                oEMVData.cryptotranstype = this.cryptotranstypeField;
            }
            if (!string.IsNullOrEmpty(this.terminalcountrycodeField))
            {
                oEMVData.terminalcountrycode = this.terminalcountrycodeField;
            }
            if (!string.IsNullOrEmpty(this.panseqnumberField))
            {
                oEMVData.panseqnumber = this.panseqnumberField;
            }
            if (!string.IsNullOrEmpty(this.terminalcapprofileField))
            {
                oEMVData.terminalcapprofile = this.terminalcapprofileField;
            }
            if (!string.IsNullOrEmpty(this.cvresultsField))
            {
                oEMVData.cvresults = this.cvresultsField;
            }
            if (!string.IsNullOrEmpty(this.terminalserialnumField))
            {
                oEMVData.terminalserialnum = this.terminalserialnumField;
            }
            if (!string.IsNullOrEmpty(this.applicationidField))
            {
                oEMVData.applicationid = this.applicationidField;
            }
            if (!string.IsNullOrEmpty(this.terminaltypeField))
            {
                oEMVData.terminaltype = this.terminaltypeField;
            }
            if (!string.IsNullOrEmpty(this.dedicatedfilenameField))
            {
                oEMVData.dedicatedfilename = this.dedicatedfilenameField;
            }
            if (!string.IsNullOrEmpty(this.transseqcounterField))
            {
                oEMVData.transseqcounter = this.transseqcounterField;
            }
            if (!string.IsNullOrEmpty(this.transrefcurrencycodeField))
            {
                oEMVData.transrefcurrencycode = this.transrefcurrencycodeField;
            }
            if (!string.IsNullOrEmpty(this.appusagecontrolField))
            {
                oEMVData.appusagecontrol = this.appusagecontrolField;
            }
            if (!string.IsNullOrEmpty(this.aidterminalField))
            {
                oEMVData.aidterminal = this.aidterminalField;
            }
            if (!string.IsNullOrEmpty(this.issuerscriptresultField))
            {
                oEMVData.issuerscriptresult = this.issuerscriptresultField;
            }
            if (!string.IsNullOrEmpty(this.secondarypinblockField))
            {
                oEMVData.secondarypinblock = this.secondarypinblockField;
            }
            if (!string.IsNullOrEmpty(this.formfactorindField))
            {
                oEMVData.formfactorind = this.formfactorindField;
            }
            if (!string.IsNullOrEmpty(this.customerexcdataField))
            {
                oEMVData.customerexcdata = this.customerexcdataField;
            }
            if (!string.IsNullOrEmpty(this.transcatcodeField))
            {
                oEMVData.transcatcode = this.transcatcodeField;
            }




            return oEMVData;
        }



        #region "iDisposible Members"
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.emvflagField = 0;
                this.cryptogramField = "";
                this.cryptoidField = "";
                this.issuerdataField = "";
                this.transcounterField = "";
                this.interchangeprofileField = "";
                this.randomnumberField = "";
                this.termverresultsField = "";
                this.languagepreferenceField = "";
                this.terminaltransdateField = "";
                this.transactionstatusField = "";
                this.terminalversionnoField = "";
                this.issueractiondefaultField = "";
                this.issueractiondenialField = "";
                this.issueractiononlineField = "";
                this.additionaltermcapabilityField = "";
                this.iccdynamicnumberField = "";
                this.cryptotranstypeField = "";
                this.terminalcountrycodeField = "";
                this.panseqnumberField = "";
                this.terminalcapprofileField = "";
                this.cvresultsField = "";
                this.terminalserialnumField = "";
                this.applicationidField = "";
                this.terminaltypeField = "";
                this.dedicatedfilenameField = "";
                this.transseqcounterField = "";
                this.transrefcurrencycodeField = "";
                this.appusagecontrolField = "";
                this.aidterminalField = "";
                this.issuerscriptresultField = "";
                this.secondarypinblockField = "";
                this.formfactorindField = "";
                this.customerexcdataField = "";
                this.transcatcodeField = "";

            }
            //GC.Collect();

        }
        #endregion

    }
}
