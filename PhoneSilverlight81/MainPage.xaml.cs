using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Windows.Phone.Media.Capture;
using Microsoft.Devices;
using UniversalJupiter;

namespace PhoneSilverlight81
{
    public partial class MainPage
    {
        private CameraCapture cameraCapture;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Init and show preview
            cameraCapture = new CameraCapture();
            await cameraCapture.Initialize();

            // Wire up the interop between new Windows.Media.Capture API and the SL 8.0 Windows.Phone.Media.Capture to show the preview in a VideoBrush
            var previewSink = new MediaCapturePreviewSink();
            var videoBrush = new VideoBrush();
            videoBrush.SetSource(previewSink);
            PreviewElement.Fill = videoBrush;

            // Find the supported preview size that's closest to the desired size
            var desiredPreviewArea = PreviewElement.Height * PreviewElement.Width;
            await cameraCapture.StartPreview(previewSink, desiredPreviewArea);
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Release resources
            if (cameraCapture != null)
            {
                await cameraCapture.StopPreview();
                PreviewElement.Fill = null;
                cameraCapture.Dispose();
                cameraCapture = null;
            }
        }

        private async void BtnCapturePhoto_Click(object sender, RoutedEventArgs e)
        {
            // Take snapshot and add to ListBox
            // Disable button to prevent exception due to parallel capture usage
            BtnCapturePhoto.IsEnabled = false;
            var photoStorageFile = await cameraCapture.CapturePhoto();
            var bitmap = new BitmapImage();

            // Need to use .NET stream here and non-async SetSource method
            bitmap.SetSource(await photoStorageFile.OpenStreamForReadAsync());
            PhotoListBox.Items.Add(bitmap);
            BtnCapturePhoto.IsEnabled = true;
        }
    }
}