using GenerativeAI;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;

namespace MySins
{
    public partial class MainWindow : Window
    {
        public int sinsCount = 0;
        private ChatSession chatSession;

        public MainWindow()
        {
            InitializeComponent();
            _ = GoogleModel();

        }

        public static string LoadApiKey()
        {
            string filePath = "My_API_Key.txt";
            if(!File.Exists(filePath))
            {
                return "";
            }
            return File.ReadAllText(filePath).Trim();
        }

        private async Task GoogleModel()
        {
            string ApiKeyFromFile = LoadApiKey();
            var googleApiKey = new GoogleAi(ApiKeyFromFile);
            var model = googleApiKey.CreateGenerativeModel("models/gemini-2.0-flash-lite");
            chatSession = model.StartChat();
            var baseResponse = await chatSession.GenerateContentAsync("Я зараз буду описувати тобі гріхи по черзі, а ти мусиш відповідати лише T/F чи це є гріхом чи ні. Надавай відповіді згідно християнської етики. У відповідь на це повідомлення дай лише відповідь - Зрозуміло. А далі лише відповідай T або F і нічого інакше. ");
            Debug.WriteLine(baseResponse.Text());
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).ScrollToHome();
        }

        private async void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                TextBox_LostFocus(sender, e);
                var request = new TraversalRequest(FocusNavigationDirection.Next);

                TextBox box = sender as TextBox;
                string text = box.Text;
                box.IsReadOnly = true;

                UIElement element = sender as UIElement;
                if (element != null)
                {
                    element.MoveFocus(request);
                }
                await SinToBot(text, box);
            }
        }

        private async Task SinToBot(string message, TextBox targetBox)
        {
            TextBox box = targetBox;

            if(chatSession == null)
            {
                return;
            }

            var responce = await chatSession.GenerateContentAsync(message);

            string reply = responce.Text();
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
                    SmileChanges(sinsCount);
                }
                else if (answer.Trim() == "F")
                {
                    box.IsReadOnly = false;
                }
                else
                {
                    //вікно з введи гріх нормально даун
                }
            }
        }

        private void SmileChanges(int targetCount)
        {
            if(targetCount <= 4)
            {
                Smile.Visibility = Visibility.Visible;
                Normal.Visibility = Visibility.Hidden;
                Sad.Visibility = Visibility.Hidden;
            }
            else if(targetCount >= 5 && targetCount <= 8)
            {
                Smile.Visibility = Visibility.Hidden;
                Normal.Visibility = Visibility.Visible;
            }
            else
            {
                Normal.Visibility = Visibility.Hidden;
                Sad.Visibility = Visibility.Visible;
            }
        }
    }
}