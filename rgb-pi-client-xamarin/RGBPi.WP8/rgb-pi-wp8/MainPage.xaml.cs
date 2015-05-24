using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RGB.Resources;
using System.Threading;
using Coding4Fun.Toolkit.Controls;
using RGBPi;
using System.IO.IsolatedStorage;
using RGB.Services;
using RGBPi.Core.Model;
using RGBPi.Core.Model.Commands;

namespace RGB
{
    public partial class MainPage : PhoneApplicationPage
    {
        private IWP8Socket client = new WP8Socket();
        private ColorPicker copickDimColor;
        private ColorPicker colorPicker;
        private ColorPicker copickPulseStart, copickPulseEnd;

        

       
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();

            colorPicker = new ColorPicker();
            colorPicker.ColorChanged += colorPicker_ColorChanged;
            gridChooseColor.Children.Add(colorPicker);

            copickDimColor = new ColorPicker();
            gridDimColor.Children.Add(copickDimColor);

            copickPulseStart = new ColorPicker();
            copickPulseEnd = new ColorPicker();
            gridPulseStartColor.Children.Add(copickPulseStart);
            gridPulseEndColor.Children.Add(copickPulseEnd);

            
            LoadSettings();
        }

        private void LoadSettings()
        {
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            LoadSettings();
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);
            LoadSettings();
        }

        void colorPicker_ColorChanged(object sender, System.Windows.Media.Color color)
        {
            Message cmd = new Message(new List<RGBPi.Core.Model.Commands.Command>());
            cmd.commands.Add(new CC(new RGBPi.Core.Model.DataTypes.Color(color.R /255f, color.G / 255f, color.B / 255f)));
            client.Send(cmd);
        }

        
        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }



        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton abbOn = new ApplicationBarIconButton(new Uri("/Assets/Icons/on.png", UriKind.Relative));
            abbOn.Text = "On";
            abbOn.Click += delegate(object s, EventArgs ea)
            {
                Message cmd = new Message(new List<RGBPi.Core.Model.Commands.Command>());
                cmd.commands.Add(new Fade(2f, new RGBPi.Core.Model.DataTypes.Color(1f, 1f, 1f)));
                client.Send(cmd);
            };
            ApplicationBar.Buttons.Add(abbOn);

            ApplicationBarIconButton abbOff = new ApplicationBarIconButton(new Uri("/Assets/Icons/off.png", UriKind.Relative));
            abbOff.Text = "Off";
            abbOff.Click += delegate(object s, EventArgs ea)
            {
                Message cmd = new Message(new List<RGBPi.Core.Model.Commands.Command>());
                cmd.commands.Add(new Fade(2f, new RGBPi.Core.Model.DataTypes.Color(0f, 0f, 0f)));
                client.Send(cmd);
            };
            ApplicationBar.Buttons.Add(abbOff);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMISettings = new ApplicationBarMenuItem("settings");
            appBarMISettings.Click += delegate(object s, EventArgs ea)
            {
                NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
            };
            ApplicationBar.MenuItems.Add(appBarMISettings);
        }

       

        private void btnDim_Click(object sender, RoutedEventArgs e)
        {
            Message cmd = new Message(new List<RGBPi.Core.Model.Commands.Command>());
            cmd.commands.Add(new Fade((slideDimTime.Value >= 60 ? (int)slideDimTime.Value - (((int)slideDimTime.Value) % 60) : (int)slideDimTime.Value), new RGBPi.Core.Model.DataTypes.Color(0f, 0f, 0f), new RGBPi.Core.Model.DataTypes.Color(copickDimColor.Color.R / 255f, copickDimColor.Color.G/255f, copickDimColor.Color.B/255f)));
            client.Send(cmd);
        }

        private void btnSpecialsJamaica_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void slideDimTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (slideDimTime == null)
                return;

            int s = (int)slideDimTime.Value;
            

            if(s <= 60)
                btnDim.Content = "Dim over "+s+" seconds";
            else
                btnDim.Content = "Dim over " + (s/60) + " minutes";
        }


        private void sliderPulseTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (btnPulseStart != null) btnPulseStart.Content = "start pulse (interval = " + ((int)sliderPulseTime.Value) + " second" + (sliderPulseTime.Value >= 2 ? "s" : "") + ")";
        }

        private void btnPulseStart_Click(object sender, RoutedEventArgs e)
        {
            Message cmd = new Message(new List<RGBPi.Core.Model.Commands.Command>());
            Loop loop = new Loop(new RGBPi.Core.Model.DataTypes.Condition(true), new List<Command>());
            loop.commands.Add(new Fade((float)sliderPulseTime.Value, new RGBPi.Core.Model.DataTypes.Color(copickPulseStart.Color.R / 255f, copickPulseStart.Color.G / 255f, copickPulseStart.Color.B / 255f)));
            loop.commands.Add(new Fade((float)sliderPulseTime.Value, new RGBPi.Core.Model.DataTypes.Color(copickPulseEnd.Color.R / 255f, copickPulseEnd.Color.G / 255f, copickPulseEnd.Color.B / 255f)));
            cmd.commands.Add(loop);
            client.Send(cmd);
        }

        

        private void piDim_Loaded(object sender, RoutedEventArgs e)
        {
            copickDimColor.Color = colorPicker.Color;
        }

        private void btnRF_Click(object sender, RoutedEventArgs e)
        {
            Message cmd = new Message(new List<RGBPi.Core.Model.Commands.Command>());
            Loop loop = new Loop(new RGBPi.Core.Model.DataTypes.Condition(true), new List<Command>());
            loop.commands.Add(new Fade(new RGBPi.Core.Model.DataTypes.Time(((float)slideMinTime.Value), ((float)slideMaxTime.Value)), new RGBPi.Core.Model.DataTypes.Color(((float)slideMinBrightness.Value / 100f), ((float)slideMaxBrightness.Value / 100f), ((float)slideMinBrightness.Value / 100f), ((float)slideMaxBrightness.Value / 100f), ((float)slideMinBrightness.Value / 100f), ((float)slideMaxBrightness.Value / 100f))));
            cmd.commands.Add(loop);
            client.Send(cmd);
        }

        private void cbDim_Checked(object sender, RoutedEventArgs e)
        {
            copickDimColor.Visibility = (bool)cbDim.IsChecked ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
        }


    }

}