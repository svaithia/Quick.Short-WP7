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
using System.IO;

namespace urlShortner
{
    public partial class History : PhoneApplicationPage
    {
        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            bindHistory();
        }

        public History()
        {
            InitializeComponent();
        }

        private void menubar_home_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menubar_delete_Click_1(object sender, EventArgs e)
        {
            deleteDialog.Visibility = System.Windows.Visibility.Visible;
        }

        private void menubar_settings_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menubar_help_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Help.xaml", UriKind.RelativeOrAbsolute));
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
            bindHistory();
        }

        private void bindHistory()
        {
            IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication();

            string[] filelist = appStorage.GetFileNames("*.txt");
            List<URL> links = new List<URL>();
            
            
            foreach (string file in filelist.Reverse())
            {
                // Retrieve the file
                string fileName = file;
                string loonglink;
                using (var urlfile = appStorage.OpenFile(fileName, System.IO.FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(urlfile)){
                        loonglink = sr.ReadToEnd().ToString();
                    }
                }
                // Plunk out the date parts
                
                string year = file.Substring(0, 4);
                string month = file.Substring(5, 2);
                string day = file.Substring(8, 2);
                string hour = file.Substring(11, 2);
                string minute = file.Substring(14, 2);
                string second = file.Substring(17, 2);
                int source = int.Parse(file.Substring(19, 1));
                // Create a new DateTime object
                    //if (int.Parse(year) > 2011 && int.Parse(month) < 13 && int.Parse(day) < 32 && int.Parse(hour) < 25 && int.Parse(minute) < 60 && int.Parse(second) < 60)
                    //{
                        DateTime dateCreated = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second));

                        // Parse out shortlink
                        string shortLink = file.Substring(20);
                        string chbxid = shortLink.Substring(0, shortLink.Length - 4);
                        if (source == 1) { shortLink = "http://is.gd/" + chbxid; }
                        else { shortLink = "http://v.gd/" + chbxid; }

                        links.Add(new URL() { ShortUrl = shortLink, DateCreated = dateCreated.ToShortDateString() + " ~" + dateCreated.ToShortTimeString(), LongUrl = loonglink });
                    //}
            }
            linksListBox.ItemsSource = links;
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
    }
}