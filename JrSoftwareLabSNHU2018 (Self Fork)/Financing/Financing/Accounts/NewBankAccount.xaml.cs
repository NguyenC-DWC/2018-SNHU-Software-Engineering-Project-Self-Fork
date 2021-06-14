using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Financing.Accounts
{
    /// <summary>
    /// Interaction logic for NewBankAccount.xaml
    /// </summary>
    public partial class NewBankAccount : Window
    {
        int User = -1;
        MainWindow updater;

        public NewBankAccount(int userID, MainWindow original)
        {
            InitializeComponent();
            User = userID;
            updater = original;

            // Offsets the window to top-left of mainwindow.
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = SystemParameters.WorkArea.Width / 3.5;
            this.Top = SystemParameters.WorkArea.Height / 3.5;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(AccountNameBox.Text == "")
                {
                    throw new Exception("Please specify an account name.");
                }

                SQLFunctions.CreateBankAccount(User,AccountNameBox.Text,AccountTypeBox.SelectedValue.ToString());
                updater.LoadAccounts(User);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AccountTypeBox_Loaded(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> AccTypeDict = new Dictionary<string, string>();

            AccTypeDict.Add(Properties.Resources.ResourceManager.GetString("Savings"), "Savings");
            AccTypeDict.Add(Properties.Resources.ResourceManager.GetString("Checking"), "Checking");

            AccountTypeBox.DisplayMemberPath = "Key";
            AccountTypeBox.SelectedValuePath = "Value";
            AccountTypeBox.ItemsSource = AccTypeDict;
            AccountTypeBox.SelectedIndex = 0;
        }
    }
}
