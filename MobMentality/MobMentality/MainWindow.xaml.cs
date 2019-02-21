using MobMentality.Settings;
using MobMentality.ViewModels;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MobMentality
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        private RunningPage _runningPage;
        private SettingsPage _settingsPage;
        private SwitchPersonPage _switchPersonPage;

        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = true;
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            this.MouseEnter += MainWindow_MouseEnter;
            this.MouseLeave += MainWindow_MouseLeave;
            this.DataContextChanged += MainWindow_DataContextChanged;
            this.SizeChanged += MainWindow_SizeChanged;
            this.StateChanged += MainWindow_StateChanged;

            InitializeSession();
            InitializePages();

            SwitchWindow(State.Settings);
        }

        

        #region Initialize

        private void InitializeSession()
        {
            string jsonSettings = MobSettings.Default.ModelJson;

            if (string.IsNullOrWhiteSpace(jsonSettings)) return;

            if (JsonConvert.DeserializeObject<MasterViewModel>(jsonSettings) is MasterViewModel m)
            {
                DataContext = m;

                m.TimerUp += MOnTimerUp;
            }
        }

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
        
        #endregion

        #region Methods

        private void SetWindowSize()
        {
            this.Width = 101;
            this.Height = 101;
        }

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

        #region MainWindow

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetWindowSize();
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            SetWindowSize();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetWindowSize();
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
                //if (DataContext is MasterViewModel m)
                    SwitchWindow(State.SwitchPerson);
            });
        }

        private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            SetWindowSize();
        }

        private void MainWindow_MouseEnter(object sender, MouseEventArgs e)
        {
            SetWindowSize();
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
        
        private void MainFrame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            SetWindowSize();

            if (e.NavigationMode == NavigationMode.Forward || e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #endregion Events

        #region Enums

        enum State { SwitchPerson, Running, Settings }

        #endregion

    }
}
