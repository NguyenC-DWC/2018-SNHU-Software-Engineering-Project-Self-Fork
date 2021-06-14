using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
namespace Financing.Transactions
{
    public class Transaction
    {
        public int AccountID { get; set; }
        public decimal Amount { get; set; }
        public string displayAmount { get; }
        public string Type { get; set; }  
        public string displayType { get; }
        public string Category { get; set; }
        public string displayCategory { get; }
        public string Date { get; set; }
        public string displayDate { get; }
        public string Description { get; set; }
        public string Merchant { get; set; }

        //Without Merchant.
        public Transaction(int _AccountID, decimal _Amount, string _Type, string _Category, string _Date, string _Description)
        {
            AccountID = _AccountID;

            Amount = _Amount;
            displayAmount = Amount.ToString("F",CultureInfo.CurrentUICulture);

            Type = _Type;
            displayType = Properties.Resources.ResourceManager.GetString(Type);

            Category = _Category;
            displayCategory = Properties.Resources.ResourceManager.GetString(Category);

            Date = _Date;
            displayDate = DateTime.Parse(Date, CultureInfo.InvariantCulture).ToShortDateString();

            Description = _Description;

            Merchant = "None";
        }

        //With Merchant
        public Transaction(int _AccountID, decimal _Amount, string _Type, string _Category, string _Date, string _Description, string _Merchant)
        {
            AccountID = _AccountID;

            Amount = _Amount;
            displayAmount = Amount.ToString("F", CultureInfo.CurrentUICulture);

            Type = _Type;
            displayType = Properties.Resources.ResourceManager.GetString(Type);

            Category = _Category;
            displayCategory = Properties.Resources.ResourceManager.GetString(Category);

            Date = _Date;
            displayDate = DateTime.Parse(Date, CultureInfo.InvariantCulture).ToShortDateString();

            Description = _Description;

            Merchant = _Merchant;
        }
    }
}
