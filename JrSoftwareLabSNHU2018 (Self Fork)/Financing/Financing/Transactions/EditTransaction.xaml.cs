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
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;

namespace Financing.Transactions
{
    /// <summary>
    /// Interaction logic for EditTransaction.xaml
    /// </summary>
    public partial class EditTransaction : Window
    {
        int Account = -1;

        Transaction currentTrans;
        private ObservableCollection<Transaction> recieveList;
        
        Regex numericalOnly = new Regex("[^0-9.,]+");
        Regex moneyFormat = new Regex("^[0-9]{1,3}([.,][0-9]{1,2})?$");

        public EditTransaction(int AccountID, Transaction edit, ObservableCollection<Transaction> transactionList)
        {
            Account = AccountID;
            recieveList = transactionList;
            currentTrans = edit;
            InitializeComponent();

            loadTransactionInformation();

            // Offsets the window to top-left of mainwindow.
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = SystemParameters.WorkArea.Width / 3.5;
            this.Top = SystemParameters.WorkArea.Height / 3.5;
        }

        private void loadTransactionInformation()
        {
            AmountTextInput.Text = currentTrans.Amount.ToString("F");
            TransactionDateBox.Text = currentTrans.displayDate;
            TransactionDescriptionText.Text = currentTrans.Description;
            MerchantInput.Text = currentTrans.Merchant;

        }

        private void TransactionTypeBox_Loaded(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> TypeDict = new Dictionary<string, string>
            {
                { Properties.Resources.Withdrawal,"Withdrawal"},
                { Properties.Resources.Deposit,"Deposit"},
                { Properties.Resources.Check,"Check"},
                { Properties.Resources.Transfer,"Transfer"},
                { Properties.Resources.Debit,"Debit"}
            };

            TransactionTypeBox.DisplayMemberPath = "Key";
            TransactionTypeBox.SelectedValuePath = "Value";
            TransactionTypeBox.ItemsSource = TypeDict;

            if (currentTrans.Type == "Withdrawal")
                TransactionTypeBox.SelectedIndex = 0;
            else
                TransactionTypeBox.SelectedIndex = 1;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Makes sure that any input in the amount text box is a decimal or a numeric.
            e.Handled = numericalOnly.IsMatch(e.Text);
        }

        private void isInputValid()
        {
            if (TransactionDateBox.ToString() == "")
            {
                throw new Exception(Properties.Resources.SpecifyDate);
            }
            else if (TransactionDescriptionText.Text == "")
            {
                throw new Exception(Properties.Resources.SpecifyDetails);
            }
            else if (MerchantInput.Text == "")
            {
                throw new Exception(Properties.Resources.SpecifyMerchant);
            }
            else if (!moneyFormat.IsMatch(AmountTextInput.Text))
            {
                if (AmountTextInput.Text == "")
                {
                    throw new Exception(Properties.Resources.SpecifyAmount);
                }
                else
                {
                    throw new Exception(Properties.Resources.IncorrectFormat);
                }
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isInputValid();
                string message = Properties.Resources.ConfirmEdits;
                if (confirmResult(message))
                {
                    Transaction nTransaction = new Transaction(
                    _AccountID: Account,
                    _Amount: decimal.Parse(AmountTextInput.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                    _Type: TransactionTypeBox.SelectedValue.ToString(),
                    _Category: TransactionCategoryBox.SelectedValue.ToString(),
                    _Date: DateTime.Parse(TransactionDateBox.Text).ToString("M/d/yyyy"),
                    _Description: TransactionDescriptionText.Text,
                    _Merchant: MerchantInput.Text);

                    Decimal testA = currentTrans.Amount;
                    string testT = currentTrans.Type;
                    string testC = currentTrans.Category;
                    string testD = currentTrans.Date;
                    string testDe = currentTrans.Description;
                    
                    SQLFunctions.updateTransaction(currentTrans,nTransaction);

                    recieveList.Remove(currentTrans);
                    recieveList.Add(nTransaction);
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private bool confirmResult(String message)
        {
            MessageBoxResult confirmAction = MessageBox.Show(message, Properties.Resources.ConfirmQuestion, MessageBoxButton.YesNo);
            if (confirmAction == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string message = Properties.Resources.ConfirmDelete;
            if (confirmResult(message))
            {
                SQLFunctions.removeTransaction(currentTrans);
                recieveList.Remove(currentTrans);
                this.Close();
            }
        }

        private void TransactionCategoryBox_Loaded(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> CategorieDict = new Dictionary<string, string>();
            List<string> Categories = new List<string>();

            // Sql results.
            DataTable dt = SQLFunctions.GetCategories();

            // Iterate through categories.
            foreach (DataRow Category in dt.Rows)
            {
                // Add category name to category list.
                string label = Properties.Resources.ResourceManager.GetString(Category[0].ToString()); //Category in set language.
                string value = Category[0].ToString(); //Category in english.
                CategorieDict.Add(label, value);
                Categories.Add(value);
            }

            // Set category ddl itemsource to list of categories.
            TransactionCategoryBox.DisplayMemberPath = "Key";
            TransactionCategoryBox.SelectedValuePath = "Value";
            TransactionCategoryBox.ItemsSource = CategorieDict;

            setCategoryBox(Categories);
            checkCategoryEnable();
        }

        private void setCategoryBox(List<string> categoryList)
        {
            for (int i = 0; i < TransactionCategoryBox.Items.Count; i++)
            {
                if (categoryList.ElementAt(i) == currentTrans.Category)
                {
                    TransactionCategoryBox.SelectedIndex = i;
                    return;
                }
            }
        }

        private void TransactionTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkCategoryEnable();
        }

        private void checkCategoryEnable()
        {
            if (TransactionTypeBox.SelectedValue.ToString() == "Withdrawal")
            {
                TransactionCategoryBox.IsEnabled = true;
            }
            else
            {
                disableCategory();
            }
        }

        private void disableCategory()
        {
            TransactionCategoryBox.SelectedIndex = 0;
            TransactionCategoryBox.IsEnabled = false;
        }
    }
}
