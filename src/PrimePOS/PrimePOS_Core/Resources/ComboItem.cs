using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.Resources
{
    public class ComboItem
    {
        public int Value { set; get; }
        public string Text { set; get; }

        public ComboItem(int value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}
