using GenerativeAI;
using GenerativeAI.Types;
using System.Diagnostics;
using System.IO;
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

namespace MySins
{
    public partial class MainWindow : Window
    {
        public int usedTextBoxesCount = 0;
        public int sinsCount = 0;
        private ChatSession chatSession;

        public MainWindow()
        {
            //надбудова щоб ця хуйня не запускала основне вікно без перевірки
            if (!File.Exists("My_API_Key.txt"))
            {
                new MessageWindowNoAPIKey().Show();
                Close();
                return;
            }
           
             InitializeComponent();
             _ = GoogleModel();
        }

        public static string LoadApiKey()
        {
            string filePath = "My_API_Key.txt";
            return File.ReadAllText(filePath).Trim();
        }

        private async Task GoogleModel()
        {
            string ApiKeyFromFile = LoadApiKey();
            var googleApiKey = new GoogleAi(ApiKeyFromFile);
            var model = googleApiKey.CreateGenerativeModel("models/gemini-2.5-flash");
            chatSession = model.StartChat(new List<Content>
            {
                new Content
                {
                    Role = "user",
                    Parts = new List<Part>{new Part{Text ="Ти відповідаєш виключно 'T' або 'F'. Ніколи не додаєш пояснень. Якщо питання не дозволяє відповісти 'T/F' — відповідай 'F'."}}
                }
            });
            var baseResponse = await chatSession.GenerateContentAsync("Я зараз буду описувати тобі гріхи по черзі, а ти мусиш відповідати лише T/F чи це є гріхом чи ні. Надавай відповіді згідно християнської етики. У відповідь на це повідомлення дай лише відповідь - Зрозуміло. А далі лише відповідай T або F і нічого інакше. ");
            Debug.WriteLine(baseResponse.Text());
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox box)
            {
                box.ScrollToHome();
            }
        }

        private async void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (usedTextBoxesCount < 9)
            {
                if (e.Key != Key.Enter)
                    return;

                if (sender is not TextBox box)
                    return;

                TextBox_LostFocus(sender, e);
                var request = new TraversalRequest(FocusNavigationDirection.Next);

                string? text = box.Text;
                box.IsReadOnly = true;

                UIElement? element = sender as UIElement;
                if (element != null && element is TextBox)
                {
                    element.MoveFocus(request);
                    
                    
                }

                //await SinToBot(text, box);
            }
            else if (usedTextBoxesCount == 10)
            {
                if (sender is not TextBox box)
                    return;
                string? text = box.Text;
                box.IsReadOnly = true;
                Debug.WriteLine("Визиваєтсья");
                //await SinToBot(text, box);
            }
            usedTextBoxesCount++;
            Debug.WriteLine(usedTextBoxesCount);
        }

        private async Task SinToBot(string message, TextBox targetBox)
        {
            TextBox box = targetBox;

            if(chatSession == null)
            {
                return;
            }

            var responce = await chatSession.GenerateContentAsync($"Нагадую, відповідай лише T або F. Більше нічого. Гріх: {message}");

            string? reply = responce.Text();
            SinCounting(reply, box);

            Debug.WriteLine(reply);
        }

        private void SinCounting(string answer, TextBox targetBox)
        {
            TextBox box = targetBox;
            if(answer != null)
            {
                if (answer.Trim() == "T")
                {
                    sinsCount++;
                    Debug.WriteLine(sinsCount);
                    EmojesAndBackgroundChanges(sinsCount);
                }
                else if (answer.Trim() == "F")
                {
                    box.IsReadOnly = false;
                }
                else
                {
                    //це кароч тут якщо геміні ахуєє і видасть якесь дерьмо
                    MessageWindow openWindow = new MessageWindow();
                    openWindow.Show();
                }
            }
        }

        private void EmojesAndBackgroundChanges(int targetCount)
        {
            if(targetCount <= 4)
            {
                Smile.Visibility = Visibility.Visible;
                Normal.Visibility = Visibility.Hidden;
                Sad.Visibility = Visibility.Hidden;

                Background.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/heaven.png"));
            }
            else if(targetCount >= 5 && targetCount <= 8)
            {
                Smile.Visibility = Visibility.Hidden;
                Normal.Visibility = Visibility.Visible;

                Background.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/so so.png"));
            }
            else
            {
                Normal.Visibility = Visibility.Hidden;
                Sad.Visibility = Visibility.Visible;

                Background.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/hell.png"));
            }
        }

        private void ImmaConfess_Click(object sender, RoutedEventArgs e)
        {
            YesNoConfess window = new YesNoConfess();
            window.Owner = this;
            window.Show();
            ImmaConfess.IsEnabled = false;
        }

        private void ProgramExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void WindowControl(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}