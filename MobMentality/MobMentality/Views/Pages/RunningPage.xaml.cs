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
using MobMentality.ViewModels;

namespace MobMentality
{
    /// <summary>
    /// Interaction logic for SmallView.xaml
    /// </summary>
    public partial class RunningPage
    {
        #region Ctor

        public RunningPage()
        {
            InitializeComponent();

            Loaded += RunningPage_Loaded;
            MouseEnter += RunningPage_MouseEnter;
            MouseLeave += RunningPage_MouseLeave;
        }

        private void RunningPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is MasterViewModel m)) return;

            TurnsTakenPanel.Children.Clear();

            for (int i = 0; i < m.TurnsLeft; i++)
            {
                TurnsTakenPanel.Children.Add(new Ellipse { Fill = new SolidColorBrush(Colors.White), Width = 10, Height = 10 });
            }
        }

        #endregion

        #region Events

        #region Handler

        public event EventHandler GoToSettingsEvent;

        #endregion

        #region UI

        private void RunningPage_MouseLeave(object sender, MouseEventArgs e)
        {
            RunningGrid.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Star);
            RunningGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
            RunningGrid.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
        }

        private void RunningPage_MouseEnter(object sender, MouseEventArgs e)
        {
            RunningGrid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
            RunningGrid.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Star);
            RunningGrid.RowDefinitions[3].Height = new GridLength(0, GridUnitType.Star);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            GoToSettingsEvent?.Invoke(this, EventArgs.Empty);
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MasterViewModel m)
            {
                if (m.Timer.Enabled)
                {
                    m.PauseTimerCommand.Execute(null);
                    if (sender is Button b)
                    {
                        b.Content = "4";
                    }
                }
                else
                {
                    m.StartTimerCommand.Execute(null);
                    if (sender is Button b)
                    {
                        b.Content = ";";
                    }
                }
            }
        }

        private void SkipDriverButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MasterViewModel m)
            {
                m.ResetTimerCommand.Execute(null);
                m.NextPersonCommand.Execute(null);
                m.StartTimerCommand.Execute(null);
            }
        }

        #endregion

        #endregion
    }
}
