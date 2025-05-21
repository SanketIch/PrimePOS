using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDevice.ISC
{
    public class InputTags
    {
        public enum PromptTags
        {
            Home_Phone = 222,
            Email_Address = 212,
            DOB = 213,
            SSN = 205,
            NUMBERS = 250
        }
        public enum FormatTags
        {
            SSN = 28,
            DOB = 9,
            Phone = 7,
            Text = 1,
            Numbers = 2
        }
    }
}
