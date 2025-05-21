using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vantiv.RequestModels
{
    public class ScrollingDisplay
    {
        public string laneId { get; set; }
        public string lineItem { get; set; }
        public string subtotal { get; set; }
        public string tax { get; set; }
        public string total { get; set; }
    }
}
