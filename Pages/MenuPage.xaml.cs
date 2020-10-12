﻿using System;
using System.Collections.Generic;
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

namespace DeweyDecimalTrainer.Pages
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void ReplaceBooks_Click(object sender, RoutedEventArgs e) =>
            // Navigate the user to the Replace Books page
            this.NavigationService.Navigate(new ReplaceBooks());

        private void IdentifyAreas_Click(object sender, RoutedEventArgs e) =>

            // Navigate the user to the Identify Area page
            this.NavigationService.Navigate(new IdentifyAreas());
    }
}
