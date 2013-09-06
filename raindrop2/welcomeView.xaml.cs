using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;


namespace raindrop2
{
    public sealed partial class welcomeView : raindrop2.Common.LayoutAwarePage
    {
        public welcomeView()
        {
            this.InitializeComponent();
            loadAnimation();
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void loadAnimation()
        {
            var a0 = this.Resources["img0Animation"] as Storyboard;
            if (a0 != null) a0.Begin();

            var a1 = this.Resources["img1Animation"] as Storyboard;
            if (a1 != null) a1.Begin();

            var a2 = this.Resources["img2Animation"] as Storyboard;
            if (a2 != null) a2.Begin();

            var a3 = this.Resources["img3Animation"] as Storyboard;
            if (a3 != null) a3.Begin();

            var a4 = this.Resources["img4Animation"] as Storyboard;
            if (a4 != null) a4.Begin();

            var a5 = this.Resources["img5Animation"] as Storyboard;
            if (a5 != null) a5.Begin();
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
