using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace raindrop2
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class locations : raindrop2.Common.LayoutAwarePage
    {
        string[] locationsResults ;
        string[] coordinates;

        string[] recentLocation = new string[1];


        public locations()
        {
            this.InitializeComponent();

            recentLocation[0] = (string)ApplicationData.Current.LocalSettings.Values["recentLocation"];
            if (recentLocation[0] != null)
                recentsList.ItemsSource = recentLocation;

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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = locationsList.SelectedIndex;
            string selectedCity = locationsResults[selectedIndex];
            string selectedCoordinates = coordinates[selectedIndex];

            string[] tmpData = Regex.Split(selectedCoordinates,",");
            string tmpLat = tmpData[0].TrimStart('\r','\n','[');
            string tmpLon = tmpData[1].TrimStart('\r', '\n');

            tmpLon = tmpLon.TrimEnd('\r', '\n', ']');

            tmpLat = tmpLat.Trim();
            tmpLon = tmpLon.Trim();
            
            ApplicationData.Current.LocalSettings.Values["Latitude"] = tmpLat;
            ApplicationData.Current.LocalSettings.Values["Longitude"] = tmpLon;
            ApplicationData.Current.LocalSettings.Values["newLocSet"] = "T";

            ApplicationData.Current.LocalSettings.Values["recentLocation"] = selectedCity;
            ApplicationData.Current.LocalSettings.Values["recentLat"] = tmpLat;
            ApplicationData.Current.LocalSettings.Values["recentLon"] = tmpLon;

            this.Frame.Navigate(typeof(MainPage));
            
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string inputText = inputLocation.Text;
            var locationClient = new HttpClient();
            var locationResponse = await locationClient.GetStringAsync(new Uri(string.Format("http://dev.virtualearth.net/REST/v1/Locations?q={0}&key=AolSXFvYCT3GN2OahxHsZUnS3gyh7bdpyEb-S2UVrBRorRw3Qn1av22WuXcgA7VJ", inputText)));
            var locationObject = JObject.Parse(locationResponse);


            int numOfResources = (int)locationObject["resourceSets"][0]["estimatedTotal"];
            locationsResults = new string[numOfResources];
            coordinates = new string[numOfResources];


            for (int i = 0; i < numOfResources; i++)
            {
                locationsResults[i] = locationObject["resourceSets"][0]["resources"][i]["name"].ToString();
                coordinates[i] = locationObject["resourceSets"][0]["resources"][i]["point"]["coordinates"].ToString();
            }
            locationsList.ItemsSource = locationsResults;
        }

        private new void ManipulationStarted(object sender, RoutedEventArgs e)
        {
            inputLocation.Text = "";
        }

        private void myLocBtn_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["newLocSet"] = "F";
            this.Frame.Navigate(typeof(MainPage));
        }

        private void recentsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string recentLat = (string)ApplicationData.Current.LocalSettings.Values["recentLat"];
            string recentLon = (string)ApplicationData.Current.LocalSettings.Values["recentLon"];

            ApplicationData.Current.LocalSettings.Values["Latitude"] = recentLat;
            ApplicationData.Current.LocalSettings.Values["Longitude"] = recentLon;
            ApplicationData.Current.LocalSettings.Values["newLocSet"] = "T";

            this.Frame.Navigate(typeof(MainPage));
        }


    
    }
}
