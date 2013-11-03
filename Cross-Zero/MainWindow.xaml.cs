using System.Windows;
using System.Windows.Controls;

namespace Cross_Zero
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            menuCanvas.Visibility = Visibility.Hidden;
            optionsCanvas.Visibility = Visibility.Visible;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            menuCanvas.Visibility = Visibility.Hidden;
            optionsCanvas.Visibility = Visibility.Hidden;
            gameCanvas.Visibility = Visibility.Visible;
            UIController.Instance.Canvas = gameCanvas;
            GameController.Instance.CellWidth = 24;
            ComboBoxItem cbi = (ComboBoxItem) sizeComboBox.ItemContainerGenerator.ContainerFromIndex(sizeComboBox.SelectedIndex);
            GameController.Instance.StartGame(int.Parse((string)cbi.Content));
        }
    }
}
