using MobMentality.Pages;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MobMentality.ViewModels;

namespace MobMentality
{
    /// <summary>
    /// Interaction logic for SwitchPersonPage.xaml
    /// </summary>
    public partial class SwitchPersonPage
    {
        private RetroPage retroPage;

        public SwitchPersonPage()
        {
            InitializeComponent();

            this.Loaded += SwitchPersonPage_Loaded;

            retroPage = new RetroPage();
            retroPage.DoneReflectingEvent += RetroPage_DoneReflectingEvent;

            //people = myAppDictionary["People"] as MobPeople;
        }

        #region Events

        public event EventHandler GoToSettingsEvent;
        public event EventHandler StartMobbingEvent;

        private void RetroPage_DoneReflectingEvent(object sender, EventArgs e)
        {
            RetroFrame.HorizontalAlignment = HorizontalAlignment.Center;
            RetroFrame.VerticalAlignment = VerticalAlignment.Center;
            RetroFrame.Navigate(null);
        }

        private void SwitchPersonPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MasterViewModel m && m.TimeForBreak)
            {
                RetroFrame.HorizontalAlignment = HorizontalAlignment.Stretch;
                RetroFrame.VerticalAlignment = VerticalAlignment.Stretch;
                RetroFrame.Navigate(retroPage);
            }

            UpdateDriverNavigatorLabels();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            GoToSettingsEvent?.Invoke(this, EventArgs.Empty);
        }

        private void ContinueMobbingButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MasterViewModel m)
            {
                m.ResetTimerCommand.Execute(null);
                m.StartTimerCommand.Execute(null);
            }

            StartMobbingEvent?.Invoke(this, EventArgs.Empty);
        }


        private void BackPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MasterViewModel m)
            {
                if (m.TimeForBreak)
                {
                    m.TimeForBreak = false;
                }
                else
                {
                    m.LastPersonCommand.Execute(null);
                }
            }

            UpdateDriverNavigatorLabels();
        }

        private void UpdateDriverNavigatorLabels()
        {
            if (DataContext is MasterViewModel m)
            {
                if (m.TimeForBreak)
                {
                    DriverLabel.Content = "It's time for a break!";
                    NavigatorLabel.Content = $"Next driver is {m.Driver}";
                    ContinueMobbingButton.Content = "Take Break";
                }
                else
                {
                    DriverLabel.Content = $"{m.Driver}, please take control of the keyboard";
                    NavigatorLabel.Content = $"{m.Navigator}, get ready to navigate";
                    ContinueMobbingButton.Content = "Continue Mobbing";
                }
            }

        }

        private void ForwardPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MasterViewModel m)
            {
                if (m.TimeForBreak)
                {
                    m.TimeForBreak = false;
                }
                else
                {
                    m.NextPersonCommand.Execute(null);
                }
            }

            UpdateDriverNavigatorLabels();
        }

        #endregion
    }
}
