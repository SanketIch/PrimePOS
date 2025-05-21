using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMile;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.IO;

namespace FirstMileTest.ViewModel
{
    class SignatureViewModel : BaseViewModel, INotifyPropertyChanged
    {

        private BitmapImage _Image;

        public BitmapImage Image
        {
            get
            {
                return _Image;
            }

            set
            {
                _Image = value;
                OnPropertyChanged("Image");
            }
        }

        public SignatureViewModel(string sigstring)
        {
            byte[] imageBytes = System.Convert.FromBase64String(sigstring);
            BitmapImage tempImage = new BitmapImage();
            tempImage.BeginInit();
            tempImage.StreamSource = new MemoryStream(imageBytes);
            tempImage.EndInit();
            Image = tempImage;
            tempImage = null;
        }



        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
