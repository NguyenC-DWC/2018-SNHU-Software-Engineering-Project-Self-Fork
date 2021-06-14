using Financing.Transactions;
using Financing.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.Win32;
using System.Text;

namespace Financing
{
    public partial class MainWindow : Window
    {
        int UserID = -1;
        int AccID = -1;

        decimal positiveFlow = 0m;
        decimal negativeFlow = 0m;

        //Former is used to store the transactions, and is observed when a change occurs. Latter is used to bulk insert all transactions from an account to the former without triggering the list change event.
        public ObservableCollection<Transaction> transactionList = new ObservableCollection<Transaction>();

        GridViewColumnHeader lastHeaderClicked = null;
        ListSortDirection lastDirection = ListSortDirection.Ascending;

        decimal balance = 0.00m;
        
        // Open the mainwindow with the specified profile loaded.
        public MainWindow(int _UserID)
        {
            InitializeComponent();

            UserID = _UserID;
            LoadAccounts(UserID);

            TransList.ItemsSource = transactionList;
            // Used for the collectionChanged event. Everytime the observable 
            // collection changes in number of item,  the event triggers.
            transactionList.CollectionChanged += transactionList_CollectionChanged;

            //Hides the add transaction button initially until an account has been clicked.
            Add_Transaction.Visibility = Visibility.Hidden;
            Export.Visibility = Visibility.Hidden;
        }

        //Query db for specified profiles accounts and displays to window.
        public void LoadAccounts(int UserID)
        {
            AccountSidenav.Children.Clear();

            // Query db for all accounts belonging to user.
            DataTable dt = SQLFunctions.GetAccounts(UserID);   

            // Iterate through each account.
            foreach (DataRow dr in dt.Rows)
            {
                // Store account information.
                int AccID = (int)dr["ID"];
                string AccName = dr["Accname"].ToString();
                string AccType = dr["AccType"].ToString();
                decimal AccBalance = (decimal)dr["Balance"];

                // Initalize framework controls.
                Border Spacer = new Border();
                Button Account = new Button();
                Grid AccountGrid = new Grid();
                TextBox AccountName = new TextBox();
                TextBox AccountType = new TextBox();
                TextBox AccountBalance = new TextBox();

                // Make textbox's intangible.
                AccountName.Focusable = false;
                AccountType.Focusable = false;
                AccountBalance.Focusable = false;

                // Maintain textbox cursor style.
                AccountName.Cursor = Cursors.Arrow;
                AccountType.Cursor = Cursors.Arrow;
                AccountBalance.Cursor = Cursors.Arrow;

                // Apply styles to controls.
                Account.Style = FindResource("AccountButton") as Style;
                AccountName.Style = FindResource("AccountName") as Style;
                AccountType.Style = FindResource("AccountType") as Style;
                AccountBalance.Style = FindResource("AccountBalance") as Style;

                // Set grid size (Determines size of stackpanel).
                AccountGrid.Width = 170;
                AccountGrid.Height = 50;

                // Set button content to sql data.
                AccountName.Text = AccName;
                AccountType.Text = Properties.Resources.ResourceManager.GetString(AccType);
                AccountBalance.Text = AccBalance.ToString("C", CultureInfo.GetCultureInfo(App.GetLanguage()));

                // Button event handler.
                Account.Click += delegate (object sender, RoutedEventArgs e){ Button_Click(sender, e, AccID, AccountName.Text); };

                // Add controls to grid.
                AccountGrid.Children.Add(AccountName);
                AccountGrid.Children.Add(AccountType);
                AccountGrid.Children.Add(AccountBalance);

                // Add grid to button.
                Account.Content = AccountGrid;

                // Add button to stackpanel.
                AccountSidenav.Children.Add(Account);
            }
        }

        private void Add_Trans_Click(object sender, RoutedEventArgs e)
        {
            if (checkIfWindowOpen())
            {
                MessageBox.Show(Properties.Resources.CompleteTransaction);
            }
            else
            {
                new AddTransaction(AccID,transactionList).Show();
            }
        }

        private void Add_Account_Click(object sender, RoutedEventArgs e)
        {
            if (checkIfWindowOpen())
            {
                MessageBox.Show(Properties.Resources.CompleteTransaction);
            }
            else
            {
                new NewBankAccount(UserID,this).Show();
            }
        }

        private void TransList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Transaction edit = TransList.SelectedItem as Transaction;
            if (TransList.SelectedItem as Transaction != null)
            {
                if (checkIfWindowOpen())
                {
                    MessageBox.Show(Properties.Resources.CompleteTransaction);
                }
                else
                {
                    EditTransaction transactionEdit = new EditTransaction(AccID, edit, transactionList);
                    transactionEdit.Show();
                }
            }
        }

        private void TransactionListSort(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection currentDirection;

            if (headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding)
            {
                if (checkLastHeaderDifferent(headerClicked))
                {
                    //All headers except the same one that was clicked will start in ascending order,
                    currentDirection = ListSortDirection.Ascending;
                }
                else
                {
                    currentDirection = setLastDirection();
                }
                string header = headerClicked.Column.Header as string;
                SortList(header, currentDirection);

                lastHeaderClicked = headerClicked;
                lastDirection = currentDirection;
            }

        }

        private void SortList(string header, ListSortDirection direction)
        {
            ICollectionView listSort = CollectionViewSource.GetDefaultView(TransList.ItemsSource);

            listSort.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(header, direction);
            listSort.SortDescriptions.Add(sd);
            listSort.Refresh();
        }

        private bool checkLastHeaderDifferent(GridViewColumnHeader headerClicked)
        {
            if (headerClicked != lastHeaderClicked)
            {
                return true;
            }
            return false;
        }

        private ListSortDirection setLastDirection()
        {
            if (lastDirection == ListSortDirection.Ascending)
            {
                return ListSortDirection.Descending;
            }
            else
            {
                return ListSortDirection.Ascending;
            }
        }

        public bool checkIfWindowOpen()
        {
            //Assures that only one window is open at a time.
            foreach (Window w in Application.Current.Windows)
            {
                if (w is EditTransaction || w is AddTransaction)
                {
                    return true;
                }
            }
            return false;
        }

        void transactionList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            updateBalance();
            RefreshCashFlow();
        }

        public void updateBalance()
        {
            balance = 0.00m;
            for (int i = 0; i < transactionList.Count(); i++)
            {
                if (transactionList[i].Type == "Deposit")
                {
                    balance += transactionList[i].Amount;
                }
                else
                {
                    balance -= transactionList[i].Amount;
                }
            }

            Balance.Text = balance.ToString("C", 
                CultureInfo.GetCultureInfo(App.GetLanguage()));
        }

        // Loads transactions of the clicked account.
        private void Button_Click(object sender, RoutedEventArgs e, int AccountID, string Name)
        {
            if (checkIfWindowOpen())
            {
                MessageBox.Show(Properties.Resources.CompleteTransaction);
            }
            else
            {
                TransactionTableHeader.Header = Name + " " + Properties.Resources.Transactions;

                Add_Transaction.Visibility = Visibility.Visible;
                Export.Visibility = Visibility.Visible;

                DataTable dt = SQLFunctions.GetTransactions(AccountID);
                transactionList.Clear();

                CashFlowGroupBox.Header = "Cash Flow: " + DateTime.Now.ToString("MMM");

                // Iterate through each Transaction.
                foreach (DataRow dr in dt.Rows)
                {
                    transactionList.Add(new Transaction(
                         _AccountID: Convert.ToInt32(dr["AccID"].ToString()),
                         _Amount: (decimal)dr["Value"],
                         _Type: dr["Type"].ToString(),
                         _Category: dr["Category"].ToString(),
                         _Date: dr["Date"].ToString(),
                         _Description: dr["Description"].ToString(),
                         _Merchant: dr["Merchant"].ToString()));
                }

                AccID = AccountID;
                updateBalance();

                // Set cashflow values.
                PositiveCashFlowBar.Value = (double)positiveFlow;
                NegativeCashFlowBar.Value = (double)negativeFlow;

                // Set cashflow maximum to largest cashflow value.
                if (positiveFlow > negativeFlow)
                {
                    PositiveCashFlowBar.Maximum = (double)positiveFlow;
                    NegativeCashFlowBar.Maximum = (double)positiveFlow;
                }
                else
                {
                    PositiveCashFlowBar.Maximum = (double)negativeFlow;
                    NegativeCashFlowBar.Maximum = (double)negativeFlow;
                }
            }
        }

        // Filters out transactions from the list that do not match the searched category.
        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Clear filter if empty.
            if (FilterBox.Text == "")
            {
                TransList.ItemsSource = transactionList;
            }
            else
            {
                //Set Listviews source to filtered datasource.
                List<Transaction> filteredList = transactionList.Where(x =>
                x.displayCategory.ToLower().Contains(FilterBox.Text.ToLower())).ToList();

                TransList.ItemsSource = filteredList;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder createCSV = new StringBuilder();

            SaveFileDialog saveTransactions = new SaveFileDialog();
            saveTransactions.Filter = "Comma Seperated Value (*.csv)|*.csv";

            if(saveTransactions.ShowDialog() == true)
            {
                foreach(GridViewColumn headers in TransListGrid.Columns)
                {
                    createCSV.Append(headers.Header + ",");
                }

                createCSV.Remove(createCSV.Length - 1, 1);
                createCSV.Append(Environment.NewLine);

                foreach (var exportTrans in transactionList)
                {
                    createCSV.Append("\"" + exportTrans.displayDate + "\",");
                    createCSV.Append("\"" + exportTrans.Merchant + "\",");
                    createCSV.Append("\"" + exportTrans.Description + "\",");
                    createCSV.Append("\"" + exportTrans.displayType + "\",");
                    createCSV.Append(exportTrans.displayAmount + ",");
                    createCSV.Append("\"" + exportTrans.displayCategory + "\"\n");
                }
                File.WriteAllText(saveTransactions.FileName, createCSV.ToString());
                MessageBox.Show("CSV sucessfully exported to: " + saveTransactions.FileName);
                
            }
            
        }
        
        private void UpdateCashFlow(string type, decimal Amount)
        {
            if (type == "Withdrawal" || type == "Debit" || type == "Check" || type == "Transfer")
            {
                negativeFlow += Amount;
                Spent_lbl.Content = "Spent: " + negativeFlow.ToString("C",
                    CultureInfo.GetCultureInfo(App.GetLanguage()));
            }
            else
            {
                positiveFlow += Amount;
                Earned_lbl.Content = "Earned: " + positiveFlow.ToString("C",
                    CultureInfo.GetCultureInfo(App.GetLanguage()));
            }
        }

        private void RefreshCashFlow()
        {
            negativeFlow = 0;
            positiveFlow = 0;

            foreach (var transaction in transactionList)
            {
                //
                if (DateTime.ParseExact(transaction.Date, "M/d/yyyy", null).Month == DateTime.Now.Month)
                {
                    UpdateCashFlow(transaction.Type, transaction.Amount);
                }
            }

            // Set cashflow maximum to largest cashflow value.
            if (positiveFlow > negativeFlow)
            {
                PositiveCashFlowBar.Maximum = (double)positiveFlow;
                NegativeCashFlowBar.Maximum = (double)positiveFlow;
                PositiveCashFlowBar.Value = (double)positiveFlow;
                NegativeCashFlowBar.Value = (double)negativeFlow;
            }
            else
            {
                PositiveCashFlowBar.Maximum = (double)negativeFlow;
                NegativeCashFlowBar.Maximum = (double)negativeFlow;
                PositiveCashFlowBar.Value = (double)positiveFlow;
                NegativeCashFlowBar.Value = (double)negativeFlow;
            }
        }

        //********************************************
        //Ignore below for now.
        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {

            //TransactionWindow newW = new TransactionWindow();
            //newW.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            //TransactionWindow newW = new TransactionWindow();
            //newW.Show();
        }
    }
}