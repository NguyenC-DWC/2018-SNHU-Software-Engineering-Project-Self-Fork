using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;

namespace Financing
{
    /// <summary>
    /// Interaction logic for loginWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        // Constructor, Initializes window and thread.
        public Login()
        {
            InitializeComponent();
            //Starts with username textbox focus.
            UsernameTxtBx.Focus();

            Login_Button.Content += " >";

            // Creates a dictionary of languages and their locale codes.
            Dictionary<string, string> Languages = new Dictionary<string, string>();
            Languages.Add("en-US", "English");
            Languages.Add("es-ES", "Español");
            Languages.Add("fr-FR", "français");
            Languages.Add("it-IT", "Italiano");

            // Sets the combobox's itemsource to the language dictionary.
            LangComboBox.ItemsSource = Languages;

            // Set the selected item to the current language.
            foreach (KeyValuePair<string, string> item in Languages)
            {
                if (item.Key == App.GetLanguage())
                {
                    LangComboBox.SelectedItem = item;
                }
            }

            //MessageBox.Show(LangComboBox.SelectedValue.ToString());
        }

        // Validates credentials and opens mainwindow on success.
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            string userName = UsernameTxtBx.Text.ToLower();
            string pass = Password_Box.Password;
            int UserID = -1;
            
            // Enables hourglass cursor while calidating credentials.
            Mouse.OverrideCursor = Cursors.Wait;

            // Disables the username, password, and login button 
            // while validating already entered credentials.
            UsernameTxtBx.IsEnabled = false;
            Password_Box.IsEnabled = false;
            Login_Button.IsEnabled = false;

            // Get Users login ID.
            UserID = SQLFunctions.ValidateLogin(userName, pass);

            // Validates user credentials.
            if (UserID != -1)
            {
                ForgotPass_Lbl.Content = "LOGIN SUCCESSFUL";
                Mouse.OverrideCursor = null;
                UsernameTxtBx.IsEnabled = true;
                Password_Box.IsEnabled = true;
                Login_Button.IsEnabled = true;

                var newW = new MainWindow(UserID);
                newW.Show();

                this.Close();
            }
            else
            {
                // Re-enables username and password fields and sets them to red.
                UsernameTxtBx.IsEnabled = true;
                UserNameBorder.BorderBrush = Brushes.Red;
                Password_Box.IsEnabled = true;
                PasswordBorder.BorderBrush = Brushes.Red;
                Login_Button.IsEnabled = true;

                // Disables hourglass cursor.
                Mouse.OverrideCursor = null;
            }
        }

        // Exit button closes window
        private void Close_Button_Click(object sender, RoutedEventArgs e) => App.Current.Shutdown();

        // Lets the window be dragged.
        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            // Must left click and hold.
            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        // Returns border to white if text has changed after incorrectly entering username
        private void UsernameTxtBx_TextChanged(object sender, TextChangedEventArgs e) => UserNameBorder.BorderBrush = Brushes.White;

        // Returns border to white if text has changed after incorrectly entering password
        private void Password_Box_PasswordChanged(object sender, RoutedEventArgs e) => PasswordBorder.BorderBrush = Brushes.White;

        // If enter is pressed moves to password field.
        private void UsernameTxtBx_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
                UsernameTxtBx.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        // If enter is pressed, activate login button.
        private void Password_Box_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                Login_Button_Click(sender, e);
            }
        }

        // Changes the localization to the selected language and updates UI.
        private void LangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ignore when selecting the current language.
            if (App.GetLanguage() != LangComboBox.SelectedValue.ToString())
            {
                App.SetLanguage(LangComboBox.SelectedValue.ToString());
            }
        }

        private void Register_Button_Click(object sender, RoutedEventArgs e)
        {
            new Register().Show();
            Close();
        }

        // Opens Register window and closes login window.
        private void ForgotPass_Lbl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("That's unfortunate. We haven't implemented that feature yet.");
        }
    }
}