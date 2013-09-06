using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace raindrop2
{
    public sealed partial class alertView : raindrop2.Common.LayoutAwarePage
    {
        public alertView()
        {
            this.InitializeComponent();
            loadAlert();
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void loadAlert()
        {
            date.Text = ApplicationData.Current.LocalSettings.Values["alertDate"].ToString();
            expires.Text = ApplicationData.Current.LocalSettings.Values["alertExpires"].ToString();
            description.Text = ApplicationData.Current.LocalSettings.Values["alertDescription"].ToString();
            message.Text = ApplicationData.Current.LocalSettings.Values["alertMessage"].ToString();
        }

        private new void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(weatherView));
        }

        private async void GoToWunderground(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.wunderground.com/?apiref=5aeed8d448b15bb8"));
        }
    }
}
