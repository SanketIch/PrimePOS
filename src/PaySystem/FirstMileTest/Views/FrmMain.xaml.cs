using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FirstMileTest.Views
{
    /// <summary>
    /// Interaction logic for FrmMain.xaml
    /// </summary>
    public partial class FrmMain : Window
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnSale_Click(object sender, RoutedEventArgs e)
        {
            FrmSale osale = new FrmSale();
            osale.ShowDialog();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            FrmReturns oReturn = new FrmReturns();
            oReturn.ShowDialog();
        }
    }
}
