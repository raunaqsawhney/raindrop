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
using Windows.UI.Xaml.Navigation;
using Windows.Storage;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace raindrop2
{
    public sealed partial class PreferencesUserControl : UserControl
    {
        string TempUnit = "";
        string DistUnit = "";

        public PreferencesUserControl()
        {
            this.InitializeComponent();
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("FirstName"))
            {
                firstName.Text = (string)ApplicationData.Current.LocalSettings.Values["FirstName"];
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TempUnit"))
            {
                string TempUnit1 = (string)ApplicationData.Current.LocalSettings.Values["TempUnit"];
                if (TempUnit1 == "Celsius")
                {
                    tempRadioC.IsChecked = true;
                    tempRadioF.IsChecked = false;
                }
                if (TempUnit1 == "Fahrenheit")
                {
                    tempRadioF.IsChecked = true;
                    tempRadioC.IsChecked = false;
                }
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("DistUnit"))
            {
                string DistUnit1 = (string)ApplicationData.Current.LocalSettings.Values["DistUnit"];
                if (DistUnit1 == "Metric")
                {
                    distRadioM.IsChecked = true;
                    distRadioI.IsChecked = false;
                }
                if (DistUnit1 == "Imperial")
                {
                    distRadioI.IsChecked = true;
                    distRadioM.IsChecked = false;
                }
            }

        }

        private void tempRadioC_Checked(object sender, RoutedEventArgs e)
        {
            TempUnit = "Celsius";
            ApplicationData.Current.LocalSettings.Values["TempUnit"] = TempUnit;
        }

        private void tempRadioF_Checked(object sender, RoutedEventArgs e)
        {
            TempUnit = "Fahrenheit";
            ApplicationData.Current.LocalSettings.Values["TempUnit"] = TempUnit;
        }

        private void distRadioM_Checked(object sender, RoutedEventArgs e)
        {
            DistUnit = "Metric";
            ApplicationData.Current.LocalSettings.Values["DistUnit"] = DistUnit;
        }

        private void distRadioI_Checked(object sender, RoutedEventArgs e)
        {
            DistUnit = "Imperial";
            ApplicationData.Current.LocalSettings.Values["DistUnit"] = DistUnit;
        }

        private void firstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["FirstName"] = firstName.Text;
        }
    }
}
