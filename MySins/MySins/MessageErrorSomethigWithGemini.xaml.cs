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
    /// Interaction logic for MessageErrorSomethigWithGemini.xaml
    /// </summary>
    public partial class MessageErrorSomethigWithGemini : Window
    {
        public MessageErrorSomethigWithGemini()
        {
            InitializeComponent();
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

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true
            };

            System.Diagnostics.Process.Start(psi);
            e.Handled = true;
        }
    }
}

