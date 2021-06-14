using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Financing
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class FoundAccount : Window
    {
        bool EmailValidated = false;
        bool PasswordValidated = false;
        bool ConfirmValidated = false;

        public FoundAccount()
        {
            InitializeComponent();

            Password_box.ToolTip = Properties.Resources.PasswordValidCheck;
        }

        //Changes focus to next text box upon pressing enter.
        private void Email_box_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
                Password_box.Focus();
        }

        //Changes focus to next text box upon pressing enter.
        private void Password_box_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
                Confirm_box.Focus();
        }

        //Changes focus to Submit button upon pressing enter.
        private void Confirm_box_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
                Submit.Focus();
        }

        //Submits registration info and returns to login.
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (EmailValidated == true && PasswordValidated == true && ConfirmValidated == true)
            {
                var login = new Login();
                login.Show();
                this.Close();
            }
        }

        // Lets the window be dragged.
        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        //Closes the application.
        private void BtnClose_Click(object sender, RoutedEventArgs e) => App.Current.Shutdown();

        //Validates Email. Must be format of [username]@[server].[domain].
        private void Email_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Returns true if in proper email format.
            var match = Regex.Match(Email_box.Text, @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$");

            //Warns of improper Email, removes warning if content is valid.
            if (!match.Success)
            {
                Email_Warning.Content = Properties.Resources.InvalidEmail;
                EmailValidated = false;
            }
            else
            {
                Email_Warning.Content = "";
                EmailValidated = true;
            }
        }

        //Validates password. Must contain 1 captial, 1 special character, and must be 8 or more characters.
        private void Password_box_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Returns false if there are special characters.
            var match = Regex.Match(Password_box.Password.ToString(), @"^[a-zA-Z0-9]*$");

            //Warns of improper Password, removes warning if content is valid.
            if (match.Success || Password_box.Password.Length < 8 || Password_box.Password == Password_box.Password.ToLower())
            {
                Password_Warning.Content = Properties.Resources.InvalidPassword;
                PasswordValidated = false;
            }
            else
            {
                Password_Warning.Content = "";
                PasswordValidated = true;
            }
        }

        //User re-enters password. Passwords must match.
        private void Confirm_box_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Warns of improper confirmation password, removes warning if content is valid.
            if (Password_box.Password != Confirm_box.Password)
            {
                Confirm_Warning.Content = Properties.Resources.PasswordsNotMatch;
                ConfirmValidated = false;
            }
            else
            {
                Confirm_Warning.Content = "";
                ConfirmValidated = true;
            }
        }
    }
}
