using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UniversalJupiter.Helpers;

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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            xRMConnector.CreateLead("Candidate", "Laetitia", lastName.Text);
        }

        private void lastName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
