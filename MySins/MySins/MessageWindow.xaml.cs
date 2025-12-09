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
    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            InitializeComponent();
        }

        private bool isExpanded = false;

        private void Toggle_Click(object sender, RoutedEventArgs e)
        {
            if (!isExpanded)
            {
                this.Height = 400;   
                HiddenRow.Height = new GridLength(3, GridUnitType.Star); 
                ShowHiddenPart.Content = "Згорнути"; 
                isExpanded = true;
            }
            else
            {
                this.Height = 200;   
                HiddenRow.Height = new GridLength(0); 
                ShowHiddenPart.Content = "Показати";
                isExpanded = false;
            }
        }
    }
}
