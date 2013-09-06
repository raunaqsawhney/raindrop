#region Using
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.DataTransfer;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
#endregion Using

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace raindrop2
{
    public sealed partial class MainPage : raindrop2.Common.LayoutAwarePage
    {
        #region Global Declaratives       
        string firstName = (string)ApplicationData.Current.LocalSettings.Values["FirstName"];
        string tempUnit = (string)ApplicationData.Current.LocalSettings.Values["TempUnit"];
        string distUnit = (string)ApplicationData.Current.LocalSettings.Values["DistUnit"];

        bool? tempUnitB = null;
        bool? distUnitB = null;

        string tmpLat = null; 
        string tmpLon = null;
         
        string _newLocSet = null;

        public enum Codes
        {
            Rain = 1,
            Thunderstorm = 2,
            Snow = 3,
            Cloudy = 4,
            Clear = 5,
            Hail = 6,
            Fog = 7,
            Sand = 8,
            Dust = 9,
            Sqalls = 10,
            Funnel = 11
        }

        #endregion Global Declaratives

        #region Main
        public MainPage()
        {
            this.InitializeComponent();
            doLoadBG();
            checkInitialization();
            loadBasicLocation();
        }
        #endregion Main

        #region State Managers (Useless)
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }
        #endregion State Managers

        #region Get Bing Background
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
                catch
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
                showGenericAlert();
            }

        }
        #endregion Get Bing Background

        #region Check Initialization
        private void checkInitialization()
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Initialized") == true)
                {
                    continueButton.Visibility = Visibility.Visible;
                    beginButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    beginButton.Visibility = Visibility.Visible;
                    continueButton.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                showGenericAlert();
            }
        }
        #endregion Check Initialization

        #region Load Basic Location
        private async void loadBasicLocation()
        {
            tmpLat = (string)ApplicationData.Current.LocalSettings.Values["Latitude"];
            tmpLon = (string)ApplicationData.Current.LocalSettings.Values["Longitude"];

            _newLocSet = (string)ApplicationData.Current.LocalSettings.Values["newLocSet"];

            if (continueButton.Visibility == Visibility.Visible)
            {
                var loc = new Geolocator();
                try
                {
                    if ((_newLocSet == "F" || _newLocSet == null))
                    {
                            loc.DesiredAccuracy = PositionAccuracy.High;
                            Geoposition pos = await loc.GetGeopositionAsync();

                            string curLoclat = pos.Coordinate.Latitude.ToString();
                            string curLocLon = pos.Coordinate.Longitude.ToString();

                            // Show weather info for current location
                            ApplicationData.Current.LocalSettings.Values["Latitude"] = curLoclat;
                            ApplicationData.Current.LocalSettings.Values["Longitude"] = curLocLon;

                            loadBasicWeather(curLoclat, curLocLon);
                    }
                    else if (_newLocSet == "T")
                    {
                        loadBasicWeather(tmpLat, tmpLon);
                    }
                }
                catch
                {
                    showLocError();
                }
            }
            else
            {
                GreetingText.Text = "hello!";
                currentBasicWeather.Text = "This is your first time using raindrop.\nPlease click below to learn more about how to use the application.";
                mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/StoreLogo.png"));
            }
        }
        #endregion Load Basic Location

        #region Load Basic Weather
        private async void loadBasicWeather(string latitude, string longitude)
        {
            #region Check Units
            if (tempUnit == "Celsius")
                tempUnitB = true;
            else
                tempUnitB = false;

            if (distUnit == "Imperial")
                distUnitB = true;
            else
                distUnitB = false;
            #endregion Check Units

            try
            {
                #region Fetch JSON Weather Response
                if (continueButton.Visibility == Visibility.Visible)
                {
                    var client = new HttpClient();
                    var response = await client.GetStringAsync(new Uri(string.Format("http://api.wunderground.com/api/cef78de98b1ad61f/conditions/q/{0},{1}.json", latitude, longitude)));

                    var obj = JObject.Parse(response);
                    var city = obj["current_observation"]["display_location"]["city"];

                    ApplicationData.Current.LocalSettings.Values["cityName"] = city.ToString();

                    var weather = obj["current_observation"]["weather"];
                    var temp_f = obj["current_observation"]["temp_f"];
                    var temp_c = obj["current_observation"]["temp_c"];

                    var relative_humidity = obj["current_observation"]["relative_humidity"];

                    var visibility_mi = obj["current_observation"]["visibility_mi"];
                    var visibility_km = obj["current_observation"]["visibility_km"];
                #endregion Fetch JSON Weather Response

                    #region Get Humidity
                    string humidity = relative_humidity.ToString();
                    if (humidity == "N/A%" || humidity == "-9999" || humidity == "-999")
                    {
                        humidity = "no";
                    }
                    #endregion Get Humidity

                    #region Choose C or F for Basic Weather
                    string tempCorF = "";
                    if (tempUnitB == true)
                    {
                        tempCorF = temp_c.ToString() + "°C";
                    }
                    else
                    {
                        tempCorF = temp_f.ToString() + "°F";
                    }
                    #endregion Choose C or F for Basic Weather

                    #region Choose M or I for Basic Weather
                    string visibilityKorM = "";
                    if (distUnitB == false)
                    {
                        visibilityKorM = visibility_km.ToString() + " km";
                    }
                    else
                    {
                        visibilityKorM = visibility_mi.ToString() + " mi";
                    }
                    #endregion Choose M or I for Basic Weather

                    #region Get Time of Day
                    string timeOfDay;
                    if (DateTime.Now.Hour < 12)
                    {
                        timeOfDay = "Good Morning " + firstName + "!";
                    }
                    else if (DateTime.Now.Hour < 17)
                    {
                        timeOfDay = "Good Afternoon " + firstName + "!";
                    }
                    else
                    {
                        timeOfDay = "Good Evening " + firstName + "!";
                    }
                    #endregion Get Time of Day

                    #region Decide Icon
                    string timeOfDayIcon = "";
                    if (DateTime.Now.Hour < 12)
                    {
                        timeOfDayIcon = "Morning";
                    }
                    else if (DateTime.Now.Hour < 17)
                    {
                        timeOfDayIcon = "Afternoon";
                    }
                    else
                    {
                        timeOfDayIcon = "Evening";
                    }

                    int result = classifyWeather(weather.ToString());


                    //Rain
                    if (result == 1 && (timeOfDayIcon == "Morning" || timeOfDayIcon == "Afternoon"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                    }
                    else if (result == 1 && (timeOfDayIcon == "Evening"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Moon_Rain.png"));
                    }

                    //Thunderstorm
                    else if (result == 2 && (timeOfDayIcon == "Morning" || timeOfDayIcon == "Afternoon"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                    }
                    else if (result == 2 && (timeOfDayIcon == "Evening"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning_Moon.png"));
                    }

                    //Snow
                    else if (result == 3 && (timeOfDayIcon == "Morning" || timeOfDayIcon == "Afternoon"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                    }
                    else if (result == 3 && (timeOfDayIcon == "Evening"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow_Moon.png"));
                    }

                    //Cloudy
                    else if (result == 4 && (timeOfDayIcon == "Morning" || timeOfDayIcon == "Afternoon"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                    }
                    else if (result == 4 && (timeOfDayIcon == "Evening"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                    }

                    //Clear
                    else if (result == 5 && (timeOfDayIcon == "Morning" || timeOfDayIcon == "Afternoon"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                    }
                    else if (result == 5 && (timeOfDayIcon == "Evening"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Moon.png"));
                    }

                    //Hail
                    else if (result == 6 && (timeOfDayIcon == "Morning" || timeOfDayIcon == "Afternoon"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                    }
                    else if (result == 6 && (timeOfDayIcon == "Evening"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail_Moon.png"));
                    }

                    //Fog
                    else if (result == 7 && (timeOfDayIcon == "Morning" || timeOfDayIcon == "Afternoon"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                    }
                    else if (result == 7 && (timeOfDayIcon == "Evening"))
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog_Moon.png"));
                    }

                    //Sand
                    else if (result == 8)
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                    }

                    //Dust
                    else if (result == 9)
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                    }

                    //Squalls
                    else if (result == 10)
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                    }

                    //Funnel Cloud
                    else if (result == 11)
                    {
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                    }
                    #endregion Decide Icon

                    #region Display Text Weather
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Initialized"))
                    {
                        GreetingText.Text = timeOfDay;
                        currentBasicWeather.Text = "It is currently " + tempCorF + " and " + weather.ToString().ToLower() + " in " + city.ToString() + ". There appears to be a humidity of " + humidity + " and a visibility of " + visibilityKorM + ".";
                    }
                    else
                    {
                        GreetingText.Text = "hello!";
                        currentBasicWeather.Text = "This is your first time using raindrop.\nPlease click below to learn more about how to use the application.";
                        mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/StoreLogo.png"));
                    }

                    ApplicationViewState viewState = ApplicationView.Value;
                    if (viewState == ApplicationViewState.FullScreenLandscape)
                    {
                        this.RegisterBackgroundTask();
                    }
                    #endregion Display Text Weather
                }
                else 
                {
                    GreetingText.Text = "hello!";
                    currentBasicWeather.Text = "This is your first time using raindrop.\nPlease click below to learn more about how to use the application.";
                    mainIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/StoreLogo.png"));
                }

            }
            catch
            {
                showGenericAlert();
            }
        }

        private int classifyWeather(string weatherDescription)
        {
            string stringToCheck = weatherDescription;
            int ret = 0;

            if (stringToCheck.Contains("Rain") || stringToCheck.Contains("Overcast") || stringToCheck.Contains("Drizzle"))
            {
                ret = (int)Codes.Rain;
            }
            else if (stringToCheck.Contains("Thunderstorms") || stringToCheck.Contains("Thundertorm"))
            {
                ret = (int)Codes.Thunderstorm;
            }
            else if (stringToCheck.Contains("Snow"))
            {
                ret = (int)Codes.Snow;
            }
            else if (stringToCheck.Contains("Hail") || stringToCheck.Contains("Ice"))
            {
                ret = (int)Codes.Hail;
            }
            else if (stringToCheck.Contains("Fog") || stringToCheck.Contains("Mist") || stringToCheck.Contains("Haze"))
            {
                ret = (int)Codes.Fog;
            }
            else if (stringToCheck.Contains("Sand"))
            {
                ret = (int)Codes.Sand;
            }
            else if (stringToCheck.Contains("Dust") || stringToCheck.Contains("Smoke"))
            {
                ret = (int)Codes.Dust;
            }
            else if (stringToCheck.Contains("Cloudy") || stringToCheck.Contains("Clouds"))
            {
                ret = (int)Codes.Cloudy;
            }
            else if (stringToCheck.Contains("Squalls"))
            {
                ret = (int)Codes.Sqalls;
            }
            else if (stringToCheck.Contains("Funnel"))
            {
                ret = (int)Codes.Funnel;
            }
            else if (stringToCheck.Contains("Clear"))
            {
                ret = (int)Codes.Clear;
            }
            else
            {
                ret = 0;
            }
            return ret;
        }
        #endregion Load Basic Weather

        #region Error Messages

        private async void showGenericAlert()
        {
            MessageDialog genericAlert = new MessageDialog("This bug is temporary, and will be fixed as soon as possible.\nWe advise you to click on \"Close\" below, then close the app, and restart it again from the Windows Start Screen.\n\nPlease email us regarding this issue, so we can fix it for the next update. Thanks.", "Oops! Something doesn't seem right!");
            //OK Button
            UICommand closeBtn = new UICommand("close");
            genericAlert.Commands.Add(closeBtn);

            //Show message
            await genericAlert.ShowAsync();
        }

        private async void showLocError()
        {
            MessageDialog locError = new MessageDialog("Your location cannot be accessed.\nWe can try to close the application and start location services again.", "error");
            //OK Button
            UICommand resartBtn = new UICommand("close");
            locError.Commands.Add(resartBtn);

            //Show message
            await locError.ShowAsync();
        }

        #endregion Error Messages

        #region Load Different Views 
        private async void GoToWunderground(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.wunderground.com/?apiref=5aeed8d448b15bb8"));
        }

        private void loadWelcomeView(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["Initialized"] = true;
            this.Frame.Navigate(typeof(welcomeView));
        }

        private void loadWeatherView(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(weatherView));
        }

        private async void DownloadImage(object sender, RoutedEventArgs e)
        {
            MessageDialog downloadMessage = new MessageDialog("You will need to be re-directed to an external webpage to download the image and save it to your computer.", "download wallpaper");
            UICommand goBtn = new UICommand("Continue");
            goBtn.Invoked = goBtnClk;
            downloadMessage.Commands.Add(goBtn);


            await downloadMessage.ShowAsync();


        }

        private async void goBtnClk(IUICommand command)
        {
            System.Xml.Linq.XDocument xmlDoc = XDocument.Load("http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-US");
            IEnumerable<string> strTest = from node in xmlDoc.Descendants("url") select node.Value;
            string strURL = "http://www.bing.com" + strTest.First();
            await Launcher.LaunchUriAsync(new Uri(strURL));
        }
        #endregion Load Different Views

        #region New Locations
        private void locationManager(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(locations));
        }

        private void showForCurrentLocation(object sender, RoutedEventArgs e)
        {
            _newLocSet = "F";
            ApplicationData.Current.LocalSettings.Values["newLocSet"] = "F";
            loadBasicLocation();
        }
        #endregion New Locations

        #region Background Task
        private async void RegisterBackgroundTask()
        {
            BackgroundAccessStatus backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        task.Value.Unregister(true);
                    }
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = taskName;
                taskBuilder.TaskEntryPoint = taskEntryPoint;
                taskBuilder.SetTrigger(new TimeTrigger(60, false));
                var registration = taskBuilder.Register();
            }
        }

        private const string taskName = "BackgroundTask";
        private const string taskEntryPoint = "BackgroundTask.LiveTileUpdate";
        #endregion Background Task
    }
}