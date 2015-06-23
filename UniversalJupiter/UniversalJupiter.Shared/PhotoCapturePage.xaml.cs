using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using UniversalJupiter.Helpers;
using Windows.Storage;
using Windows.Storage.Streams;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Windows.Security.Cryptography;

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
            StorageFolder storageFolder = KnownFolders.DocumentsLibrary;

            var photoStorageFile = await cameraCapture.CapturePhoto();
            var docStorageFile = await storageFolder.CreateFileAsync("CV.pdf", CreationCollisionOption.ReplaceExisting);

            byte[] fileBytes = null;

            string srcFilename = photoStorageFile.Path;
            string dstFilename = docStorageFile.Path;

            IRandomAccessStream fileStream = await photoStorageFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
            BitmapImage srcImage = new BitmapImage();
            srcImage.SetSource(fileStream);

            Rectangle pageSize = null;

            pageSize = new Rectangle(0, 0, srcImage.PixelWidth, srcImage.PixelHeight);
            using (var ms = new MemoryStream())
            {
                var document = new Document(pageSize, 0, 0, 0, 0);
                PdfWriter.GetInstance(document, ms).SetFullCompression();
                document.Open();
                var image = iTextSharp.text.Image.GetInstance(srcFilename);
                document.Add(image);
                document.Close();

                var buffer = CryptographicBuffer.ConvertStringToBinary(ms.ToString(), BinaryStringEncoding.Utf8);

                await FileIO.WriteBufferAsync(docStorageFile, buffer);
            }

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

            // xRMConnector.CreateLead("Candidate", "Laetitia", "Casta", "CV", "CV.pdf", "application/pdf", fileBytes);
        }
    }
}