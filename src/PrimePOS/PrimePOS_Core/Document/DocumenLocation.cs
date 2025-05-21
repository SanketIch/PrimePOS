using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.Document
{
    public enum DocumentLocation
    {
        LocalDrivePath = 0,
        SQLDatabase = 1,
        AmazonCloudStorage = 2
    }
}