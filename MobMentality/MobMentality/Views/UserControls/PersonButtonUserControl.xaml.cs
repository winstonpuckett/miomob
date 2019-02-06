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

namespace MobMentality.Pages.UserControls
{
    /// <summary>
    /// Interaction logic for PersonButtonUserControl.xaml
    /// </summary>
    public partial class PersonButtonUserControl : UserControl
    {
        public PersonButtonUserControl()
        {
            InitializeComponent();
        }

        private void SwitchActiveStateButton_Click(object sender, RoutedEventArgs e)
        {
            string name = SetCurrentPersonButton.Content.ToString();

            if (Application.Current.MainWindow?.DataContext is MasterViewModel m)
            {
                m.RemovePersonCommand.Execute(name);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            StartDragging(e);
        }

        private void SetCurrentPersonButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
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

                if (Application.Current.MainWindow?.DataContext is MasterViewModel context)
                {
                    string origin = dataString;
                    string target = SetCurrentPersonButton.Content.ToString();
                    List<string> list = new List<string> { origin, target };

                    context.MovePersonCommand.Execute(list);
                }

                e.Handled = true;
            }
        }

    }
}
