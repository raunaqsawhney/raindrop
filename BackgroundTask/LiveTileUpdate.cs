using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
// Added during quickstart
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.Web.Syndication;

namespace BackgroundTask
{
    public sealed class LiveTileUpdate : IBackgroundTask
    {
        bool? tempUnitB = null;

        string tempUnit = (string)ApplicationData.Current.LocalSettings.Values["TempUnit"];
        string distUnit = (string)ApplicationData.Current.LocalSettings.Values["DistUnit"];

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely 
            // while asynchronous code is still running.
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            //Download Live Feed
            var feed = await getWeatherFeed();

            //Update the live tile with weather feed
            UpdateTile(feed);

            //Inform the system
            deferral.Complete();
        }

        private void UpdateTile(JObject feed)
        {
            //Check if Celsius or Fahrenheit
            if (tempUnit == "Celsius")
                tempUnitB = true;
            else
                tempUnitB = false;
            
            var weather = feed["currently"]["summary"];
            string titleText = (string)weather;

            string temp_f = feed["currently"]["temperature"].ToString() ;

            double TmpFtoC = (double.Parse(temp_f));
            TmpFtoC = TmpFtoC - 32;
            TmpFtoC = TmpFtoC * 5;
            TmpFtoC = TmpFtoC / 9;
            TmpFtoC = Math.Truncate(TmpFtoC);

            string temp_c = TmpFtoC.ToString();

            string tempCorF = "";
            if (tempUnitB == true)
            {
                tempCorF = temp_c + "°C";
            }
            else
            {
                tempCorF = temp_f + "°F";
            }

            string cityName = (string)ApplicationData.Current.LocalSettings.Values["CITY"];

            // Create a tile update manager for the specified syndication feed.
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideText03);
            
            tileXml.GetElementsByTagName("text")[0].InnerText = tempCorF + " and " + titleText + " in " + cityName;

            /*
            tileXml.GetElementsByTagName("text")[1].InnerText = "Location";
            tileXml.GetElementsByTagName("text")[2].InnerText = cityName;

            tileXml.GetElementsByTagName("text")[3].InnerText = "P.O.P";
            tileXml.GetElementsByTagName("text")[4].InnerText = pop;

            tileXml.GetElementsByTagName("text")[5].InnerText = "Conditions";
            tileXml.GetElementsByTagName("text")[6].InnerText = stringCloudCover ;
            */

            updater.Update(new TileNotification(tileXml));
        }

        //string textElementName = "text";

        private static async Task<JObject> getWeatherFeed()
        {
            string tmpLat = (string)ApplicationData.Current.LocalSettings.Values["Latitude"];
            string tmpLon = (string)ApplicationData.Current.LocalSettings.Values["Longitude"];

            var client = new HttpClient();
            var response = await client.GetStringAsync(new Uri(string.Format("https://api.forecast.io/forecast/d565832a53472f2eef3d3b9359dd7302/{0},{1}", tmpLat, tmpLon)));

            var obj = JObject.Parse(response);

            return obj;
        }
    }
}
