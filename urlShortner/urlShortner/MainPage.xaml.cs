using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace urlShortner
{
    public partial class MainPage : PhoneApplicationPage
    {
        /*http://is.gd/create.php?format=web&callback=myfunction&url=www.example.com&logstats=1 */
        // Constructor
        bool copyp = false; // since same method is used except for clipboard part, this bool tells when the CopyG button is clicked instead
        bool VCBsave = true; // these two are for isolatedsetting storage info. its public so can be accessed anywhere
        bool VCBlight = false;
        public MainPage()
        {
            InitializeComponent();
        }

        private void getpage(string inputurl)
        {
            var webClient = new WebClient();

            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

            if (appSettings.Contains("BService") && !((bool)appSettings["BService"]))
            {
                webClient.OpenReadAsync(new Uri("http://v.gd/create.php?format=simple&callback=myfunction&url=" + inputurl + "&logstats=1" + DateTime.Now));
            }
            else webClient.OpenReadAsync(new Uri("http://is.gd/create.php?format=simple&callback=myfunction&url=" + inputurl + "&logstats=1" + DateTime.Now));
                
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadComplete);
        }

        void webClient_OpenReadComplete(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                using (var reader = new StreamReader(e.Result))
                {
                    string shortUrl = reader.ReadToEnd();
                    tbOut.Text = shortUrl;
                    //save url :: shortUrl
                    if(VCBsave) save2history();
                    if (copyp) Clipboard.SetText(shortUrl);
                    copyp = false;
                }
            }
            catch
            {
                MessageBox.Show("Please check your internet connection");
            }

                
        }

        private void bGenerate_Click(object sender, RoutedEventArgs e)
        {
            string inputurl = tbIn.Text;
            getpage(inputurl);
        }

        private void bGCopy_Click(object sender, RoutedEventArgs e)
        {
            getpage(tbIn.Text.ToString());
            copyp = true;
            
        }

        private void appbar_clear_Click_1(object sender, EventArgs e)
        {
            tbIn.Text = "";
            tbOut.Text = "Your URL";
        }

        private void appbar_settings_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.RelativeOrAbsolute));
        }

        private void appbar_help_Click_1(object sender, EventArgs e)
        {
            // NavigationService.Navigate(new Uri("/Help.xaml", UriKind.RelativeOrAbsolute));
            // JUST TO TEST THINGS OUT //
            /*
            string fileName = "2012_10_31_13_42_01_ISGDS.txt";
            string fileContent = "This is just test";

            var appStorage = IsolatedStorageFile.GetUserStoreForApplication();

            if (!appStorage.FileExists(fileName))
            {
                using (var file = appStorage.CreateFile(fileName))
                {
                    using (var writer = new StreamWriter(file))
                    {
                        writer.WriteLine(fileContent);
                    }
                }
            }
             */
            NavigationService.Navigate(new Uri("/Help.xaml", UriKind.RelativeOrAbsolute));
        }

        private void appbar_history_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/History.xaml", UriKind.RelativeOrAbsolute));
        }

        private void save2history()
        {
            if (!VCBsave) return;
            IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

            // GETS INPUT/OUTPUT AND CONVERTS INTO VARIABLE SO IT CAN STORE AS FILE NAME AND FILE CONTENT
            // CREATES THE FILE NAME //
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.Year);
            sb.Append("_");            
            sb.Append(String.Format("{0:00}", DateTime.Now.Month));
            sb.Append("_");
            sb.Append(String.Format("{0:00}", DateTime.Now.Day));
            sb.Append("_");
            sb.Append(String.Format("{0:00}", DateTime.Now.Hour));
            sb.Append("_");
            sb.Append(String.Format("{0:00}", DateTime.Now.Minute));
            sb.Append("_");
            sb.Append(String.Format("{0:00}", DateTime.Now.Second));

            if (appSettings.Contains("BService") && !((bool)appSettings["BService"])) { sb.Append("0"); }
            else { sb.Append("1"); }     //deciding which service to usee 1 is for is.gd and 0 is for v.gd

            string tbOutstring = tbOut.Text.ToString();

             //IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

             string shortisvurl;
             if (appSettings.Contains("BService") && !((bool)appSettings["BService"]))
             {
                 shortisvurl = tbOutstring.Substring(12, tbOutstring.Length - 12);
             }
             else
             {
                 shortisvurl = tbOutstring.Substring(13, tbOutstring.Length - 13);
             }

            sb.Append(shortisvurl);
            sb.Append(".txt");
            string fileloc = sb.ToString();

            // WRITES THE FILE INTO A FILE NAME CREATED EARLIER//
            string loong = tbIn.Text;

            using (var fileStream = appStorage.OpenFile(fileloc, System.IO.FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fileStream))
                {
                    sw.WriteLine(loong);
                }
            }

        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
            if (appSettings.Contains("CBoxSave"))
            {
                VCBsave = (bool)appSettings["CBoxSave"];

            }
            if (appSettings.Contains("CBoxLight"))
            {
                VCBlight = (bool)appSettings["CBoxLight"];
            }
            if (VCBlight) canvasLightningMode.Visibility = System.Windows.Visibility.Visible;
        }

        private void lmAbort_Click_1(object sender, RoutedEventArgs e)
        {
            canvasLightningMode.Visibility = System.Windows.Visibility.Collapsed;   
        }

        private void lmContinue_Click_1(object sender, RoutedEventArgs e)
        {
            // Lightning mode
            copyp = true;
            string longlink = Clipboard.GetText().ToString();
            getpage(longlink);
            // 
            canvasLightningMode.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}