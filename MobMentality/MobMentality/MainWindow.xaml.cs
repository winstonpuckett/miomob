using MobMentality.Pages;
using MobMentality.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
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
using MobMentality.ViewModels;
using Newtonsoft.Json;

namespace MobMentality
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RunningPage _runningPage;
        private SettingsPage _settingsPage;
        private SwitchPersonPage _switchPersonPage;

        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = true;
            this.Closing += MainWindow_Closing;
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            this.MouseEnter += MainWindow_MouseEnter;
            this.MouseLeave += MainWindow_MouseLeave;
            this.DataContextChanged += MainWindow_DataContextChanged;

            InitializeSession();
            InitializePages();

            SwitchWindow(State.Settings);
        }

        private void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is null ||
                _runningPage is null ||
                _settingsPage is null ||
                _switchPersonPage is null) return;

            if (!(DataContext is MasterViewModel m)) return;

            m.TimerUp += MOnTimerUp;

            _runningPage.DataContext = this.DataContext;
            _settingsPage.DataContext = this.DataContext;
            _switchPersonPage.DataContext = this.DataContext;
        }

        private void MOnTimerUp(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (DataContext is MasterViewModel m)
                    SwitchWindow(State.SwitchPerson);
            });
        }

        #region Initialize
        private void InitializePages()
        {
            _runningPage = new RunningPage();
            _runningPage.GoToSettingsEvent += AnyPage_GoToSettingsEvent;
            _runningPage.DataContext = this.DataContext;

            _settingsPage = new SettingsPage();
            _settingsPage.StartMobbingEvent += SettingsPage_StartMobbingEvent;
            _settingsPage.DataContext = this.DataContext;

            _switchPersonPage = new SwitchPersonPage();
            _switchPersonPage.GoToSettingsEvent += AnyPage_GoToSettingsEvent;
            _switchPersonPage.StartMobbingEvent += SwitchPersonPage_StartMobbingEvent;
            _switchPersonPage.DataContext = this.DataContext;
        }

        private void InitializeSession()
        {
            string JsonSettings = MobSettings.Default.ModelJson;

            if (string.IsNullOrWhiteSpace(JsonSettings)) return;

            if (JsonConvert.DeserializeObject<MasterViewModel>(JsonSettings) is MasterViewModel m)
            {
                DataContext = m;
                
                m.TimerUp += MOnTimerUp;
            }
        }

        #endregion

        #region Methods

        private void SwitchWindow(State state)
        {
            this.Dispatcher.Invoke(() =>
            {
                switch (state)
                {
                    case State.SwitchPerson:
                        this.WindowState = WindowState.Maximized;
                        MainFrame.Navigate(_switchPersonPage);
                        break;
                    case State.Running:
                        this.WindowState = WindowState.Normal;
                        MainFrame.Navigate(_runningPage);
                        break;
                    case State.Settings:
                        this.WindowState = WindowState.Maximized;
                        MainFrame.Navigate(_settingsPage);
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
            SwitchWindow(State.Running);
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

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MobSettings.Default.ModelJson = JsonConvert.SerializeObject(DataContext);

            MobSettings.Default.Save();
        }

        #endregion Events

        #region Enums

        enum State { SwitchPerson, Running, Settings }

        #endregion

        private void MainFrame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Forward || e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }
    }
}
