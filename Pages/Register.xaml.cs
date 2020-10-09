using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Dewey_Training_DAL;
using Dewey_Training_DAL.Models;

namespace Dewey_Training.Pages
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {

            // Color variable used for validation
            Brush red = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ca3e47"));
            Brush black = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));

            UserAccess ua = new UserAccess();

            // Sets user information from user input
            string userName = username.Text;
            string pass = password.Password;
            string confirmPass = confirmPassword.Password;
            bool validation = true;

            username.Foreground = black;
            password.Foreground = black;
            confirmPassword.Foreground = black;

            if (userName.Equals(""))
            {
                username.Foreground = red;
                MessageBox.Show("Username cannot be empty!", "Error");
                validation = false;
            }
            if (pass.Equals("") || pass.Length < 6)
            {
                password.Foreground = red;
                MessageBox.Show("Password cannot be empty!", "Error");
                validation = false;
            }
            if (confirmPass.Equals("") || !pass.Equals(confirmPass))
            {
                confirmPassword.Foreground = red;
                MessageBox.Show("Passwords do not match!", "Error");
                validation = false;
            }

            // Check that the username is not in use by another user
            if (!ua.CheckUsernameInUse(userName) && validation)
            {

                int id = ua.GetNewUserId();

                if (id == -9)
                {
                    // Confirmation message
                    MessageBox.Show("Cannot create a unique id for the user.", "Error");
                }
                else
                {

                    User user = new User()
                    {
                        Id = id,
                        Username = userName,
                        Password = pass
                    };

                    if (ua.CreateUser(user))
                    {

                        // Confirmation message
                        MessageBox.Show("The account has been created successfully.", "Account Created");

                        // Navigate to the Login page
                        this.NavigationService.Navigate(new Login());

                    }
                    else
                    {

                        // Error message
                        MessageBox.Show("The account cannot be created.", "Error");

                    }

                }

            }
            else if (ua.CheckUsernameInUse(userName))
            {

                // Form validation
                username.Foreground = red;
                MessageBox.Show("Entered username is in use!\nPlease choose a different username.", "Error");

            }

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {

            // Navigate the user to the Login page
            this.NavigationService.Navigate(new Login());

        }
    }
}
