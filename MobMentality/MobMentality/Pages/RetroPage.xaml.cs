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

namespace MobMentality.Pages
{
    /// <summary>
    /// Interaction logic for RetroPage.xaml
    /// </summary>
    public partial class RetroPage : Page
    {
        public RetroPage()
        {
            InitializeComponent();
        }

        public event EventHandler DoneReflectingEvent;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoneReflectingEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
