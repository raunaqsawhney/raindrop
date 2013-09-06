using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace raindrop2
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class hourlyView : raindrop2.Common.LayoutAwarePage
    {
        string tempUnit = (string)ApplicationData.Current.LocalSettings.Values["TempUnit"];
        string distUnit = (string)ApplicationData.Current.LocalSettings.Values["DistUnit"];

        string latitude_ = (string)ApplicationData.Current.LocalSettings.Values["Latitude"];
        string longitude_ = (string)ApplicationData.Current.LocalSettings.Values["Longitude"];

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

        public hourlyView()
        {
            this.InitializeComponent();
            fetchHourlyWeather();
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void fetchHourlyWeather()
        {
            //Check if Celsius or Fahrenheit
            if (tempUnit == "Celsius")
                tempUnitB = true;
            else
                tempUnitB = false;

            //Check if Metric or Imperial
            if (distUnit == "Imperial")
                distUnitB = true;
            else
                distUnitB = false;

            try
            {
                cityName.Text = ApplicationData.Current.LocalSettings.Values["CITY"].ToString();

                #region get JSON response
                var client = new HttpClient();
                var response = await client.GetStringAsync(new Uri(string.Format("http://api.wunderground.com/api/cef78de98b1ad61f/hourly/q/{0},{1}.json", latitude_, longitude_)));
                var hourlyObj = JObject.Parse(response);
                #endregion get JSON response

                #region show 12 Hour Weather

                #region Hour 1
                string timeStamp = hourlyObj["hourly_forecast"][0]["FCTTIME"]["civil"].ToString();
                hourHead.Text = timeStamp + " (now)";

                string weather_1 = hourlyObj["hourly_forecast"][0]["condition"].ToString();
                desc.Text = weather_1;
                int result = checkIcon(weather_1);

                #region Icon + Weather

                //Rain
                if (result == 1)
                {
                    Random randomObject = new Random();
                    int rainRandomNumber = randomObject.Next(0, 5);

                    string[] rainImages = new string[6];

                    rainImages[0] = "Assets/Images/Rain_Day.jpg";
                    rainImages[1] = "Assets/Images/Rain_Day2.jpg";
                    rainImages[2] = "Assets/Images/Rain_Day3.jpg";
                    rainImages[3] = "Assets/Images/Rain_Night.jpg";
                    rainImages[4] = "Assets/Images/Rain_Night2.jpg";
                    rainImages[5] = "Assets/Images/Rain_Night3.jpg";

                    string selectedIndexString = rainImages[rainRandomNumber];

                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                    hourlyViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
                }

                //Thunderstorm
                else if (result == 2)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                    Random randomObject = new Random();
                    int thunderstormRandomNumber = randomObject.Next(0, 5);

                    string[] thunderstormImages = new string[6];

                    thunderstormImages[0] = "Assets/Images/Thunderstorm_Day.jpg";
                    thunderstormImages[1] = "Assets/Images/Thunderstorm_Day2.jpg";
                    thunderstormImages[2] = "Assets/Images/Thunderstorm_Day3.jpg";
                    thunderstormImages[0] = "Assets/Images/Thunderstorm_Night.jpg";
                    thunderstormImages[1] = "Assets/Images/Thunderstorm_Night2.jpg";
                    thunderstormImages[2] = "Assets/Images/Thunderstorm_Night3.jpg";

                    string selectedIndexString = thunderstormImages[thunderstormRandomNumber];

                    hourlyViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
                }

                //Snow
                else if (result == 3)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                    hourlyViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Snow_Day.jpg"));

                }

                //Cloudy
                else if (result == 4)
                {
                    Random randomObject = new Random();
                    int cloudyRandomNumber = randomObject.Next(0, 17);

                    string[] cloudyImages = new string[18];

                    cloudyImages[0] = "Assets/Images/Cloudy_Day.jpg";
                    cloudyImages[1] = "Assets/Images/Cloudy_Day2.jpg";
                    cloudyImages[2] = "Assets/Images/Cloudy_Day3.jpg";
                    cloudyImages[3] = "Assets/Images/Cloudy_Day4.jpg";
                    cloudyImages[4] = "Assets/Images/Cloudy_Day5.jpg";
                    cloudyImages[5] = "Assets/Images/Cloudy_Day6.jpg";
                    cloudyImages[6] = "Assets/Images/Cloudy_Day7.jpg";
                    cloudyImages[7] = "Assets/Images/Cloudy_Day8.jpg";
                    cloudyImages[8] = "Assets/Images/Cloudy_Day9.jpg";
                    cloudyImages[9] = "Assets/Images/Cloudy_Night.jpg";
                    cloudyImages[10] = "Assets/Images/Cloudy_Night2.jpg";
                    cloudyImages[11] = "Assets/Images/Cloudy_Night3.jpg";
                    cloudyImages[12] = "Assets/Images/Cloudy_Night4.jpg";
                    cloudyImages[13] = "Assets/Images/Cloudy_Night5.jpg";
                    cloudyImages[14] = "Assets/Images/Cloudy_Night6.jpg";
                    cloudyImages[15] = "Assets/Images/Cloudy_Night7.jpg";
                    cloudyImages[16] = "Assets/Images/Cloudy_Night8.jpg";
                    cloudyImages[17] = "Assets/Images/Cloudy_Night9.jpg";

                    string selectedIndexString = cloudyImages[cloudyRandomNumber];

                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                    hourlyViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
                }

                //Clear
                else if (result == 5)
                {
                    Random randomObject = new Random();
                    int clearRandomNumber = randomObject.Next(0, 15);

                    string[] clearImages = new string[16];

                    clearImages[0] = "Assets/Images/Clear_Day.jpg";
                    clearImages[1] = "Assets/Images/Clear_Day2.jpg";
                    clearImages[2] = "Assets/Images/Clear_Day3.jpg";
                    clearImages[3] = "Assets/Images/Clear_Day4.jpg";
                    clearImages[4] = "Assets/Images/Clear_Day5.jpg";
                    clearImages[5] = "Assets/Images/Clear_Day6.jpg";
                    clearImages[6] = "Assets/Images/Clear_Day7.jpg";
                    clearImages[7] = "Assets/Images/Clear_Day8.jpg";
                    clearImages[8] = "Assets/Images/Clear_Night.jpg";
                    clearImages[9] = "Assets/Images/Clear_Night2.jpg";
                    clearImages[10] = "Assets/Images/Clear_Night3.jpg";
                    clearImages[11] = "Assets/Images/Clear_Night4.jpg";
                    clearImages[12] = "Assets/Images/Clear_Night5.jpg";
                    clearImages[13] = "Assets/Images/Clear_Night6.jpg";
                    clearImages[14] = "Assets/Images/Clear_Night7.jpg";
                    clearImages[15] = "Assets/Images/Clear_Night8.jpg";

                    string selectedIndexString = clearImages[clearRandomNumber];

                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                    hourlyViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
                }

                //Hail
                else if (result == 6)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                    hourlyViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Hail.jpg"));
                }

                //Fog
                else if (result == 7)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                    hourlyViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Fog.jpg"));
                }

                //Sand
                else if (result == 8)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                    hourlyViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Sand.jpg"));
                }

                //Dust
                else if (result == 9)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                    hourlyViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Dust.jpg"));

                }

                //Squalls
                else if (result == 10)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                    hourlyViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Squalls.jpg"));

                }

                //Funnel Cloud
                else if (result == 11)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                    hourlyViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Funnel_Cloud.jpg"));

                }

                #endregion Icon + Weather

                string temp_1_C = hourlyObj["hourly_forecast"][0]["temp"]["metric"].ToString();
                string temp_1_F = hourlyObj["hourly_forecast"][0]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp.Text = temp_1_C + "°";
                else
                    temp.Text = temp_1_F + "°";

                #region Rain
                string pop_1 = hourlyObj["hourly_forecast"][0]["pop"].ToString() + "%";
                rainData.Text = pop_1;
                #endregion Rain

                #region Wind
                string wind_Speed_I_1 = hourlyObj["hourly_forecast"][0]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_1 = hourlyObj["hourly_forecast"][0]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData.Text = wind_Speed_I_1;
                }
                else
                {
                    windData.Text = wind_Speed_M_1;
                }
                #endregion Wind

                #region Snow
                string snow_I_1 = hourlyObj["hourly_forecast"][0]["snow"]["english"].ToString();
                string snow_M_1 = hourlyObj["hourly_forecast"][0]["snow"]["metric"].ToString();

                if (snow_I_1 == "" || snow_M_1 == "")
                {
                    snowData.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData.Text = snow_I_1 + " in";
                    }
                    else
                    {
                        snowData.Text = snow_M_1 + " m";
                    }
                }
                #endregion Snow

                #endregion Hour 1

                #region Hour 2
                string timeStamp2 = hourlyObj["hourly_forecast"][1]["FCTTIME"]["civil"].ToString();
                hourHead2.Text = timeStamp2;

                string weather_2 = hourlyObj["hourly_forecast"][1]["condition"].ToString();
                desc2.Text = weather_2;
                int result2 = checkIcon(weather_1);

                #region Icon Deciding
                //Rain
                if (result2 == 1)
                {
                    weatherIcon2.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }

                //Thunderstorm
                else if (result2 == 2)
                {
                    weatherIcon2.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }

                //Snow
                else if (result2 == 3)
                {
                    weatherIcon2.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Cloudy
                else if (result2 == 4)
                {
                    weatherIcon2.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result2 == 5)
                {
                    weatherIcon2.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Hail
                else if (result2 == 6)
                {
                    weatherIcon2.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                }

                //Fog
                else if (result2 == 7)
                {
                    weatherIcon2.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                }

                //Sand
                else if (result2 == 8)
                {
                    weatherIcon2.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result2 == 9)
                {
                    weatherIcon2.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

                //Squalls
                else if (result2 == 10)
                {
                    weatherIcon2.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result2 == 11)
                {
                    weatherIcon2.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion Icon Deciding

                string temp_2_C = hourlyObj["hourly_forecast"][1]["temp"]["metric"].ToString();
                string temp_2_F = hourlyObj["hourly_forecast"][1]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp2.Text = temp_2_C + "°";
                else
                    temp2.Text = temp_2_F + "°";

                #region Rain
                string pop_2 = hourlyObj["hourly_forecast"][1]["pop"].ToString() + "%";
                rainData2.Text = pop_2;
                #endregion Rain

                #region Wind
                string wind_Speed_I_2 = hourlyObj["hourly_forecast"][1]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_2 = hourlyObj["hourly_forecast"][1]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData2.Text = wind_Speed_I_2;
                }
                else
                {
                    windData2.Text = wind_Speed_M_2;
                }
                #endregion Wind

                #region Snow
                string snow_I_2 = hourlyObj["hourly_forecast"][1]["snow"]["english"].ToString();
                string snow_M_2 = hourlyObj["hourly_forecast"][1]["snow"]["metric"].ToString();

                if (snow_I_2 == "" || snow_M_2 == "")
                {
                    snowData2.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData2.Text = snow_I_2 + " in";
                    }
                    else
                    {
                        snowData2.Text = snow_M_2 + " m";
                    }
                }

                #endregion Snow

                #endregion Hour 2

                #region Hour 3
                string timeStamp3 = hourlyObj["hourly_forecast"][2]["FCTTIME"]["civil"].ToString();
                hourHead3.Text = timeStamp3;

                string weather_3 = hourlyObj["hourly_forecast"][2]["condition"].ToString();
                desc3.Text = weather_3;

                int result3 = checkIcon(weather_3);

                #region Icon Deciding
                //Rain
                if (result3 == 1)
                {
                    weatherIcon3.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }

                //Thunderstorm
                else if (result3 == 2)
                {
                    weatherIcon3.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }

                //Snow
                else if (result3 == 3)
                {
                    weatherIcon3.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Cloudy
                else if (result3 == 4)
                {
                    weatherIcon3.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result3 == 5)
                {
                    weatherIcon3.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Hail
                else if (result3 == 6)
                {
                    weatherIcon3.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                }

                //Fog
                else if (result3 == 7)
                {
                    weatherIcon3.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                }

                //Sand
                else if (result3 == 8)
                {
                    weatherIcon3.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result3 == 9)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

                //Squalls
                else if (result3 == 10)
                {
                    weatherIcon3.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result3 == 11)
                {
                    weatherIcon3.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion Icon Deciding

                string temp_3_C = hourlyObj["hourly_forecast"][2]["temp"]["metric"].ToString();
                string temp_3_F = hourlyObj["hourly_forecast"][2]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp3.Text = temp_3_C + "°";
                else
                    temp3.Text = temp_3_F + "°";

                #region Rain
                string pop_3 = hourlyObj["hourly_forecast"][2]["pop"].ToString() + "%";
                rainData3.Text = pop_3;
                #endregion Rain

                #region Wind
                string wind_Speed_I_3 = hourlyObj["hourly_forecast"][2]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_3 = hourlyObj["hourly_forecast"][2]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData3.Text = wind_Speed_I_3;
                }
                else
                {
                    windData3.Text = wind_Speed_M_3;
                }
                #endregion Wind

                #region Snow
                string snow_I_3 = hourlyObj["hourly_forecast"][2]["snow"]["english"].ToString();
                string snow_M_3 = hourlyObj["hourly_forecast"][2]["snow"]["metric"].ToString();

                if (snow_I_3 == "" || snow_M_3 == "")
                {
                    snowData3.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData3.Text = snow_I_3 + " in";
                    }
                    else
                    {
                        snowData3.Text = snow_M_3 + " m";
                    }
                }
                #endregion Snow

                #endregion Hour 3

                #region Hour 4
                string timeStamp4 = hourlyObj["hourly_forecast"][3]["FCTTIME"]["civil"].ToString();
                hourHead4.Text = timeStamp4;

                string weather_4 = hourlyObj["hourly_forecast"][3]["condition"].ToString();
                desc4.Text = weather_4;

                int result4 = checkIcon(weather_4);

                #region Icon Deciding
                //Rain
                if (result4 == 1)
                {
                    weatherIcon4.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }

                //Thunderstorm
                else if (result4 == 2)
                {
                    weatherIcon4.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }

                //Snow
                else if (result4 == 3)
                {
                    weatherIcon4.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Cloudy
                else if (result4 == 4)
                {
                    weatherIcon4.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result4 == 5)
                {
                    weatherIcon4.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Hail
                else if (result4 == 6)
                {
                    weatherIcon4.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                }

                //Fog
                else if (result4 == 7)
                {
                    weatherIcon4.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                }

                //Sand
                else if (result4 == 8)
                {
                    weatherIcon4.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result4 == 9)
                {
                    weatherIcon4.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

                //Squalls
                else if (result4 == 10)
                {
                    weatherIcon4.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result4 == 11)
                {
                    weatherIcon4.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion Icon Deciding
                string temp_4_C = hourlyObj["hourly_forecast"][3]["temp"]["metric"].ToString();
                string temp_4_F = hourlyObj["hourly_forecast"][3]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp4.Text = temp_4_C + "°";
                else
                    temp4.Text = temp_4_F + "°";

                #region Rain
                string pop_4 = hourlyObj["hourly_forecast"][3]["pop"].ToString() + "%";
                rainData4.Text = pop_4;
                #endregion Rain

                #region Wind
                string wind_Speed_I_4 = hourlyObj["hourly_forecast"][3]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_4 = hourlyObj["hourly_forecast"][3]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData4.Text = wind_Speed_I_4;
                }
                else
                {
                    windData4.Text = wind_Speed_M_4;
                }
                #endregion Wind

                #region Snow
                string snow_I_4 = hourlyObj["hourly_forecast"][3]["snow"]["english"].ToString();
                string snow_M_4 = hourlyObj["hourly_forecast"][3]["snow"]["metric"].ToString();

                if (snow_I_4 == "" || snow_M_4 == "")
                {
                    snowData4.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData4.Text = snow_I_4 + " in";
                    }
                    else
                    {
                        snowData4.Text = snow_M_4 + " m";
                    }
                }
                #endregion Snow

                #endregion Hour 4

                #region Hour 5
                string timeStamp5 = hourlyObj["hourly_forecast"][4]["FCTTIME"]["civil"].ToString();
                hourHead5.Text = timeStamp5;

                string weather_5 = hourlyObj["hourly_forecast"][4]["condition"].ToString();
                desc5.Text = weather_5;
                int result5 = checkIcon(weather_5);

                #region Icon Deciding
                //Rain
                if (result5 == 1)
                {
                    weatherIcon5.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }

                //Thunderstorm
                else if (result5 == 2)
                {
                    weatherIcon5.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }

                //Snow
                else if (result5 == 3)
                {
                    weatherIcon5.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Cloudy
                else if (result5 == 4)
                {
                    weatherIcon5.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result5 == 5)
                {
                    weatherIcon5.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Hail
                else if (result5 == 6)
                {
                    weatherIcon5.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                }

                //Fog
                else if (result5 == 7)
                {
                    weatherIcon5.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                }

                //Sand
                else if (result5 == 8)
                {
                    weatherIcon5.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result5 == 9)
                {
                    weatherIcon5.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

                //Squalls
                else if (result5 == 10)
                {
                    weatherIcon5.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result5 == 11)
                {
                    weatherIcon5.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion Icon Deciding


                string temp_5_C = hourlyObj["hourly_forecast"][4]["temp"]["metric"].ToString();
                string temp_5_F = hourlyObj["hourly_forecast"][4]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp5.Text = temp_5_C + "°";
                else
                    temp5.Text = temp_5_F + "°";

                #region Rain
                string pop_5 = hourlyObj["hourly_forecast"][4]["pop"].ToString() + "%";
                rainData5.Text = pop_5;
                #endregion Rain

                #region Wind
                string wind_Speed_I_5 = hourlyObj["hourly_forecast"][4]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_5 = hourlyObj["hourly_forecast"][4]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData5.Text = wind_Speed_I_5;
                }
                else
                {
                    windData5.Text = wind_Speed_M_5;
                }
                #endregion Wind

                #region Snow
                string snow_I_5 = hourlyObj["hourly_forecast"][4]["snow"]["english"].ToString();
                string snow_M_5 = hourlyObj["hourly_forecast"][4]["snow"]["metric"].ToString();

                if (snow_I_5 == "" || snow_M_5 == "")
                {
                    snowData5.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData5.Text = snow_I_5 + " in";
                    }
                    else
                    {
                        snowData5.Text = snow_M_5 + " m";
                    }
                }
                #endregion Snow

                #endregion Hour 5

                #region Hour 6
                string timeStamp6 = hourlyObj["hourly_forecast"][5]["FCTTIME"]["civil"].ToString();
                hourHead6.Text = timeStamp6;

                string weather_6 = hourlyObj["hourly_forecast"][5]["condition"].ToString();
                desc6.Text = weather_6;

                int result6 = checkIcon(weather_6);

                #region Icon Deciding
                //Rain
                if (result6 == 1)
                {
                    weatherIcon6.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }

                //Thunderstorm
                else if (result6 == 2)
                {
                    weatherIcon6.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }

                //Snow
                else if (result6 == 3)
                {
                    weatherIcon6.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Cloudy
                else if (result6 == 4)
                {
                    weatherIcon6.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result6 == 5)
                {
                    weatherIcon6.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Hail
                else if (result6 == 6)
                {
                    weatherIcon6.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                }

                //Fog
                else if (result6 == 7)
                {
                    weatherIcon6.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                }

                //Sand
                else if (result6 == 8)
                {
                    weatherIcon6.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result6 == 9)
                {
                    weatherIcon6.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

               //Squalls
                else if (result6 == 10)
                {
                    weatherIcon6.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result6 == 11)
                {
                    weatherIcon6.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion Icon Deciding

                string temp_6_C = hourlyObj["hourly_forecast"][5]["temp"]["metric"].ToString();
                string temp_6_F = hourlyObj["hourly_forecast"][5]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp6.Text = temp_6_C + "°";
                else
                    temp6.Text = temp_6_F + "°";

                #region Rain
                string pop_6 = hourlyObj["hourly_forecast"][5]["pop"].ToString() + "%";
                rainData6.Text = pop_6;
                #endregion Rain

                #region Wind
                string wind_Speed_I_6 = hourlyObj["hourly_forecast"][5]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_6 = hourlyObj["hourly_forecast"][5]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData6.Text = wind_Speed_I_6;
                }
                else
                {
                    windData6.Text = wind_Speed_M_6;
                }
                #endregion Wind

                #region Snow
                string snow_I_6 = hourlyObj["hourly_forecast"][5]["snow"]["english"].ToString();
                string snow_M_6 = hourlyObj["hourly_forecast"][5]["snow"]["metric"].ToString();

                if (snow_I_6 == "" || snow_M_6 == "")
                {
                    snowData6.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData6.Text = snow_I_6 + " in";
                    }
                    else
                    {
                        snowData6.Text = snow_M_6 + " m";
                    }
                }
                #endregion Snow

                #endregion Hour 6

                #region Hour 7
                string timeStamp7 = hourlyObj["hourly_forecast"][6]["FCTTIME"]["civil"].ToString();
                hourHead7.Text = timeStamp7;

                string weather_7 = hourlyObj["hourly_forecast"][6]["condition"].ToString();
                desc7.Text = weather_7;

                int result7 = checkIcon(weather_7);

                #region Icon Deciding
                //Rain
                if (result7 == 1)
                {
                    weatherIcon7.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }

                //Thunderstorm
                else if (result7 == 2)
                {
                    weatherIcon7.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }

                //Snow
                else if (result7 == 3)
                {
                    weatherIcon7.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Cloudy
                else if (result7 == 4)
                {
                    weatherIcon7.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result7 == 5)
                {
                    weatherIcon7.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Hail
                else if (result7 == 6)
                {
                    weatherIcon7.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                }

                //Fog
                else if (result7 == 7)
                {
                    weatherIcon7.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                }

                //Sand
                else if (result7 == 8)
                {
                    weatherIcon7.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result7 == 9)
                {
                    weatherIcon7.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

               //Squalls
                else if (result7 == 10)
                {
                    weatherIcon7.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result6 == 11)
                {
                    weatherIcon6.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion Icon Deciding

                string temp_7_C = hourlyObj["hourly_forecast"][6]["temp"]["metric"].ToString();
                string temp_7_F = hourlyObj["hourly_forecast"][6]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp6.Text = temp_7_C + "°";
                else
                    temp7.Text = temp_7_F + "°";

                #region Rain
                string pop_7 = hourlyObj["hourly_forecast"][6]["pop"].ToString() + "%";
                rainData7.Text = pop_7;
                #endregion Rain

                #region Wind
                string wind_Speed_I_7 = hourlyObj["hourly_forecast"][6]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_7 = hourlyObj["hourly_forecast"][6]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData7.Text = wind_Speed_I_7;
                }
                else
                {
                    windData7.Text = wind_Speed_M_7;
                }
                #endregion Wind

                #region Snow
                string snow_I_7 = hourlyObj["hourly_forecast"][6]["snow"]["english"].ToString();
                string snow_M_7 = hourlyObj["hourly_forecast"][6]["snow"]["metric"].ToString();

                if (snow_I_7 == "" || snow_M_7 == "")
                {
                    snowData7.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData7.Text = snow_I_7 + " in";
                    }
                    else
                    {
                        snowData7.Text = snow_M_7 + " m";
                    }
                }
                #endregion Snow

                #endregion Hour 7

                #region Hour 8
                string timeStamp8 = hourlyObj["hourly_forecast"][7]["FCTTIME"]["civil"].ToString();
                hourHead8.Text = timeStamp8;

                string weather_8 = hourlyObj["hourly_forecast"][7]["condition"].ToString();
                desc8.Text = weather_8;

                int result8 = checkIcon(weather_6);

                #region Icon Deciding
                //Rain
                if (result8 == 1)
                {
                    weatherIcon8.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }

                //Thunderstorm
                else if (result8 == 2)
                {
                    weatherIcon8.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }

                //Snow
                else if (result8 == 3)
                {
                    weatherIcon8.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Cloudy
                else if (result8 == 4)
                {
                    weatherIcon8.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result8 == 5)
                {
                    weatherIcon8.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Hail
                else if (result8 == 6)
                {
                    weatherIcon8.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                }

                //Fog
                else if (result8 == 7)
                {
                    weatherIcon8.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                }

                //Sand
                else if (result8 == 8)
                {
                    weatherIcon8.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result8 == 9)
                {
                    weatherIcon8.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

               //Squalls
                else if (result8 == 10)
                {
                    weatherIcon8.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result8 == 11)
                {
                    weatherIcon8.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion Icon Deciding

                string temp_8_C = hourlyObj["hourly_forecast"][7]["temp"]["metric"].ToString();
                string temp_8_F = hourlyObj["hourly_forecast"][7]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp8.Text = temp_8_C + "°";
                else
                    temp8.Text = temp_8_F + "°";

                #region Rain
                string pop_8= hourlyObj["hourly_forecast"][7]["pop"].ToString() + "%";
                rainData8.Text = pop_8;
                #endregion Rain

                #region Wind
                string wind_Speed_I_8 = hourlyObj["hourly_forecast"][7]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_8 = hourlyObj["hourly_forecast"][7]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData8.Text = wind_Speed_I_8;
                }
                else
                {
                    windData8.Text = wind_Speed_M_8;
                }
                #endregion Wind

                #region Snow
                string snow_I_8 = hourlyObj["hourly_forecast"][7]["snow"]["english"].ToString();
                string snow_M_8 = hourlyObj["hourly_forecast"][7]["snow"]["metric"].ToString();

                if (snow_I_8 == "" || snow_M_8 == "")
                {
                    snowData8.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData8.Text = snow_I_8 + " in";
                    }
                    else
                    {
                        snowData8.Text = snow_M_8 + " m";
                    }
                }
                #endregion Snow

                #endregion Hour 8

                #region Hour 9
                string timeStamp9 = hourlyObj["hourly_forecast"][8]["FCTTIME"]["civil"].ToString();
                hourHead9.Text = timeStamp9;

                string weather_9 = hourlyObj["hourly_forecast"][8]["condition"].ToString();
                desc9.Text = weather_9;

                int result9 = checkIcon(weather_9);

                #region Icon Deciding
                //Rain
                if (result9 == 1)
                {
                    weatherIcon9.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }

                //Thunderstorm
                else if (result9 == 2)
                {
                    weatherIcon9.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }

                //Snow
                else if (result9 == 3)
                {
                    weatherIcon9.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Cloudy
                else if (result9 == 4)
                {
                    weatherIcon9.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result9 == 5)
                {
                    weatherIcon9.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Hail
                else if (result9 == 6)
                {
                    weatherIcon9.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                }

                //Fog
                else if (result9 == 7)
                {
                    weatherIcon9.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                }

                //Sand
                else if (result9 == 8)
                {
                    weatherIcon9.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result9 == 9)
                {
                    weatherIcon9.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

               //Squalls
                else if (result9 == 10)
                {
                    weatherIcon9.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result9 == 11)
                {
                    weatherIcon9.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion Icon Deciding

                string temp_9_C = hourlyObj["hourly_forecast"][8]["temp"]["metric"].ToString();
                string temp_9_F = hourlyObj["hourly_forecast"][8]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp9.Text = temp_9_C + "°";
                else
                    temp9.Text = temp_9_F + "°";

                #region Rain
                string pop_9 = hourlyObj["hourly_forecast"][8]["pop"].ToString() + "%";
                rainData9.Text = pop_9;
                #endregion Rain

                #region Wind
                string wind_Speed_I_9 = hourlyObj["hourly_forecast"][8]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_9 = hourlyObj["hourly_forecast"][8]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData9.Text = wind_Speed_I_9;
                }
                else
                {
                    windData9.Text = wind_Speed_M_9;
                }
                #endregion Wind

                #region Snow
                string snow_I_9 = hourlyObj["hourly_forecast"][8]["snow"]["english"].ToString();
                string snow_M_9 = hourlyObj["hourly_forecast"][8]["snow"]["metric"].ToString();

                if (snow_I_9 == "" || snow_M_9 == "")
                {
                    snowData9.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData9.Text = snow_I_9 + " in";
                    }
                    else
                    {
                        snowData9.Text = snow_M_9 + " m";
                    }
                }
                #endregion Snow

                #endregion Hour 9

                #region Hour 10
                string timeStamp10 = hourlyObj["hourly_forecast"][9]["FCTTIME"]["civil"].ToString();
                hourHead10.Text = timeStamp10;

                string weather_10 = hourlyObj["hourly_forecast"][9]["condition"].ToString();
                desc10.Text = weather_10;

                int result10 = checkIcon(weather_10);

                #region Icon Deciding
                //Rain
                if (result10 == 1)
                {
                    weatherIcon10.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }

                //Thunderstorm
                else if (result10 == 2)
                {
                    weatherIcon10.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }

                //Snow
                else if (result10 == 3)
                {
                    weatherIcon10.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Cloudy
                else if (result10 == 4)
                {
                    weatherIcon10.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result10 == 5)
                {
                    weatherIcon10.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Hail
                else if (result10 == 6)
                {
                    weatherIcon10.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                }

                //Fog
                else if (result10 == 7)
                {
                    weatherIcon10.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                }

                //Sand
                else if (result10 == 8)
                {
                    weatherIcon10.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result10 == 9)
                {
                    weatherIcon10.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

               //Squalls
                else if (result10 == 10)
                {
                    weatherIcon10.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result10 == 11)
                {
                    weatherIcon10.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion Icon Deciding

                string temp_10_C = hourlyObj["hourly_forecast"][9]["temp"]["metric"].ToString();
                string temp_10_F = hourlyObj["hourly_forecast"][9]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp10.Text = temp_10_C + "°";
                else
                    temp10.Text = temp_10_F + "°";

                #region Rain
                string pop_10 = hourlyObj["hourly_forecast"][9]["pop"].ToString() + "%";
                rainData10.Text = pop_10;
                #endregion Rain

                #region Wind
                string wind_Speed_I_10 = hourlyObj["hourly_forecast"][9]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_10 = hourlyObj["hourly_forecast"][9]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData10.Text = wind_Speed_I_10;
                }
                else
                {
                    windData10.Text = wind_Speed_M_10;
                }
                #endregion Wind

                #region Snow
                string snow_I_10 = hourlyObj["hourly_forecast"][9]["snow"]["english"].ToString();
                string snow_M_10 = hourlyObj["hourly_forecast"][9]["snow"]["metric"].ToString();

                if (snow_I_10 == "" || snow_M_10 == "")
                {
                    snowData10.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData10.Text = snow_I_10 + " in";
                    }
                    else
                    {
                        snowData10.Text = snow_M_10 + " m";
                    }
                }
                #endregion Snow

                #endregion Hour 10

                #region Hour 11
                string timeStamp11 = hourlyObj["hourly_forecast"][10]["FCTTIME"]["civil"].ToString();
                hourHead11.Text = timeStamp11;

                string weather_11 = hourlyObj["hourly_forecast"][10]["condition"].ToString();
                desc11.Text = weather_11;

                int result11 = checkIcon(weather_11);

                #region Icon Deciding
                //Rain
                if (result11 == 1)
                {
                    weatherIcon11.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }

                //Thunderstorm
                else if (result11 == 2)
                {
                    weatherIcon11.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }

                //Snow
                else if (result11 == 3)
                {
                    weatherIcon11.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Cloudy
                else if (result11 == 4)
                {
                    weatherIcon11.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result11 == 5)
                {
                    weatherIcon11.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Hail
                else if (result11 == 6)
                {
                    weatherIcon11.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                }

                //Fog
                else if (result11 == 7)
                {
                    weatherIcon11.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                }

                //Sand
                else if (result11 == 8)
                {
                    weatherIcon11.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result11 == 9)
                {
                    weatherIcon11.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

               //Squalls
                else if (result11 == 10)
                {
                    weatherIcon11.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result11 == 11)
                {
                    weatherIcon11.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion Icon Deciding

                string temp_11_C = hourlyObj["hourly_forecast"][10]["temp"]["metric"].ToString();
                string temp_11_F = hourlyObj["hourly_forecast"][10]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp11.Text = temp_11_C + "°";
                else
                    temp11.Text = temp_11_F + "°";

                #region Rain
                string pop_11 = hourlyObj["hourly_forecast"][10]["pop"].ToString() + "%";
                rainData11.Text = pop_11;
                #endregion Rain

                #region Wind
                string wind_Speed_I_11 = hourlyObj["hourly_forecast"][10]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_11 = hourlyObj["hourly_forecast"][10]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData11.Text = wind_Speed_I_11;
                }
                else
                {
                    windData11.Text = wind_Speed_M_11;
                }
                #endregion Wind

                #region Snow
                string snow_I_11 = hourlyObj["hourly_forecast"][10]["snow"]["english"].ToString();
                string snow_M_11 = hourlyObj["hourly_forecast"][10]["snow"]["metric"].ToString();

                if (snow_I_11 == "" || snow_M_11 == "")
                {
                    snowData11.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData11.Text = snow_I_11 + " in";
                    }
                    else
                    {
                        snowData11.Text = snow_M_11 + " m";
                    }
                }
                #endregion Snow

                #endregion Hour 11

                #region Hour 12
                string timeStamp12 = hourlyObj["hourly_forecast"][11]["FCTTIME"]["civil"].ToString();
                hourHead12.Text = timeStamp12;

                string weather_12 = hourlyObj["hourly_forecast"][11]["condition"].ToString();
                desc12.Text = weather_12;

                int result12 = checkIcon(weather_12);

                #region Icon Deciding
                //Rain
                if (result12 == 1)
                {
                    weatherIcon12.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }

                //Thunderstorm
                else if (result12 == 2)
                {
                    weatherIcon12.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }

                //Snow
                else if (result12 == 3)
                {
                    weatherIcon12.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Cloudy
                else if (result12 == 4)
                {
                    weatherIcon12.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result11 == 5)
                {
                    weatherIcon12.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Hail
                else if (result12 == 6)
                {
                    weatherIcon12.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                }

                //Fog
                else if (result12 == 7)
                {
                    weatherIcon12.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                }

                //Sand
                else if (result12 == 8)
                {
                    weatherIcon12.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result12 == 9)
                {
                    weatherIcon12.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

               //Squalls
                else if (result12 == 10)
                {
                    weatherIcon12.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result12 == 11)
                {
                    weatherIcon12.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion Icon Deciding

                string temp_12_C = hourlyObj["hourly_forecast"][11]["temp"]["metric"].ToString();
                string temp_12_F = hourlyObj["hourly_forecast"][11]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp12.Text = temp_12_C + "°";
                else
                    temp12.Text = temp_12_F + "°";

                #region Rain
                string pop_12 = hourlyObj["hourly_forecast"][11]["pop"].ToString() + "%";
                rainData12.Text = pop_12;
                #endregion Rain

                #region Wind
                string wind_Speed_I_12 = hourlyObj["hourly_forecast"][11]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_12 = hourlyObj["hourly_forecast"][11]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData12.Text = wind_Speed_I_12;
                }
                else
                {
                    windData12.Text = wind_Speed_M_12;
                }
                #endregion Wind

                #region Snow
                string snow_I_12 = hourlyObj["hourly_forecast"][11]["snow"]["english"].ToString();
                string snow_M_12 = hourlyObj["hourly_forecast"][11]["snow"]["metric"].ToString();

                if (snow_I_12 == "" || snow_M_12 == "")
                {
                    snowData12.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData12.Text = snow_I_12 + " in";
                    }
                    else
                    {
                        snowData12.Text = snow_M_12 + " m";
                    }
                }
                #endregion Snow

                #endregion Hour 12

                #region Hour 13
                string timeStamp13 = hourlyObj["hourly_forecast"][12]["FCTTIME"]["civil"].ToString();
                hourHead13.Text = timeStamp13;

                string weather_13 = hourlyObj["hourly_forecast"][12]["condition"].ToString();
                desc13.Text = weather_13;

                int result13 = checkIcon(weather_13);

                #region Icon Deciding
                //Rain
                if (result13 == 1)
                {
                    weatherIcon13.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Rain.png"));
                }

                //Thunderstorm
                else if (result13 == 2)
                {
                    weatherIcon13.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Lightning.png"));
                }

                //Snow
                else if (result13 == 3)
                {
                    weatherIcon13.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Cloudy
                else if (result13 == 4)
                {
                    weatherIcon13.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud.png"));
                }

                //Clear
                else if (result13 == 5)
                {
                    weatherIcon13.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Hail
                else if (result13 == 6)
                {
                    weatherIcon13.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                }

                //Fog
                else if (result13 == 7)
                {
                    weatherIcon13.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                }

                //Sand
                else if (result13 == 8)
                {
                    weatherIcon13.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                }

                //Dust
                else if (result13 == 9)
                {
                    weatherIcon13.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                }

               //Squalls
                else if (result13 == 10)
                {
                    weatherIcon13.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                }

                //Funnel Cloud
                else if (result13 == 11)
                {
                    weatherIcon13.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                }
                #endregion Icon Deciding

                string temp_13_C = hourlyObj["hourly_forecast"][12]["temp"]["metric"].ToString();
                string temp_13_F = hourlyObj["hourly_forecast"][12]["temp"]["english"].ToString();

                if (tempUnitB == true)
                    temp13.Text = temp_13_C + "°";
                else
                    temp13.Text = temp_13_F + "°";

                #region Rain
                string pop_13 = hourlyObj["hourly_forecast"][12]["pop"].ToString() + "%";
                rainData13.Text = pop_13;
                #endregion Rain

                #region Wind
                string wind_Speed_I_13 = hourlyObj["hourly_forecast"][12]["wspd"]["english"].ToString() + " mph";
                string wind_Speed_M_13 = hourlyObj["hourly_forecast"][12]["wspd"]["metric"].ToString() + " kph";


                if (distUnitB == true)
                {
                    windData13.Text = wind_Speed_I_13;
                }
                else
                {
                    windData13.Text = wind_Speed_M_13;
                }
                #endregion Wind

                #region Snow
                string snow_I_13 = hourlyObj["hourly_forecast"][12]["snow"]["english"].ToString();
                string snow_M_13 = hourlyObj["hourly_forecast"][12]["snow"]["metric"].ToString();

                if (snow_I_13 == "" || snow_M_13 == "")
                {
                    snowData13.Text = "N/A";
                }
                else
                {
                    if (distUnitB == true)
                    {
                        snowData13.Text = snow_I_13 + " in";
                    }
                    else
                    {
                        snowData13.Text = snow_M_13 + " m";
                    }
                }
                #endregion Snow

                #endregion Hour 13


                #endregion show Twelve Hour Weather
            }
            catch
            {
                showGenericAlert();
            }
        }

        private int checkIcon(string weather_input)
        {
            string stringToCheck = weather_input;
            int ret = 0;

            if (stringToCheck.Contains("Rain") || stringToCheck.Contains("Overcast") || stringToCheck.Contains("Drizzle"))
            {
                ret = (int)Codes.Rain;
            }
            else if (stringToCheck.Contains("Thunderstorms") || stringToCheck.Contains("Thunderstorm"))
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
            else if (stringToCheck.Contains("Cloudy"))
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

        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async void showGenericAlert()
        {
            MessageDialog genericAlert = new MessageDialog("This bug is temporary, and will be fixed as soon as possible.\nWe advise you to click on \"Close\" below, then close the app, and restart it again from the Windows Start Screen.\n\nPlease email us regarding this issue, so we can fix it for the next update. Thanks.", "Oops! Something doesn't seem right!");
            //OK Button
            UICommand closeBtn = new UICommand("close");
            genericAlert.Commands.Add(closeBtn);

            //Show message
            await genericAlert.ShowAsync();
        }

        private async void GoToWunderground(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.wunderground.com/?apiref=5aeed8d448b15bb8"));
        }

        private void locationManager(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(locations));
        }
    }
}
