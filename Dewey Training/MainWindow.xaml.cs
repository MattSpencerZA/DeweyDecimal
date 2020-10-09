using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using DeweyDecimalTrainer.Pages;

namespace DeweyDecimalTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Delegate used to invoke window state method
        public delegate void initializeDelegate();

        public MainWindow()
        {
            InitializeComponent();

            // Creating a delegate object
            initializeDelegate setWindowState = new initializeDelegate(InitializeWindowState);
            // Running window state method
            setWindowState();

        }

        public void InitializeWindowState()
        {
            // Sets default startup location
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        
        private void minimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        
        private void Frame_Loaded(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new MenuPage());
        }

        // Exit button pressed
        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            // Close the application
            Close();
        }

        // Enables window dragging on the top portion of the window
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
