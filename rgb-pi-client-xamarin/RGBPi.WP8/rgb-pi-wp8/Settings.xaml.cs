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
using RGBPi.Core;
using RGB.Services;

namespace RGB
{
    public partial class Settings : PhoneApplicationPage
    {
        public string IP { get; set; }
        public int Port { get; set; }
        private ISettings settings;

        public Settings()
        {
            settings = new WP8Settings();

            InitializeComponent();

            LoadSettings();

            BuildLocalizedApplicationBar();
        }

        private void LoadSettings()
        {
            Host activeHost = settings.ActiveHost;

            if (activeHost == null)
            {
                var hosts = settings.GetHosts();
                if (hosts.Count == 0)
                {
                    activeHost = new Host("default", "192.168.0.10", 4321);
                    settings.AddHost(activeHost);
                }
                else
                {
                    hosts[0] = activeHost = new Host("default", "192.168.0.10", 4321);
                    settings.UpdateHost(activeHost.name, activeHost);
                    settings.ActiveHost = activeHost;
                }
                
            }
            
            txtSettingsIP.Text = activeHost.ip;
            txtSettingsPort.Text = activeHost.port + string.Empty;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadSettings();
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton abbOn = new ApplicationBarIconButton(new Uri("/Assets/Icons/WP8/save.png", UriKind.Relative));
            abbOn.Text = "Save";
            abbOn.Click += delegate(object s, EventArgs ea)
            {
                int e = 0;
                try
                {
                    e++;
                    IP = txtSettingsIP.Text;
                    e++;
                    Port = int.Parse(txtSettingsPort.Text);
                    e++;

                    var activeHost = settings.ActiveHost;
                    activeHost.ip = IP;
                    activeHost.port = Port;
                    settings.UpdateHost(activeHost.name, activeHost);
                    settings.ActiveHost = activeHost;

                    settings.ActiveHost = activeHost;
                    e++;
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    ShellToast toast = new ShellToast();
                    toast.Title = "RGB-Pi";
                    toast.Content = "Saved!";
                    toast.Show();
                }
                catch (Exception ex)
                {
                    ShellToast toast = new ShellToast();
                    toast.Title = "RGB-Pi";
                    toast.Content = e == 1 ? "Invalid IP" : e == 2 ? "Invalid Port" : e == 3 ? "Error during saving: " + ex.Message : ex.Message;
                    toast.Show();
                }
            };
            ApplicationBar.Buttons.Add(abbOn);

            ApplicationBarIconButton abbOff = new ApplicationBarIconButton(new Uri("/Assets/Icons/WP8/cancel.png", UriKind.Relative));
            abbOff.Text = "Cancel";
            abbOff.Click += delegate(object s, EventArgs ea)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            };
            ApplicationBar.Buttons.Add(abbOff);


        }
    }

    public class RGBPiConnection
    {
        private string name;
        private string ip;
        private ushort port;

        public RGBPiConnection(string name, string ip, ushort port)
        {
            this.name = name;
            this.ip = ip;
            this.port = port;
        }

        public string IP { get { return ip; } set { ip = value; } }
        public ushort Port { get { return port; } set { port = value; } }
        public string Name { get { return name; } set { name = value; } }
    }
}