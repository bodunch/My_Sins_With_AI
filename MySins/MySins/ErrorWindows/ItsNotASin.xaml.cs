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
    /// Interaction logic for ItsNotASin.xaml
    /// </summary>
    public partial class ItsNotASin : Window
    {
        public ItsNotASin()
        {
            InitializeComponent();
        }

        private void CloseThisWindow(object sender, RoutedEventArgs e)
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
