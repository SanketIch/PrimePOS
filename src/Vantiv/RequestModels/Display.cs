﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vantiv.RequestModels
{
    public class Display
    {
        public string laneId { get; set; }
        public string text { get; set; }
        public List<string> multiLineText { get; set; }
    }
}
