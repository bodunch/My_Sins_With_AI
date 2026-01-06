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
using System.Windows.Shapes;

namespace MySins
{
    /// <summary>
    /// Interaction logic for YesNoConfess.xaml
    /// </summary>
    public partial class YesNoConfess : Window
    {
        public YesNoConfess()
        {
            InitializeComponent();
        }

        private void YepImmaConfess(object sender, RoutedEventArgs e)
        {
            if(Owner is MainWindow mainWindow)
            {
                mainWindow.ManConfess();
            }
            CloseAndEnabledWindow();
        }

        private void NoImmaNotConfess(object sender, RoutedEventArgs e)
        {
            CloseAndEnabledWindow();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            CloseAndEnabledWindow();
        }

        private void CloseAndEnabledWindow()
        {
            if (Owner is MainWindow mainWindow)
            {
                mainWindow.ImmaConfess.IsEnabled = true;
            }
            Close();
        }
    }
}
