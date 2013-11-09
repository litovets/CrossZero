﻿using System;
using System.Windows;
using System.Windows.Controls;
using Cross_Zero.Logic;

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

        private void OnEndCreateGame()
        {
            menuCanvas.Visibility = Visibility.Hidden;
            optionsCanvas.Visibility = Visibility.Hidden;
            gameCanvas.Visibility = Visibility.Visible;
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            menuCanvas.Visibility = Visibility.Hidden;
            optionsCanvas.Visibility = Visibility.Visible;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            GameController.Instance.EndCreateGame += OnEndCreateGame;

            UIController.Instance.Canvas = gameCanvas;
            UIController.Instance.ActivePlayerLabel = ActivePlayerLabel;
            ComboBoxItem cbi = (ComboBoxItem) sizeComboBox.ItemContainerGenerator.ContainerFromIndex(sizeComboBox.SelectedIndex);
            if (cbi == null) return;
            GameController.Instance.StartGame(int.Parse((string)cbi.Content));
        }
    }
}
