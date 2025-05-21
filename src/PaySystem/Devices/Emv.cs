using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace EDevice
{
    internal class Emv
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();


        #region Emv Properties
        /// <summary>
        /// Emv Properites Data
        /// </summary>
        public struct Emv_EncryptedData
        {
            public string TrkData { get; set; }
            public string Name { get; set; }
            public string Expire { get; set; }//no need
            public string eTrk2Data { get; set; } // no need
            public string eData { get; set; }
        }
        /// <summary>
        /// Emv Required Data
        /// </summary>
        public Emv_EncryptedData EmvRequestData;
        #endregion Emv Properties

        #region Build Emv Data
        /// <summary>
        /// Build the EMV Data for the equivlent track
        /// </summary>
        /// <param name="emvData"></param>
        /// <returns></returns>
        public string BuildEmvData(Emv_EncryptedData emvData)
        {
            logger.Trace("In BuildEmvData()");
            string data = string.Empty;
            if (!string.IsNullOrEmpty(emvData.eTrk2Data) && !string.IsNullOrEmpty(emvData.Expire)
                && !string.IsNullOrEmpty(emvData.eData))
            {
                string trk = "%B" + emvData.TrkData.Replace('d', 'D').Split('D')[0].ToString() + "^" + emvData.Name.HexToAscii() + "^" + emvData.TrkData.Replace('d', 'D').Split('D')[1].ToString() + "?;";
                string trk2 = emvData.TrkData.Replace('D', '=').Replace('d', '=') + "?:";
                string edata = emvData.eData.HexToAscii();
                data = trk + trk2 + edata;
            }
            return data;
        }
        #endregion Build Emv Data
    }
}
