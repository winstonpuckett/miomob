using MobMentality.Pages;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MobMentality
{
    /// <summary>
    /// Interaction logic for SwitchPersonPage.xaml
    /// </summary>
    public partial class SwitchPersonPage : Page
    {
        // TODO: Add skip button
        private ResourceDictionary myAppDictionary;
        private RetroPage retroPage;
        private MobPeople people;

        public bool TimeForBreak { get; set; }

        public SwitchPersonPage()
        {
            InitializeComponent();

            this.Loaded += SwitchPersonPage_Loaded;
            
            myAppDictionary = Application.Current.Resources;
            retroPage = new RetroPage();
            retroPage.DoneReflectingEvent += RetroPage_DoneReflectingEvent;

            people = myAppDictionary["People"] as MobPeople;
        }

        private void RetroPage_DoneReflectingEvent(object sender, EventArgs e)
        {
            RetroFrame.HorizontalAlignment = HorizontalAlignment.Center;
            RetroFrame.VerticalAlignment = VerticalAlignment.Center;
            RetroFrame.Navigate(null);
        }

        private void SwitchPersonPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (TimeForBreak)
            {
                RetroFrame.HorizontalAlignment = HorizontalAlignment.Stretch;
                RetroFrame.VerticalAlignment = VerticalAlignment.Stretch;
                RetroFrame.Navigate(retroPage);
            }

            UpdateDriverNavigatorLabels();
        }

        #region Events
        public event EventHandler GoToSettingsEvent;
        public event EventHandler StartMobbingEvent;

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            GoToSettingsEvent?.Invoke(this, EventArgs.Empty);
        }

        private void ContinueMobbingButton_Click(object sender, RoutedEventArgs e)
        {
            StartMobbingEvent?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        private void BackPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimeForBreak)
            {
                TimeForBreak = !TimeForBreak;

            }
            else
            {
                people.LastPerson();
            }

            UpdateDriverNavigatorLabels();
        }

        private void UpdateDriverNavigatorLabels()
        {
            if (TimeForBreak)
            {
                DriverLabel.Content = "It's time for a break!";
                NavigatorLabel.Content = $"Next driver is {people.DriverNavigator[0]}";
                ContinueMobbingButton.Content = "Take Break";
            }
            else
            {
                DriverLabel.Content = $"{people.DriverNavigator[0]}, please take control of the keyboard";
                NavigatorLabel.Content = $"{people.DriverNavigator[1]}, get ready to navigate";
                ContinueMobbingButton.Content = "Continue Mobbing";
            }
        }

        private void ForwardPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimeForBreak)
            {
                TimeForBreak = !TimeForBreak;
            }
            else
            {
                people.NextPerson();
            }
            
            UpdateDriverNavigatorLabels();
        }
    }
}
