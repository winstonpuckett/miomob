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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using MobMentality.Settings;
using MobMentality.ViewModels;

namespace MobMentality
{
    /// <summary>
    /// Interaction logic for BigView.xaml
    /// </summary>
    public partial class SettingsPage
    {
        #region Ctor

        public SettingsPage()
        {
            InitializeComponent();

            NewPersonTextBox.KeyDown += NewPersonTextBox_KeyDown;
        }

        #endregion

        #region Events

        private void NewPersonTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddPerson();
            }
        }

        public event EventHandler<object> StartMobbingEvent;

        protected virtual void RaiseStartMobbing(bool startOver)
        {
            if (!(DataContext is MasterViewModel m)) return;

            if (startOver || m.TimeLeft < 1 || m.TimeLeft > (m.TurnMinutes * 60))
            {

                m.ResetTimerCommand.Execute(null);

                if (startOver)
                {
                    m.ResetBreaksCommand.Execute(null);
                }
            }

            m.StartTimerCommand.Execute(null);

            StartMobbingEvent?.Invoke(this, EventArgs.Empty);
        }

        #region UI Click Events

        private void StartMobbingButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseStartMobbing(true);
        }

        private void ContinueMobbingButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseStartMobbing(false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsExportImport.Save(DataContext);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsExportImport.Load();
        }

        private void AddPersonButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddPerson();
        }

        #endregion UI Click Events

        #endregion Events

        #region Methods

        private void AddPerson()
        {
            if (DataContext is MasterViewModel m)
            {
                m.AddPersonCommand.Execute(NewPersonTextBox.Text);
                NewPersonTextBox.Clear();
            }
        }

        #endregion
    }
}
