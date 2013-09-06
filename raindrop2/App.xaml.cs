using raindrop2;
using raindrop2.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Background;
using Callisto.Controls;


// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace raindrop2
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private Color _background = Color.FromArgb(255, 255, 187, 51);
        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
            SettingsPane.GetForCurrentView().CommandsRequested += DisplayAbout;
            SettingsPane.GetForCurrentView().CommandsRequested += DisplayPrivacyPolicy;


            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active

            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            // Add a Preferences command
            var preferences = new SettingsCommand("preferences", "Preferences", (handler)
            =>
            {
                var settings = new Callisto.Controls.SettingsFlyout();
                settings.Content = new PreferencesUserControl();
                settings.HeaderBrush = new SolidColorBrush(_background);
                settings.Background = new SolidColorBrush(_background);
                settings.HeaderText = "Preferences";
                settings.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(preferences);
        }


        private void DisplayPrivacyPolicy(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            // Add a Preferences command
            var privacyPolicyCommand = new SettingsCommand("privacy policy", "Privacy Policy", (handler)
            =>
            {
                var pp = new Callisto.Controls.SettingsFlyout();
                pp.Content = new ppUserControl();
                pp.HeaderBrush = new SolidColorBrush(_background);
                pp.Background = new SolidColorBrush(_background);
                pp.HeaderText = "Privacy Policy";
                pp.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(privacyPolicyCommand);
        }

        private async void LaunchPrivacyPolicyUrl()
        {
            Uri privacyPolicyUrl = new Uri("http://sdrv.ms/15lf8c2");
            var result = await Windows.System.Launcher.LaunchUriAsync(privacyPolicyUrl);
        }

        private void DisplayAbout(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            // Add a Preferences command
            var aboutCommand = new SettingsCommand("about", "About", (handler)
            =>
            {
                var about = new Callisto.Controls.SettingsFlyout();
                about.Content = new aboutUserControl();
                about.HeaderBrush = new SolidColorBrush(_background);
                about.Background = new SolidColorBrush(_background);
                about.HeaderText = "About";
                about.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(aboutCommand);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
