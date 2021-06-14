using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financing.Accounts
{
    public class BankAccounts
    {
        public int UserID { get; set; }
        public string AccName { get; set; }
        public string AccType { get; set; }
        public decimal Balance { get; set; }

        public BankAccounts(int _UserID, string _AccName, string _AccType, decimal _Balance)
        {
            UserID = _UserID;
            AccName = _AccName;
            AccType = _AccType;
            Balance = _Balance;
        }
    }
}
