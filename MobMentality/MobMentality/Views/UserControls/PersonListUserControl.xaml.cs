using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MobMentality.Pages.UserControls
{
    /// <summary>
    /// Interaction logic for PersonListUserControl.xaml
    /// </summary>
    public partial class PersonListUserControl
    {
        public PersonListUserControl()
        {
            InitializeComponent();
        }

        private void PeopleItemsControl_Drop(object sender, DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string dataString = (string)e.Data.GetData(DataFormats.StringFormat);

                if (Application.Current.MainWindow?.DataContext is MasterViewModel context)
                {
                    if (DataContext is ObservableCollection<string> thisList)
                    {
                        if (thisList.Contains(dataString))
                        {
                            context.MovePersonCommand.Execute(new List<string> { dataString, thisList.Last() });
                            return;
                        }
                    }

                    context.SwitchPersonStateCommand.Execute(dataString);
                }
            }
        }
    }
}
