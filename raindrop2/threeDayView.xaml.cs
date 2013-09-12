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
using System.Globalization;


namespace raindrop2
{
    public sealed partial class threeDayView : raindrop2.Common.LayoutAwarePage
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

        public threeDayView()
        {
            this.InitializeComponent();

            fetchThreeDayWeather();
        }

        private void Next_Data(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void fetchThreeDayWeather()
        {
            DateTime time = DateTime.Now;
            String s = time.ToString("hh:mm tt");

            int int_month = DateTime.Now.Month;
            string month = "";

            switch (int_month)
            {
                case 1:
                    month = "January";
                    break;
                case 2:
                    month = "February";
                    break;
                case 3:
                    month = "March";
                    break;
                case 4:
                    month = "April";
                    break;
                case 5:
                    month = "May";
                    break;
                case 6:
                    month = "June";
                    break;
                case 7:
                    month = "July";
                    break;
                case 8:
                    month = "August";
                    break;
                case 9:
                    month = "September";
                    break;
                case 10:
                    month = "October";
                    break;
                case 11:
                    month = "November";
                    break;
                case 12:
                    month = "December";
                    break;
            }

            string updatedDateTime = month + " " + DateTime.Now.Day + ", " + s.ToUpper();
            observation_time.Text = "Last Updated on " + updatedDateTime;

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
                cityName.Text = ApplicationData.Current.LocalSettings.Values["CITY"].ToString();
                
                #region get JSON response
                var client = new HttpClient();
                var response = await client.GetStringAsync(new Uri(string.Format("http://api.wunderground.com/api/cef78de98b1ad61f/forecast10day/q/{0},{1}.json", latitude_, longitude_)));
                var threeDayObj = JObject.Parse(response);

                #endregion get JSON response
                


                #region show Seven Day Weather

                #region Day 1

                #region Weekday
                string weekday_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["date"]["weekday"].ToString();
                DayHead.Text = weekday_1 + " (today)";
                #endregion Weekday

                #region High Low

                //High
                string tempH_C_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["high"]["celsius"].ToString();
                string tempH_F_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["high"]["fahrenheit"].ToString();

                #region Choose C or F for High Weather

                string temp_H_CorF = "";
                if (tempUnitB == true)
                {
                    temp_H_CorF = tempH_C_1.ToString() + "°";
                }
                else
                {
                    temp_H_CorF = tempH_F_1.ToString() + "°";
                }

                #endregion Choose C or F for High Weather

                //Low
                string tempL_C_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["low"]["celsius"].ToString();
                string tempL_F_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["low"]["fahrenheit"].ToString();

                #region Choose C or F for Low Weather

                string temp_L_CorF = "";
                if (tempUnitB == true)
                {
                    temp_L_CorF = tempL_C_1.ToString() + "°";
                }
                else
                {
                    temp_L_CorF = tempL_F_1.ToString() + "°";
                }
                #endregion Choose C or F for Low Weather

                hilo.Text = temp_H_CorF + "/" + temp_L_CorF;

                #endregion High Low


                #region Icon + Weather
                //Icon + Weather
                string weather_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["conditions"].ToString();
                int result = checkIcon(weather_1);
                desc.Text = weather_1.ToLower();

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
                    threeDayViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
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

                    threeDayViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
                }

                //Snow
                else if (result == 3)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                    threeDayViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Snow_Day.jpg"));

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
                    threeDayViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
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
                    threeDayViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @selectedIndexString));
                }

                //Hail
                else if (result == 6)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Hail.png"));
                    threeDayViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Hail.jpg"));
                }

                //Fog
                else if (result == 7)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Fog.png"));
                    threeDayViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Fog.jpg"));
                }

                //Sand
                else if (result == 8)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Sun.png"));
                    threeDayViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Sand.jpg"));
                }

                //Dust
                else if (result == 9)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));
                    threeDayViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Dust.jpg"));

                }

                //Squalls
                else if (result == 10)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Snow.png"));
                    threeDayViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Squalls.jpg"));

                }

                //Funnel Cloud
                else if (result == 11)
                {
                    weatherIcon.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));
                    threeDayViewBackground.ImageSource = new BitmapImage(new Uri(this.BaseUri, @"Assets/Images/Funnel_Cloud.jpg"));

                }

                #endregion Icon + Weather

                #region Rain
                string pop_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["pop"].ToString() + "%";
                string rain_I_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["qpf_allday"]["in"].ToString() + " in";
                string rain_M_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["qpf_allday"]["mm"].ToString() + " mm";

                //pop.Text = "P.O.P: " + pop_1;

                if (distUnitB == true)
                {
                    rainData.Text = pop_1;
                }
                else
                {
                    rainData.Text = pop_1;
                }
                #endregion Rain

                #region Wind
                string wind_Speed_I_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["avewind"]["mph"].ToString() + " mph";
                string wind_Speed_M_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["avewind"]["kph"].ToString() + " kph";
                string wind_Dir_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["avewind"]["dir"].ToString();

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
                string snow_I_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["snow_allday"]["in"].ToString() + " in";
                string snow_M_1 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][0]["snow_allday"]["cm"].ToString() + " cm";

                if (distUnitB == true)
                {
                    snowData.Text = snow_I_1;
                }
                else
                {
                    snowData.Text = snow_M_1;
                }
                #endregion Snow

                #endregion Day 1

                #region Day 2

                #region Weekday

                string weekday_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["date"]["weekday"].ToString();
                DayHead2.Text = weekday_2;

                #endregion Weekday

                #region High Low

                //High
                string tempH_C_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["high"]["celsius"].ToString();
                string tempH_F_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["high"]["fahrenheit"].ToString();

                #region Choose C or F for High Weather

                string temp_H_CorF_2 = "";
                if (tempUnitB == true)
                {
                    temp_H_CorF_2 = tempH_C_2.ToString() + "°";
                }
                else
                {
                    temp_H_CorF_2 = tempH_F_2.ToString() + "°";
                }

                #endregion Choose C or F for High Weather

                //Low
                string tempL_C_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["low"]["celsius"].ToString();
                string tempL_F_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["low"]["fahrenheit"].ToString();

                #region Choose C or F for Low Weather

                string temp_L_CorF_2 = "";
                if (tempUnitB == true)
                {
                    temp_L_CorF_2 = tempL_C_2.ToString() + "°";
                }
                else
                {
                    temp_L_CorF_2 = tempL_F_2.ToString() + "°";
                }

                #endregion Choose C or F for Low Weather

                hilo2.Text = temp_H_CorF_2 + "/" + temp_L_CorF_2;
                #endregion High Low

                #region Icon + Weather
                //Icon + Weather
                string weather_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["conditions"].ToString();
                int result2 = checkIcon(weather_2);
                desc2.Text = weather_2.ToLower();

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
                #endregion Icon + Weather

                #region Rain

                string pop_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["pop"].ToString() + "%";
                string rain_I_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["qpf_allday"]["in"].ToString() + " in";
                string rain_M_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["qpf_allday"]["mm"].ToString() + " mm";

                //pop2.Text = "P.O.P: " + pop_2;

                if (distUnitB == true)
                {
                    rainData2.Text = pop_2;
                }
                else
                {
                    rainData2.Text = pop_2;
                }

                #endregion Rain

                #region Wind

                string wind_Speed_I_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["avewind"]["mph"].ToString() + " mph";
                string wind_Speed_M_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["avewind"]["kph"].ToString() + " kph";
                string wind_Dir_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["avewind"]["dir"].ToString();

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

                string snow_I_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["snow_allday"]["in"].ToString() + " in";
                string snow_M_2 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][1]["snow_allday"]["cm"].ToString() + " cm";

                if (distUnitB == true)
                {
                    snowData2.Text = snow_I_2;
                }
                else
                {
                    snowData2.Text = snow_M_2;
                }

                #endregion Snow

                #endregion Day 2

                #region Day 3

                #region Weekday

                string weekday_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["date"]["weekday"].ToString();
                DayHead3.Text = weekday_3;

                #endregion Weekday

                #region High Low

                //High
                string tempH_C_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["high"]["celsius"].ToString();
                string tempH_F_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["high"]["fahrenheit"].ToString();

                #region Choose C or F for High Weather

                string temp_H_CorF_3 = "";
                if (tempUnitB == true)
                {
                    temp_H_CorF_3 = tempH_C_3.ToString() + "°";
                }
                else
                {
                    temp_H_CorF_3 = tempH_F_3.ToString() + "°";
                }

                #endregion Choose C or F for High Weather

                //Low
                string tempL_C_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["low"]["celsius"].ToString();
                string tempL_F_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["low"]["fahrenheit"].ToString();
                #region Choose C or F for Low Weather
                string temp_L_CorF_3 = "";
                if (tempUnitB == true)
                {
                    temp_L_CorF_3 = tempL_C_3.ToString() + "°";
                }
                else
                {
                    temp_L_CorF_3 = tempL_F_3.ToString() + "°";
                }
                #endregion Choose C or F for Low Weather

                hilo3.Text = temp_H_CorF_3 + "/" + temp_L_CorF_3;
                #endregion High Low

                #region Icon + Weather
                //Icon + Weather
                string weather_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["conditions"].ToString();
                int result3 = checkIcon(weather_3);
                desc3.Text = weather_3.ToLower();

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
                    weatherIcon3.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Cloud_Wind.png"));

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

                #endregion Icon + Weather

                #region Rain

                string pop_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["pop"].ToString() + "%";
                string rain_I_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["qpf_allday"]["in"].ToString() + " in";
                string rain_M_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["qpf_allday"]["mm"].ToString() + " mm";

                if (distUnitB == true)
                {
                    rainData3.Text = pop_3;
                }
                else
                {
                    rainData3.Text = pop_3;
                }

                #endregion Rain

                #region Wind

                string wind_Speed_I_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["avewind"]["mph"].ToString() + " mph";
                string wind_Speed_M_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["avewind"]["kph"].ToString() + " kph";
                string wind_Dir_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["avewind"]["dir"].ToString();

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

                string snow_I_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["snow_allday"]["in"].ToString() + " in";
                string snow_M_3 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][2]["snow_allday"]["cm"].ToString() + " cm";

                if (distUnitB == true)
                {
                    snowData3.Text = snow_I_3;
                }
                else
                {
                    snowData3.Text = snow_M_3;
                }

                #endregion Snow

                #endregion Day 1

                #region Day 4

                #region Weekday

                string weekday_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["date"]["weekday"].ToString();
                DayHead4.Text = weekday_4;

                #endregion Weekday

                #region High Low
                //High
                string tempH_C_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["high"]["celsius"].ToString();
                string tempH_F_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["high"]["fahrenheit"].ToString();

                #region Choose C or F for High Weather

                string temp_H_CorF_4 = "";
                if (tempUnitB == true)
                {
                    temp_H_CorF_4 = tempH_C_4.ToString() + "°";
                }
                else
                {
                    temp_H_CorF_4 = tempH_F_4.ToString() + "°";
                }

                #endregion Choose C or F for High Weather

                //Low
                string tempL_C_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["low"]["celsius"].ToString();
                string tempL_F_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["low"]["fahrenheit"].ToString();
                #region Choose C or F for Low Weather
                string temp_L_CorF_4 = "";
                if (tempUnitB == true)
                {
                    temp_L_CorF_4 = tempL_C_4.ToString() + "°";
                }
                else
                {
                    temp_L_CorF_4 = tempL_F_4.ToString() + "°";
                }

                #endregion Choose C or F for Low Weather

                hilo4.Text = temp_H_CorF_4 + "/" + temp_L_CorF_4;
                #endregion High Low

                #region Icon + Weather
                //Icon + Weather
                string weather_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["conditions"].ToString();
                int result4 = checkIcon(weather_4);
                desc4.Text = weather_4.ToLower();

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
                #endregion Icon + Weather

                #region Rain
                string pop_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["pop"].ToString() + "%";
                string rain_I_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["qpf_allday"]["in"].ToString() + " in";
                string rain_M_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["qpf_allday"]["mm"].ToString() + " mm";

                //pop.Text = "P.O.P: " + pop_1;

                if (distUnitB == true)
                {
                    rainData4.Text = pop_4;
                }
                else
                {
                    rainData4.Text = pop_4;
                }
                #endregion Rain

                #region Wind
                string wind_Speed_I_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["avewind"]["mph"].ToString() + " mph";
                string wind_Speed_M_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["avewind"]["kph"].ToString() + " kph";
                string wind_Dir_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["avewind"]["dir"].ToString();

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
                string snow_I_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["snow_allday"]["in"].ToString() + " in";
                string snow_M_4 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][3]["snow_allday"]["cm"].ToString() + " cm";

                if (distUnitB == true)
                {
                    snowData4.Text = snow_I_4;
                }
                else
                {
                    snowData4.Text = snow_M_4;
                }
                #endregion Snow

                #endregion Day 4

                #region Day 5

                #region Weekday

                string weekday_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["date"]["weekday"].ToString();
                DayHead5.Text = weekday_5;

                #endregion Weekday

                #region High Low
                //High
                string tempH_C_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["high"]["celsius"].ToString();
                string tempH_F_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["high"]["fahrenheit"].ToString();

                #region Choose C or F for High Weather

                string temp_H_CorF_5 = "";
                if (tempUnitB == true)
                {
                    temp_H_CorF_5 = tempH_C_5.ToString() + "°";
                }
                else
                {
                    temp_H_CorF_5 = tempH_F_5.ToString() + "°";
                }

                #endregion Choose C or F for High Weather

                //Low
                string tempL_C_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["low"]["celsius"].ToString();
                string tempL_F_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["low"]["fahrenheit"].ToString();
                #region Choose C or F for Low Weather
                string temp_L_CorF_5 = "";
                if (tempUnitB == true)
                {
                    temp_L_CorF_5 = tempL_C_5.ToString() + "°";
                }
                else
                {
                    temp_L_CorF_5 = tempL_F_5.ToString() + "°";
                }

                #endregion Choose C or F for Low Weather

                hilo5.Text = temp_H_CorF_5 + "/" + temp_L_CorF_5;
                #endregion High Low

                #region Icon + Weather
                //Icon + Weather
                string weather_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["conditions"].ToString();
                int result5 = checkIcon(weather_5);
                desc5.Text = weather_5.ToLower();

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
                #endregion Icon + Weather

                #region Rain
                string pop_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["pop"].ToString() + "%";
                string rain_I_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["qpf_allday"]["in"].ToString() + " in";
                string rain_M_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["qpf_allday"]["mm"].ToString() + " mm";

                //pop.Text = "P.O.P: " + pop_1;

                if (distUnitB == true)
                {
                    rainData5.Text = pop_5;
                }
                else
                {
                    rainData5.Text = pop_5;
                }
                #endregion Rain

                #region Wind
                string wind_Speed_I_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["avewind"]["mph"].ToString() + " mph";
                string wind_Speed_M_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["avewind"]["kph"].ToString() + " kph";
                string wind_Dir_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["avewind"]["dir"].ToString();

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
                string snow_I_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["snow_allday"]["in"].ToString() + " in";
                string snow_M_5 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][4]["snow_allday"]["cm"].ToString() + " cm";

                if (distUnitB == true)
                {
                    snowData5.Text = snow_I_5;
                }
                else
                {
                    snowData5.Text = snow_M_5;
                }
                #endregion Snow

                #endregion Day 5

                #region Day 6

                #region Weekday

                string weekday_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["date"]["weekday"].ToString();
                DayHead6.Text = weekday_6;

                #endregion Weekday

                #region High Low
                //High
                string tempH_C_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["high"]["celsius"].ToString();
                string tempH_F_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["high"]["fahrenheit"].ToString();

                #region Choose C or F for High Weather

                string temp_H_CorF_6 = "";
                if (tempUnitB == true)
                {
                    temp_H_CorF_6 = tempH_C_6.ToString() + "°";
                }
                else
                {
                    temp_H_CorF_6 = tempH_F_6.ToString() + "°";
                }

                #endregion Choose C or F for High Weather

                //Low
                string tempL_C_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["low"]["celsius"].ToString();
                string tempL_F_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["low"]["fahrenheit"].ToString();
                #region Choose C or F for Low Weather
                string temp_L_CorF_6 = "";
                if (tempUnitB == true)
                {
                    temp_L_CorF_6 = tempL_C_6.ToString() + "°";
                }
                else
                {
                    temp_L_CorF_6 = tempL_F_6.ToString() + "°";
                }

                #endregion Choose C or F for Low Weather

                hilo6.Text = temp_H_CorF_6 + "/" + temp_L_CorF_6;
                #endregion High Low

                #region Icon + Weather
                //Icon + Weather
                string weather_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["conditions"].ToString();
                int result6 = checkIcon(weather_6);
                desc6.Text = weather_6.ToLower();

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
                #endregion Icon + Weather

                #region Rain
                string pop_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["pop"].ToString() + "%";
                string rain_I_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["qpf_allday"]["in"].ToString() + " in";
                string rain_M_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["qpf_allday"]["mm"].ToString() + " mm";

                //pop.Text = "P.O.P: " + pop_1;

                if (distUnitB == true)
                {
                    rainData6.Text = pop_6;
                }
                else
                {
                    rainData6.Text = pop_6;
                }
                #endregion Rain

                #region Wind
                string wind_Speed_I_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["avewind"]["mph"].ToString() + " mph";
                string wind_Speed_M_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["avewind"]["kph"].ToString() + " kph";
                string wind_Dir_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["avewind"]["dir"].ToString();

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
                string snow_I_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["snow_allday"]["in"].ToString() + " in";
                string snow_M_6 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][5]["snow_allday"]["cm"].ToString() + " cm";

                if (distUnitB == true)
                {
                    snowData6.Text = snow_I_6;
                }
                else
                {
                    snowData6.Text = snow_M_6;
                }
                #endregion Snow

                #endregion Day 6

                #region Day 7

                #region Weekday

                string weekday_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["date"]["weekday"].ToString();
                DayHead7.Text = weekday_7;

                #endregion Weekday

                #region High Low
                //High
                string tempH_C_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["high"]["celsius"].ToString();
                string tempH_F_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["high"]["fahrenheit"].ToString();

                #region Choose C or F for High Weather

                string temp_H_CorF_7 = "";
                if (tempUnitB == true)
                {
                    temp_H_CorF_7 = tempH_C_7.ToString() + "°";
                }
                else
                {
                    temp_H_CorF_7 = tempH_F_7.ToString() + "°";
                }

                #endregion Choose C or F for High Weather

                //Low
                string tempL_C_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["low"]["celsius"].ToString();
                string tempL_F_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["low"]["fahrenheit"].ToString();
                #region Choose C or F for Low Weather
                string temp_L_CorF_7 = "";
                if (tempUnitB == true)
                {
                    temp_L_CorF_7 = tempL_C_7.ToString() + "°";
                }
                else
                {
                    temp_L_CorF_7 = tempL_F_7.ToString() + "°";
                }

                #endregion Choose C or F for Low Weather

                hilo7.Text = temp_H_CorF_7 + "/" + temp_L_CorF_7;
                #endregion High Low

                #region Icon + Weather
                //Icon + Weather
                string weather_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["conditions"].ToString();
                int result7 = checkIcon(weather_7);
                desc7.Text = weather_7.ToLower();

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
                else if (result7 == 11)
                {
                    weatherIcon7.Source = new BitmapImage(new Uri(this.BaseUri, @"Assets/Icons/Wind.png"));

                }
                #endregion Icon + Weather

                #region Rain
                string pop_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["pop"].ToString() + "%";
                string rain_I_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["qpf_allday"]["in"].ToString() + " in";
                string rain_M_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["qpf_allday"]["mm"].ToString() + " mm";

                //pop.Text = "P.O.P: " + pop_1;

                if (distUnitB == true)
                {
                    rainData7.Text = pop_7;
                }
                else
                {
                    rainData7.Text = pop_7;
                }
                #endregion Rain

                #region Wind
                string wind_Speed_I_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["avewind"]["mph"].ToString() + " mph";
                string wind_Speed_M_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["avewind"]["kph"].ToString() + " kph";
                string wind_Dir_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["avewind"]["dir"].ToString();

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
                string snow_I_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["snow_allday"]["in"].ToString() + " in";
                string snow_M_7 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][6]["snow_allday"]["cm"].ToString() + " cm";

                if (distUnitB == true)
                {
                    snowData7.Text = snow_I_7;
                }
                else
                {
                    snowData7.Text = snow_M_7;
                }
                #endregion Snow

                #endregion Day 7

                #region Day 8

                #region Weekday

                string weekday_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["date"]["weekday"].ToString();
                DayHead8.Text = weekday_8;

                #endregion Weekday

                #region High Low
                //High
                string tempH_C_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["high"]["celsius"].ToString();
                string tempH_F_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["high"]["fahrenheit"].ToString();

                #region Choose C or F for High Weather

                string temp_H_CorF_8 = "";
                if (tempUnitB == true)
                {
                    temp_H_CorF_8 = tempH_C_8.ToString() + "°";
                }
                else
                {
                    temp_H_CorF_8 = tempH_F_8.ToString() + "°";
                }

                #endregion Choose C or F for High Weather

                //Low
                string tempL_C_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["low"]["celsius"].ToString();
                string tempL_F_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["low"]["fahrenheit"].ToString();
                #region Choose C or F for Low Weather
                string temp_L_CorF_8 = "";
                if (tempUnitB == true)
                {
                    temp_L_CorF_8 = tempL_C_8.ToString() + "°";
                }
                else
                {
                    temp_L_CorF_8 = tempL_F_8.ToString() + "°";
                }

                #endregion Choose C or F for Low Weather

                hilo8.Text = temp_H_CorF_8 + "/" + temp_L_CorF_8;
                #endregion High Low

                #region Icon + Weather
                //Icon + Weather
                string weather_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["conditions"].ToString();
                int result8 = checkIcon(weather_8);
                desc8.Text = weather_8.ToLower();

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
                #endregion Icon + Weather

                #region Rain
                string pop_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["pop"].ToString() + "%";
                string rain_I_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["qpf_allday"]["in"].ToString() + " in";
                string rain_M_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["qpf_allday"]["mm"].ToString() + " mm";

                //pop.Text = "P.O.P: " + pop_1;

                if (distUnitB == true)
                {
                    rainData8.Text = pop_8;
                }
                else
                {
                    rainData8.Text = pop_8;
                }
                #endregion Rain

                #region Wind
                string wind_Speed_I_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["avewind"]["mph"].ToString() + " mph";
                string wind_Speed_M_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["avewind"]["kph"].ToString() + " kph";
                string wind_Dir_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["avewind"]["dir"].ToString();

                if (distUnitB == true)
                {
                    windData8.Text = wind_Speed_I_7;
                }
                else
                {
                    windData8.Text = wind_Speed_M_7;
                }
                #endregion Wind

                #region Snow
                string snow_I_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["snow_allday"]["in"].ToString() + " in";
                string snow_M_8 = threeDayObj["forecast"]["simpleforecast"]["forecastday"][7]["snow_allday"]["cm"].ToString() + " cm";

                if (distUnitB == true)
                {
                    snowData8.Text = snow_I_8;
                }
                else
                {
                    snowData8.Text = snow_M_8;
                }
                #endregion Snow

                #endregion Day 8

                #endregion show Seven Day Weather

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

        private new void GoBack(object sender, RoutedEventArgs e)
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

        private void doManualRefresh(object sender, RoutedEventArgs e)
        {

            fetchThreeDayWeather();
        }

    }
}
