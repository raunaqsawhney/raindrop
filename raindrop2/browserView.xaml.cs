using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace raindrop2
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class browserView : raindrop2.Common.LayoutAwarePage
    {
        string backAddr = "";
        public browserView()
        {
            this.InitializeComponent();
            doLoadBG();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void doLoadBG()
        {
            try
            {
                System.Xml.Linq.XDocument xmlDoc = XDocument.Load("http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-US");
                IEnumerable<string> strTest = from node in xmlDoc.Descendants("url") select node.Value;
                string strURL = "http://www.bing.com" + strTest.First();

                Uri source = new Uri(strURL);
                StorageFile destinationFile;
                try
                {
                    destinationFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("raindropBG.jpg",
                        CreationCollisionOption.ReplaceExisting);
                }
                catch (FileNotFoundException ex)
                {
                    return;
                }
                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation download = downloader.CreateDownload(source, destinationFile);
                await download.StartAsync();
                ResponseInformation response = download.GetResponseInformation();

                Uri imageUri;
                BitmapImage image = null;

                if (Uri.TryCreate(destinationFile.Path, UriKind.RelativeOrAbsolute, out imageUri))
                {
                    image = new BitmapImage(imageUri);
                }

                imgBrush.ImageSource = image;

            }
            catch
            {
                //
            }

        }

        private void loadPage(object sender, KeyRoutedEventArgs e)
        {
            string uri = "http://" + inputBox.Text;

            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Uri targetUri = new Uri(uri);
                webView.Navigate(targetUri);
                backAddr = uri;
            }
            backAddr = uri;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            Uri targetUri = new Uri(backAddr);
            webView.Navigate(targetUri);
        }

    }
}
