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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace MySins
{
    /// <summary>
    /// Interaction logic for EnterApiKeyWindow.xaml
    /// </summary>
    public partial class EnterApiKeyWindow : Window
    {
        public string ApiKey { get; private set; } = "";

        public EnterApiKeyWindow()
        {
            InitializeComponent();
        }

        private void Approved_APIKey(object sender, RoutedEventArgs e)
        {
            string key = EnterAPI.Text.Trim();
            if(IsRealApiKey(key))
            {
                ApiKey = EnterAPI.Text.Trim();
                DialogResult = true;
                Close();
            }
        }

        private bool IsRealApiKey(string apiKey)
        {
            if(!(apiKey.Length == 39) || !(apiKey.StartsWith("AIza")) || (string.IsNullOrWhiteSpace(apiKey)))
            {
                IsNotRealApiKey notApi = new IsNotRealApiKey();
                notApi.Show();
                ApiKey = "";
                EnterAPI.Text = "";
                return false;
            }
            else
            {
                return true;
            }
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
