using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Cross_Zero.Logic;
using Cross_Zero.Network;

namespace Cross_Zero
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int savedFieldSize = 5;

        public MainWindow()
        {
            InitializeComponent();

            UIController.Instance.Canvas = gameCanvas;
            UIController.Instance.ActivePlayerLabel = ActivePlayerLabel;
            //UIController.Instance.LinePos = LinePosLabel;
            UIController.Instance.ConnectedListBox = ConnectedListBox;
            UIController.Instance.StartGameButton = StartNetGameButton;
            UIController.Instance.ConnectionListCanvas = ConnectedListCanvas;
        }
        
        private void OnEndCreateGame()
        {
            menuCanvas.Visibility = Visibility.Hidden;
            optionsCanvas.Visibility = Visibility.Hidden;
            gameCanvas.Visibility = Visibility.Visible;
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            SelectMulGameCanvas.Visibility = Visibility.Hidden;
            optionsCanvas.Visibility = Visibility.Visible;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            GameController.Instance.EndCreateGame += OnEndCreateGame;

            ComboBoxItem cbi = (ComboBoxItem) sizeComboBox.ItemContainerGenerator.ContainerFromIndex(sizeComboBox.SelectedIndex);
            if (cbi == null) return;
            GameController.Instance.StartGame(int.Parse((string)cbi.Content));
            UIController.Instance.StartListenNextTurnEvent();

        }

        private void StartMulGameButton_Click(object sender, RoutedEventArgs e)
        {
            menuCanvas.Visibility = Visibility.Hidden;
            SelectMulGameCanvas.Visibility = Visibility.Visible;
        }

        private void SingleGameButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Start single player game with AI
        }

        private void NetworkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectMulGameCanvas.Visibility = Visibility.Hidden;
            NetworkMenuCanvas.Visibility = Visibility.Visible;
        }

        private void BackButtonSelMulGame_Click(object sender, RoutedEventArgs e)
        {
            SelectMulGameCanvas.Visibility = Visibility.Hidden;
            menuCanvas.Visibility = Visibility.Visible;
        }

        private void BackButtonNetMenu_Click(object sender, RoutedEventArgs e)
        {
            NetworkMenuCanvas.Visibility = Visibility.Hidden;
            SelectMulGameCanvas.Visibility = Visibility.Visible;
        }

        private void BackButtonCreateMulGame_Click(object sender, RoutedEventArgs e)
        {
            CreateMulGameCanvas.Visibility = Visibility.Hidden;
            NetworkMenuCanvas.Visibility = Visibility.Visible;
        }

        private void BackButtonOptGame_Click(object sender, RoutedEventArgs e)
        {
            optionsCanvas.Visibility = Visibility.Hidden;
            SelectMulGameCanvas.Visibility = Visibility.Visible;
        }

        private void CreateMulGameButton_Click(object sender, RoutedEventArgs e)
        {
            NetworkMenuCanvas.Visibility = Visibility.Hidden;
            ConnectToServerButton.Visibility = Visibility.Hidden;
            CreateMulGameCanvas.Visibility = Visibility.Visible;
            CreateServerButton.Visibility = Visibility.Visible;
            NetSizeComboBox.Visibility = Visibility.Visible;
            FieldSizeLabel.Visibility = Visibility.Visible;
            NetworkSetupLabel.Content = "Create network game";
            IpAddressTextBox.Text = NetworkManager.Instance.GetHostAddress();
            PortTextBox.Text = 11000.ToString();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            NetworkMenuCanvas.Visibility = Visibility.Hidden;
            CreateServerButton.Visibility = Visibility.Hidden;
            CreateMulGameCanvas.Visibility = Visibility.Visible;
            ConnectToServerButton.Visibility = Visibility.Visible;
            NetSizeComboBox.Visibility = Visibility.Hidden;
            FieldSizeLabel.Visibility = Visibility.Hidden;
            NetworkSetupLabel.Content = "Connect to server";
            IpAddressTextBox.Text = NetworkManager.Instance.GetHostAddress();
            PortTextBox.Text = 11000.ToString();
        }

        private void CreateServerButton_Click(object sender, RoutedEventArgs e)
        {
            NetworkManager.Instance.IsServer = true;
            NetworkManager.Instance.IsMultiplayerGame = true;
            ComboBoxItem cbi = (ComboBoxItem)NetSizeComboBox.ItemContainerGenerator.ContainerFromIndex(NetSizeComboBox.SelectedIndex);
            if (cbi == null) return;

            savedFieldSize = int.Parse((string) cbi.Content);

            CreateMulGameCanvas.Visibility = Visibility.Hidden;
            UIController.Instance.StartListenNetworkEvents();
            ConnectedListCanvas.Visibility = Visibility.Visible;
            //Create player
            MultiplayerGameController.Instance.CreatePlayer(0, PlayerNameTextBox.Text, "X", true);
            //Start server
            NetworkManager.Instance.StartServer(IpAddressTextBox.Text, PortTextBox.Text);
        }

        private void ConnectToServerButton_Click(object sender, RoutedEventArgs e)
        {
            NetworkManager.Instance.IsMultiplayerGame = true;
            CreateMulGameCanvas.Visibility = Visibility.Hidden;
            ConnectedListCanvas.Visibility = Visibility.Visible;

            UIController.Instance.StartListenNetworkEvents();

            //Create player
            MultiplayerGameController.Instance.CreatePlayer(1,PlayerNameTextBox.Text, "O", true);
            //Start server
            NetworkManager.Instance.StartClient(IpAddressTextBox.Text, PortTextBox.Text);
        }

        private void StartNetGameButton_Click(object sender, RoutedEventArgs e)
        {
            MultiplayerGameController.Instance.EndCreateGame += OnEndCreateNetGame;

            //ComboBoxItem cbi = (ComboBoxItem)sizeComboBox.ItemContainerGenerator.ContainerFromIndex(sizeComboBox.SelectedIndex);
            //if (cbi == null) return;
            //GameController.Instance.StartGame(int.Parse((string)cbi.Content));
            MultiplayerGameController.Instance.StartGame(savedFieldSize);
            NetworkManager.Instance.StartGame(savedFieldSize);
        }

        private void OnEndCreateNetGame()
        {
            ConnectedListCanvas.Visibility = Visibility.Hidden;
            gameCanvas.Visibility = Visibility.Visible;
        }
    }
}
