using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hps.Exchange.PosGateway.Client;
using Newtonsoft.Json;
//using Logger = AppLogger.AppLogger;
using NLog;
namespace MMS.HPS
{
    // added by Rohit Nair for POS-2374 on Feb 14 2017
    internal static class Helper
    {
        static ILogger logger = LogManager.GetCurrentClassLogger();
        internal static string SerializeResponse(PosResponse input)
        {
            logger.Trace("Searialize Response to JSON String");
            string result = string.Empty;
            try
            {
                JsonSerializerSettings osettings = new JsonSerializerSettings();
                osettings.NullValueHandling = NullValueHandling.Include;
                osettings.Formatting = Formatting.Indented;
                /*osettings.FloatFormatHandling = FloatFormatHandling.DefaultValue;
                osettings.FloatParseHandling = FloatParseHandling.Decimal;
                osettings.TypeNameHandling = TypeNameHandling.All;
                */
                osettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
                result = JsonConvert.SerializeObject(input, osettings);
            }
            catch (Exception ex)
            {
                logger.Error(ex,ex.Message);
            }



            return result;
        }

        internal static string SerializeRequest(PosRequest input)
        {
            logger.Trace("Searialize Request to JSON String");
            string result = string.Empty;
            try
            {
                JsonSerializerSettings osettings = new JsonSerializerSettings();
                osettings.NullValueHandling = NullValueHandling.Include;
                osettings.Formatting = Formatting.Indented;
                /*osettings.FloatFormatHandling = FloatFormatHandling.DefaultValue;
                osettings.FloatParseHandling = FloatParseHandling.Decimal;
                osettings.TypeNameHandling = TypeNameHandling.All;
                */
                osettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
                result = JsonConvert.SerializeObject(input, osettings);
            }
            catch (Exception ex)
            {
                logger.Trace(ex,ex.Message);
            }



            return result;
        }
    }
}

