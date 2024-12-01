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
using System.Windows.Threading;

namespace mastermind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();

        SolidColorBrush[] colorSelection = [Brushes.Red, Brushes.Blue, Brushes.Green, Brushes.White, Brushes.Yellow, Brushes.Orange];
        string[] colorSelectionString = ["Red", "Blue", "Green", "White", "Yellow", "Orange"];
        int[] colorsRandom = new int[4];

        int attempts = 0;
        bool isPlaying = true;

        DispatcherTimer timer = new DispatcherTimer();
        DateTime startTime;

        public MainWindow()
        {
            InitializeComponent();
            PickColors();
            ComboBoxItemsInit();
            UpdateAttempts();
            StartCountdown();
        }

        private void PickColors()
        {
            for (int i = 0; i < colorsRandom.Length; i++)
            {
                colorsRandom[i] = rnd.Next(1, 6);
            }

            debugTextBox.Text = $"Oplossing: ({colorSelectionString[colorsRandom[0]]}, {colorSelectionString[colorsRandom[1]]}, {colorSelectionString[colorsRandom[2]]}, {colorSelectionString[colorsRandom[3]]})";
        }

        private void ComboBoxItemsInit()
        {
            color1ComboBox.ItemsSource = colorSelectionString;
            color2ComboBox.ItemsSource = colorSelectionString;
            color3ComboBox.ItemsSource = colorSelectionString;
            color4ComboBox.ItemsSource = colorSelectionString;
        }

        private void ComboBox_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.Name == "color1ComboBox")
            {
                color1Ellipse.Fill = colorSelection[comboBox.SelectedIndex];
            }
            else if (comboBox.Name == "color2ComboBox")
            {
                color2Ellipse.Fill = colorSelection[comboBox.SelectedIndex];
            }
            else if (comboBox.Name == "color3ComboBox")
            {
                color3Ellipse.Fill = colorSelection[comboBox.SelectedIndex];
            }
            else if (comboBox.Name == "color4ComboBox")
            {
                color4Ellipse.Fill = colorSelection[comboBox.SelectedIndex];
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                UpdateAttempts();
                StartCountdown();
                CheckComboBox(color1Ellipse, color1ComboBox, 0);
                CheckComboBox(color2Ellipse, color2ComboBox, 1);
                CheckComboBox(color3Ellipse, color3ComboBox, 2);
                CheckComboBox(color4Ellipse, color4ComboBox, 3);
            }
            
        }

        private void CheckComboBox(Ellipse elipse, ComboBox combobox, int number)
        {
            elipse.Stroke = Brushes.Black;
            elipse.StrokeThickness = 1;

            if (combobox.SelectedIndex == colorsRandom[number])
            {
                elipse.Stroke = Brushes.DarkRed;
                elipse.StrokeThickness = 5;
            }
            else if (colorsRandom.Contains(combobox.SelectedIndex))
            {
                elipse.Stroke = Brushes.Wheat;
                elipse.StrokeThickness = 5;
            }
        }

        private void UpdateAttempts()
        {
            attempts += 1;
            this.Title = $"Poging {attempts}";

            if (attempts >= 10)
            {
                isPlaying = false ;
            }
        }

        private void mastermind_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                ToggleDebug();
            }
        }

        /// <summary>
        /// De ToggleDebug functie togglet tussen het wel en niet
        /// tonen van een textbox met daarin de willekeurig gegenereerde oplossing
        /// </summary>
        private void ToggleDebug()
        {
            if (debugTextBox.Visibility == Visibility.Hidden)
            {
                debugTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                debugTextBox.Visibility = Visibility.Hidden;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - startTime;
            timerLabel.Content = elapsed.ToString(@"ss");

            if (elapsed.TotalSeconds > 11)
            {
                StopCountdown();
            }
        }

        /// <summary>
        /// De StartCountdown functie start een secondenteller die
        /// zal tellen vanaf 0 en door zal lopen tot 10
        /// </summary>
        private void StartCountdown()
        {
            if (isPlaying)
            {
                timerLabel.Content = "00";
                timer.Tick += Timer_Tick;
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Start();
                startTime = DateTime.Now;
            }
        }

        /// <summary>
        /// <para>De StopCountdown functie stopt de secondenteller en
        /// zal deze opnieuw laten starten.</para>
        /// <para> Na het stoppen van de voorgaande teller zal ook
        /// de UpdateAttempts functie aangeroepen worden.</para>
        /// </summary>
        private void StopCountdown()
        {
            timer.Stop();
            UpdateAttempts();
            StartCountdown();
        }
    }
}