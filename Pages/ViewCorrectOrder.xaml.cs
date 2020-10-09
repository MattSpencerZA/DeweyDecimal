using Dewey_Training.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Dewey_Training.Pages
{
    /// <summary>
    /// Interaction logic for ViewCorrectOrder.xaml
    /// </summary>
    public partial class ViewCorrectOrder : Page
    {

        public ObservableCollection<DeweyDecimal> DeweyDecimal { get; set; }
        public List<DeweyDecimal> decimals = new List<DeweyDecimal>();

        public ViewCorrectOrder(List<string> correctOrder)
        {
            InitializeComponent();

            for (int i = 0; i < correctOrder.Count; i++)
            {

                decimals.Add(new DeweyDecimal()
                {
                    Decimal = correctOrder[i].Split(" ")[0],
                    Author = correctOrder[i].Split(" ")[1]
                });

            }

            DeweyDecimal = new ObservableCollection<DeweyDecimal>(decimals);

            (this.Content as FrameworkElement).DataContext = this;

        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {

            // Navigate the user to the menu page
            this.NavigationService.Navigate(new MenuPage());

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
