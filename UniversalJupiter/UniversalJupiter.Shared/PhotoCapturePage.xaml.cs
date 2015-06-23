using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using UniversalJupiter.Helpers;
using Windows.Storage.Streams;

namespace UniversalJupiter
{
    public sealed partial class PhotoCapturePage
    {
        private CameraCapture cameraCapture;

        public PhotoCapturePage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Init and show preview
            cameraCapture = new CameraCapture();
            PreviewElement.Source = await cameraCapture.Initialize();
            await cameraCapture.StartPreview();
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Release resources
            if (cameraCapture != null)
            {
                await cameraCapture.StopPreview();
                PreviewElement.Source = null;
                cameraCapture.Dispose();
                cameraCapture = null;
            }
        }

        private async void BtnCapturePhoto_Click(object sender, RoutedEventArgs e)
        {
            // Take snapshot and add to ListView
            // Disable button to prevent exception due to parallel capture usage
            BtnCapturePhoto.IsEnabled = false;
            var photoStorageFile = await cameraCapture.CapturePhoto();

            byte[] fileBytes = null;

            using (IRandomAccessStreamWithContentType stream = await photoStorageFile.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];

                using (DataReader reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }

            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(await photoStorageFile.OpenReadAsync());
            PhotoListView.Items.Add(bitmap);
            BtnCapturePhoto.IsEnabled = true;

            xRMConnector.CreateLead("Candidate", "Laetitia", "Casta", "CV", "CV.jpeg", "image/jpeg", fileBytes);
        }
    }
}