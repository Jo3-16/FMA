// Christopher Braun 2016

using System.Windows.Media.Imaging;
using FMA.Contracts;

namespace FMA.View.Models
{
    public class LogoModel : MaterialChildModel
    {
        private byte[] logo = new byte[0];
        private double leftMargin;
        private double topMargin;
        private double height;
        private double width;
        private BitmapImage logoImage;

        public LogoModel()
        {
            FieldName = "Logo";
        }

        public bool HasLogo => Logo != null && Logo.Length > 0;

        public BitmapImage LogoImage
        {
            get { return logoImage; }
            private set
            {
                logoImage = value;
                OnPropertyChanged();
            }
        }

        public byte[] Logo
        {
            get { return logo; }
            set
            {
                logo = value;
                LogoImage = Logo.ToBitmapImage();
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLogo));
            }
        }

        public double LeftMargin
        {
            get { return leftMargin; }
            set
            {
                leftMargin = value;
                OnPropertyChanged();
            }
        }

        public double TopMargin
        {
            get { return topMargin; }
            set
            {
                topMargin = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged();
            }
        }


        public void DeleteLogo()
        {
            Logo = new byte[0];
        }
    }
}