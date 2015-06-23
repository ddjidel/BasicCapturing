using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalJupiter
{
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void BtnGotoPhoto_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PhotoCapturePage));
        }

        private void BtnGotoVideo_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(VideoCapturePage));
        }
    }
}
