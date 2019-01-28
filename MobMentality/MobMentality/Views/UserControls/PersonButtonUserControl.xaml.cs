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

namespace MobMentality.Pages.UserControls
{
    /// <summary>
    /// Interaction logic for PersonButtonUserControl.xaml
    /// </summary>
    public partial class PersonButtonUserControl : UserControl
    {
        // TODO: Remove dead code.
        private MobPeople mobPeople;
        private ResourceDictionary myAppDictionary;

        public PersonButtonUserControl()
        {
            myAppDictionary = Application.Current.Resources;
            mobPeople = myAppDictionary["People"] as MobPeople;

            InitializeComponent();
        }

        // TODO: Translate this to up and down arrows next to active people
        //private void SetCurrentPersonButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string name = (sender as Button).Content.ToString();

        //    if (mobPeople.ActivePeople.Contains(name))
        //    {
        //        mobPeople.RotateToPerson(name);
        //    }
        //    else if (mobPeople.InactivePeople.Contains(name))
        //    {
        //        mobPeople.SwitchPersonState(name);
        //    }
        //}

        private void SwitchActiveStateButton_Click(object sender, RoutedEventArgs e)
        {
            string name = SetCurrentPersonButton.Content.ToString();

            mobPeople.RemovePerson(name);

            // TODO: Remove dead code.
            //if (mobPeople.ActivePeople.Contains(name))
            //{
            //    mobPeople.SwitchPersonState(name);
            //}
            //else if (mobPeople.InactivePeople.Contains(name))
            //{
            //    mobPeople.RemovePerson(name);
            //}
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            StartDragging(e);
        }

        private void StartDragging(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData(DataFormats.StringFormat, SetCurrentPersonButton.Content.ToString());
                data.SetData("Object", this);

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string dataString = (string)e.Data.GetData(DataFormats.StringFormat);
                //int dataPosition = mobPeople.ActivePeople.IndexOf(SetCurrentPersonButton.Content.ToString());
                mobPeople.MoveActivePerson(dataString, SetCurrentPersonButton.Content.ToString());

                e.Handled = true;
            }
        }

        private void SetCurrentPersonButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            StartDragging(e);
        }
    }
}
