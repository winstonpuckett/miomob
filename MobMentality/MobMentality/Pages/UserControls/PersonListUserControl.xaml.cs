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
    /// Interaction logic for PersonListUserControl.xaml
    /// </summary>
    public partial class PersonListUserControl : UserControl
    {
        private MobPeople mobPeople;
        private ResourceDictionary myAppDictionary;

        public PersonListUserControl()
        {
            myAppDictionary = Application.Current.Resources;
            mobPeople = myAppDictionary["People"] as MobPeople;

            InitializeComponent();
            
        }
        
        private void PeopleItemsControl_Drop(object sender, DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string dataString = (string)e.Data.GetData(DataFormats.StringFormat);

                var s = sender as Grid;
                var parent = s.Parent as PersonListUserControl;

                if (parent.Name == "ActivePeopleItemsControl")
                {
                    if (mobPeople.ActivePeople.Contains(dataString))
                    {
                        mobPeople.RemovePerson(dataString);
                        mobPeople.AddActivePerson(dataString);
                    }
                    else
                    {
                        mobPeople.SwitchPersonState(dataString);
                    }
                }
                else if (parent.Name == "InactivePeopleItemsControl")
                {
                    if (mobPeople.InactivePeople.Contains(dataString))
                    {
                        mobPeople.InactivePeople.Move(mobPeople.InactivePeople.IndexOf(dataString), mobPeople.InactivePeople.Count - 1);
                    }
                    else
                    {
                        mobPeople.SwitchPersonState(dataString);
                    }
                }
            }
        }
    }
}
