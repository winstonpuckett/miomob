using MobMentality.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RunningPage runningPage;
        private SettingsPage settingsPage;
        private SwitchPersonPage switchPersonPage;

        private ResourceDictionary myAppDictionary;

        private Point myWindowPosition;

        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = true;
            this.Closing += MainWindow_Closing;
            this.MouseUp += MainWindow_MouseUp;
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            this.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
            this.MouseEnter += MainWindow_MouseEnter;
            this.MouseLeave += MainWindow_MouseLeave;
            //this.LocationChanged += MainWindow_LocationChanged;

            InitializeAppDictionary();
            InitializePages();

            SwitchWindow(State.Settings);
        }
        
        //private void MainWindow_LocationChanged(object sender, EventArgs e)
        //{
        //    if (runningPage.IsLoaded && System.Windows.Input.Mouse.LeftButton != MouseButtonState.Pressed)
        //    {
        //        if (this.Top != myWindowPosition.X || this.Left != myWindowPosition.Y)
        //        {
        //            this.Top = myWindowPosition.X;
        //            this.Left = myWindowPosition.Y;
        //        }
        //    }
        //}

        #region Initialize
        private void InitializePages()
        {
            runningPage = new RunningPage();
            runningPage.GoToSettingsEvent += AnyPage_GoToSettingsEvent;
            runningPage.TimerUpEvent += RunningPage_TimerUpEvent;
            
            settingsPage = new SettingsPage();
            settingsPage.StartMobbingEvent += SettingsPage_StartMobbingEvent;

            switchPersonPage = new SwitchPersonPage();
            switchPersonPage.GoToSettingsEvent += AnyPage_GoToSettingsEvent;
            switchPersonPage.StartMobbingEvent += SwitchPersonPage_StartMobbingEvent;
        }

        private void InitializeAppDictionary()
        {
            // Gets state from user settings
            myAppDictionary = Application.Current.Resources;

            MobPeople people = new MobPeople();
            StringCollection activeStringCollection = MobSettings.Default.ActivePeople;
            if (!(activeStringCollection is null))
                people.ActivePeople = new ObservableCollection<string>(activeStringCollection.Cast<string>().ToList());
            StringCollection inactiveStringCollection = MobSettings.Default.InactivePeople;
            if (!(inactiveStringCollection is null))
                people.InactivePeople = new ObservableCollection<string>(inactiveStringCollection.Cast<string>().ToList());
            myAppDictionary["People"] = people;

            myAppDictionary["TurnMinutes"] = (Int16)MobSettings.Default.TurnMinutes;
            myAppDictionary["BreakMinutes"] = (Int16)MobSettings.Default.BreakMinutes;
            myAppDictionary["TurnsTillBreak"] = (Int16)MobSettings.Default.TurnsTillBreak;
        }
        #endregion

        #region Methods
        private void SwitchWindow(State state, bool timeForBreak = false)
        {
            this.Dispatcher.Invoke(() =>
            {
                switch (state)
                {
                    case State.SwitchPerson:
                        this.WindowState = WindowState.Maximized;
                        switchPersonPage.TimeForBreak = timeForBreak;
                        MainFrame.Navigate(switchPersonPage);
                        break;
                    case State.Running:
                        this.WindowState = WindowState.Normal;
                        this.Width = 140;
                        this.Height = 140;

                        runningPage.TimeForBreak = timeForBreak;
                        MainFrame.Navigate(runningPage);
                        break;
                    case State.Settings:
                        this.WindowState = WindowState.Maximized;
                        MainFrame.Navigate(settingsPage);
                        break;
                }
            });
        }
        #endregion Methods

        #region Events
        private void SettingsPage_StartMobbingEvent(object sender, object e)
        {
            SwitchWindow(State.Running);
        }

        private void SwitchPersonPage_StartMobbingEvent(object sender, EventArgs e)
        {
            SwitchWindow(State.Running, switchPersonPage.TimeForBreak);
        }

        private void RunningPage_TimerUpEvent(object sender, RunningPage.TimerUpEventArgs e)
        {
            SwitchWindow(State.SwitchPerson, e.TimeForBreak);
        }

        private void AnyPage_GoToSettingsEvent(object sender, EventArgs e)
        {
            SwitchWindow(State.Settings);
        }
        
        private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Width = 101;
            this.Height = 101;
        }

        private void MainWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Width = 140;
            this.Height = 140;
        }
        
        private void RunningPage_Loaded(object sender, RoutedEventArgs e)
        {
            Left = myWindowPosition.X;
            Top = myWindowPosition.Y;
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (runningPage.IsLoaded)
            {
                // TODO: Remove this code if it fixes the drag bug
                // TODO: Wire this to drag move instead of left button
                //myWindowPosition = new Point(Left, Top);

                DragMove();
            }
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (runningPage.IsLoaded)
            {
                myWindowPosition = new Point(Left, Top);
            }
        }

        private void MainWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!(this.WindowState == WindowState.Maximized) && !runningPage.IsLoaded)
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Saves state to user settings
            MobPeople people = myAppDictionary["People"] as MobPeople;
            StringCollection activeStringCollection = new StringCollection();
            foreach (string name in people.ActivePeople)
            {
                activeStringCollection.Add(name);
            }
            StringCollection inactiveStringCollection = new StringCollection();
            foreach (string name in people.InactivePeople)
            {
                inactiveStringCollection.Add(name);
            }

            MobSettings.Default.ActivePeople = activeStringCollection;
            MobSettings.Default.InactivePeople = inactiveStringCollection;

            MobSettings.Default.TurnMinutes = (Int16)myAppDictionary["TurnMinutes"];
            MobSettings.Default.BreakMinutes = (Int16)myAppDictionary["BreakMinutes"];
            MobSettings.Default.TurnsTillBreak = (Int16)myAppDictionary["TurnsTillBreak"];

            MobSettings.Default.Save();
        }
        #endregion Events

        #region Enums
        enum State { SwitchPerson, Running, Settings }
        #endregion
    }
}
