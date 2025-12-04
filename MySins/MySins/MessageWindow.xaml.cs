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
                // розкриваємо вікно повністю
                this.Height = 678;   // або будь-яка потрібна висота
                HiddenRow.Height = new GridLength(3, GridUnitType.Star); // робимо прихований рядок видимим
                ShowHiddenPart.Content = "Згорнути"; // змінюємо текст кнопки
                isExpanded = true;
            }
            else
            {
                // згортаємо назад
                this.Height = 200;   // початкова висота
                HiddenRow.Height = new GridLength(0); // прихований рядок знову 0
                ShowHiddenPart.Content = "Показати";
                isExpanded = false;
            }
        }
    }
}
