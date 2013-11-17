﻿using System;
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
        public MainWindow()
        {
            InitializeComponent();

            UIController.Instance.Canvas = gameCanvas;
            UIController.Instance.ActivePlayerLabel = ActivePlayerLabel;
            UIController.Instance.LinePos = LinePosLabel;
            UIController.Instance.ConnectedListBox = ConnectedListBox;
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

        private void CreateMulGameButton_Click(object sender, RoutedEventArgs e)
        {
            NetworkMenuCanvas.Visibility = Visibility.Hidden;
            ConnectToServerButton.Visibility = Visibility.Hidden;
            CreateMulGameCanvas.Visibility = Visibility.Visible;
            CreateServerButton.Visibility = Visibility.Visible;
            IpAddressTextBox.Text = NetworkManager.Instance.GetHostAddress();
            PortTextBox.Text = 11000.ToString();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            NetworkMenuCanvas.Visibility = Visibility.Hidden;
            CreateServerButton.Visibility = Visibility.Hidden;
            CreateMulGameCanvas.Visibility = Visibility.Visible;
            ConnectToServerButton.Visibility = Visibility.Visible;
            IpAddressTextBox.Text = NetworkManager.Instance.GetHostAddress();
            PortTextBox.Text = 11000.ToString();
        }

        private void CreateServerButton_Click(object sender, RoutedEventArgs e)
        {
            CreateMulGameCanvas.Visibility = Visibility.Hidden;
            UIController.Instance.StartListenNetworkEvents();
            NetworkManager.Instance.StartServer(IpAddressTextBox.Text, PortTextBox.Text);

        }
    }
}
