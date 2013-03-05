using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace urlShortner
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
            if (appSettings.Contains("CBoxSave"))
            {
                cbSave.IsChecked = (bool)appSettings["CBoxSave"];
            }
            if (appSettings.Contains("CBoxLight"))
            {
                cbLight.IsChecked = (bool)appSettings["CBoxLight"];
            }
            if (appSettings.Contains("BService") && !((bool)appSettings["BService"]))
            {// false means v.gd ~ true means is.gd
                bVGD_Click(null, null);
            }
            else bISGD_Click(null, null);

        }

        private void menubar_save_Click_1(object sender, EventArgs e)
        {
            // save setttings to isolated storage
            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

            if (appSettings.Contains("CBoxSave")) appSettings["CBoxSave"] = cbSave.IsChecked;
            else appSettings.Add("CBoxSave", cbSave.IsChecked);

            if (appSettings.Contains("CBoxLight")) appSettings["CBoxLight"] = cbLight.IsChecked;
            else appSettings.Add("CBoxLight", cbLight.IsChecked);

        }

        private void menubar_clear_Click_1(object sender, EventArgs e)
        {
            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
            if (appSettings.Contains("CBoxSave"))
            {
                cbSave.IsChecked = (bool)appSettings["CBoxSave"];
            }
            if (appSettings.Contains("CBoxLight"))
            {
                cbLight.IsChecked = (bool)appSettings["CBoxLight"];
            }

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menubar_help_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Help.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menubar_history_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/History.xaml", UriKind.RelativeOrAbsolute));
        }

        private void deleteChosenHistory()
        {
            // DELETE CODE
            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();
            string[] filelist = appStorage.GetFileNames("*.txt");

            foreach (string file in filelist)
            {
                appStorage.DeleteFile(file);
            }
        }

        private void bCancel_Click_1(object sender, RoutedEventArgs e)
        {
            deleteDialog.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void bDelete_Click_1(object sender, RoutedEventArgs e)
        {
            deleteDialog.Visibility = System.Windows.Visibility.Collapsed;
            deleteChosenHistory();
        }

        private void deleteHistory_Click_1(object sender, RoutedEventArgs e)
        {
            deleteDialog.Visibility = System.Windows.Visibility.Visible;
        }



        private void bISGD_Click(object sender, RoutedEventArgs e)
        {
            bISGD.IsEnabled = false;
            bVGD.IsEnabled = true;

            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

            if (appSettings.Contains("BService")) appSettings["BService"] = true;
            else appSettings.Add("BService", true);
            tbServiceStatus.Text = "IS.GD";

        }
        // false means v.gd ~ true means is.gd
        private void bVGD_Click(object sender, RoutedEventArgs e)
        {
            bISGD.IsEnabled = true;
            bVGD.IsEnabled = false;

            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
            if (appSettings.Contains("BService")) appSettings["BService"] = false;
            else appSettings.Add("BService", false);
            tbServiceStatus.Text = "V.GD";
        }
    }
}