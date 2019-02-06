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
    public partial class SettingsPage : Page
    {
        //private ResourceDictionary myAppDictionary;
        //private MobPeople mobPeople;

        public SettingsPage()
        {
            InitializeComponent();

            NewPersonTextBox.KeyDown += NewPersonTextBox_KeyDown;
        }
        #region Events

        private void NewPersonTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddPerson();
            }
        }
        
        public event EventHandler<object> StartMobbingEvent;

        protected virtual void RaiseStartMobbing()
        {
            if (DataContext is MasterViewModel m)
            {
                m.ResetTimerCommand.Execute(null);
                m.ResetBreaksCommand.Execute(null);
                m.StartTimerCommand.Execute(null);
            }

            StartMobbingEvent?.Invoke(this, EventArgs.Empty);
        }

        #region UI Click Events

        private void StartMobbingButton_Click(object sender, RoutedEventArgs e)
        {
            //// TODO: This line makes sure that the MobTimer uses the correct BreakCounter value the first time. It belongs somewhere else
            //myAppDictionary["BreakCounter"] = myAppDictionary["TurnsTillBreak"];
            RaiseStartMobbing();
        }

        //private void TimerPlusButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //IncDecDictionary("TurnMinutes", true);
        //}

        //private void TimerMinusButton_Click(object sender, RoutedEventArgs e)
        //{
        //    IncDecDictionary("TurnMinutes", false);
        //}

        //private void BreakMinutesPlusButton_Click(object sender, RoutedEventArgs e)
        //{
        //    IncDecDictionary("BreakMinutes", false);
        //}

        //private void BreakMinutesPlusButton_Click_1(object sender, RoutedEventArgs e)
        //{
        //    IncDecDictionary("BreakMinutes", true);
        //}

        //private void TurnsTillBreakPlusButton_Click(object sender, RoutedEventArgs e)
        //{
        //    IncDecDictionary("TurnsTillBreak", true);
        //}

        //private void TurnsTillBreakMinusButton_Click(object sender, RoutedEventArgs e)
        //{
        //    IncDecDictionary("TurnsTillBreak", false);
        //}

        //private void AddPersonButton_Click(object sender, RoutedEventArgs e)
        //{
        //    AddPerson();
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NextPersonButton_Click(object sender, RoutedEventArgs e)
        {
            //mobPeople.NextPerson();
        }

        private void LastPersonButton_Click(object sender, RoutedEventArgs e)
        {
            //mobPeople.LastPerson();
        }

        private void RandomizeButton_Click(object sender, RoutedEventArgs e)
        {
            //mobPeople.RandomizeActive();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {


            //List<string> mobbers = new List<string>();
            //mobbers.AddRange(mobPeople.ActivePeople);
            //mobbers.AddRange(mobPeople.InactivePeople);

            //SettingsModel currentSettings = new SettingsModel
            //{
            //    Mobbers = mobbers,
            //    TurnMinutes = (Int16)myAppDictionary["TurnMinutes"],
            //    BreakMinutes = (Int16)myAppDictionary["BreakMinutes"],
            //    BreakTurns = (Int16)myAppDictionary["TurnsTillBreak"]
            //};

            SettingsExportImport.Save(DataContext);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsExportImport.Load();
        }

        #endregion UI Click Events

        //private void AddPerson()
        //{
        //bool success = mobPeople.AddActivePerson(NewPersonTextBox.Text);

        //if (success)
        //{
        //    ErrorMessageOnAddLabel.Content = "";
        //    NewPersonTextBox.Text = "";
        //}
        //else
        //{
        //    ErrorMessageOnAddLabel.Content = "* Enter a name not in the list of mobbers";
        //}
        //}


        #endregion Events

        private void AddPersonButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddPerson();
        }

        private void AddPerson()
        {
            if (DataContext is MasterViewModel m)
            {
                m.AddPersonCommand.Execute(NewPersonTextBox.Text);
                NewPersonTextBox.Clear();
            }
        }
    }
}
