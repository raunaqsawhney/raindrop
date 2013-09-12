using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace raindrop2
{
    public sealed partial class weatherView : raindrop2.Common.LayoutAwarePage
    {
        string tempUnit = (string)ApplicationData.Current.LocalSettings.Values["TempUnit"];
        string distUnit = (string)ApplicationData.Current.LocalSettings.Values["DistUnit"];

        string tmpLat = (string)ApplicationData.Current.LocalSettings.Values["Latitude"];
        string tmpLon = (string)ApplicationData.Current.LocalSettings.Values["Longitude"];

        bool? tempUnitB = null;
        bool? distUnitB = null;

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

        private Color backgroundColor_ = Color.FromArgb(255, 255, 187, 51);

        alert alertClass = new alert();

        public weatherView()
        {
            this.InitializeComponent();
            beginFetchLocation();
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void beginFetchLocation()
        {
            //Check if Celsius or Fahrenheit
            if (tempUnit == "Celsius")
                tempUnitB = true;
            else
                tempUnitB = false;

            if (distUnit == "Imperial")
                distUnitB = true;
            else
                distUnitB = false;

            try
            {
                #region Check for Alerts
                var alertClient = new HttpClient();
                var alertResponse = await alertClient.GetStringAsync(new Uri(string.Format("http://api.wunderground.com/api/cef78de98b1ad61f/alerts/q/{0},{1}.json", tmpLat, tmpLon)));
                var alertObj = JObject.Parse(alertResponse);

                if (!(alertObj["alerts"].ToString() == "[]"))
                {
                    ApplicationData.Current.LocalSettings.Values["alertDate"] = alertObj["alerts"][0]["date"].ToString();
                    ApplicationData.Current.LocalSettings.Values["alertExpires"] = alertObj["alerts"][0]["expires"].ToString();

                    if (alertClass.expires == "" || alertClass.date == "" || alertClass.date == null || alertClass.expires == null)
                    {
                        ApplicationData.Current.LocalSettings.Values["alertDate"] = "N/A";
                        ApplicationData.Current.LocalSettings.Values["alertExpires"] = "N/A";
                    }

                    ApplicationData.Current.LocalSettings.Values["alertDescription"] = alertObj["alerts"][0]["description"].ToString();
                    ApplicationData.Current.LocalSettings.Values["alertMessage"] = alertObj["alerts"][0]["message"].ToString();

                    alertBtn.Visibility = Visibility.Visible;
                    alertBtn.Click += new RoutedEventHandler(showAlert);

                }
                #endregion Check for Alerts

                beginFetchWeather(tmpLat, tmpLon);
            }
            catch
            {
                showGenericAlert();
            }
        }

        #region Show Alert
        private void showAlert(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(alertView), alertClass);
        }
        #endregion Show Alert

        #region Fetch Weather
        private async void beginFetchWeather(string latitude, string longitude)
        {
            try
            {
                //Get Basic Data for current conditions
                var client = new HttpClient();
                var response = await client.GetStringAsync(new Uri(string.Format("http://api.wunderground.com/api/cef78de98b1ad61f/conditions/q/{0},{1}.json", tmpLat, tmpLon)));

                var obj = JObject.Parse(response);

                #region city
                cityName.Text = obj["current_observation"]["display_location"]["city"].ToString();

                ApplicationData.Current.LocalSettings.Values["CITY"] = obj["current_observation"]["display_location"]["city"].ToString();

                #endregion city
                var observationTime = obj["current_observation"]["observation_time"];
                observation_time.Text = observationTime.ToString();


                #region Time Of Day
                string timeOfDay;
                if (DateTime.Now.Hour < 12)
                {
                    timeOfDay = "Morning";
                }
                else if (DateTime.Now.Hour < 17)
                {
                    timeOfDay = "Afternoon";
                }
                else
                {
                    timeOfDay = "Evening";
                }
                #endregion Time of Day

                #region set appropriate background image
                var weather = obj["current_observation"]["weather"];
                string weatherDescription = weather.ToString();
                desc.Text = weatherDescription;
                int result = classifyWeather(weatherDescription);


                //Rain
                if (result == 1 && (timeOfDay == "Morning" || timeOfDay == "Afternoon"))
                {
                    Random randomObject = new Random();
                    int rainRandomNumber = randomObject.Next(0, 2);

                    string[] rainImages = new string[3];

                    rainImages[0] = "Assets/Images/Rain_Day.jpg";
                    rainImages[1] = "Assets/Images/Rain_Day2.jpg";
                    rainImages[2] = "Assets/Images/Rain_Day3.jpg";

                    string selectedIndexString = rainImages[rainRandomNumber];

                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }
                else if (result == 1 && (timeOfDay == "Evening"))
                {
                    Random randomObject = new Random();
                    int rainRandomNumber = randomObject.Next(0, 2);

                    string[] rainImages = new string[3];

                    rainImages[0] = "Assets/Images/Rain_Night.jpg";
                    rainImages[1] = "Assets/Images/Rain_Night2.jpg";
                    rainImages[2] = "Assets/Images/Rain_Night3.jpg";

                    string selectedIndexString = rainImages[rainRandomNumber];

                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Moon_Rain.png"));
                }

                //Thunderstorm
                else if (result == 2 && (timeOfDay == "Morning" || timeOfDay == "Afternoon"))
                {
                    Random randomObject = new Random();
                    int thunderstormRandomNumber = randomObject.Next(0, 2);

                    string[] thunderstormImages = new string[3];

                    thunderstormImages[0] = "Assets/Images/Thunderstorm_Day.jpg";
                    thunderstormImages[1] = "Assets/Images/Thunderstorm_Day2.jpg";
                    thunderstormImages[2] = "Assets/Images/Thunderstorm_Day3.jpg";

                    string selectedIndexString = thunderstormImages[thunderstormRandomNumber];

                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString)); weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }
                else if (result == 2 && (timeOfDay == "Evening"))
                {
                    Random randomObject = new Random();
                    int thunderstormRandomNumber = randomObject.Next(0, 2);

                    string[] thunderstormImages = new string[3];

                    thunderstormImages[0] = "Assets/Images/Thunderstorm_Night.jpg";
                    thunderstormImages[1] = "Assets/Images/Thunderstorm_Night2.jpg";
                    thunderstormImages[2] = "Assets/Images/Thunderstorm_Night3.jpg";

                    string selectedIndexString = thunderstormImages[thunderstormRandomNumber];

                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString)); 
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning_Moon.png"));
                }

                //Snow
                else if (result == 3 && (timeOfDay == "Morning" || timeOfDay == "Afternoon"))
                {
                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Snow_Day.jpg"));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }
                else if (result == 3 && (timeOfDay == "Evening"))
                {
                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Snow_Night.jpg"));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow_Moon.png"));
                }

                //Cloudy
                else if (result == 4 && (timeOfDay == "Morning" || timeOfDay == "Afternoon"))
                {
                    Random randomObject = new Random();
                    int cloudyRandomNumber = randomObject.Next(0, 8);

                    string[] cloudyImages = new string[9];

                    cloudyImages[0] = "Assets/Images/Cloudy_Day.jpg";
                    cloudyImages[1] = "Assets/Images/Cloudy_Day2.jpg";
                    cloudyImages[2] = "Assets/Images/Cloudy_Day3.jpg";
                    cloudyImages[3] = "Assets/Images/Cloudy_Day4.jpg";
                    cloudyImages[4] = "Assets/Images/Cloudy_Day5.jpg";
                    cloudyImages[5] = "Assets/Images/Cloudy_Day6.jpg";
                    cloudyImages[6] = "Assets/Images/Cloudy_Day7.jpg";
                    cloudyImages[7] = "Assets/Images/Cloudy_Day8.jpg";
                    cloudyImages[8] = "Assets/Images/Cloudy_Day9.jpg";

                    string selectedIndexString = cloudyImages[cloudyRandomNumber];

                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }
                else if (result == 4 && (timeOfDay == "Evening"))
                {
                    Random randomObject = new Random();
                    int cloudyRandomNumber = randomObject.Next(0,8);

                    string[] cloudyImages = new string[9];

                    cloudyImages[0] = "Assets/Images/Cloudy_Night.jpg";
                    cloudyImages[1] = "Assets/Images/Cloudy_Night2.jpg";
                    cloudyImages[2] = "Assets/Images/Cloudy_Night3.jpg";
                    cloudyImages[3] = "Assets/Images/Cloudy_Night4.jpg";
                    cloudyImages[4] = "Assets/Images/Cloudy_Night5.jpg";
                    cloudyImages[5] = "Assets/Images/Cloudy_Night6.jpg";
                    cloudyImages[6] = "Assets/Images/Cloudy_Night7.jpg";
                    cloudyImages[7] = "Assets/Images/Cloudy_Night8.jpg";
                    cloudyImages[8] = "Assets/Images/Cloudy_Night9.jpg";


                    string selectedIndexString = cloudyImages[cloudyRandomNumber];

                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result == 5 && (timeOfDay == "Morning" || timeOfDay == "Afternoon"))
                {
                    Random randomObject = new Random();
                    int clearRandomNumber = randomObject.Next(0, 7);

                    string[] clearImages = new string[8];

                    clearImages[0] = "Assets/Images/Clear_Day.jpg";
                    clearImages[1] = "Assets/Images/Clear_Day2.jpg";
                    clearImages[2] = "Assets/Images/Clear_Day3.jpg";
                    clearImages[3] = "Assets/Images/Clear_Day4.jpg";
                    clearImages[4] = "Assets/Images/Clear_Day5.jpg";
                    clearImages[5] = "Assets/Images/Clear_Day6.jpg";
                    clearImages[6] = "Assets/Images/Clear_Day7.jpg";
                    clearImages[7] = "Assets/Images/Clear_Day8.jpg";


                    string selectedIndexString = clearImages[clearRandomNumber];

                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }
                else if (result == 5 && (timeOfDay == "Evening"))
                {
                    Random randomObject = new Random();
                    int clearRandomNumber = randomObject.Next(0, 7);

                    string[] clearImages = new string[8];

                    clearImages[0] = "Assets/Images/Clear_Night.jpg";
                    clearImages[1] = "Assets/Images/Clear_Night2.jpg";
                    clearImages[2] = "Assets/Images/Clear_Night3.jpg";
                    clearImages[3] = "Assets/Images/Clear_Night4.jpg";
                    clearImages[4] = "Assets/Images/Clear_Night5.jpg";
                    clearImages[5] = "Assets/Images/Clear_Night6.jpg";
                    clearImages[6] = "Assets/Images/Clear_Night7.jpg";
                    clearImages[7] = "Assets/Images/Clear_Night8.jpg";


                    string selectedIndexString = clearImages[clearRandomNumber];

                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Moon.png"));
                }

                //Hail
                else if (result == 6 && (timeOfDay == "Morning" || timeOfDay == "Afternoon"))
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Hail.jpg"));
                }
                else if (result == 6 && (timeOfDay == "Evening"))
                {
                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Hail.jpg"));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail_Moon.png"));
                }

                //Fog
                else if (result == 7 && (timeOfDay == "Morning" || timeOfDay == "Afternoon"))
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Fog.jpg"));
                }
                else if (result == 7 && (timeOfDay == "Evening"))
                {
                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Fog.jpg"));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog_Moon.png"));
                }

                //Sand
                else if (result == 8)
                {
                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Sand.jpg"));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result == 9)
                {
                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Dust.jpg"));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

                //Squalls
                else if (result == 10)
                {
                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Squalls.jpg"));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result == 11)
                {
                    weatherViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Funnel_Cloud.jpg"));
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion set appropriate background image

                #region Use Appropriate Temp Unit
                if (tempUnitB == true)
                {
                    var temp_c = obj["current_observation"]["temp_c"];
                    num.Text = temp_c.ToString() + "°";

                    var dewpoint_c = obj["current_observation"]["dewpoint_c"];
                    dewpointData.Text = dewpoint_c.ToString() + "°";

                    var feelslike_c = obj["current_observation"]["feelslike_c"];
                    feelsLikeData.Text = feelslike_c.ToString() + "°";

                }
                else
                {
                    var temp_f = obj["current_observation"]["temp_f"];
                    num.Text = temp_f.ToString() + "°";

                    var dewpoint_f = obj["current_observation"]["dewpoint_f"];
                    dewpointData.Text = dewpoint_f.ToString() + "°";

                    var feelslike_f = obj["current_observation"]["feelslike_f"];
                    feelsLikeData.Text = feelslike_f.ToString() + "°";
                }
                #endregion Use Appropriate Temp Unit

                #region Use Appropriate Distance Unit
                if (distUnitB == true)
                {
                    var wind_mph = obj["current_observation"]["wind_mph"];
                    windSpeedData.Text = wind_mph.ToString() + " mph";

                    var wind_gust_mph = obj["current_observation"]["wind_gust_mph"];
                    windGustData.Text = wind_gust_mph.ToString() + " mph";

                    var pressure_in = obj["current_observation"]["pressure_in"];
                    pressureData.Text = pressure_in.ToString() + " in";

                    var visibility_mi = obj["current_observation"]["visibility_mi"];
                    visibilityData.Text = visibility_mi.ToString() + " mi";

                    var precip_1hr_in = obj["current_observation"]["precip_1hr_in"];
                    if (precip_1hr_in.ToString() == "-9999.00")
                    {
                        precip1hrData.Text = "0.0 in";
                    }
                    else
                    {
                        precip1hrData.Text = precip_1hr_in.ToString() + " in";
                    }

                    var precip_today_in = obj["current_observation"]["precip_today_in"];
                    precipTodayData.Text = precip_today_in.ToString() + " in";
                }
                else
                {
                    var wind_kph = obj["current_observation"]["wind_kph"];
                    windSpeedData.Text = wind_kph.ToString() + " kph";

                    var wind_gust_kph = obj["current_observation"]["wind_gust_kph"];
                    windGustData.Text = wind_gust_kph.ToString() + " kph";

                    var pressure_mb = obj["current_observation"]["pressure_mb"];
                    pressureData.Text = pressure_mb.ToString() + " mb";

                    var visibility_km = obj["current_observation"]["visibility_km"];
                    visibilityData.Text = visibility_km.ToString() + " km";

                    var precip_1hr_metric = obj["current_observation"]["precip_1hr_metric"];
                    if (precip_1hr_metric.ToString() == "-9999.00")
                    {
                        precip1hrData.Text = "0.0 m";
                    }
                    else
                    {
                        precip1hrData.Text = precip_1hr_metric.ToString() + " m";
                    }

                    var precip_today_metric = obj["current_observation"]["precip_today_metric"];
                    precipTodayData.Text = precip_today_metric.ToString() + " m";
                }
                #endregion Use Appropriate Distance Unit

                #region Humidity Info
                var relative_humidity = obj["current_observation"]["relative_humidity"];
                string relHumid = relative_humidity.ToString();
                if (relHumid == "N/A%")
                {
                    humidityData.Text = "0%";
                }
                else
                {
                    humidityData.Text = relative_humidity.ToString();
                }
                #endregion Humidity Info

                #region Wind Direction Info
                var wind_dir = obj["current_observation"]["wind_dir"];
                string windDir = wind_dir.ToString();

                windDirData.Text = windDir;

                #endregion Wind Direction Info

                #region Heat and UV Info
                var uv = obj["current_observation"]["UV"];
                uvData.Text = uv.ToString();
                #endregion Heat and UV Info
            }
            catch
            {
                showGenericAlert();
            }
        }

        #endregion Fetch Weather

        #region Classify Weather
        private int classifyWeather(string weatherDescription)
        {
            string stringToCheck = weatherDescription;
            int ret = 0;

            if (stringToCheck.Contains("Rain") || stringToCheck.Contains("Overcast") || stringToCheck.Contains("Drizzle"))
            {
                ret = (int)Codes.Rain;
                greeting.Text = "Umbrella time!";
            }
            else if (stringToCheck.Contains("Thunderstorms") || stringToCheck.Contains("Thundertorm"))
            {
                ret = (int)Codes.Thunderstorm;
                greeting.Text = "Seek shelter";
            }
            else if (stringToCheck.Contains("Snow"))
            {
                ret = (int)Codes.Snow;
                greeting.Text = "Bring those shovels!";
            }
            else if (stringToCheck.Contains("Hail") || stringToCheck.Contains("Ice"))
            {
                ret = (int)Codes.Hail;
                greeting.Text = "Avoid the outdoors";
            }
            else if (stringToCheck.Contains("Fog") || stringToCheck.Contains("Mist") || stringToCheck.Contains("Haze"))
            {
                ret = (int)Codes.Fog;
                greeting.Text = "Safe Driving";
            }
            else if (stringToCheck.Contains("Sand"))
            {
                ret = (int)Codes.Sand;
                greeting.Text = "Stay indoors";
            }
            else if (stringToCheck.Contains("Dust") || stringToCheck.Contains("Smoke"))
            {
                ret = (int)Codes.Dust;
                greeting.Text = "Stay indoors";
            }
            else if (stringToCheck.Contains("Cloudy") || stringToCheck.Contains("Clouds"))
            {
                ret = (int)Codes.Cloudy;
                greeting.Text = "Enjoy the outdoors!";
            }
            else if (stringToCheck.Contains("Squalls"))
            {
                ret = (int)Codes.Sqalls;
                greeting.Text = "Stay indoors";
            }
            else if (stringToCheck.Contains("Funnel"))
            {
                ret = (int)Codes.Funnel;
                greeting.Text = "Stay safe";
            }
            else if (stringToCheck.Contains("Clear"))
            {
                ret = (int)Codes.Clear;
                greeting.Text = "Enjoy a gorgeous day!";
            }
            else
            {
                ret = 0;
            }
            return ret;
        }

        #endregion Classify Weather

        #region Show Menu
        private Rect GetRect(object sender)
        {
            FrameworkElement element = sender as FrameworkElement;
            GeneralTransform elementTransform = element.TransformToVisual(null);
            Point point = elementTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }


        private async void showMenu(object sender, RoutedEventArgs e)
        {
            PopupMenu p = new PopupMenu();
            p.Commands.Add(new Windows.UI.Popups.UICommand("Currently", (command) =>
            {
                this.Frame.Navigate(typeof(weatherView));
            }));
            p.Commands.Add(new Windows.UI.Popups.UICommand("12 Hour", (command) =>
            {
                this.Frame.Navigate(typeof(hourlyView));
            }));
            p.Commands.Add(new Windows.UI.Popups.UICommand("7 Day", (command) =>
            {
                this.Frame.Navigate(typeof(threeDayView));
            }));
            p.Commands.Add(new UICommandSeparator());
            p.Commands.Add(new Windows.UI.Popups.UICommand("Cover View", (command) =>
            {
                this.Frame.Navigate(typeof(MainPage));
            }));
            await p.ShowForSelectionAsync(GetRect(sender), Placement.Below);
        }
        #endregion Show Menu


        #region Show Generic Alert
        private async void showGenericAlert()
        {
            MessageDialog genericAlert = new MessageDialog("This bug is temporary, and will be fixed as soon as possible.\nWe advise you to click on \"Close\" below, then close the app, and restart it again from the Windows Start Screen.\n\nPlease email us regarding this issue, so we can fix it for the next update. Thanks.", "Oops! Something doesn't seem right!");
            //OK Button
            UICommand closeBtn = new UICommand("close");
            genericAlert.Commands.Add(closeBtn);

            //Show message
            await genericAlert.ShowAsync();
        }

        #endregion Show Generic Alert


        #region Navigate Back

        private new void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        #endregion Navigate Back

        private async void GoToWunderground(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.wunderground.com/?apiref=5aeed8d448b15bb8"));
        }

        private void locationManager(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(locations));
        }

        private void doManualRefresh(object sender, RoutedEventArgs e)
        {
            beginFetchLocation();
        }

    }
}
