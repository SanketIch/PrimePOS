using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMile.Helpers
{
    public class ResponseTag:IDisposable
    {
        public string Tag { set; get; }
        public string Value { set; get; }

        public ResponseTag(string response,string splitchar)
        {
            Tag = "";
            Value = "";

            string[] split;
            string[] delimiter = new string[] { splitchar };
            if(!string.IsNullOrEmpty(response) && response.Contains(splitchar))
            {
                split = response.Split(delimiter,2,StringSplitOptions.None);

                if (split.Length > 0)
                {
                    Tag = split[0];
                    if (split.Length > 0)
                    {
                        Value = split[1];
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Tag != null)
                {
                    Tag = null;
                }
                if (Value != null)
                {
                    Value = null;
                }
            }
        }
    }
}
