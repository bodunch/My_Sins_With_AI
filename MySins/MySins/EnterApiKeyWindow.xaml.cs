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
            ApiKey = EnterAPI.Text.Trim();
            DialogResult = true;
            Close();
        }

    }
}
