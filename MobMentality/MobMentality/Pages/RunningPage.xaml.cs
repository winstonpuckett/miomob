using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MobMentality
{
    /// <summary>
    /// Interaction logic for SmallView.xaml
    /// </summary>
    public partial class RunningPage : Page
    {
        private MobTimer timer;
        private MobPeople people;
        private ResourceDictionary myAppDictionary;
        public bool TimeForBreak { get; set; }

        public RunningPage()
        {
            InitializeComponent();

            timer = new MobTimer();
            timer.TimerUpEvent += Timer_TimerUpEvent;

            this.Loaded += RunningPage_Loaded;
            MouseEnter += RunningPage_MouseEnter;
            MouseLeave += RunningPage_MouseLeave;

            myAppDictionary = Application.Current.Resources;

            people = myAppDictionary["People"] as MobPeople;

            Int16 time = (Int16)myAppDictionary["TimeCounter"];
            myAppDictionary["TimeCounterFriendly"] = MobTimer.ConvertTime((Int16)(time));
        }

        private void RunningPage_MouseLeave(object sender, MouseEventArgs e)
        {
            RunningGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Star);
        }

        private void RunningPage_MouseEnter(object sender, MouseEventArgs e)
        {
            RunningGrid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
        }

        private void RunningPage_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> driverNavigator = people.DriverNavigator;

            if (TimeForBreak)
            {
                myAppDictionary["TimeCounter"] = (Int16)((Int16)myAppDictionary["BreakMinutes"] * 60);

                CurrentPersonLabel.Content = "Break time";
                NextPersonLabel.Content = $"Next: {driverNavigator[0]}";
            }
            else
            {
                myAppDictionary["TimeCounter"] = (Int16)((Int16)myAppDictionary["TurnMinutes"] * 60);

                CurrentPersonLabel.Content = driverNavigator[0];
                NextPersonLabel.Content = driverNavigator[1];
            }

            timer.ResetTimer(TimeForBreak);
            timer.StartTimer();
        }

        #region Events
        public class TimerUpEventArgs : EventArgs
        {
            public TimerUpEventArgs(bool timeForBreak)
            {
                TimeForBreak = timeForBreak;
            }

            public bool TimeForBreak { get; }
        }

        public event EventHandler GoToSettingsEvent;
        public event EventHandler<TimerUpEventArgs> TimerUpEvent;

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            GoToSettingsEvent?.Invoke(this, EventArgs.Empty);
        }

        private void Timer_TimerUpEvent(object sender, MobTimer.TimerUpEventArgs e)
        {
            // If it was just a break, the next driver is already there
            if (!TimeForBreak)
            {
                this.Dispatcher.Invoke(() =>
                {
                    people.NextPerson();
                });
            }
            
            RaiseTimerUp(new TimerUpEventArgs(e.TimeForBreak));
        }

        protected virtual void RaiseTimerUp(TimerUpEventArgs e)
        {
            TimerUpEvent?.Invoke(this, e);
        }
        #endregion

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsRunning)
            {
                timer.PauseTimer();
                PauseButtonImage.Source = new BitmapImage(new Uri(@"/icons/play.png", UriKind.Relative));
            }
            else
            {
                timer.StartTimer();
                PauseButtonImage.Source = new BitmapImage(new Uri(@"/icons/pause.png", UriKind.Relative));
            }
        }

        private void SkipDriverButton_Click(object sender, RoutedEventArgs e)
        {
            people.NextPerson();

            if (timer.IsRunning)
            {
                timer.ResetTimer(false);
                timer.StartTimer();
            }
            else
            {
                timer.ResetTimer(false);
            }
            

            CurrentPersonLabel.Content = people.DriverNavigator[0];
            NextPersonLabel.Content = people.DriverNavigator[1];
        }
    }
}
