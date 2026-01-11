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
using System.Linq;
using System.Windows.Resources;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;


//збереження  данних до БД (перший запуск?)
//вставка АПІ при першому запуску
//шифрування файлу
//перевірка чи це норм ключ


namespace MySins
{
    public partial class MainWindow : Window
    {
        public int usedTextBoxesCount = 0;
        public int sinsCount = 0;
        public string time = "";
        public bool isFirstStart = true;

        private ChatSession chatSession;

        public MainWindow()
        {
            //закомітити це нахой
            Properties.Settings.Default.IsFirstRun = true;
            Properties.Settings.Default.Save();
            
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.IsFirstRun)
            {
                if (!Directory.Exists(TakeDirectoryAPI()))
                    Directory.CreateDirectory(TakeDirectoryAPI());

                if (!File.Exists(TakeFileAPI()))
                {
                    var enterWindow = new EnterApiKeyWindow();
                    bool? res = enterWindow.ShowDialog();

                    if (res == true && IsRealApiKey(enterWindow.ApiKey))
                    {
                        File.WriteAllText(TakeFileAPI(), enterWindow.ApiKey);
                    }
                    else if(!IsRealApiKey(enterWindow.ApiKey))
                    {
                        //якщо файл з неправильним ключом то нахй його делітнути. А ще цей кусок треба перемістити бо це відтворюється тільки при першому запуску
                        File.Delete(TakeFileAPI());
                    }
                    else
                    {
                        //якщо немає ключа чота тут намутить помилку
                        Close();
                        return;
                    }
                }
                //додати це коли буде йти на прод
                //Properties.Settings.Default.IsFirstRun = false;
                //Properties.Settings.Default.Save();
            }
            //тут запуск моделі
            await GoogleModel();
        }

        private bool IsRealApiKey(string key)
        {
            return (key.Length == 39 && key.StartsWith("AIza") && !string.IsNullOrWhiteSpace(key));
        }

        public string TakeDirectoryAPI()
        {
            //шлях до директорії з апі файлом
            string appDataPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "MySins");
            return appDataPath;
        }

        public string TakeFileAPI()
        {
            //шлях до ключа
            string appDataPath = TakeDirectoryAPI();
            string apiKeyPath = System.IO.Path.Combine(appDataPath, "My_API_Key.txt");
            return apiKeyPath;
        }

        public static string LoadApiKey()
        {
            string filePath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),"MySins","My_API_Key.txt");
            return File.ReadAllText(filePath).Trim();
        }
        
        private async Task GoogleModel()
        {
            try
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
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                new MessageErrorSomethigWithGemini().Show();
            }
            
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

                if (box.Text == "")
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

                await SinToBot(text, box);
            }
            else if (usedTextBoxesCount == 10)
            {
                if (sender is not TextBox box)
                    return;
                string? text = box.Text;
                box.IsReadOnly = true;
                Debug.WriteLine("Визиваєтсья");
                await SinToBot(text, box);
            }
            usedTextBoxesCount++;
            Debug.WriteLine(usedTextBoxesCount);
        }

        private async Task SinToBot(string message, TextBox targetBox)
        {
            TextBox box = targetBox;
            GenerateContentResponse? responce = null;;
            if (chatSession == null)
            {
                return;
            }
            try 
            {
                responce = await chatSession.GenerateContentAsync($"Нагадую, відповідай лише T або F. Більше нічого. Гріх: {message}");
                string? reply = responce.Text();
                SinCounting(reply, box);

                Debug.WriteLine(reply);
            }
            catch(Exception ex)
            {
                if(ex.Message.Contains("RESOURCE_EXHAUSTED"))
                {
                    time = ParseSeconds(ex.Message);

                    RateLimitWindow rateLimit = new RateLimitWindow(time);
                    rateLimit.Show();
                    Debug.WriteLine(time);
                }
            }
        }

        private string ParseSeconds(string str)
        {
            var match = Regex.Match(str, @"Please retry in ([0-9.]+s)");
            if(match.Success)
            {
                return match.Groups[1].Value;
            }
            return "";
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
                    ItsNotASin notASin = new ItsNotASin();
                    notASin.Show();
                    box.Text = "";
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
            if(targetCount <= 3)
            {
                Smile.Visibility = Visibility.Visible;
                Normal.Visibility = Visibility.Hidden;
                Sad.Visibility = Visibility.Hidden;

                Background.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/heaven.png"));
            }
            else if(targetCount >= 4 && targetCount <= 8)
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

        public void ManConfess()
        {
            usedTextBoxesCount = 0;
            sinsCount = 0;
            Box0.Focus();
            Box0.Select(0, 0);
            List<TextBox> allTextBoxes = FindVisualChildren<TextBox>(this).ToList();
            foreach(var textbox in allTextBoxes)
            {
                textbox.IsReadOnly = false;
                textbox.Text = "";
            }
        }


        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child is T)
                        yield return (T)child;

                    foreach (T grandChild in FindVisualChildren<T>(child))
                        yield return grandChild;
                }
            }
        }
    }
}