/// <summary>
/// added wpf screen to draw signature by using PEN or MOUSE or FINGURE
/// </summary>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Ink;
using POS_Core.CommonData;
using NLog;

namespace POS_Core_UI
{
    /// <summary>
    /// Interaction logic for frmDrawSignInWPF.xaml
    /// </summary>
    public partial class frmDrawSignInWPF : Window
    {
        //private string connectionString;
        public byte[] imgData;
        public eCalledFrom eCalledFromScreen;
        public string patientCounceling = string.Empty;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        private string m_patName=string.Empty;
        private string m_patAddress = string.Empty;

        public frmDrawSignInWPF()
        {
            InitializeComponent();
            SetColor();
            imgData = null;
        }

        public frmDrawSignInWPF(string patName, string patAddr)
        {
            m_patName = patName;
            m_patAddress = patAddr;
            InitializeComponent();
            SetColor();
            imgData = null;
        }

        #region 18-Jan-2018 JY set appearance for controls
        private void SetColor()
        {
            byte[] Window_BackColor = new byte[] { 192, 192, 192 };
            SolidColorBrush brushColor = new SolidColorBrush();
            brushColor.Color = Color.FromArgb(255, Window_BackColor[0], Window_BackColor[1], Window_BackColor[2]);  //sets the form back ground color
            this.Background = brushColor;

            string[] Window_BackColor3 = POS_Core.Resources.Configuration.Window_Color.Split(',');
            SolidColorBrush brushColor3 = new SolidColorBrush();
            brushColor3.Color = Color.FromArgb(255, System.Convert.ToByte(Window_BackColor3[0]), System.Convert.ToByte(Window_BackColor3[1]), System.Convert.ToByte(Window_BackColor3[2]));  
            btnClear.Background = btnSave.Background = brushColor3;

            SolidColorBrush brushColor1 = new SolidColorBrush();
            brushColor1.Color = Color.FromArgb(255, System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[0].ToString()), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[1]), System.Convert.ToByte(POS_Core.Resources.Configuration.arrCloseBk[2]));
            btnClose.Background = brushColor1;

            string[] Window_ForeColor = POS_Core.Resources.Configuration.Window_ForeColor.Split(',');
            SolidColorBrush brushColor2 = new SolidColorBrush();
            brushColor2.Color = Color.FromArgb(255, System.Convert.ToByte(Window_ForeColor[0].ToString()), System.Convert.ToByte(Window_ForeColor[1]), System.Convert.ToByte(Window_ForeColor[2]));
            btnClear.Foreground = btnSave.Foreground = btnClose.Foreground = brushColor2;
        }
        #endregion

        private void icDrawSign_Gesture(object sender, InkCanvasGestureEventArgs e)
        {

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.icDrawSign.Strokes.Clear();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tedPatName.Text = m_patName;
            tedPatAddress.Text = m_patAddress;
            if (eCalledFromScreen == eCalledFrom.ItemMonitoring)
            {
                this.Title = "Customer Signature";
                //Counseling hide
                Row2.Height = new GridLength(0);
                this.Height = this.Height - 45;

                //PrivacyText hide
                Row1.Height = new GridLength(0);
                this.Height = this.Height - 130;
            }
            else if (eCalledFromScreen == eCalledFrom.HIPPASign)
            {
                //Counseling hide
                Row2.Height = new GridLength(0);
                this.Height = this.Height - 45;

                this.Title = "HIPAA Signature";
                if (POS_Core.Resources.Configuration.CInfo.PrivacyText.Trim() == string.Empty)
                {
                    //PrivacyText hide
                    Row1.Height = new GridLength(0);
                    this.Height = this.Height - 140;
                }
                else
                {
                    txtPrivacyText.Text = POS_Core.Resources.Configuration.CInfo.PrivacyText;
                }
            }
            else if (eCalledFromScreen == eCalledFrom.RXSign)
            {
                this.Title = "Rx Signature";
                //PrivacyText hide
                Row1.Height = new GridLength(0);
                this.Height = this.Height - 140;

                if (POS_Core.Resources.Configuration.CInfo.PatientCounceling.ToUpper() == "N")
                {
                    rdoNo.IsChecked = true;
                    patientCounceling = "N";
                }
                else
                {
                    rdoYes.IsChecked = true;
                    patientCounceling = "Y";
                }
            }
            else if(eCalledFromScreen == eCalledFrom.CreditCardSign)
            {
                this.Title = "Credit Card Signature";
                //Counseling hide
                Row2.Height = new GridLength(0);
                this.Height = this.Height - 45;

                //PrivacyText hide
                Row1.Height = new GridLength(0);
                this.Height = this.Height - 130;
            }
            else if(eCalledFromScreen == eCalledFrom.HIPPATextOnly)
            {
                Row2.Height = new GridLength(0);
                this.Height = this.Height - 45;
                Row0.Height = new GridLength(0);
                this.Height = this.Height - 175;

                this.Title = "Privacy Acknowledgement";
                if (string.IsNullOrEmpty(POS_Core.Resources.Configuration.CInfo.PrivacyText))
                {
                    txtPrivacyText.Text = "By Signing below, I hereby acknowledge that I have received and reviewed the NOPP (Notice Of Privacy Practices) and that I hereby adhere to all the contents mentioned in the policy.";
                }
                else
                {
                    txtPrivacyText.Text = POS_Core.Resources.Configuration.CInfo.PrivacyText;
                }
                btnClear.IsEnabled = false;
                btnClear.Visibility = Visibility.Hidden; //PRIMEPOS-3231N
                btnSave.Content = "Accept";
            }
            //string ServerName = ConfigurationSettings.AppSettings["ServerName"];
            //string DatabaseName = ConfigurationSettings.AppSettings["DataBase"];
            //connectionString = @"server=" + ServerName + ";Database=" + DatabaseName + ";User ID =MMSPOS;Password =POS;;Max Pool Size=60;Min Pool Size=5;Pooling=True;";
            //PopulateCombo();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //POS_Core.ErrorLogging.Logs.Logger("\tDraw Sign", "CaptureSignature()", clsPOSDBConstants.Log_Entering);
            logger.Trace("btnSave_Click(object sender, RoutedEventArgs e) - " + clsPOSDBConstants.Log_Entering);

            StrokeCollection strokes = icDrawSign.Strokes;
            if (strokes.Count == 0 && eCalledFromScreen != eCalledFrom.HIPPATextOnly)
            {
                MessageBox.Show("Signature should not be blank.", "Customer Signature", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (strokes.Count > 0)
                imgData = SignatureToBytes(strokes);
            DialogResult = true;
            //POS_Core.ErrorLogging.Logs.Logger("\tDraw Sign", "CaptureSignature()", clsPOSDBConstants.Log_Exiting);
            logger.Trace("btnSave_Click(object sender, RoutedEventArgs e) - " + clsPOSDBConstants.Log_Exiting);
            this.Close();

            #region commented
            //string cmdText = "INSERT INTO TestSign(SignData) VALUES (@SignData)";
            //using (SqlConnection con = new SqlConnection(connectionString))
            //using (SqlCommand cmd = new SqlCommand(cmdText, con))
            //{
            //    con.Open();
            //    cmd.Parameters.Add(new SqlParameter
            //    {
            //        ParameterName = "@SignData",
            //        SqlDbType = SqlDbType.VarBinary,
            //        Size = imgData.Length,
            //        Value = imgData
            //    });
            //    cmd.ExecuteNonQuery();
            //    MessageBox.Show("Signature saved successfully");
            //    icDrawSign.Strokes.Clear();
            //    PopulateCombo();
            //}
            #endregion
        }

        private byte[] SignatureToBytes(StrokeCollection strokes)
        {
            byte[] imgData;
            Rect strokesBounds = strokes.GetBounds();
            DrawingVisual strokesVisual = new DrawingVisual();
            DrawingContext drawingContext = strokesVisual.RenderOpen();
            drawingContext.PushTransform(new TranslateTransform(-strokesBounds.X, -strokesBounds.Y));
            drawingContext.DrawRectangle(Brushes.White, null, strokesBounds);
            strokes.Draw(drawingContext);
            drawingContext.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)strokesBounds.Width, (int)strokesBounds.Height, 96, 96, PixelFormats.Default);
            bmp.Render(strokesVisual);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            //BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                //get the bitmap bytes from the memory stream
                ms.Position = 0;
                imgData = ms.ToArray();
            }

            return imgData;
        }

        #region Added for testing
        //private void PopulateCombo()
        //{
        //    try
        //    {
        //        //cboSign.ItemsSource = null;
        //        DataTable dt = GetDataTable();
        //        cboSign.ItemsSource = dt.DefaultView;
        //        cboSign.DisplayMemberPath = dt.Columns[0].ToString();
        //        cboSign.SelectedValuePath = dt.Columns[0].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void btnLoad_Click(object sender, RoutedEventArgs e)
        //{
        //    if (cboSign.SelectedValue == null) return;

        //    byte[] imgData;
        //    string cmdText = "SELECT SignData from TestSign where id =" + cboSign.SelectedValue.ToString();

        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    using (SqlCommand cmd = new SqlCommand(cmdText, con))
        //    {
        //        con.Open();
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                // First call to get the length of binary data that we want to read back
        //                long bufLength = reader.GetBytes(0, 0, null, 0, 0);

        //                // Now allocate a buffer big enough to receive the bits...
        //                imgData = new byte[bufLength];

        //                // Get all bytes from the reader
        //                reader.GetBytes(0, 0, imgData, 0, (int)bufLength);

        //                // Transfer everything to the Image property of the picture box....
        //                MemoryStream ms = new MemoryStream(imgData);
        //                ms.Position = 0;

        //                byte[] bytes = ms.ToArray();
        //                Stream stream = new MemoryStream(bytes);
        //                icDrawSign.Strokes.Clear();
        //                icDrawSign.Strokes = new System.Windows.Ink.StrokeCollection(stream);
        //            }
        //        }
        //    }
        //}

        //private DataTable GetDataTable()
        //{
        //    SqlConnection con = new SqlConnection(connectionString);
        //    DataTable dt = new DataTable();
        //    string strSql = "select id, SignData from TestSign";
        //    SqlCommand cmd = new SqlCommand(strSql, con);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    return dt;
        //}
        #endregion

        private void rdoYes_Checked(object sender, RoutedEventArgs e)
        {
            patientCounceling = "Y";
        }

        private void rdoNo_Checked(object sender, RoutedEventArgs e)
        {
            patientCounceling = "N";
        }
    }

    public enum eCalledFrom
    {
        ItemMonitoring,
        HIPPASign,
        RXSign,
        CreditCardSign,
        HIPPATextOnly
    }
}
