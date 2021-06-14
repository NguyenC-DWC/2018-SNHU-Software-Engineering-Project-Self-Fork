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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Financing.Transactions
{
    /// <summary>
    /// Interaction logic for TransactionWindow.xaml
    /// </summary>
    public partial class TransactionWindow : Window
    {
        public ObservableCollection<Transaction> transactionList = new ObservableCollection<Transaction>();
        private List<String> categories = new List<String> { "None", "Food", "Utilities", "Shopping", "Services" ,"Other" };

        GridViewColumnHeader lastHeaderClicked = null;
        ListSortDirection lastDirection = ListSortDirection.Ascending;

        double balance = 0.00;

        public TransactionWindow()
        {
            InitializeComponent();
            TransList.ItemsSource = transactionList;

            Balance.Text = "$ 0.00";

            //Used for the collectionChanged event. Everytime the observable collection changes in number of item, the event triggers.
            transactionList.CollectionChanged += transactionList_CollectionChanged;

            checkForExport();
        }

        private void Add_Trans_Click(object sender, RoutedEventArgs e)
        {
            if (checkIfWindowOpen())
            {
                MessageBox.Show("Please complete the current transaction you want to add or edit.");
            }
            else
            {
                //AddTransaction transactionAdd = new AddTransaction(transactionList);
                //transactionAdd.Show();
            }
        }

        private void TransList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Transaction edit = TransList.SelectedItem as Transaction;
            if (TransList.SelectedItem as Transaction != null)
            {
                if (checkIfWindowOpen())
                {
                    MessageBox.Show("Please complete the current transaction you want to add or edit.");
                }
                else
                {
                    EditTransaction transactionEdit = new EditTransaction(-1,edit,transactionList);
                    transactionEdit.Show();
                }
            }
        }

        private void TransactionListSort(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection currentDirection;
            string header = headerClicked.Column.Header as string;

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

        void transactionList_CollectionChanged(object sender,System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            updateBalance();
            checkForExport();
        }

        public void updateBalance()
        {
            balance = 0.00;
            for (int i = 0; i < transactionList.Count(); i++)
            {
                if (transactionList[i].Type == "Deposit")
                {
                    //balance += transactionList[i].Amount;
                }
                else
                {
                    //balance -= transactionList[i].Amount;
                }
            }

            Balance.Text = "$ " + balance.ToString();
        }

        // Closes transactionwindow.
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void checkForExport()
        {
            if(transactionList.Count() > 0)
            {
                Export.IsEnabled = true;
            }
            else
            {
                Export.IsEnabled = false;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
