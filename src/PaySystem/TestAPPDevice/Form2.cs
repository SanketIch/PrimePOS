using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestAPPDevice
{
    public partial class Form2 : Form
    {
        Dictionary<string, string> Result = new Dictionary<string, string>();
        public Form2(Dictionary<string, string> passResult)
        {
            InitializeComponent();
            Result = passResult;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (Result != null)
            {
                foreach (var r in Result)
                {
                    listBox1.Items.Add(string.Format("{0,0} {1,25}", r.Key, r.Value));
                }
            }
        }
    }
}
